
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.EventSystems;
using System;
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
        internal void AddButtonHandler(int index, EventTrigger.Entry entry);

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

        public void ButtonInit(int count, float fixCanvasWidth, Action<int> SlotChangeAction) {
            _presenter.ButtonInit(count, fixCanvasWidth, _portraitUiData_list, SlotChangeAction);
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

        void ISelecteBottomPortraitView_UI.AddButtonHandler(int index, EventTrigger.Entry entry) {
            _portraitUiData_list[index].AddButtonHandler(entry);
        }
        #endregion
    }
}
