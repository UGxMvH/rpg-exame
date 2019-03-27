using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using ProFunctions;
using System.Collections.Generic;

namespace ProFunctions
{
    class ProjectSizeWindow : EditorWindow
    {
        private long projectSize;
        private long assetsSize;
        private long librarySize;
        private long tempCacheSize;

        private Dictionary<string, long> sizes;
        private Dictionary<string, string> fileExtWarnings;

        private bool show;

        private Vector2 scrollPos;

        [MenuItem("Window/Pro Functions/Project Size")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(ProjectSizeWindow));
            window.titleContent = new GUIContent("Project Size");
            window.Show();
        }

        private void Awake()
        {
            Scan();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Refresh")) Scan();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.LabelField("Unity Project size", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Project folder: " + Utilities.BytesToString(projectSize), EditorStyles.label);
            EditorGUILayout.LabelField("Library folder: " + Utilities.BytesToString(librarySize), EditorStyles.label);
            EditorGUILayout.LabelField("Temp cache folder: " + Utilities.BytesToString(tempCacheSize), EditorStyles.label);

            GUILayout.BeginVertical(EditorStyles.helpBox);
            {
                show = EditorGUILayout.Foldout(show, "Assets: " + Utilities.BytesToString(assetsSize));

                if (show)
                {
                    if (sizes != null && fileExtWarnings != null)
                    {
                        // Foreach type of files
                        foreach (KeyValuePair<string, long> size in sizes)
                        {
                            // Check if they have a warning
                            if (fileExtWarnings.ContainsKey(size.Key))
                            {
                                // There is a message for this file extension
                                EditorGUILayout.LabelField(new GUIContent(size.Key + ": " + Utilities.BytesToString(size.Value), fileExtWarnings[size.Key]), EditorStyles.label);
                                EditorGUILayout.HelpBox(fileExtWarnings[size.Key], MessageType.Warning);
                            }
                            else
                            {
                                EditorGUILayout.LabelField(size.Key + ": " + Utilities.BytesToString(size.Value), EditorStyles.label);
                            }
                        }
                    }
                }
            }
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void Scan()
        {
            // Show progress bar
            EditorUtility.DisplayProgressBar("Busy", "Scanning project size", 0);

            SetUpWarnings();

            // Clear previous sizes
            sizes = new Dictionary<string, long>();

            // Get whole project size
            projectSize = Utilities.GetDirectorySize(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")));

            // Get assets folder size
            assetsSize = Utilities.GetDirectorySize(Application.dataPath);

            // Get library folder size
            if (Directory.Exists(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")) + "/Library"))
            {
                librarySize = Utilities.GetDirectorySize(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")) + "/Library");
            }
            else
            {
                librarySize = 0;
            }

            // Get temp cache folder size
            tempCacheSize = Utilities.GetDirectorySize(Application.temporaryCachePath);

            // Collect all files
            foreach (string file in Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories))
            {
                FileInfo fi = new FileInfo(file);

                if (fi.Extension.Length == 0)
                {
                    continue;
                }
                    
                string ext = fi.Extension.Substring(1);

                if (!sizes.ContainsKey(ext))
                {
                    sizes.Add(ext, 0L);
                }

                sizes[ext] += fi.Length;
            }

            // Clear progress bar
            EditorUtility.ClearProgressBar();
        }

        private void SetUpWarnings()
        {
            fileExtWarnings = new Dictionary<string, string>();

            fileExtWarnings.Add("tif", "We recommend avoiding tif files. Try exporting as jpg or png(if transparency is needed)");
            fileExtWarnings.Add("psd", "We recommend avoiding psd files. Try exporting as jpg or png(if transparency is needed)");
            fileExtWarnings.Add("tga", "We recommend avoiding tga files. Try exporting as jpg or png(if transparency is needed)");
        }
    }
}