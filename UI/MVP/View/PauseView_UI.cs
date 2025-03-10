
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using N.Utills;
using UnityEngine.SceneManagement;
using N.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface IPauseView_UI : IView_UI {
        // Your logic here
        internal void UpdateCharacterSlotUI(int index, Sprite portraitSprite, string name, float damagePoint, float defensePoint);
    }

    public class PauseView_UI : View_UI<PausePresenter_UI,PauseModel_UI> ,IPauseView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new PausePresenter_UI();
            _presenter.Init(this);  
        }

        // Your logic here
        [Header("prefab")]
        [SerializeField] private GameObject _characterSlotPrefab;
        [Header("Panel")]
        [SerializeField] private GameObject _pauseButtonPanel;
        [SerializeField] private GameObject _pausePanel;
        [Header("Button")]
        [SerializeField] private EventTrigger _pauseButton;
        [SerializeField] private EventTrigger _closeButton;
        [SerializeField] private EventTrigger _exitButton;
        [SerializeField] private EventTrigger _retryButton;
        [Header("Content")]
        [SerializeField] private Transform _contentTransform;

        /// UI Data
        private List<CharacterPauseSlot_UI> _pauseSlot_list = new();
        private bool _isActive = false;
        public bool IsActive { get { return _isActive; } }

        private void Awake() {
#if UNITY_EDITOR
            // Assertion
            string scriptName = nameof(PauseView_UI);
            Assert.IsNotNull(_pauseButtonPanel, scriptName);
            Assert.IsNotNull(_pausePanel, scriptName);
            Assert.IsNotNull(_pauseButton, scriptName);
            Assert.IsNotNull(_closeButton, scriptName);
            Assert.IsNotNull(_exitButton, scriptName);
            Assert.IsNotNull(_retryButton, scriptName);
#endif
            ButtonInit();

            _pausePanel.SetActive(false);
        }

        private void OnDestroy() {
            // release Data
            for (int i = 0; i < _pauseSlot_list.Count; i++) {
                _presenter.ReleaseSpirte(i);
            }
        }

        #region public
        public void ButtonInit() {
            string scriptName = nameof(PauseView_UI);
            // Pause Button
            _pauseButton.AddEventButton(EventTriggerType.PointerDown, OnPauseButton, scriptName + "." + nameof(OnPauseButton));   
            // Close Button
            _closeButton.AddEventButton(EventTriggerType.PointerDown, OnCloseButton, scriptName + "." + nameof(OnCloseButton));
            // Exit Button
            _exitButton.AddEventButton(EventTriggerType.PointerDown, OnExitButton, scriptName + "." + nameof(OnExitButton));
            // Retry Button
            _retryButton.AddEventButton(EventTriggerType.PointerDown, OnRetryButton, scriptName + "." + nameof(OnRetryButton));
            
        }
        private void OnPauseButton() {
            // panel 키기
            _pausePanel.SetActive(true);
            // Button 끄기
            _pauseButtonPanel.SetActive(false);
            // Pause
            _presenter.Pause(true);
            _isActive = true;
        }

        private void OnCloseButton() {
            // panel 끄기
            _pausePanel.SetActive(false);
            // Button 키기
            _pauseButtonPanel.SetActive(true);
            // Pause false
            _presenter.Pause(false);
            _isActive = false;
        }
        private void OnExitButton() {
            // Lobby Scene으로 되돌아 가기
            _presenter.LoadLobbyScene();

            // Pause false
            _presenter.Pause(false);
            _isActive = false;
        }
        private void OnRetryButton() {
            // 씬 다시 로드
            _presenter.ReloadScene();

            // Pause false
            _presenter.Pause(false);
            _isActive = false;
        }
        /// <summary>
        /// Character Slot 추가
        /// </summary>
        /// <param name="characterStats"></param>
        public void AddCharacterSlot(CharacterStats characterStats) {
            // ui 생성
            var slotObject = GameObject.Instantiate(_characterSlotPrefab);
            // 부모 설정
            slotObject.transform.SetParent(_contentTransform);
            // list에 추가
            _pauseSlot_list.Add(slotObject.GetComponent<CharacterPauseSlot_UI>());
            // 위치 설정
            int index = _pauseSlot_list.Count - 1;
            slotObject.transform.localScale = Vector3.one;
            slotObject.transform.localPosition = new Vector3(0, index * -400 + 600, 0);
            // slot 설정
            _presenter.UpdateCharacterStat(index, characterStats);
        }
        
        public void SetCharacterPoint(int index, float dealPoint, float dealFillAmount, float defensePoint, float defenseFillAmount) {
            var slot = _pauseSlot_list[index];
            slot.SetDealText(dealPoint.ToString());
            slot.SetDealFillAmout(dealFillAmount);
            slot.SetDefenseText(defensePoint.ToString());
            slot.SetDefenseFillAmout(defenseFillAmount);
        }


        #endregion

        #region internal

        void IPauseView_UI.UpdateCharacterSlotUI(int index, Sprite portraitSprite, string name, float damagePoint, float defensePoint) {
            var slot = _pauseSlot_list[index];
            // 설정
            slot.SetPortraitImage(portraitSprite);
            slot.SetNameText(name);
            slot.SetDealText(damagePoint.ToString());
            slot.SetDefenseText(defensePoint.ToString());
        }

        #endregion
    }
}
