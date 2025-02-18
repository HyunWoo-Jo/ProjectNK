using UnityEngine;
using N.UI;
using UnityEngine.Assertions;
using N.Data;
namespace N.Game
{
    public class LoadSceneController : MonoBehaviour
    {
        [SerializeField] private LoadingSceneView_UI _loadingView;

        private void Awake() {
#if UNITY_EDITOR
            string scriptName = typeof(LoadSceneController).Name;
            Assert.IsNotNull(_loadingView, scriptName);
            Debug.Log("Load Scene : " + Settings.nextSceneName);
#endif
        }
        public void Start() {
            // SceneLoad 시작
            LoadManager.Instance.LoadAsync(Settings.nextSceneName, false);
            // UI Handler 추가
            LoadManager.Instance.AddProgressHandler(_loadingView.UpdateUI);
        }

        public void OnDestroy() {
            // 제거될때 UI Handler 삭제
            LoadManager.Instance.RemoveProgressHandler(_loadingView.UpdateUI);
        }

        public void LoadDoneScene() {
            // 로드가 완료 되면 자동 씬 전환
            if(_loadingView.GetDisplayProgress() >= 0.9f || LoadManager.Instance.IsDone()) {
                LoadManager.Instance.Done();
            }
        }

        private void Update() {
            LoadDoneScene();
        }
    }
}
