#region [Copyright (c) 2015 Cristian Alexandru Geambasu]

//	Distributed under the terms of an MIT-style license:
//
//	The MIT License
//
//	Copyright (c) 2015 Cristian Alexandru Geambasu
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy of this software
//	and associated documentation files (the "Software"), to deal in the Software without restriction,
//	including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//	and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//	subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all copies or substantial
//	portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//	PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
//	FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//	ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion [Copyright (c) 2015 Cristian Alexandru Geambasu]

using UnityEngine;
using UnityEditor;
using UnityInputConverter;

namespace TeamUtilityEditor.IO.InputManager
{
    /// <summary>
    /// Class MenuCommands.
    /// </summary>
    public static class MenuCommands
    {
        /// <summary>
        /// Creates the input manager.
        /// </summary>
        [MenuItem("Lerp2Dev Team Tools/Input Manager/Create Input Manager", false, 2)]
        public static void CreateInputManager()
        {
            GameObject gameObject = new GameObject("Input Manager");
            gameObject.AddComponent<TeamUtility.IO.InputManager>();

            // Register Input Manager for undo, mark scene as dirty.
            Undo.RegisterCreatedObjectUndo(gameObject, "Create Input Manager");

            Selection.activeGameObject = gameObject;
        }

        [MenuItem("Lerp2Dev Team Tools/Input Manager/Convert Unity Input", false, 5)]
        private static void ConvertInput()
        {
            string sourcePath = EditorUtility.OpenFilePanel("Select Unity input settings asset", "", "asset");
            if (!string.IsNullOrEmpty(sourcePath))
            {
                string destinationPath = EditorUtility.SaveFilePanel("Save imported input axes", "", "input_manager", "xml");
                if (!string.IsNullOrEmpty(destinationPath))
                {
                    try
                    {
                        InputConverter converter = new InputConverter();
                        converter.ConvertUnityInputManager(sourcePath, destinationPath);

                        EditorUtility.DisplayDialog("Success", "Unity input converted successfuly!", "OK");
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                        EditorUtility.DisplayDialog("Error", "Failed to convert Unity input!", "OK");
                    }
                }
            }
        }

        /// <summary>
        /// Checks for updates.
        /// </summary>
        [MenuItem("Lerp2Dev Team Tools/Input Manager/Check For Updates", false, 400)]
        public static void CheckForUpdates()
        {
            Application.OpenURL("https://github.com/daemon3000/InputManager");
        }

        /// <summary>
        /// Opens the documentation page.
        /// </summary>
        [MenuItem("Lerp2Dev Team Tools/Input Manager/Documentation", false, 401)]
        public static void OpenDocumentationPage()
        {
            Application.OpenURL("https://github.com/daemon3000/InputManager/wiki");
        }

        /// <summary>
        /// Opens the report bug page.
        /// </summary>
        [MenuItem("Lerp2Dev Team Tools/Input Manager/Report Bug", false, 402)]
        public static void OpenReportBugPage()
        {
            Application.OpenURL("https://github.com/daemon3000/InputManager/issues");
        }

        /// <summary>
        /// Opens the contact dialog.
        /// </summary>
        [MenuItem("Lerp2Dev Team Tools/Input Manager/Contact", false, 403)]
        public static void OpenContactDialog()
        {
            string message = "Email: daemon3000@hotmail.com";
            EditorUtility.DisplayDialog("Contact", message, "Close");
        }

        /// <summary>
        /// Opens the forum page.
        /// </summary>
        [MenuItem("Lerp2Dev Team Tools/Input Manager/Forum", false, 404)]
        public static void OpenForumPage()
        {
            Application.OpenURL("http://forum.unity3d.com/threads/223321-Free-Custom-Input-Manager");
        }

        /// <summary>
        /// Opens the about dialog.
        /// </summary>
        [MenuItem("Lerp2Dev Team Tools/Input Manager/About", false, 405)]
        public static void OpenAboutDialog()
        {
            string message = "Input Manager, MIT licensed\nCopyright \u00A9 2015 Cristian Alexandru Geambasu\nhttps://github.com/daemon3000/InputManager";
            EditorUtility.DisplayDialog("About", message, "OK");
        }
    }
}