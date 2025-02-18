
using UnityEngine;
using System.Runtime.CompilerServices;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class LoadingScenePresenter_UI : Presenter_UI<LoadingSceneModel_UI, ILoadingSceneView_UI> {
        // Your logic here
        #region internal
        internal void InitTime() {
            _model.startTime = Time.time;
        }
        internal void UpdateUI(float progress) {
            float elapsedTime = Time.time - _model.startTime; // ��� �ð�

            if (elapsedTime < LoadingSceneModel_UI.minLoadTime) {
                // �ּ� �ε� �ð� ������ ��¥ ����� ���
                _model.fakeLoading = true;
                _model.fakeProgress = Mathf.Lerp(_model.fakeProgress, 1f, Time.deltaTime / LoadingSceneModel_UI.minLoadTime);
            } else {
                _model.fakeLoading = false;
            }
            _model.displayProgress = _model.fakeLoading ? _model.fakeProgress : progress;
             _view.UpdateUI(_model.displayProgress);
        }
        internal float GetDisplayProgress() => _model.displayProgress;
        #endregion
    }
}
