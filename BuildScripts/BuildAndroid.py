# ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
# python script to build android apk using unity command line.
# ------ Calls editor method "PerformBuildAndroidDevelopment" and 
# outputs build and log files to specified paths. 
# ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

import subprocess
import sys

# Check if enough arguments are passed
if len(sys.argv) < 4:
    print("Usage: python BuildAndroid.py <projectPath> <customBuildPath> <logFilePath>")
    sys.exit(1)

UnityPath = r"C:/Program Files/Unity/Hub/Editor/2020.3.38f1/Editor/Unity.exe"
Arguments = [
    '-projectPath', sys.argv[1],  # First argument: Project path
    '-executeMethod', 'BuildScript.PerformBuildAndroidDevelopment',
    '-customBuildPath', sys.argv[2],  # Second argument: Custom build path
    '-batchmode',
    '-logFile', sys.argv[3],  # Third argument: Log file path
    '-quit'
]

subprocess.run([UnityPath] + Arguments)