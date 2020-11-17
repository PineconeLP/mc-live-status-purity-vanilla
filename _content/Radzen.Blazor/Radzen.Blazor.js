if (!Element.prototype.matches) {
  Element.prototype.matches =
    Element.prototype.msMatchesSelector ||
    Element.prototype.webkitMatchesSelector;
}

if (!Element.prototype.closest) {
  Element.prototype.closest = function (s) {
    var el = this;

    do {
      if (el.matches(s)) return el;
      el = el.parentElement || el.parentNode;
    } while (el !== null && el.nodeType === 1);
    return null;
  };
}

var resolveCallbacks = [];
var rejectCallbacks = [];

window.Radzen = {
  addContextMenu: function (id, ref) {
     var el = document.getElementById(id);
     if (el) {
        var handler = function (e) {
            e.preventDefault(); e.pa
            ref.invokeMethodAsync('RadzenComponent.RaiseContextMenu',
                {
                    ClientX: e.clientX,
                    ClientY: e.clientY,
                    ScreenX: e.screenX,
                    ScreenY: e.screenY,
                    AltKey: e.altKey,
                    ShiftKey: e.shiftKey,
                    CtrlKey: e.ctrlKey,
                    MetaKey: e.metaKey,
                    Button: e.button,
                    Buttons: e.buttons,
                });
            return false;
        };
        Radzen[id + 'contextmenu'] = handler;
        el.addEventListener('contextmenu', handler, true);
     }
  },
  addMouseEnter: function (id, ref) {
     var el = document.getElementById(id);
     if (el) {
        var handler = function (e) {
            ref.invokeMethodAsync('RadzenComponent.RaiseMouseEnter');
        };
        Radzen[id + 'mouseenter'] = handler;
        el.addEventListener('mouseenter', handler, false);
     }
  },
  addMouseLeave: function (id, ref) {
     var el = document.getElementById(id);
     if (el) {
        var handler = function (e) {
            ref.invokeMethodAsync('RadzenComponent.RaiseMouseLeave');;
        };
        Radzen[id + 'mouseleave'] = handler;
        el.addEventListener('mouseleave', handler, false);
     }
  },
  removeContextMenu: function (id) {
      var el = document.getElementById(id);
      if (el && Radzen[id + 'contextmenu']) {
          el.removeEventListener('contextmenu', Radzen[id + 'contextmenu']);
      }
  },
  removeMouseEnter: function (id) {
      var el = document.getElementById(id);
      if (el && Radzen[id + 'mouseenter']) {
          el.removeEventListener('mouseenter', Radzen[id + 'mouseenter']);
      }
  },
  removeMouseLeave: function (id) {
      var el = document.getElementById(id);
      if (el && Radzen[id + 'mouseleave']) {
          el.removeEventListener('mouseleave', Radzen[id + 'mouseleave']);
      }
  },
  adjustDataGridHeader: function (scrollableHeader, scrollableBody) {
    if (scrollableHeader && scrollableBody) {
      scrollableHeader.style.cssText =
        scrollableBody.clientHeight < scrollableBody.scrollHeight
          ? 'margin-left:0px;margin-right: ' +
            (scrollableBody.offsetWidth - scrollableBody.clientWidth) +
            'px'
          : 'margin-left:0px;';
    }
  },
  preventArrows: function (el) {
    var preventDefault = function (e) {
      if (e.keyCode === 38 || e.keyCode === 40) {
        e.preventDefault();
        return false;
      }
    };
    el.addEventListener('keydown', preventDefault, false);
  },
  loadGoogleMaps: function (defaultView, apiKey, resolve, reject) {
    resolveCallbacks.push(resolve);
    rejectCallbacks.push(reject);

    if (defaultView['rz_map_init']) {
      return;
    }

    defaultView['rz_map_init'] = function () {
      for (var i = 0; i < resolveCallbacks.length; i++) {
        resolveCallbacks[i](defaultView.google);
      }
    };

    var document = defaultView.document;
    var script = document.createElement('script');

    script.src =
      'https://maps.googleapis.com/maps/api/js?' +
      (apiKey ? 'key=' + apiKey + '&' : '') +
      'callback=rz_map_init';

    script.async = true;
    script.defer = true;
    script.onerror = function (err) {
      for (var i = 0; i < rejectCallbacks.length; i++) {
        rejectCallbacks[i](err);
      }
    };

    document.body.appendChild(script);
  },
  createMap: function (wrapper, ref, id, apiKey, zoom, center, markers) {
    var api = function () {
      var defaultView = document.defaultView;

      return new Promise(function (resolve, reject) {
        if (defaultView.google && defaultView.google.maps) {
          return resolve(defaultView.google);
        }

        Radzen.loadGoogleMaps(defaultView, apiKey, resolve, reject);
      });
    };

    api().then(function (google) {
      Radzen[id] = ref;
      Radzen[id].google = google;

      Radzen[id].instance = new google.maps.Map(wrapper, {
        center: center,
        zoom: zoom
      });

      Radzen[id].instance.addListener('click', function (e) {
        Radzen[id].invokeMethodAsync('RadzenGoogleMap.OnMapClick', {
          Position: {Lat: e.latLng.lat(), Lng: e.latLng.lng()}
        });
      });

      Radzen.updateMap(id, zoom, center, markers);
    });
  },
  updateMap: function (id, zoom, center, markers) {
    if (Radzen[id] && Radzen[id].instance) {
      if (Radzen[id].instance.markers && Radzen[id].instance.markers.length) {
        for (var i = 0; i < Radzen[id].instance.markers.length; i++) {
          Radzen[id].instance.markers[i].setMap(null);
        }
      }

      Radzen[id].instance.markers = [];

      markers.forEach(function (m) {
        var marker = new this.google.maps.Marker({
          position: m.position,
          title: m.title,
          label: m.label
        });

        marker.addListener('click', function (e) {
          Radzen[id].invokeMethodAsync('RadzenGoogleMap.OnMarkerClick', {
            Title: marker.title,
            Label: marker.label,
            Position: marker.position
          });
        });

        marker.setMap(Radzen[id].instance);

        Radzen[id].instance.markers.push(marker);
      });

      Radzen[id].instance.setZoom(zoom);

      Radzen[id].instance.setCenter(center);
    }
  },
  destroyMap: function (id) {
    if (Radzen[id].instance) {
      delete Radzen[id].instance;
    }
  },
  createSlider: function (
    id,
    slider,
    parent,
    range,
    minHandle,
    maxHandle,
    min,
    max,
    value,
    step
  ) {
    Radzen[id] = {};
    Radzen[id].mouseMoveHandler = function (e) {
      if (!slider.canChange) return;
      e.preventDefault();
      var handle = slider.isMin ? minHandle : maxHandle;
      var offsetX =
        e.targetTouches && e.targetTouches[0]
          ? e.targetTouches[0].pageX - e.target.getBoundingClientRect().left
          : e.pageX - handle.getBoundingClientRect().left;
      var percent = (handle.offsetLeft + offsetX) / parent.offsetWidth;

      var newValue = percent * max;
      var oldValue = range ? value[slider.isMin ? 0 : 1] : value;

      if (
        slider.canChange &&
        newValue >= min &&
        newValue <= max &&
        newValue != oldValue
      ) {
        slider.invokeMethodAsync(
          'RadzenSlider.OnValueChange',
          newValue,
          !!slider.isMin
        );
      }
    };

    Radzen[id].mouseDownHandler = function (e) {
      if (parent.classList.contains('ui-state-disabled')) return;
      if (minHandle == e.target || maxHandle == e.target) {
        slider.canChange = true;
        slider.isMin = minHandle == e.target;
      } else {
        var offsetX =
          e.targetTouches && e.targetTouches[0]
            ? e.targetTouches[0].pageX - e.target.getBoundingClientRect().left
            : e.offsetX;
        var percent = offsetX / parent.offsetWidth;
        var newValue = percent * max;
        var oldValue = range ? value[slider.isMin ? 0 : 1] : value;
        if (newValue >= min && newValue <= max && newValue != oldValue) {
          slider.invokeMethodAsync(
            'RadzenSlider.OnValueChange',
            newValue,
            !!slider.isMin
          );
        }
      }
    };

    Radzen[id].mouseUpHandler = function (e) {
      slider.canChange = false;
    };

    document.addEventListener('mousemove', Radzen[id].mouseMoveHandler);
    document.addEventListener('touchmove', Radzen[id].mouseMoveHandler, {
      passive: true
    });

    document.addEventListener('mouseup', Radzen[id].mouseUpHandler);
    document.addEventListener('touchend', Radzen[id].mouseUpHandler, {
      passive: true
    });

    parent.addEventListener('mousedown', Radzen[id].mouseDownHandler);
    parent.addEventListener('touchstart', Radzen[id].mouseDownHandler, {
      passive: true
    });
  },
  destroySlider: function (id, parent) {
    if (!Radzen[id]) return;

    if (Radzen[id].mouseMoveHandler) {
      document.removeEventListener('mousemove', Radzen[id].mouseMoveHandler);
      document.removeEventListener('touchmove', Radzen[id].mouseMoveHandler);
      delete Radzen[id].mouseMoveHandler;
    }
    if (Radzen[id].mouseUpHandler) {
      document.removeEventListener('mouseup', Radzen[id].mouseUpHandler);
      document.removeEventListener('touchend', Radzen[id].mouseUpHandler);
      delete Radzen[id].mouseUpHandler;
    }
    if (Radzen[id].mouseDownHandler) {
      parent.removeEventListener('mousedown', Radzen[id].mouseDownHandler);
      parent.removeEventListener('touchstart', Radzen[id].mouseDownHandler);
      delete Radzen[id].mouseDownHandler;
    }

    Radzen[id] = null;
  },
  focusElement: function (elementId) {
    var el = document.getElementById(elementId);
    if (el) {
      el.focus();
    }
  },
  focusListItem: function (input, ul, isDown, startIndex) {
    if (!input || !ul) return;

    var childNodes = ul.getElementsByTagName('LI');

    if (!childNodes || childNodes.length == 0) return;

    if (startIndex == undefined || startIndex == null) {
      startIndex = -1;
    }

    ul.nextSelectedIndex = startIndex;

    ul.prevSelectedIndex = ul.nextSelectedIndex;

    if (isDown) {
      if (ul.nextSelectedIndex < childNodes.length - 1) {
        ul.nextSelectedIndex++;
      }
    } else {
      if (ul.nextSelectedIndex > 0) {
        ul.nextSelectedIndex--;
      }
    }

    var highlighted = ul.querySelectorAll('.ui-state-highlight');
    if (highlighted.length) {
      for (var i = 0; i < highlighted.length; i++) {
        highlighted[i].classList.remove('ui-state-highlight');
      }
    }

    if (
      ul.nextSelectedIndex >= 0 &&
      ul.nextSelectedIndex <= childNodes.length - 1
    ) {
      childNodes[ul.nextSelectedIndex].classList.add('ui-state-highlight');
      if (ul.parentNode.classList.contains('ui-autocomplete-panel')) {
        ul.parentNode.scrollTop = childNodes[ul.nextSelectedIndex].offsetTop;
      } else {
        ul.parentNode.scrollTop =
          childNodes[ul.nextSelectedIndex].offsetTop - ul.parentNode.offsetTop;
      }
    }

    return ul.nextSelectedIndex;
  },
  uploads: function (uploadComponent, id) {
    if (!Radzen.uploadComponents) {
      Radzen.uploadComponents = {};
    }
    Radzen.uploadComponents[id] = uploadComponent;
  },
  uploadChange: function (fileInput) {
    var files = [];
    for (var i = 0; i < fileInput.files.length; i++) {
      var file = fileInput.files[i];
      files.push({
        Name: file.name,
        Size: file.size,
        Url: URL.createObjectURL(file)
      });
    }

    var uploadComponent =
      Radzen.uploadComponents && Radzen.uploadComponents[fileInput.id];
    if (uploadComponent) {
      uploadComponent.invokeMethodAsync('RadzenUpload.OnChange', files);
    }

    for (var i = 0; i < fileInput.files.length; i++) {
      var file = fileInput.files[i];
      URL.revokeObjectURL(file.Url);
    }
  },
  skipFileFromUpload: function (name) {
    if (!Radzen.filesToSkipOnUpload) {
      Radzen.filesToSkipOnUpload = [];
    }

    Radzen.filesToSkipOnUpload.push(name);
  },
  upload: function (fileInput, url, multiple) {
    var data = new FormData();
    var files = [];
    for (var i = 0; i < fileInput.files.length; i++) {
      var file = fileInput.files[i];
      if (
        Radzen.filesToSkipOnUpload &&
        Radzen.filesToSkipOnUpload.indexOf(file.name) != -1
      )
        continue;
      data.append(multiple ? 'files' : 'file', file, file.name);
      files.push({Name: file.name, Size: file.size});
    }
    var xhr = new XMLHttpRequest();
    xhr.upload.onprogress = function (e) {
      if (e.lengthComputable) {
        var uploadComponent =
          Radzen.uploadComponents && Radzen.uploadComponents[fileInput.id];
        if (uploadComponent) {
          var progress = parseInt((e.loaded / e.total) * 100);
          uploadComponent.invokeMethodAsync(
            'RadzenUpload.OnProgress',
            progress,
            e.loaded,
            e.total,
            files
          );
        }
      }
    };
    xhr.onreadystatechange = function (e) {
      if (xhr.readyState === XMLHttpRequest.DONE) {
        var status = xhr.status;
        var uploadComponent =
          Radzen.uploadComponents && Radzen.uploadComponents[fileInput.id];
        if (uploadComponent) {
          if (status === 0 || (status >= 200 && status < 400)) {
            uploadComponent.invokeMethodAsync(
              'RadzenUpload.OnComplete',
              xhr.responseText
            );
          } else {
            uploadComponent.invokeMethodAsync(
              'RadzenUpload.OnError',
              xhr.responseText
            );
          }
        }
      }
    };
    xhr.open('POST', url, true);
    xhr.send(data);
  },
  getCookie: function (name) {
    var value = '; ' + decodeURIComponent(document.cookie);
    var parts = value.split('; ' + name + '=');
    if (parts.length == 2) return parts.pop().split(';').shift();
  },
  getCulture: function () {
    var cultureCookie = Radzen.getCookie('.AspNetCore.Culture');
    var uiCulture = cultureCookie
      ? cultureCookie.split('|').pop().split('=').pop()
      : null;
    return uiCulture || 'en-US';
  },
  numericOnPaste: function (e) {
    if (e.clipboardData) {
      var value = e.clipboardData.getData('text');

      if (value && /^-?\d*\.?\d*$/.test(value)) {
        return;
      }

      e.preventDefault();
    }
  },
  numericKeyPress: function (e) {
    if (
      e.metaKey ||
      e.ctrlKey ||
      e.keyCode == 9 ||
      e.keyCode == 8 ||
      e.keyCode == 13
    ) {
      return;
    }

    var ch = String.fromCharCode(e.charCode);

    if (/^[-\d,.]$/.test(ch)) {
      return;
    }

    e.preventDefault();
  },
  openContextMenu: function (x,y,id) {
    Radzen.closePopup(id);
    Radzen.removePopup(id);

    Radzen.openPopup(null, id, false, null, x, y);
  },
  openTooltip: function (target, id, duration, position) {
    Radzen.closePopup(id);
    Radzen.removePopup(id);

    if (Radzen[id + 'duration']) {
      clearTimeout(Radzen[id + 'duration']);
    }

    Radzen.openPopup(target, id, false, position);

    if (duration) {
      Radzen[id + 'duration'] = setTimeout(Radzen.closePopup, duration, id);
    }
  },
  findPopup: function (id) {
    var popups = [];
    for (var i = 0; i < document.body.children.length; i++) {
      if (document.body.children[i].id == id) {
        popups.push(document.body.children[i]);
      }
    }
    return popups;
  },
  removePopup: function (id) {
    var popups = Radzen.findPopup(id);

    for (var i = 0; i < popups.length; i++) {
      document.body.removeChild(popups[i]);
    }
  },
  openPopup: function (parent, id, syncWidth, position, x, y) {
    var popup = document.getElementById(id);
    if (!popup) return;

    var parentRect = parent ? parent.getBoundingClientRect() : { top: 0, bottom: 0, left: 0, right: 0, width: 0, height: 0 };

    if (/Edge/.test(navigator.userAgent)) {
      var scrollLeft = document.body.scrollLeft;
      var scrollTop = document.body.scrollTop;
    } else {
      var scrollLeft = document.documentElement.scrollLeft;
      var scrollTop = document.documentElement.scrollTop;
    }

    var top = y ? y : parentRect.bottom + scrollTop;
    var left = x ? x : parentRect.left + scrollLeft;

    if (syncWidth) {
        popup.style.width = parentRect.width + 'px';
        if (!popup.style.minWidth) {
            popup.style.minWidth = parentRect.width + 'px';
        }
    } 

    popup.style.display = 'block';

    var rect = popup.getBoundingClientRect();

    var smartPosition = !position || position == 'bottom';

    if (
      smartPosition &&
      top + rect.height > window.innerHeight &&
      parentRect.top > rect.height
    ) {
      top = parentRect.top - rect.height + scrollTop;

      if (position) {
        top = top - 40;
        var tooltipContent = popup.children[0];
        var tooltipContentClassName = 'rz-' + position + '-tooltip-content';
        if (tooltipContent.classList.contains(tooltipContentClassName)) {
          tooltipContent.classList.remove(tooltipContentClassName);
          tooltipContent.classList.add('rz-top-tooltip-content');
        }
      }
    }

    if (
      smartPosition &&
      left + rect.width - scrollLeft > window.innerWidth &&
      window.innerWidth > rect.width
    ) {
      left = window.innerWidth - rect.width + scrollLeft;

      if (position) {
        var tooltipContent = popup.children[0];
        var tooltipContentClassName = 'rz-' + position + '-tooltip-content';
        if (tooltipContent.classList.contains(tooltipContentClassName)) {
          tooltipContent.classList.remove(tooltipContentClassName);
          tooltipContent.classList.add('rz-left-tooltip-content');
          left = parentRect.left - rect.width - 5 + scrollLeft;
          top = parentRect.top - parentRect.height + scrollTop;
        }
      }
    }

    if (smartPosition) {
      if (position) {
        top = top + 20;
      }

      popup.style.top = top + 'px';
    }

    if (smartPosition) {
      popup.style.left = left + 'px';
    }

    if (position == 'left') {
      popup.style.left = parentRect.left - rect.width - 5 + 'px';
      popup.style.top = parentRect.top + scrollTop + 'px';
    }

    if (position == 'right') {
      popup.style.left = parentRect.right + 10 + 'px';
      popup.style.top = parentRect.top + scrollTop + 'px';
    }

    if (position == 'top') {
      popup.style.top = parentRect.top + scrollTop - rect.height + 5 + 'px';
      popup.style.left = parentRect.left + scrollLeft + 'px';
    }

    popup.style.zIndex = 1000;

    Radzen[id] = function (e) {
        if (parent) {
            if (!parent.contains(e.target) && !popup.contains(e.target)) {
                Radzen.closePopup(id);
            }
        } else {
            if (!popup.contains(e.target)) {
                Radzen.closePopup(id);
            }
        }
    };

    document.body.appendChild(popup);
    document.removeEventListener('click', Radzen[id]);
    document.addEventListener('click', Radzen[id]);

    if (!parent) {
        document.removeEventListener('contextmenu', Radzen[id]);
        document.addEventListener('contextmenu', Radzen[id]);
    }
  },
  closePopup: function (id) {
    var popup = document.getElementById(id);
    if (popup) {
      popup.style.display = 'none';
    }
    document.removeEventListener('click', Radzen[id]);
    Radzen[id] = null;
  },
  togglePopup: function (parent, id, syncWidth) {
    var popup = document.getElementById(id);
    if (!popup) return;
    if (popup.style.display == 'block') {
      Radzen.closePopup(id);
    } else {
      Radzen.openPopup(parent, id, syncWidth);
    }
  },
  destroyPopup: function (id) {
    var popup = document.getElementById(id);
    if (popup) {
      popup.parentNode.removeChild(popup);
    }
    document.removeEventListener('click', Radzen[id]);
  },
  scrollDataGrid: function (e) {
    var scrollLeft =
      (e.target.scrollLeft ? '-' + e.target.scrollLeft : 0) + 'px';

    e.target.previousElementSibling.style.marginLeft = scrollLeft;

    if (e.target.nextElementSibling) {
      e.target.nextElementSibling.style.marginLeft = scrollLeft;
    }
  },
  openDialog: function () {
    if (
      document.documentElement.scrollHeight >
      document.documentElement.clientHeight
    ) {
      document.body.classList.add('no-scroll');
    }
  },
  closeDialog: function () {
    document.body.classList.remove('no-scroll');
  },
  getInputValue: function (arg) {
    var input =
      arg instanceof Element || arg instanceof HTMLDocument
        ? arg
        : document.getElementById(arg);
    return input ? input.value : '';
  },
  setInputValue: function (arg, value) {
    var input =
      arg instanceof Element || arg instanceof HTMLDocument
        ? arg
        : document.getElementById(arg);
    if (input) {
      input.value = value;
    }
  },
  readFileAsBase64: function (fileInput, maxFileSize) {
    var readAsDataURL = function (fileInput) {
      return new Promise(function (resolve, reject) {
        var reader = new FileReader();
        reader.onerror = function () {
          reader.abort();
          reject('Error reading file.');
        };
        reader.addEventListener(
          'load',
          function () {
            resolve(reader.result);
          },
          false
        );
        var file = fileInput.files[0];
        if (file.size <= maxFileSize) {
          reader.readAsDataURL(file);
        } else {
          reject('File too large.');
        }
      });
    };

    return readAsDataURL(fileInput);
  },
  closeMenuItems: function (event) {
    var menu = event.target.closest('.menu');

    if (!menu) {
      var targets = document.querySelectorAll(
        '.navigation-item-wrapper-active'
      );

      if (targets) {
        for (var i = 0; i < targets.length; i++) {
          Radzen.toggleMenuItem(targets[i], false);
        }
      }
      document.removeEventListener('click', Radzen.closeMenuItems);
    }
  },
  closeOtherMenuItems: function (current) {
    var targets = document.querySelectorAll('.navigation-item-wrapper-active');
    if (targets) {
      for (var i = 0; i < targets.length; i++) {
        var target = targets[i];
        var item = target.closest('.navigation-item');

        if (!current || !item.contains(current)) {
          Radzen.toggleMenuItem(target, false);
        }
      }
    }
  },
  toggleMenuItem: function (target, selected) {
    Radzen.closeOtherMenuItems(target);

    var item = target.closest('.navigation-item');

    if (selected === undefined) {
      selected = !item.classList.contains('navigation-item-active');
    }

    item.classList.toggle('navigation-item-active', selected);

    target.classList.toggle('navigation-item-wrapper-active', selected);

    var children = item.querySelector('.navigation-menu');

    if (children) {
      children.style.display = selected ? '' : 'none';
    } else if (selected) {
      Radzen.closeOtherMenuItems(null);
    }

    var icon = item.querySelector('.navigation-item-icon-children');

    if (icon) {
      var deg = selected ? '180deg' : 0;
      icon.style.transform = 'rotate(' + deg + ')';
    }

    document.removeEventListener('click', Radzen.closeMenuItems);
    document.addEventListener('click', Radzen.closeMenuItems);
  },
  destroyChart: function (ref) {
    if (ref.mouseMoveHandler) {
      ref.removeEventListener(ref.mouseMoveHandler);
      delete ref.mouseMoveHandler;
    }

    this.destroyResizable(ref);
  },
  destroyGauge: function (ref) {
    this.destroyResizable(ref);
  },
  destroyResizable: function (ref) {
    if (ref.resizeObserver) {
      ref.resizeObserver.disconnect();
      delete ref.resizeObserver;
    }
    if (ref.resizeHandler) {
      window.removeEventListener('resize', ref.resizeHandler);
      delete ref.resizeHandler;
    }
  },
  createResizable: function (ref, instance) {
    ref.resizeHandler = function () {
      var rect = ref.getBoundingClientRect();

      instance.invokeMethodAsync('Resize', rect.width, rect.height);
    };

    if (window.ResizeObserver) {
      ref.resizeObserver = new ResizeObserver(ref.resizeHandler);
      ref.resizeObserver.observe(ref);
    } else {
      window.addEventListener('resize', ref.resizeHandler);
    }

    var rect = ref.getBoundingClientRect();

    return {width: rect.width, height: rect.height};
  },
  createChart: function (ref, instance) {
    ref.mouseMoveHandler = function (e) {
      var rect = ref.getBoundingClientRect();
      var x = e.clientX - rect.left;
      var y = e.clientY - rect.top;
      instance.invokeMethodAsync('MouseMove', x, y);
    };

    ref.addEventListener('mousemove', ref.mouseMoveHandler);

    return this.createResizable(ref, instance);
  },
  createGauge: function (ref, instance) {
    return this.createResizable(ref, instance);
  },
  destroyScheduler: function (ref) {
    if (ref.resizeHandler) {
      window.removeEventListener('resize', ref.resizeHandler);
      delete ref.resizeHandler;
    }
  },
  createScheduler: function (ref, instance) {
    ref.resizeHandler = function () {
      var rect = ref.getBoundingClientRect();

      instance.invokeMethodAsync('Resize', rect.width, rect.height);
    };

    window.addEventListener('resize', ref.resizeHandler);

    var rect = ref.getBoundingClientRect();
    return {width: rect.width, height: rect.height};
  }
};
