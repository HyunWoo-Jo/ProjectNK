
using UnityEngine;
using System.Runtime.CompilerServices;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class InventoryPresenter_UI : Presenter_UI<InventoryModel_UI, IInventoryView_UI> {
        // Your logic here
        #region internal
        internal void UpdateTypeButton(InventoryModel_UI.InvenType type) {
            if(_model.invenType != type) {
                _model.seletedType = 0;
            }
            _model.invenType = type;

            _view.UpdateTypeButton(type);
        }
        internal void UpdateSeleteValue(int value) {
            if (_model.seletedType != value) {
                _model.seletedType = value;
                UpdateUI();
            }
            
        }
        internal void UpdateUI() {
            _view.UpdateTypeButton(_model.invenType);
            _view.UpdateSeletedColor(_model.invenType, _model.seletedType);
        }
        #endregion
    }
}
