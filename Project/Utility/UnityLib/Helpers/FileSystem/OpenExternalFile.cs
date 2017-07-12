// opens external file in default viewer for that filetype
// for example: powerpoint file would open in powerpoint

using UnityEngine;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

/// <summary>
/// Class OpenExternalFile.
/// </summary>
public class OpenExternalFile : MonoBehaviour
{
    // opens external file in default viewer
    /// <summary>
    /// Opens the file.
    /// </summary>
    /// <param name="fullPath">The full path.</param>
    public static void OpenFile(string fullPath)
    {
        Debug.Log("opening:" + fullPath);

        if (File.Exists(fullPath))
        {
            try
            {
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = fullPath;
                myProcess.Start();
                //				myProcess.WaitForExit();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}