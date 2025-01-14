using UnityEngine;

namespace N.UI
{
    public class Presenter_UI<Model, View> : IPresenter_UI where Model : IModel_UI, new() where View : IView_UI
    {
        protected Model _model;
        protected View _view;

        public IPresenter_UI Init(IView_UI view) {
            _model = new Model();
            _view = (View)view;
            return this;
        }
    }
}
