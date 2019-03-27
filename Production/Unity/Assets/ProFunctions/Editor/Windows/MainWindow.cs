using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace ProFunctions
{
    class MainWindow : EditorWindow
    {
        private EditorWindow prevEditorWindow;

        private GameObject activeGO;
        public GameObject prevObject;

        private Material activeMat;

        private bool dontClearPrevObject;

        [MenuItem("Window/Pro Functions/Main")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(MainWindow));
            window.titleContent = new GUIContent("Pro Functions");
            window.Show();
        }

        private void Update()
        {
            if (!focusedWindow) return;

            if (focusedWindow.GetType() == typeof(MainWindow)) return;

            if (focusedWindow != prevEditorWindow)
            {
                this.Repaint();
                prevEditorWindow = focusedWindow;
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

            if (GUILayout.Button("Project Size"))
            {
                // Open project size window
                ProjectSizeWindow.ShowWindow();
            }

            // If GameObject selected
            if (activeGO = Selection.activeGameObject)
            {
                EditorGUILayout.LabelField("Game Object", EditorStyles.boldLabel);

                // Duplicate special and make settings button
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Duplicate special"))
                {
                    // Select materials applied to GameObject
                    DuplicateSpecial();
                }

                GUILayout.Button(EditorGUIUtility.IconContent("SettingsIcon"), PFEditorLayouts.IconButton(), GUILayout.Height(18), GUILayout.Width(18));
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Select Materials"))
                {
                    // Select materials applied to GameObject
                    SelectMaterials();
                }

                if (GUILayout.Button("Toggle Boundingbox"))
                {
                    // Select materials applied to GameObject
                    ToggleBoudingbox();
                }

                if (GUILayout.Button("Delete"))
                {
                    DestroyImmediate(activeGO);
                }
            }

            // If Material selected
            if (activeMat = Selection.activeObject as Material)
            {
                EditorGUILayout.LabelField("Material", EditorStyles.boldLabel);
                if (GUILayout.Button("Select GameObjects"))
                {
                    // Select materials applied to GameObject
                    SelectGameObjectWithMaterial();
                }
            }

            // If nothing selected
            if (Selection.objects.Length == 0 && prevEditorWindow)
            {
                if (prevEditorWindow.GetType() == typeof(UnityEditor.SceneView) || prevEditorWindow.GetType().ToString() == "UnityEditor.SceneHierarchyWindow")
                {
                    EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel);
                    if (GUILayout.Button("Create Empty"))
                    {
                        GameObject newGo = new GameObject();
                        CorrectPosition(newGo);
                        Selection.activeGameObject = newGo;
                    }

                    foreach (PrimitiveType pt in (PrimitiveType[])Enum.GetValues(typeof(PrimitiveType)))
                    {
                        if (GUILayout.Button("Create " + pt.ToString()))
                        {
                            GameObject newGo = GameObject.CreatePrimitive(pt);
                            CorrectPosition(newGo);
                            Selection.activeGameObject = newGo;
                        }
                    }
                }
                else if (prevEditorWindow.GetType().ToString() == "UnityEditor.ProjectBrowser")
                {
                    EditorGUILayout.LabelField("Project", EditorStyles.boldLabel);

                    if (GUILayout.Button("Create Material"))
                    {
                        CreateMaterial();
                    }
                }
            }
        }

        private void OnSelectionChange()
        {
            if (dontClearPrevObject) 
            {
                dontClearPrevObject = false;
            }
            else
            {
                prevObject = null;
            }

            this.Repaint();
        }

        #region GameObject methods
        private void DuplicateSpecial()
        {
            // Remember old object
            GameObject go = activeGO;

            // Copy object
            GameObject newGo = Instantiate(go, go.transform.position, go.transform.rotation, go.transform.parent);

            // GameObject names
            List<string> names = new List<string>();

            foreach(MonoBehaviour tempGo in FindObjectsOfType<MonoBehaviour>()) {
                names.Add(tempGo.gameObject.name);
            }

            // Correct name
            ObjectNames.SetNameSmart(newGo, ObjectNames.GetUniqueName(names.ToArray(), activeGO.name));

            // If there is no previous object make first copy
            if (prevObject)
            {
                // offset
                Vector3 offsetPos = activeGO.transform.position - prevObject.transform.position;
                Vector3 offsetRot = activeGO.transform.rotation.eulerAngles - prevObject.transform.rotation.eulerAngles;

                // Copy object
                newGo.transform.position += offsetPos;
                newGo.transform.rotation = Quaternion.Euler(newGo.transform.rotation.eulerAngles + offsetRot);
            }

            // Select new object
            dontClearPrevObject = true;
            Selection.activeGameObject = newGo;

            // Set prev object
            prevObject = go;
        }

        private void SelectMaterials()
        {
            MeshRenderer mr = activeGO.GetComponent<MeshRenderer>();

            if (mr != null)
            {
                Selection.objects = mr.sharedMaterials;
            }
        }

        private void ToggleBoudingbox()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                if (go.GetComponent<PF_ShowMeshBounds>())
                {
                    DestroyImmediate(go.GetComponent<PF_ShowMeshBounds>());
                }
                else
                {
                    if (go.GetComponent<MeshFilter>())
                        go.AddComponent<PF_ShowMeshBounds>();
                }
            }
        }
        #endregion
        #region Material
        private void SelectGameObjectWithMaterial()
        {
            List<GameObject> gosToSelect = new List<GameObject>();

            // Loop through all objects
            foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
            {
                MeshRenderer mr;

                if (mr = go.GetComponent<MeshRenderer>())
                {
                    foreach (Material mat in mr.sharedMaterials)
                    {
                        if (mat == activeMat)
                        {
                            gosToSelect.Add(go);
                        }
                    }
                }
            }

            Selection.objects = gosToSelect.ToArray();
        }
    #endregion
    #region Scene
        private void CorrectPosition(GameObject go)
        {
            Camera cam = SceneView.lastActiveSceneView.camera;

            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                Vector3 point = hit.point;

                // Position gameobject correctly on top of
                if (go.GetComponent<MeshFilter>())
                {
                    if (go.GetComponent<MeshFilter>().sharedMesh)
                    {
                        Bounds bounds = go.GetComponent<MeshFilter>().sharedMesh.bounds;

                        Vector3 offset = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z);

                        point.y += offset.y;
                    }
                }

                go.transform.position = point;
            }
        }
#endregion
#region Project
        private void CreateMaterial()
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Material diffuse = primitive.GetComponent<MeshRenderer>().sharedMaterial;
            DestroyImmediate(primitive);

            string path = AssetDatabase.GenerateUniqueAssetPath(Utilities.GetSelectedPathOrFallback() + "/new material.mat");
            Material newMat = new Material(diffuse.shader);

            AssetDatabase.CreateAsset(newMat, path);
            Selection.activeObject = newMat;
        }
    }
#endregion
}