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
                        // Inspect 생성
                        DrawInspector();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
        private void CreateStage() {
            // 파일 저장 창 띄우기
            string path = EditorUtility.SaveFilePanelInProject("Save Stage Data", "NewEnemySpawnDataList", "asset", "Enter a name for the stage data file.", _path);

            if (!string.IsNullOrEmpty(path)) {
                // ScriptableObject 생성 및 저장
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
            // 파일 선택 창 띄우기 (Unity Asset 내에서만 선택 가능)
            string path = EditorUtility.OpenFilePanel("Load Stage Data", _path, "asset");

            if (!string.IsNullOrEmpty(path)) {
                // 프로젝트 폴더 상대 경로로 변환
                _loadedPath = "Assets" + path.Replace(Application.dataPath, "");
                // 파일을 ScriptableObject로 로드
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
                // 파일이 존재하면 덮어쓰기 (갱신)
                EditorUtility.SetDirty(existingAsset);
                AssetDatabase.SaveAssets();
                Debug.Log($"Stage file updated: {fullPath}");
            } else {
                // 파일이 없으면 새로 생성
                EnemySpawnDataList newStage = ScriptableObject.CreateInstance<EnemySpawnDataList>();
                AssetDatabase.CreateAsset(newStage, fullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                _loadedStage = newStage; // 새로 생성한 데이터 로드

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
            prop.NextVisible(true); // 첫 번째 필드로 이동 (script 필드 포함됨)

            GUILayout.Space(10);
            GUILayout.Label("Enemy Spawn Data List", EditorStyles.boldLabel);

         
            while (prop.NextVisible(false)) {
                if (prop.name == "m_Script") continue; // field 숨기기
                EditorGUILayout.PropertyField(prop, true);
            }

            serializedObject.ApplyModifiedProperties();
        }

        // Graph를 그리는 매서드
        private void DrawGraph(float yPos) {
           
            if (_loadedStage == null || _loadedStage.spawnData_list == null || _loadedStage.spawnData_list.Count == 0)
                return;
            Vector2 rect = new Vector2(800f, 300f);
            Rect graphRect = new Rect(20, yPos, rect.x, rect.y); // 그래프 그릴 영역

            GUILayout.BeginHorizontal();
            {
                Handles.BeginGUI();
                // 1. spawnTime에 해당하는 개수 세기
                Dictionary<float, int> timeCount_dic = new();
                HashSet<EnemyName> enemyName_set = new();
                foreach (var data in _loadedStage.spawnData_list) {
                    if (timeCount_dic.ContainsKey(data.spawnTime)) {
                        timeCount_dic[data.spawnTime]++;
                    } else {
                        timeCount_dic.Add(data.spawnTime, 1);
                    }
                    // 고유색 추가
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

                // 2. 그래프 그리기
                float maxX = timeCount_dic.Keys.Max() + 3; // x축의 최대 값 (시간)
                float maxY = timeCount_dic.Values.Max() + 3; // y축의 최대 값 (개수)
                
                
                GUI.Box(graphRect, "Spawn TimeLine");

                // 3. 눈금 그리기 (x축과 y축)
                float xStep = maxX / 5; // x축 눈금 간격
                float yStep = maxY / 5; // y축 눈금 간격

                // x축 눈금 그리기
                for (int i = 0; i <= 5; i++) {
                    float x = Mathf.InverseLerp(0, maxX, i * xStep) * graphRect.width;
                    Handles.DrawLine(new Vector2(graphRect.x + x, graphRect.y),
                                     new Vector2(graphRect.x + x, graphRect.y + graphRect.height)); // x축 선

                    // 텍스트로 눈금 표시
                    GUI.Label(new Rect(graphRect.x + x - 15, graphRect.y + graphRect.height + 5, 30, 20),
                              ((i * xStep)).ToString("0.0s"));
                }

                // y축 눈금 그리기
                for (int i = 0; i <= 5; i++) {
                    float y = Mathf.InverseLerp(0, maxY, i * yStep) * graphRect.height;
                    if (i == 0 || i == 5) { // 테두리 그리기
                        Handles.DrawLine(new Vector2(graphRect.x, graphRect.y + graphRect.height - y),
                                         new Vector2(graphRect.x + graphRect.width, graphRect.y + graphRect.height - y)); // y축 선
                    }
                    // 텍스트로 눈금 표시
                    GUI.Label(new Rect(graphRect.x - 20, graphRect.y + graphRect.height - y - 10, 30, 20),
                              ((i * yStep)).ToString("0"));
                }

                // 4. 그래프 점 그리기
                Vector2 previousPoint = Vector2.zero;
                int index = 0;
                foreach (var data in _loadedStage.spawnData_list) {
                    float key = data.spawnTime;
                    int yKey = timeCount_dic[key]--;
                    float x = Mathf.InverseLerp(0, maxX, key) * graphRect.width; // 0과 maxX 사이의 비율로 x 좌표 변환
                    float y = Mathf.InverseLerp(0, maxY, yKey) * graphRect.height; // 0과 maxY 사이의 비율로 y 좌표 변환

                    Vector2 pointPosition = new Vector2(graphRect.x + x, graphRect.y + graphRect.height - y); // 화면 좌표로 변환

                    // 원으로 점을 그리기 (반지름 3으로 설정)
                    Handles.color = _enemyTypeColor_dic[data.enemyName];
                    Handles.DrawSolidDisc(pointPosition, Vector3.forward, 3f);
                    GUI.Label(new Rect(pointPosition.x - 3, pointPosition.y + 5, 20, 10), index.ToString());
                    index++;
                }
                Handles.color = Color.white;
                Handles.EndGUI();
            }
            GUILayout.EndHorizontal();
            
            // color 표시
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
