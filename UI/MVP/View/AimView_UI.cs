using System;
using UnityEngine;

namespace N.UI
{
    public interface IAimView_UI : IView_UI {

    }

    public class AimView_UI : View_UI<AimPresenter_UI,AimModel_UI> ,IAimView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new AimPresenter_UI();
            _presenter.Init(this);
        }

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
