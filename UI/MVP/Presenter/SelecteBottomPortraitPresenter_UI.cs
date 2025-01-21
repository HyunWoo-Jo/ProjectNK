
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
        // 버튼 초기화 및 할당 
        internal void ButtonInit(int count, float canvasWidth, List<PortraitSlot_UI_Data> slotData_list, Action<int> slotChangeAction) {
            // 버튼 간격 조절 변수 (70% 구역에서 / count 한 값)
            float width = (canvasWidth * 0.7f);
            float interval = width / (count - 1);
            float startX = width * -0.5f;
            for (int i =0;i< slotData_list.Count;i++) {
                var slotData = slotData_list[i];
                // 사용하는 버튼 만 활성화
                if (i >= count) {
                    slotData.gameObject.SetActive(false);
                    continue;
                }
                // 버튼 클릭 로직 할당
                EventTrigger.Entry entry = new ();
                entry.eventID = EventTriggerType.PointerDown;
                int index = i;
                entry.callback.AddListener((eventData) => {
                    if (index == _model.IsSelectedIndex) { // 같은 버튼 두번 클릭
                    } else { // 버튼 처음 클릭
                        _model.IsSelectedIndex = index;
                        slotChangeAction.Invoke(index);
                    }
                });
                _view.AddButtonHandler(i, entry);
                // 버튼 위치 지정
                slotData.transform.localPosition = new Vector3(startX + (interval * i), 0, 0);
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
        #endregion
    }
}
