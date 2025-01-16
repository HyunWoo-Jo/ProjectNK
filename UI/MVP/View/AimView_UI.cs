
using UnityEngine;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace N.UI
{
    public interface IAimView_UI : IView_UI {
        // Your logic here
    }

    public class AimView_UI : View_UI<AimPresenter_UI,AimModel_UI> ,IAimView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new AimPresenter_UI();
            _presenter.Init(this);  
        }
        
        // Your logic here
        #region public

        #endregion

        #region internal

        #endregion
    }
}
