
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface IReloadingView_UI : IView_UI {
        // Your logic here
        internal void UpdateFillUI(float amount);
    }

    public class ReloadingView_UI : View_UI<ReloadingPresenter_UI,ReloadingModel_UI> ,IReloadingView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new ReloadingPresenter_UI();
            _presenter.Init(this);  
        }
        [SerializeField] private Image _fillImage;
        // Your logic here
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_fillImage);    
#endif
        }
        

        #region public
        public void UpdateFill(float reloadTime, float curTime) {
            _presenter.UpdateReload(reloadTime, curTime);
        }
        public void UpdateFill(float amount) {
            _presenter.UpdateReload(amount);
        }
        #endregion

        #region internal
        void IReloadingView_UI.UpdateFillUI(float amount) {
            _fillImage.fillAmount = amount;
        }
        #endregion
    }
}
