using UnityEngine;
using UnityEditor;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
namespace N.Data
{
    public class EnemySpawnEditor : EditorWindow {
        // default
        private EnemySpawnDataList _loadedStage;
        private string _loadedPath;
        private readonly string _path = "Assets/Resources/Data/Stage";
        private Vector2 _scrollPosition;
        private Dictionary<EnemyName, Color> _enemyTypeColor_dic = new();
        // property
        private string _stageName;

        [MenuItem("Tools/StageEditor")]
        public static void OpenWindow() {
            var window = GetWindow<EnemySpawnEditor>("StageEditor");

            window.minSize = new Vector2(1400, 799);
            window.maxSize = new Vector2(1700, 800);
            window.Show();
        }
        private void OnGUI() {
            GUILayout.BeginArea(new Rect(30, 5, 1000, 900));
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("CreateStage", GUILayout.Height(100), GUILayout.Width(150))) {
                        CreateStage();
                    }
                    if (GUILayout.Button("LoadStage", GUILayout.Height(100), GUILayout.Width(150))) {
                        LoadStage();
                    }
                    if (_loadedStage != null && GUILayout.Button("SaveStage", GUILayout.Height(100), GUILayout.Width(150))) {
                        SaveStage();
                    }
                }
                GUILayout.EndHorizontal();
                if (_loadedStage != null) {
                    // Stage Label
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Stage Name", GUILayout.Width(90));
                        _stageName = GUILayout.TextField(_stageName, GUILayout.Width(100));
                    }
                    GUILayout.EndHorizontal();

