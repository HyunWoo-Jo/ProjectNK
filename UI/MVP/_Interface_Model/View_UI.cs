using System;
using UnityEngine;

namespace N.UI
{
    public class View_UI<Presenter, Model> : MonoBehaviour, IView_UI where Presenter : IPresenter_UI, new() where Model : IModel_UI
    {
        protected Presenter _presenter;

        public View_UI() {
            _presenter = new Presenter();
            _presenter.Init(this);
        }

        public virtual void UpdateUI() { }
    }
}
