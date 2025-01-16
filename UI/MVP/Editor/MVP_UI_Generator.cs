using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.VersionControl;
namespace N.UI {
    /// <summary>
    /// MVP UI 자동 생성 코드
    /// </summary>
    public class MVP_UI_Generator : EditorWindow {

        private string _mvpName;
        private string _modelContext;
        private string _viewContext;
        private string _presenterContext;
        [MenuItem("Generator/MVP_UI")]
        public static void OpenWindow() {
            var window = GetWindow<MVP_UI_Generator>("MVP_Generator");
            
            window.maxSize = new Vector2(400, 70);
            window.Show();
           

        }
        private void OnGUI() {
            GUILayout.BeginHorizontal();
            GUILayout.Label("name", EditorStyles.boldLabel, GUILayout.Width(40));
            _mvpName = EditorGUILayout.TextField(_mvpName);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Generate")) {
                SetContext();
                GenerateScript();
            }
        }
        private void SetContext() {

            _modelContext = $@"
using UnityEngine;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace N.UI {{
    public class {_mvpName}Model_UI : Model_UI {{
        // Your logic here
    }}
}}
";

             _presenterContext = $@"
using UnityEngine;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace N.UI {{

    public class {_mvpName}Presenter_UI : Presenter_UI<{_mvpName}Model_UI, {_mvpName}View_UI> {{
        // Your logic here
    }}
}}
";
            _viewContext = $@"
using UnityEngine;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace N.UI
{{
    public interface I{_mvpName}View_UI : IView_UI {{
        // Your logic here
    }}

    public class {_mvpName}View_UI : View_UI<{_mvpName}Presenter_UI,{_mvpName}Model_UI> ,I{_mvpName}View_UI
    {{
        protected override void CreatePresenter() {{
            _presenter = new {_mvpName}Presenter_UI();
            _presenter.Init(this);  
        }}
        
        // Your logic here
        #region public

        #endregion

        #region internal

        #endregion
    }}
}}
";
        }
        private void GenerateScript() {
            string path = $"{Application.dataPath}/Scripts/UI/MVP/";
            string modelPath = path + $"Model/{_mvpName}Model_UI.cs";
            string presenterPath = path + $"Presenter/{_mvpName}Presenter_UI.cs";
            string viewPath = path + $"View/{_mvpName}View_UI.cs";

            File.WriteAllText(modelPath, _modelContext);
            File.WriteAllText(viewPath, _viewContext);
            File.WriteAllText(presenterPath, _presenterContext);

            AssetDatabase.Refresh();

        }

    }
}
