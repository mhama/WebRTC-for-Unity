##### disclaimer: this repo is not under active development. It was an experiment, which wasn't going in the right direction, but nonetheless, it was good learning material to me. I will restart this project in the - near or far - future when I get the time. 

# WebRTC-for-Unity (WIP, In development)
WebRTC for Unity (WIP, In development)
This repo aims to facilitate WebRTC development in Unity, and maybe have a unifed API that behaves the same on different platforms.

## Current state:
Only for android.

The context needs to be an **GLES2** for now. Which means, in player settings, Auto Graphics API needs to be unchecked, and only the OpenGLES2 option is selected. This is due to the implementation of the EGL context on webrtc. I have submitted an [issue](https://bugs.chromium.org/p/webrtc/issues/detail?id=8094) to the webrtc repo, and proposed an fix.

The image is grabed through an external texture, Blit to a RenderTexture with a shader for external textures (similar to the VideoDecodeAndroid shader), and applied to the material.

Demo scene shows capture from camera, and screen share, from webrtc video capturer to unity, rendering to multiple Game Objects.

<img src="https://dl2.pushbulletusercontent.com/Xj8v2Wliajvr8PYRvIdKS2Yu7PlqT2PP/Screenshot_20170816-145027.png" width="500" />

## Build:
- Please ensure that Unity's normal Android build does not fail on your environment.
- Check one of the example scenes on Unity's Build Settings dialog.
    - ./Assets/WebRTCForUnity/Example/0 - Screen capture.unity
    - ./Assets/WebRTCForUnity/Example/SocketIO/example.unity
- Switch to Android Platform, and push "Build" or "Build And Run"

### Gradle setting
- If the build fails, please ensure that running webrtc-android/gradlew (Mac OS X) or webrtc-android/gradlew.bat (Windows) on command prompt or Terminal does not fail.
- If it fails, maybe you need to specify android sdk directory.
To specify android sdk directory to gradle, please create ./webrtc-android/local.properties file with below contents.

```
# Please set your Android SDK directory.
# But do not use \ as delimiter, use / instead.
sdk.dir=/Path/To/AndroidSDK

# typical android sdk path on Windows
#sdk.dir=C:/Users/YourUserName/AppData/Local/Android/Sdk

# NDK directory. Setting blank is ok.
ndk.dir=
```

## Demos

- Screen capture demo
  - Demo to show live texture with WebRTC's capturing functionality. No networking involved.
  - Tap 'Camera' button to show live view captured from the device's camera on 3D objects' textures.
  - Tap 'Screen Capture' button to show live screen capture on 3D Objects' textures.

- Socket IO demo (actually WebRTC feature is not implemented yet)
    - Currently this is just a simple chat demo, exchange messages between server and client (Unity app).
    - Server is created with nodejs. Server code is on `./Assets/WebRTCForUnity/Example/SocketIO/Server~`
    - Requirements
        - To run the server, you need to install nodejs.
        - There's several ways to install nodejs, but with Chocolatey on windows, `choco install nodejs` will do it.
        - You need to run `npm install` to install required modules.
            - With command line: Then you need to run `npm install` on the server directory `./Assets/WebRTCForUnity/Example/SocketIO/Server~` (this directory is hidden on Unity's file browser)
            - With Unity GUI: 'Window' Menu-> 'WebRTC Build' -> 'npm install'
            - Make sure that your target Android phone is on the same Network, like connecting to the same Wi-Fi access point.
    - Run Server
        - With command line: run `node index.js` on the server directory `./Assets/WebRTCForUnity/Example/SocketIO/Server~`
        - With Unity GUI: 'Window' Menu-> 'WebRTC Build' -> 'Run example server'
    - Configure client
        - Open scene `./Assets/WebRTCForUnity/Example/SocketIO/example.unity`
        - Select `SocketIO` GameObject.
        - On the property view, set `Server Url` as http://(YOUR PC's LOCAL IP ADDRESS):3000
            - example: http://192.168.0.10:3000
            - You need to confirm your PC's local IP address with network settings on your OS.
    - Run App
        - Build and run app.

## Roadmap:
- [x] Create an android plugin 
- [x] Create an proxy to move I420 frames between the WebRTC and Unity (Textures)
    -   Creating a video capturer (Camera/Screenshare) and have it send a texture to Unity for rendering
    -   This would allow later to either stream the the video, or save it to disk (since WebRTC have everything needed to support encoding/decoding).
    -   Maybe have more options and flexibilites over this system (create a video stream from a render texture, <s>hardware acceleration option; aka whether to use the GLES texture, or the YUV data of the frame</s> )
- [ ] Get a simple video call POC in Unity
- [ ] Clean up code
- [ ] Create an abstracted WebRTC API for Unity, that can be implemented for each platform
- [ ] Get rid of the hacky `setUnityContext` to get the graphics context, and actually use the Low-level Native Plugin Interface
- [ ] Support for iOS within the same API
- [ ] Support for WebGL (jslib plugin + polyfill maybe)
- [ ] Support for Standalone (Windows, OS X, Linux)
- [ ] Support for Editor (Should be easy, if standalone works)
- [ ] Create an interface API for signaling, that can be implemented for different ways of signaling (Websocket/Socket, Http, etc)

## Resources:
The official webrtc repo contains a unity plugin example, which is worth investigating.

https://chromium.googlesource.com/external/webrtc.git/+/master/examples/unityplugin/

Used without hardware acceleration, it could be used in conjunction with techniques from

https://bitbucket.org/Unity-Technologies/graphicsdemos

To render data directly to the texture created in Unity.

## Contribution:
Yes, please. If you think you can contribute to any of the points above, or have any suggestions, design thoughts, use cases, anything really, go ahead and open an issue and/or create a pull request.
