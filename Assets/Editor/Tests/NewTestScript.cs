using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;

public class NewTestScript
{
    // A test to confirm enemy run animation exists.
    [Test]
    public void NewTestScriptSimplePasses()
    {
        string assetsPath = Application.dataPath;
        string filePath = assetsPath + "/Character/Animations/EnemyRun.anim";
        bool fileExists = File.Exists(filePath);
        Debug.Log($"Test was run.");

        Assert.IsTrue(fileExists);
    }
}
