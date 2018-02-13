var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http, {
  transports: ['polling']
});

app.get('/', function(req, res){
  res.sendFile(__dirname + '/index.html');
});

// simple and dirty WebRTC signaling
var userId = 0;
io.on('connection', function(socket){
  socket.userId = userId ++;
  console.log('a user connected, user id: ' + socket.userId);
  socket.emit("welcome", { id: socket.userId });
  io.emit("join", { id: socket.userId } );

  socket.on('chat', function(msg){
    console.log('message from user#' + socket.userId + ": " + msg);
    io.emit('chat', {
      id: socket.userId,
      msg: msg
    });
  });
  socket.on('webrtc-offer', function(msg){
    console.log('webrtc-offer from user#' + socket.userId + ": " + msg);
    io.emit('webrtc-offer', {
      id: socket.userId,
      msg: msg
    });
  });
  socket.on('webrtc-answer', function(msg){
    console.log('webrtc-answer from user#' + socket.userId + ": " + msg);
    io.emit('webrtc-answer', {
      id: socket.userId,
      msg: msg
    });
  });
  socket.on('disconnect', function() {
    io.emit("exit", { id: socket.userId });
  })
});

http.listen(3000, function(){
  console.log('listening on *:3000');
});