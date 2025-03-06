
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using N.Utills;
using UnityEngine.SceneManagement;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface IPauseView_UI : IView_UI {
        // Your logic here
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
        }

        private void OnCloseButton() {
            // panel 끄기
            _pausePanel.SetActive(false);
            // Button 키기
            _pauseButtonPanel.SetActive(true);
            // Pause false
            _presenter.Pause(false);
        }
        private void OnExitButton() {
            // Lobby Scene으로 되돌아 가기
            _presenter.LoadLobbyScene();

            // Pause false
            _presenter.Pause(false);
        }
        private void OnRetryButton() {
            // 씬 다시 로드
            _presenter.ReloadScene();

            // Pause false
            _presenter.Pause(false);
        }
        #endregion

        #region internal
        
        #endregion
    }
}
