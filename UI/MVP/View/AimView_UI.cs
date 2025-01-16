
using System.Runtime.CompilerServices;
using UnityEngine;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface IAimView_UI : IView_UI {
        // Your logic here
       internal void ChangeAimPos(Vector2 pos);
    }

    public class AimView_UI : View_UI<AimPresenter_UI,AimModel_UI> ,IAimView_UI
    {
        [SerializeField] private RectTransform _rectTransform;

        protected override void CreatePresenter() {
            _presenter = new AimPresenter_UI();
            _presenter.Init(this);  
        }
        
        // Your logic here
        #region public

        public void ChangePosition(Vector2 pos) {
            _presenter.ChagneAddPosition(pos);
        }

        #endregion

        #region internal
        void IAimView_UI.ChangeAimPos(Vector2 pos) { 
            _rectTransform.localPosition = pos;
        }
        #endregion
    }
}
