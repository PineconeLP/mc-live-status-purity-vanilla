window.onloadCallback = function(elementId, siteKey, callbackWrapper, expireWrapper) {
    grecaptcha.render(elementId, {
      'sitekey' : siteKey,
      'callback': () => callbackWrapper.invokeMethod("Execute"),
      'expired-callback': () => expireWrapper.invokeMethod("Execute")
    });
  };