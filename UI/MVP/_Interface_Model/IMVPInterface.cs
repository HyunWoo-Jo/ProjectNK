using UnityEngine;

namespace N.UI
{
    public interface IModel_UI {}
    public interface IView_UI {
        public void UpdateUI();
    }
    public interface IPresenter_UI {
        public IPresenter_UI Init(IView_UI view);
        }
}
