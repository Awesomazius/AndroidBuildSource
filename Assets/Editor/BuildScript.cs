using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    static void PerformBuild()
    {
        string[] defaultScene = {
            "Assets/Scenes/SampleScene.unity",
            };

        BuildPipeline.BuildPlayer(defaultScene, "build.apk",
            BuildTarget.Android, BuildOptions.None);
    }

}