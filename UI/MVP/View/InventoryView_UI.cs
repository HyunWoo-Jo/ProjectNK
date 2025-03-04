
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using DG.Tweening;
using N.Utills;
using N.Data;
using System.Collections;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface IInventoryView_UI : IView_UI {
        // Your logic here
        internal void UpdateSelectedColor(InventoryModel_UI.InvenType type, int selectedType);
        internal void UpdateTypeButton(InventoryModel_UI.InvenType type, EquipmentType equipType);
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
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Image _allButton;
        [SerializeField] private GameObject _equipButtons;
        [SerializeField] private List<Image> _equipTypeButton_list;

        [SerializeField] private Transform _invenViewPortContent;

        [Header("Inven Equip UI")]
        [SerializeField] private GameObject _equipIconPrefab;

        [Header("Selecte Panel")]
        [SerializeField] private EventTrigger _selecteEquipButton;

        // Item
        private List<GameObject> _equip_list = new();

        // Your logic here
        #region public
        private void Awake() {
#if UNITY_EDITOR
            // Inventory Panel
            string scriptName = typeof(InventoryView_UI).Name;
            Assert.IsNotNull(_inventroyOpenAnimation, scriptName);
            Assert.IsNotNull(_allButton, scriptName);
            Assert.IsNotNull(_equipButtons, scriptName);
            Assert.IsTrue(_equipTypeButton_list.Count == 4, scriptName);
            foreach(var item in _equipTypeButton_list) {
                Assert.IsNotNull(item, scriptName);
            }
            Assert.IsNotNull(_invenViewPortContent, scriptName);

            Assert.IsNotNull(_equipButtons, scriptName);
            // Selete Panel
            Assert.IsNotNull(_selecteEquipButton, scriptName);
#endif
            InitButton();
            // EquipData 기반으로 ui 생성
            StartCoroutine(CreateInvenEquipIcon(EquipmentType.All));
        }
        private void OnEnable() {
            _allButton.gameObject.SetActive(false);
            _equipButtons.SetActive(false);
        }
       
        private void OnDestroy() {
            // Dotween 정리
            DOTween.Kill(_allButton);
            foreach(var item in _equipTypeButton_list) {
                DOTween.Kill(item);
            }
            StopAllCoroutines(); // 코루틴 정리
        }
        private void InitButton() {
            // 버튼이 생성될때 생성 후 활성화 하는 함수
            _inventroyOpenAnimation.AddCompleteHanlder(() => {
                UpdateUI();
            });
            // equip버튼 초기화
            _selecteEquipButton.AddEventButton(EventTriggerType.PointerDown, OnSelecteEquipment, typeof(InventoryView_UI).Name + "." + nameof(OnSelecteEquipment));

            // type 선택 버튼 초기화
            int selected = 0;
            EventTrigger trigger = _allButton.GetComponent<EventTrigger>();

            /*
        selected
        All = 0,
        Head,
        Armor,
        Belt,
        Shoes,
             */
            // allButton
            trigger.AddEventButton(EventTriggerType.PointerDown, () => {
                _presenter.UpdateSeleteValue(0);
            }, typeof(InventoryView_UI).Name + "." + nameof(InitButton));
            // Equip Button
           foreach (var item in _equipTypeButton_list) {
                trigger = item.GetComponent<EventTrigger>();
                int selectedValue = ++selected;
                trigger.AddEventButton(EventTriggerType.PointerDown, () => {
                    _presenter.UpdateSeleteValue(selectedValue);
                }, typeof(InventoryView_UI).Name + "." + nameof(InitButton));
            }

           // 처음 버튼 설정
            OnSelecteEquipment();
        }
        // selete panel의 equipment 버튼을 선택했을때
        public void OnSelecteEquipment() {
            _presenter.UpdateTypeButton(InventoryModel_UI.InvenType.Equipment);
        }
        private void UpdateUI() {
            _presenter.UpdateUI();
        }

        // equip icon 생성
        private IEnumerator CreateInvenEquipIcon(EquipmentType type) {
            var equipData_dic = DataManager.Instance.GetEquipData();
            int index = 0;
            Vector2 offset = new Vector3(300, -50);
            int x = 5;

            // Content 크기 설정
            var sizeDelta = _invenViewPortContent.GetComponent<RectTransform>().sizeDelta;
            sizeDelta.y = equipData_dic.Count / x * -170 + 400;
            _invenViewPortContent.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            yield return new WaitForSeconds(0.2f); // 0.2초뒤 생성
            

            foreach(var kvp in equipData_dic) {
                // 같은 종류의 UI만 생성
                if (type != EquipmentType.All && type != kvp.Value.type) continue;
                GameObject equipObj = Instantiate(_equipIconPrefab);
                equipObj.SetActive(false);
                // equip 셋팅
                equipObj.GetComponent<EquipmentUI>().SetEquipment(kvp.Value);
                equipObj.transform.SetParent(_invenViewPortContent);
                equipObj.transform.localPosition = new Vector2(index % x * 170, index / x * -170) + offset;
                equipObj.transform.localScale = Vector3.one;
                equipObj.SetActive(true);
                _equip_list.Add(equipObj);
                index++;
                // 초마다 생성
                yield return new WaitForSeconds(0.03f);
            } 
            
        }
        private void ClearEquipGameobject() {
            foreach(var equipObj in _equip_list) {
                Destroy(equipObj);
            }
            _equip_list.Clear();
        }
        #endregion

        #region internal
        // selected 버튼의 color 변경
        void IInventoryView_UI.UpdateSelectedColor(InventoryModel_UI.InvenType type, int selectedType) {
            if (selectedType == 0) {
                _allButton.DOColor(_selectedColor, 0.2f);
            } else {
                _allButton.color = _defaultColor;     
            }
            if (type == InventoryModel_UI.InvenType.Equipment) {
                for (int i = 0; i < _equipTypeButton_list.Count; i++) {
                    if (i == selectedType - 1) _equipTypeButton_list[i].DOColor(_selectedColor, 0.2f);
                    else _equipTypeButton_list[i].color = _defaultColor;
                }
            }

        }

        // type 버튼이 클릭되었을때 
        void IInventoryView_UI.UpdateTypeButton(InventoryModel_UI.InvenType type, EquipmentType equipType) {
            _allButton.gameObject.SetActive(true);
           
            if (type.Equals(InventoryModel_UI.InvenType.Equipment)) {
                _equipButtons.SetActive(true);
                StopAllCoroutines();
                // equip 정리
                ClearEquipGameobject();
                // 새로 생성
                StartCoroutine(CreateInvenEquipIcon(equipType));

            } else {
                _equipButtons.SetActive(false);
            }
        }
        #endregion
    }
}
