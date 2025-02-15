
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using DG.Tweening;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface IInventoryView_UI : IView_UI {
        // Your logic here
        internal void UpdateSeletedColor(InventoryModel_UI.InvenType type, int seletedType);
        internal void UpdateTypeButton(InventoryModel_UI.InvenType type);
    }

    public class InventoryView_UI : View_UI<InventoryPresenter_UI,InventoryModel_UI> ,IInventoryView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new InventoryPresenter_UI();
            _presenter.Init(this);  
        }
        [Header("Inventory Panel")]
        [SerializeField] private OpenAnimation_UI _inventroyOpenAnimation;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _seletedColor;
        [SerializeField] private Image _allButton;
        [SerializeField] private GameObject _equipButtons;
        [SerializeField] private List<Image> _equipTypeButton_list;
        
        [Header("Selete Panel")]
        [SerializeField] private EventTrigger _seleteEquipButton;
        // Your logic here
        #region public
        private void Awake() {
#if UNITY_EDITOR
            // Inventory Panel
            Assert.IsNotNull(_inventroyOpenAnimation, typeof(InventoryView_UI).Name);
            Assert.IsNotNull(_allButton, typeof(InventoryView_UI).Name);
            Assert.IsNotNull(_equipButtons, typeof(InventoryView_UI).Name);
            Assert.IsTrue(_equipTypeButton_list.Count == 4, typeof(InventoryView_UI).Name);
            foreach(var item in _equipTypeButton_list) {
                Assert.IsNotNull(item, typeof(InventoryView_UI).Name);
            }
            // Selete Panel
            Assert.IsNotNull(_seleteEquipButton, typeof(InventoryView_UI).Name);
#endif
            InitButton();
        }
        private void OnEnable() {
            _allButton.gameObject.SetActive(false);
            _equipButtons.SetActive(false);
        }
        private void InitButton() {
            // ��ư�� �����ɶ� ���� �� Ȱ��ȭ �ϴ� �Լ�
            _inventroyOpenAnimation.AddCompleteHanlder(() => {
                UpdateUI();
            });
            // equip��ư �ʱ�ȭ
            _seleteEquipButton.AddEventButton(EventTriggerType.PointerDown, OnSeleteEquipment, typeof(InventoryView_UI).Name + "." + nameof(OnSeleteEquipment));

            // type ���� ��ư �ʱ�ȭ
            int seleted = 0;
            EventTrigger trigger = _allButton.GetComponent<EventTrigger>();

            // allButton
            trigger.AddEventButton(EventTriggerType.PointerDown, () => {
                _presenter.UpdateSeleteValue(0);
            }, typeof(InventoryView_UI).Name + "." + nameof(InitButton));
            // Equip Button
           foreach (var item in _equipTypeButton_list) {
                trigger = item.GetComponent<EventTrigger>();
                int seletedValue = ++seleted;
                trigger.AddEventButton(EventTriggerType.PointerDown, () => {
                    _presenter.UpdateSeleteValue(seletedValue);
                }, typeof(InventoryView_UI).Name + "." + nameof(InitButton));
            }
        }
        // selete panel�� equipment ��ư�� ����������
        public void OnSeleteEquipment() {
            _presenter.UpdateTypeButton(InventoryModel_UI.InvenType.Equipment);
        }
        private void UpdateUI() {
            _presenter.UpdateUI();
        }
        #endregion

        #region internal
        void IInventoryView_UI.UpdateSeletedColor(InventoryModel_UI.InvenType type, int seletedType) {
            if (seletedType == 0) {
                _allButton.DOColor(_seletedColor, 0.2f);
            } else {
                _allButton.color = _defaultColor;     
            }
            if (type == InventoryModel_UI.InvenType.Equipment) {
                for (int i = 0; i < _equipTypeButton_list.Count; i++) {
                    if (i == seletedType - 1) _equipTypeButton_list[i].DOColor(_seletedColor, 0.2f);
                    else _equipTypeButton_list[i].color = _defaultColor;
                }
            }

        }

        void IInventoryView_UI.UpdateTypeButton(InventoryModel_UI.InvenType type) {
            _allButton.gameObject.SetActive(true);
           
            if (type.Equals(InventoryModel_UI.InvenType.Equipment)) {
                _equipButtons.SetActive(true);
            } else {
                _equipButtons.SetActive(false);
            }
        }
        #endregion
    }
}
