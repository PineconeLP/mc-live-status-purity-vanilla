const net = require('net');

const serverAddress = 'purityvanilla.com';
const port = 25565;

console.log('Connecting...');

const socket = net.connect(port, serverAddress);

socket.on('data', (buffer) => {
  const bufferJSON = buffer.toJSON();
  const data = bufferJSON.data;

  const response = [];
  let currentData = '';

  for (let i = 0; i < data.length; i++) {
    const byte = data[i];
    const nextByte = data[i + 1];

    if (byte === 0 && nextByte === 0) {
      if (currentData.length > 0) {
        response.push(currentData);
        currentData = '';
      }
    } else {
      const parsedByte = String.fromCharCode(byte);
      currentData += parsedByte;
    }
  }

  const onlinePlayers = response[4];

  console.log(`OnlinePlayers: ${onlinePlayers}`);
});

socket.on('connect', async () => {
  console.log('Connected to server.');
  socket.write(Buffer.from(['0xFE', '0x01']));
});
