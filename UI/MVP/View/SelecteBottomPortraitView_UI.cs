
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using DG.Tweening;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface ISelecteBottomPortraitView_UI : IView_UI {
        // Your logic here
        internal void UpdateAmmoText(int index, string text);
        internal void UpdateHp(int index, float amount);
        internal void UpdateShield(int index, float amount);
        internal void UpdatePortrait(int index, Sprite portraitSpite);
        internal void AddButtonHandler(int index, EventTriggerType type, Action action, string entryClassMethodName);
        internal void UpdatePortraitAnimation(int index, bool isUp);
        internal void UpdateReloadingActive(int index, bool isActive);
        internal void UpdateReloadingAmount(int index, float amount);
    }

    public class SelecteBottomPortraitView_UI : View_UI<SelecteBottomPortraitPresenter_UI,SelecteBottomPortraitModel_UI> ,ISelecteBottomPortraitView_UI
    {
        [SerializeField] private List<PortraitSlot_UI_Data> _portraitUiData_list;
        protected override void CreatePresenter() {
            _presenter = new SelecteBottomPortraitPresenter_UI();
            _presenter.Init(this);  
        }

        private void Awake() {
#if UNITY_EDITOR
            // Assertion
            Assert.IsTrue(_portraitUiData_list.Count != 0, "SelecteBottomPortraitView_UI");
            foreach (var portaitUi in _portraitUiData_list) {
                Assert.IsNotNull(portaitUi, "SelecteBottomPortraitView_UI");
            }
#endif
        }

        // Your logic here
        #region public

        public void ButtonInit(int count, float fixCanvasWidth, Action<int> slotChangeAction, Action<bool> enterExitAction) {
            _presenter.ButtonInit(count, fixCanvasWidth, _portraitUiData_list, slotChangeAction, enterExitAction);
        }

        public void SetAmmo(int index, int maxAmmo, int curAmmo) {
            _presenter.UpdateAmmoText(index, maxAmmo, curAmmo);
        }
        public void SetHp(int index, float maxHp, float curHp) {
            _presenter.UpdateHp(index, maxHp, curHp);
        }
        public void SetShield(int index, float maxShield, float curShield) {
            _presenter.UpdateShield(index, maxShield, curShield);
        }
        public void SetPortrait(int index, Sprite portraitSpite) {
            _presenter.UpdatePortrait(index, portraitSpite);
        }
        /// <summary>
        /// 초상화 버튼 클릭시 동작
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isUp"></param>
        public void OnPortraitClick(int index, bool isUp) {
           _presenter.UpdatePortaitAnimation(index, isUp);
        }
        public void SetReloadingActive(int index, bool isActive) {
            _presenter.UpdateReloadingActive(index, isActive);
        }
        public void SetReloading(int index, float amount) {
            _presenter.UpdateReloadingAmount(index, amount);
        }

        #endregion

        #region internal
        void ISelecteBottomPortraitView_UI.UpdateAmmoText(int index, string text) {
            _portraitUiData_list[index].SetAmmoText(text);
        }

        void ISelecteBottomPortraitView_UI.UpdateHp(int index, float amount) {
            _portraitUiData_list[index].SetHpFillAmount(amount);
        }

        void ISelecteBottomPortraitView_UI.UpdateShield(int index, float amount) {
            _portraitUiData_list[index].SetShieldFillAmount(amount);
        }

        void ISelecteBottomPortraitView_UI.UpdatePortrait(int index, Sprite portraitSpite) {
            _portraitUiData_list[index].SetPortraitImage(portraitSpite);
        }

        void ISelecteBottomPortraitView_UI.AddButtonHandler(int index,EventTriggerType type, Action action, string entryClassMethodName) {
            _portraitUiData_list[index].AddButtonHandler(type, action, entryClassMethodName);
        }

        void ISelecteBottomPortraitView_UI.UpdatePortraitAnimation(int index, bool isUp) {
            if (isUp) {
                // portrait 위로 이동
                _portraitUiData_list[index].GetPortraitRectTransform().DOLocalMoveY(0f, 0.2f);
                // background 이동
                _portraitUiData_list[index].GetBackgroundImg().DOFillAmount(1.0f, 0.2f);
            } else {
                // portrait 기본 이동
                _portraitUiData_list[index].GetPortraitRectTransform().DOLocalMoveY(-190f, 0.2f);
                // background 기본 이동
                _portraitUiData_list[index].GetBackgroundImg().DOFillAmount(0.68f, 0.2f);
            }
            _portraitUiData_list[index].SetActiveTextParent(!isUp);
        }

        void ISelecteBottomPortraitView_UI.UpdateReloadingActive(int index, bool isActive) {
            _portraitUiData_list[index].SetActiveReloading(isActive);
        }

        void ISelecteBottomPortraitView_UI.UpdateReloadingAmount(int index, float amount) {
            _portraitUiData_list[index].SetReloadingFillAmount(amount);
        }
        #endregion
    }
}
