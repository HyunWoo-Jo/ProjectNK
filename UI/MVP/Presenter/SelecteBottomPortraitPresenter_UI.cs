
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class SelecteBottomPortraitPresenter_UI : Presenter_UI<SelecteBottomPortraitModel_UI, ISelecteBottomPortraitView_UI> {
        // Your logic here
        #region internal
        // ��ư �ʱ�ȭ �� �Ҵ� 
        internal void ButtonInit(int count, float canvasWidth, List<PortraitSlot_UI_Data> slotData_list, Action<int> slotChangeAction, Action<bool> enterExitAction) {
            // ��ư ���� ���� ���� (70% �������� / count �� ��)
            float width = (canvasWidth * 0.7f);
            float interval = width / (count - 1);
            float startX = width * -0.5f;
            for (int i =0;i< slotData_list.Count;i++) {
                var slotData = slotData_list[i];
                // ����ϴ� ��ư �� Ȱ��ȭ
                if (i >= count) {
                    slotData.gameObject.SetActive(false);
                    continue;
                }

                // ��ư Ŭ�� ���� �Ҵ�
                string entryClassName = this.GetType().Name + ".";
                int index = i;
                _view.AddButtonHandler(i,EventTriggerType.PointerDown, () => {
                    if (index == _model.IsSelectedIndex) { // ���� ��ư �ι� Ŭ��
                    } else { // ��ư ó�� Ŭ��
                        _model.IsSelectedIndex = index;
                        slotChangeAction.Invoke(index);
                    }
                }, entryClassName + nameof(slotChangeAction));
                // ��ư Enter
                _view.AddButtonHandler(i, EventTriggerType.PointerEnter, () => { enterExitAction?.Invoke(true); } , entryClassName + nameof(enterExitAction));
                // ��ư Exit
                _view.AddButtonHandler(i, EventTriggerType.PointerExit, () => { enterExitAction?.Invoke(false); }, entryClassName + nameof(enterExitAction));
                // ��ư ��ġ ����
                slotData.transform.localPosition = new Vector3(startX + (interval * i), 100, 0);
            }
        }
        internal void UpdateHp(int index, float  maxHp, float curHp) {
            _view.UpdateHp(index, curHp / maxHp);
        }
        internal void UpdateShield(int index, float maxShield, float curShield) {
            _view.UpdateShield(index, curShield / maxShield);
        }
        internal void UpdateAmmoText(int index, int maxAmmo, int curAmmo) {
            _view.UpdateAmmoText(index, $"{curAmmo}/{maxAmmo}");
        }
        internal void UpdatePortrait(int index, Sprite portraitSprite) {
            _view.UpdatePortrait(index, portraitSprite);
        }

        internal void UpdatePortaitAnimation(int index, bool isUp) {
            _view.UpdatePortraitAnimation(index, isUp);
        }

        internal void UpdateReloadingActive(int index, bool isActive) {
            _view.UpdateReloadingActive(index, isActive);
        }
        internal void UpdateReloadingAmount(int index, float amount) {
            _view.UpdateReloadingAmount(index, amount);
        }
        #endregion
    }
}
