using UnityEngine;

namespace N.UI
{
    public class AimPresenter_UI : Presenter_UI<AimModel_UI>
    {
        internal void Move(Vector2 position) {
            _model.position = position;
            _view.UpdateUI();
        }
    }
}
