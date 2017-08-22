using UnityEngine;
using UnityEditor;
using System;

public class WebrtcBuild : EditorWindow {

	[MenuItem ("Window/WebRTC Build")]
	static void Init ()
	{ 
		// Get existing open window or if none, make a new one:
		WebrtcBuild window = (WebrtcBuild)EditorWindow.GetWindow (typeof(WebrtcBuild));
		window.titleContent.text = "WebRTC Build";
		window.Show();
	}

	void OnGUI()
	{
		EditorGUILayout.PrefixLabel ("Android");

		if (GUILayout.Button("Rebuild WebRTC library", GUILayout.MinWidth(110)))
		{
            BuildAarAndCopy();
        }

		EditorGUILayout.PrefixLabel ("SocketIO");

		EditorGUILayout.BeginVertical ();
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button("Run example server", GUILayout.MinWidth(110)))
		{
			RunSocketIOServer ();
		}
		if (GUILayout.Button("npm install", GUILayout.Width(100)))
		{
			InstallServerDependencies ();
		}
		EditorGUILayout.EndHorizontal ();
		if (GUILayout.Button("Run client in browser"))
		{
			RunInBrowser ();
		}
		EditorGUILayout.EndVertical ();

	}


    public static void BuildAarAndCopy()
    {
        if (GradleBuild())
        {
            string aarFile = System.IO.Path.GetFullPath("./webrtc-android/webrtc/build/outputs/aar/webrtc-release.aar");
            string oldAarFile = System.IO.Path.GetFullPath("./Assets/WebRTCForUnity/WebRTC/Plugins/Android/webrtc-release.aar");
            if (!System.IO.File.Exists(aarFile))
            {
                Debug.LogError("Aar file wasn't built");
            }
            System.IO.File.Copy(aarFile, oldAarFile, true);
        }
    }

    public static bool GradleBuild() {
		string androidLocation = System.IO.Path.GetFullPath("./webrtc-android/");
		if (System.IO.Directory.Exists(androidLocation))
		{
			System.Diagnostics.Process gradle = new System.Diagnostics.Process();
			gradle.StartInfo.FileName = "gradlew";
			gradle.StartInfo.Arguments = "webrtc:assembleRelease";
			gradle.StartInfo.WorkingDirectory = androidLocation;
			//TODO: hook stdout to unity console
			if (gradle.Start())
			{
				if (gradle.WaitForExit(60 * 1000))
				{
					if(gradle.ExitCode != 0)
					{
						Debug.LogError("gradle exit: " + gradle.ExitCode);
					}
					return gradle.ExitCode == 0;
				}
				try
				{
					gradle.Kill();
                    Debug.LogError("gradle timeout:" + gradle.ExitCode);
					return gradle.ExitCode == 0;
				}
				catch (Exception ex){
                    Debug.LogError("gradle error:" + ex.Message);
					return false;
				}
			}
		}
		Debug.Log("gradle error: location " + androidLocation + " not found");
		return false;
	}

	public static bool RunSocketIOServer() {
		System.Diagnostics.Process node = new System.Diagnostics.Process();
		node.StartInfo.FileName = "node";
		node.StartInfo.Arguments = "index.js";
		node.StartInfo.WorkingDirectory = System.IO.Path.GetFullPath("./Assets/WebRTCForUnity/Example/SocketIO/Server~");
		return node.Start ();

	}

	public static bool InstallServerDependencies() {
		System.Diagnostics.Process node = new System.Diagnostics.Process();
		node.StartInfo.FileName = "npm";
		node.StartInfo.Arguments = "install";
		node.StartInfo.WorkingDirectory = System.IO.Path.GetFullPath("./Assets/WebRTCForUnity/Example/SocketIO/Server~");
		return node.Start ();
	}

	public static bool RunInBrowser() {
		
		System.Diagnostics.Process.Start ("http://localhost:3000/"); 
		return true;
	}

}
