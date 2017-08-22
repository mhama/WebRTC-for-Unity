using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class AndroidBuildPreprocessor : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        if(target != BuildTarget.Android)
        {
            return;
        }

        WebrtcBuild.BuildAarAndCopy();
    }
}
