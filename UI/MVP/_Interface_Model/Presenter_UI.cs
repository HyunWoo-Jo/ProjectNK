using UnityEngine;

namespace N.UI
{
    public class Presenter_UI<Model> : IPresenter_UI where Model : IModel_UI, new()
    {
        protected Model _model;
        protected IView_UI _view;

        public IPresenter_UI Init(IView_UI view) {
            _model = new Model();
            _view = view;
            return this;
        }
    }
}
