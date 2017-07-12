using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// original source by "Mavina" http://answers.unity3d.com/answers/1204307/view.html
// usage: Place this script into Editor/ folder, then you can press F5 to enter/exit Play Mode

/// <summary>
/// Class EditorShortCutKeys.
/// </summary>
public class EditorShortCutKeys : ScriptableObject
{
    [MenuItem("Edit/Run _F5")] // shortcut key F5 to Play (and exit playmode also)
    private static void PlayGame()
    {
        if (!Application.isPlaying)
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false); // optional: save before run
        }
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}