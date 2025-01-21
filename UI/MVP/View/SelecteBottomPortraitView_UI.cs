
using UnityEngine;
using System.Runtime.CompilerServices;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface ISelecteBottomPortraitView_UI : IView_UI {
        // Your logic here
    }

    public class SelecteBottomPortraitView_UI : View_UI<SelecteBottomPortraitPresenter_UI,SelecteBottomPortraitModel_UI> ,ISelecteBottomPortraitView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new SelecteBottomPortraitPresenter_UI();
            _presenter.Init(this);  
        }
        
        // Your logic here
        #region public

        #endregion

        #region internal

        #endregion
    }
}
