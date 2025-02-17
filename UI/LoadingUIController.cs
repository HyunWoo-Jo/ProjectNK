using UnityEngine;
using UnityEngine.SceneManagement;
using N.Utills;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
namespace N.UI
{

    /// <summary>
    /// Loading scene�� UI�� SceneLoad�� ����
    /// </summary>
    public class LoadingUIController : MonoBehaviour
    {
        public string nextSceneName;
        private AsyncOperation _loadAsyncOper;

        [SerializeField] private Image _loadFillImage;
        [SerializeField] private TextMeshProUGUI _progressText;

        private float _startTime;
        private const float _minLoadTime = 2f; // �ּ� �ε� �ð�
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
            // Load ����
            _loadAsyncOper = SceneManager.LoadSceneAsync(nextSceneName);
            _loadAsyncOper.allowSceneActivation = false; // �� �ڵ� ��ȯ ����

            _startTime = Time.time;
        }
        void Update()
        {
            float realProgress = _loadAsyncOper.progress; // ���� �����
            float elapsedTime = Time.time - _startTime; // ��� �ð�

            if (elapsedTime < _minLoadTime) {
                // �ּ� �ε� �ð� ������ ��¥ ����� ���
                _fakeLoading = true;
                _fakeProgress = Mathf.Lerp(_fakeProgress, 1f, Time.deltaTime / _minLoadTime);
            } else {
                _fakeLoading = false;
            }

            float displayProgress = _fakeLoading ? _fakeProgress : realProgress;
            _loadFillImage.fillAmount = displayProgress;
            _progressText.text = ((int)(displayProgress * 100f)).ToString() + "%";

            // �ּ� �ε� �ð� ���� & ���� ������ �ε�Ǿ��� �� �Ѿ��
            if (elapsedTime >= _minLoadTime && realProgress >= 0.9f) {
                _loadAsyncOper.allowSceneActivation = true;
            }

        }
    }
}
