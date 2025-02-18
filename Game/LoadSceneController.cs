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
            // SceneLoad ����
            LoadManager.Instance.LoadAsync(Settings.nextSceneName, false);
            // UI Handler �߰�
            LoadManager.Instance.AddProgressHandler(_loadingView.UpdateUI);
        }

        public void OnDestroy() {
            // ���ŵɶ� UI Handler ����
            LoadManager.Instance.RemoveProgressHandler(_loadingView.UpdateUI);
        }

        public void LoadDoneScene() {
            // �ε尡 �Ϸ� �Ǹ� �ڵ� �� ��ȯ
            if(_loadingView.GetDisplayProgress() >= 0.9f || LoadManager.Instance.IsDone()) {
                LoadManager.Instance.Done();
            }
        }

        private void Update() {
            LoadDoneScene();
        }
    }
}
