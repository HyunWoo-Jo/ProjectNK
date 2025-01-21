using System;
using UnityEngine;

namespace N.UI
{
    public abstract class View_UI<Presenter, Model> : MonoBehaviour, IView_UI where Presenter : IPresenter_UI, new() where Model : IModel_UI
    {
        protected Presenter _presenter;

        public View_UI() {
            CreatePresenter();
        }
        protected abstract void CreatePresenter();
    }
}
