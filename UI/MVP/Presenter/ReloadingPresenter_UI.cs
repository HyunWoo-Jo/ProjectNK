
using UnityEngine;
using System.Runtime.CompilerServices;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class ReloadingPresenter_UI : Presenter_UI<ReloadingModel_UI, IReloadingView_UI> {
        // Your logic here
        #region internal
        internal void UpdateReload(float reloadTime, float curTime) {
            _view.UpdateFillUI(curTime / reloadTime);
        }
        internal void UpdateReload(float amount) {
            _view.UpdateFillUI(amount);
        }
        #endregion
    }
}
