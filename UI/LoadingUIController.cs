using UnityEngine;
using UnityEngine.SceneManagement;
using N.Utills;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
namespace N.UI
{

    /// <summary>
    /// Loading scene의 UI와 SceneLoad를 관리
    /// </summary>
    public class LoadingUIController : MonoBehaviour
    {
        public string nextSceneName;
        private AsyncOperation _loadAsyncOper;

        [SerializeField] private Image _loadFillImage;
        [SerializeField] private TextMeshProUGUI _progressText;

        private float _startTime;
        private const float _minLoadTime = 2f; // 최소 로딩 시간
        private bool _fakeLoading = false;
        private float _fakeProgress = 0f;

        private void Awake() {
#if UNITY_EDITOR
            string scriptName = typeof(LoadingUIController).Name;
            Assert.IsNotNull(_loadFillImage, scriptName);
            Assert.IsNotNull(_progressText, scriptName);
#endif
        }
        private void Start() {
            // Load 생성
            _loadAsyncOper = SceneManager.LoadSceneAsync(nextSceneName);
            _loadAsyncOper.allowSceneActivation = false; // 씬 자동 전환 방지

            _startTime = Time.time;
        }
        void Update()
        {
            float realProgress = _loadAsyncOper.progress; // 실제 진행률
            float elapsedTime = Time.time - _startTime; // 경과 시간

            if (elapsedTime < _minLoadTime) {
                // 최소 로딩 시간 전에는 가짜 진행률 사용
                _fakeLoading = true;
                _fakeProgress = Mathf.Lerp(_fakeProgress, 1f, Time.deltaTime / _minLoadTime);
            } else {
                _fakeLoading = false;
            }

            float displayProgress = _fakeLoading ? _fakeProgress : realProgress;
            _loadFillImage.fillAmount = displayProgress;
            _progressText.text = ((int)(displayProgress * 100f)).ToString() + "%";

            // 최소 로딩 시간 이후 & 씬이 완전히 로드되었을 때 넘어가기
            if (elapsedTime >= _minLoadTime && realProgress >= 0.9f) {
                _loadAsyncOper.allowSceneActivation = true;
            }

        }
    }
}
