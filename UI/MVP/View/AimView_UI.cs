using System;
using UnityEngine;

namespace N.UI
{
    public interface IAimView_UI : IView_UI {

    }

    public class AimView_UI : View_UI<AimPresenter_UI,AimModel_UI>
    {
        private void Awake() {
            AddPosition(Vector2.one);
        }
        public void AddPosition(Vector2 position) {
            _presenter.Move(position);
        }

        public override void UpdateUI() {
            base.UpdateUI();
            Debug.Log(1);
        }

    }
}
