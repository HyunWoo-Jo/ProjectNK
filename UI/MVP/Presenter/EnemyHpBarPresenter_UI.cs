
using UnityEngine;
using System.Runtime.CompilerServices;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class EnemyHpBarPresenter_UI : Presenter_UI<EnemyHpBarModel_UI, IEnemyHpBarView_UI> {
        // Your logic here
        #region internal
        internal void SetFillAmount(float hp, float curHp) {
            _view.SetFillAmount(curHp / hp);
        }
        internal void SetPosition(Vector3 position) {
            Vector3 newPos = Camera.main.WorldToScreenPoint(position);
            Debug.Log(newPos);
            _view.SetScreenPosition(newPos);
        }

        #endregion
    }
}
