
using UnityEngine;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Assertions;
using Unity.Android.Gradle.Manifest;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface ILoadingSceneView_UI : IView_UI {
        // Your logic here
        internal void UpdateUI(float progress);
    }

    public class LoadingSceneView_UI : View_UI<LoadingScenePresenter_UI,LoadingSceneModel_UI> ,ILoadingSceneView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new LoadingScenePresenter_UI();
            _presenter.Init(this);  
        }

        // Your logic here
        [SerializeField] private Image _loadFillImage;
        [SerializeField] private TextMeshProUGUI _progressText;
        private void Awake() {
#if UNITY_EDITOR
            string scriptName = typeof(LoadingSceneView_UI).Name;
            Assert.IsNotNull(_loadFillImage, scriptName);
            Assert.IsNotNull(_progressText, scriptName);
#endif

            _presenter.InitTime();
        }
        #region public
        
        public float GetDisplayProgress() {
            return _presenter.GetDisplayProgress();
        }

        public void UpdateUI(float progress) {
            _presenter.UpdateUI(progress);
        }
        #endregion

        #region internal

        void ILoadingSceneView_UI.UpdateUI(float progress) {
            if (progress >= 0.88f) progress = 1f;
            _loadFillImage.fillAmount = progress;
            _progressText.text = ((int)(progress * 100f)).ToString() + "%";
        }
        #endregion
    }
}
