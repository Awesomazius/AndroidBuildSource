using System;
using System.Linq;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.TestTools.TestRunner.Api;

/// <summary>
/// Build script for microgame. Creates production and development builds, runs one test as pre-build step.
/// </summary>
public class BuildScript
{
    // Vanilla default build.
    static void PerformBuildAndroidDefault()
    {
        string customBuildPath = EnvironmentBuildPath();
        string[] defaultScene = {
             "Assets/Scenes/SampleScene.unity",
             };


        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = defaultScene;
        options.target = BuildTarget.Android;
        options.locationPathName = customBuildPath;
        options.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(options);
    }

    // Development build, should allow debugging and profiling. 
    static void PerformBuildAndroidDevelopment()
    {

        string customBuildPath = EnvironmentBuildPath();
        string[] defaultScene = {
            "Assets/Scenes/SampleScene.unity",
            };

        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = defaultScene;
        options.target = BuildTarget.Android;
        options.locationPathName = customBuildPath;
        options.options = BuildOptions.Development | BuildOptions.AllowDebugging;

        BuildPipeline.BuildPlayer(options);
    }

    // collects build path from input.
    private static string EnvironmentBuildPath() 
    {
        string customBuildPath = Environment.GetCommandLineArgs()
            .SkipWhile(arg => arg != "-customBuildPath")
            .Skip(1)
            .FirstOrDefault();

        Debug.Log("customBuildPath: " + customBuildPath);
        char[] letters = customBuildPath.ToCharArray();

        if (string.IsNullOrEmpty(customBuildPath))
        {
            throw new Exception("customBuildPath not provided!");
        }

        if (letters[1] != ':')
        {
            throw new Exception("customBuildPath is wrong!");
        }

        return customBuildPath;
    }

}

// Runs a single test with testrunnerapi.
public class BuildPreprocessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("Running tests before build...");

        var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
        var filter = new Filter()
        {
            testMode = TestMode.EditMode
        };

        bool testsPassed = false;

        testRunnerApi.Execute(new ExecutionSettings(filter)
        {
            runSynchronously = true
        });

        testRunnerApi.RegisterCallbacks(
            new Callback{ runFinished = results =>
            {
                testsPassed = results.TestStatus == TestStatus.Passed;
                if (!testsPassed)
                {
                    Debug.LogError("Tests failed. Aborting build.");
                    throw new BuildFailedException("Pre-build tests failed.");
                }
                else
                {
                    Debug.Log("All tests passed. Continuing build...");
                }
            }
        });

        ScriptableObject.DestroyImmediate(testRunnerApi);
    }

    private class Callback : ICallbacks
    {
        public System.Action<ITestResultAdaptor> runFinished;

        public void RunFinished(ITestResultAdaptor result)
        {
            runFinished?.Invoke(result);
        }

        public void RunStarted(ITestAdaptor testsToRun) { }
        public void TestFinished(ITestResultAdaptor result) { }
        public void TestStarted(ITestAdaptor test) { }
    }
}


