
using UnityEngine;
using System.Runtime.CompilerServices;
using N.Data;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class InventoryPresenter_UI : Presenter_UI<InventoryModel_UI, IInventoryView_UI> {
        // Your logic here
        #region internal
        // selete panel�� ��ư�� ����������
        internal void UpdateTypeButton(InventoryModel_UI.InvenType type) {
            if(_model.invenType != type) {
                _model.selectedType = 0;
                _view.UpdateTypeButton(type, EquipmentType.All);
            }
            _model.invenType = type; 
        }
        internal void UpdateSeleteValue(int value) {
            if (_model.selectedType != value) {
                // TYPE ��ư�� ���� Ŭ�� �Ǿ����� ����
                _model.selectedType = value;
                UpdateUI();
            }
            
        }
        // UI ����
        internal void UpdateUI() {
            _view.UpdateTypeButton(_model.invenType, (EquipmentType)_model.selectedType);
            _view.UpdateSelectedColor(_model.invenType, _model.selectedType);
        }
        #endregion
    }
}