                    // Sort Button
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Sort Time", GUILayout.Height(30), GUILayout.Width(90))) {
                            SortTime();
                        }
                    }
                    GUILayout.EndHorizontal();
                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    DrawGraph(lastRect.y + lastRect.height + 10);
                }
            }
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(1020, 5, 600, 3000));
            {
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(500), GUILayout.Height(800));
                if (_loadedStage != null) {
                    GUILayout.BeginVertical();
                    {
                        // Inspect ����
                        DrawInspector();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
        private void CreateStage() {
            // ���� ���� â ����
            string path = EditorUtility.SaveFilePanelInProject("Save Stage Data", "NewEnemySpawnDataList", "asset", "Enter a name for the stage data file.", _path);

            if (!string.IsNullOrEmpty(path)) {
                // ScriptableObject ���� �� ����
                EnemySpawnDataList newStage = ScriptableObject.CreateInstance<EnemySpawnDataList>();
                AssetDatabase.CreateAsset(newStage, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"Stage file created : {path}");
                _loadedPath = path;
                _loadedStage = AssetDatabase.LoadAssetAtPath<EnemySpawnDataList>(_loadedPath);

                _stageName = PathToName(_loadedPath);
            }
        }
        private void LoadStage() {
            // ���� ���� â ���� (Unity Asset �������� ���� ����)
            string path = EditorUtility.OpenFilePanel("Load Stage Data", _path, "asset");

            if (!string.IsNullOrEmpty(path)) {
                // ������Ʈ ���� ��� ��η� ��ȯ
                _loadedPath = "Assets" + path.Replace(Application.dataPath, "");
                // ������ ScriptableObject�� �ε�
                _loadedStage = AssetDatabase.LoadAssetAtPath<EnemySpawnDataList>(_loadedPath);

                _stageName = PathToName(_loadedPath);
                if (_loadedStage != null) {
                    Debug.Log($"Stage file loaded: {path}");
                } else {
                    Debug.LogError("Failed to load stage data");
                }
            }
        }
        private void SaveStage() {
            string fullPath = $"{_path}/{_stageName}.asset";
            EnemySpawnDataList existingAsset = AssetDatabase.LoadAssetAtPath<EnemySpawnDataList>(fullPath);

            if (existingAsset != null) {
                // ������ �����ϸ� ����� (����)
                EditorUtility.SetDirty(existingAsset);
                AssetDatabase.SaveAssets();
                Debug.Log($"Stage file updated: {fullPath}");
            } else {
                // ������ ������ ���� ����
                EnemySpawnDataList newStage = ScriptableObject.CreateInstance<EnemySpawnDataList>();
                AssetDatabase.CreateAsset(newStage, fullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                _loadedStage = newStage; // ���� ������ ������ �ε�

                Debug.Log($"New stage file created: {fullPath}");
            }
        }
        private string PathToName(string path) {
            return path.Replace(_path + "/", "").Replace(".asset", "");
        }

        private void SortTime() {
            _loadedStage.spawnData_list.Sort((a, b)=> a.spawnTime.CompareTo(b.spawnTime));
        }

        private void DrawInspector() {
            SerializedObject serializedObject = new SerializedObject(_loadedStage);
            serializedObject.Update();

            SerializedProperty prop = serializedObject.GetIterator();
            prop.NextVisible(true); // ù ��° �ʵ�� �̵� (script �ʵ� ���Ե�)

            GUILayout.Space(10);
            GUILayout.Label("Enemy Spawn Data List", EditorStyles.boldLabel);

         
            while (prop.NextVisible(false)) {
                if (prop.name == "m_Script") continue; // field �����
                EditorGUILayout.PropertyField(prop, true);
            }

            serializedObject.ApplyModifiedProperties();
        }

        // Graph�� �׸��� �ż���
        private void DrawGraph(float yPos) {
           
            if (_loadedStage == null || _loadedStage.spawnData_list == null || _loadedStage.spawnData_list.Count == 0)
                return;
            Vector2 rect = new Vector2(800f, 300f);
            Rect graphRect = new Rect(20, yPos, rect.x, rect.y); // �׷��� �׸� ����

            GUILayout.BeginHorizontal();
            {
                Handles.BeginGUI();
                // 1. spawnTime�� �ش��ϴ� ���� ����
                Dictionary<float, int> timeCount_dic = new();
                HashSet<EnemyName> enemyName_set = new();
                foreach (var data in _loadedStage.spawnData_list) {
                    if (timeCount_dic.ContainsKey(data.spawnTime)) {
                        timeCount_dic[data.spawnTime]++;
                    } else {
                        timeCount_dic.Add(data.spawnTime, 1);
                    }
                    // ������ �߰�
                    if (!_enemyTypeColor_dic.ContainsKey(data.enemyName)) {
                        _enemyTypeColor_dic.Add(data.enemyName, new Color(UnityEngine.Random.Range(0.3f, 1f), UnityEngine.Random.Range(0.3f, 1f), UnityEngine.Random.Range(0.3f, 1f)));
                    }
                    enemyName_set.Add(data.enemyName);
                }
                List<EnemyName> name_list = new();
                foreach (var nameColor in _enemyTypeColor_dic) {
                    if (!enemyName_set.Contains(nameColor.Key)) {
                        name_list.Add(nameColor.Key);              
                    }
                }
                foreach (var name in name_list) 
                {
                    _enemyTypeColor_dic.Remove(name);
                }

                // 2. �׷��� �׸���
                float maxX = timeCount_dic.Keys.Max() + 3; // x���� �ִ� �� (�ð�)
                float maxY = timeCount_dic.Values.Max() + 3; // y���� �ִ� �� (����)
                
                
                GUI.Box(graphRect, "Spawn TimeLine");

                // 3. ���� �׸��� (x��� y��)
                float xStep = maxX / 5; // x�� ���� ����
                float yStep = maxY / 5; // y�� ���� ����

                // x�� ���� �׸���
                for (int i = 0; i <= 5; i++) {
                    float x = Mathf.InverseLerp(0, maxX, i * xStep) * graphRect.width;
                    Handles.DrawLine(new Vector2(graphRect.x + x, graphRect.y),
                                     new Vector2(graphRect.x + x, graphRect.y + graphRect.height)); // x�� ��

                    // �ؽ�Ʈ�� ���� ǥ��
                    GUI.Label(new Rect(graphRect.x + x - 15, graphRect.y + graphRect.height + 5, 30, 20),
                              ((i * xStep)).ToString("0.0s"));
                }

                // y�� ���� �׸���
                for (int i = 0; i <= 5; i++) {
                    float y = Mathf.InverseLerp(0, maxY, i * yStep) * graphRect.height;
                    if (i == 0 || i == 5) { // �׵θ� �׸���
                        Handles.DrawLine(new Vector2(graphRect.x, graphRect.y + graphRect.height - y),
                                         new Vector2(graphRect.x + graphRect.width, graphRect.y + graphRect.height - y)); // y�� ��
                    }
                    // �ؽ�Ʈ�� ���� ǥ��
                    GUI.Label(new Rect(graphRect.x - 20, graphRect.y + graphRect.height - y - 10, 30, 20),
                              ((i * yStep)).ToString("0"));
                }

                // 4. �׷��� �� �׸���
                Vector2 previousPoint = Vector2.zero;
                int index = 0;
                foreach (var data in _loadedStage.spawnData_list) {
                    float key = data.spawnTime;
                    int yKey = timeCount_dic[key]--;
                    float x = Mathf.InverseLerp(0, maxX, key) * graphRect.width; // 0�� maxX ������ ������ x ��ǥ ��ȯ
                    float y = Mathf.InverseLerp(0, maxY, yKey) * graphRect.height; // 0�� maxY ������ ������ y ��ǥ ��ȯ

                    Vector2 pointPosition = new Vector2(graphRect.x + x, graphRect.y + graphRect.height - y); // ȭ�� ��ǥ�� ��ȯ

                    // ������ ���� �׸��� (������ 3���� ����)
                    Handles.color = _enemyTypeColor_dic[data.enemyName];
                    Handles.DrawSolidDisc(pointPosition, Vector3.forward, 3f);
                    GUI.Label(new Rect(pointPosition.x - 3, pointPosition.y + 5, 20, 10), index.ToString());
                    index++;
                }
                Handles.color = Color.white;
                Handles.EndGUI();
            }
            GUILayout.EndHorizontal();
            
            // color ǥ��
            GUILayout.BeginHorizontal();
            {
                Handles.BeginGUI();
                Rect typeRect = new Rect(20, yPos + rect.y + 30, 300, _enemyTypeColor_dic.Count * 20f + 20f);
                int index = 1;
                foreach (var nameColor in _enemyTypeColor_dic) {
                    float x = typeRect.x + 10;
                    float y = typeRect.y + index++ * 20f;
                    Vector2 pointPosition = new Vector2(x, y);

                    GUI.Label(new Rect(x + 15f, y - 10f, 100, 20), nameColor.Key.ToString());
                    Handles.color = nameColor.Value;
                    Handles.DrawSolidDisc(pointPosition, Vector3.forward, 3f);
                }
                GUI.Box(typeRect, "enemy type");
                Handles.color = Color.white;
                Handles.EndGUI();
            }
            GUILayout.EndHorizontal();
        }
    }
}
