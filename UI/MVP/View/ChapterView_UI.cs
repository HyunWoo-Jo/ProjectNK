
using UnityEngine;
using System.Runtime.CompilerServices;
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
    public interface IChapterView_UI : IView_UI {
        // Your logic here
        internal void UpdateMapImageRect(Vector2 size, bool isCloseUp);
        internal void SetActiveMap(bool isActive);
    }

    public class ChapterView_UI : View_UI<ChapterPresenter_UI,ChapterModel_UI> ,IChapterView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new ChapterPresenter_UI();
            _presenter.Init(this);  
        }
        private void OnDestroy() {
            Dipose();
        }
        // Your logic here
        [Header("Map")]
        [SerializeField] private EventTrigger _mapButton;
        [SerializeField] private RectTransform _mapImageRect;
        [SerializeField] private EventTrigger _mapScaleButton;
        [SerializeField] private EventTrigger _mapCloseButton;
        [Header("Scene")]
        [SerializeField] private EventTrigger _preSceneButton;

        private Vector2 _mapStandardPosition;
        private Vector2 _mapStandardScale;
        private void Awake() {
#if UNITY_EDITOR
            string scriptName = nameof(ChapterView_UI);
            Assert.IsNotNull(_mapButton, scriptName);
            Assert.IsNotNull(_mapImageRect, scriptName);
            Assert.IsNotNull(_mapScaleButton, scriptName);
            Assert.IsNotNull(_mapCloseButton, scriptName);
            Assert.IsNotNull(_preSceneButton, scriptName);
#endif
            ButtonInit();
            // 기본 값 초기화
            _mapStandardPosition = _mapImageRect.localPosition;
            _mapStandardScale = _mapImageRect.sizeDelta;
        }

        // Button 초기화
        private void ButtonInit() {
            _presenter.MapButtonInit(_mapButton);
            _presenter.MapScaleButtonInit(_mapScaleButton);
            _presenter.PreSceneButtonInit(_preSceneButton);
            _presenter.MapCloseButtonInit(_mapCloseButton);
            
        }
        private void Dipose() {
            DOTween.Kill(_mapImageRect);
        }
        #region public

        #endregion

        #region internal
        void IChapterView_UI.UpdateMapImageRect(Vector2 size, bool isCloseUp) {
            Dipose();
            float time = 0.2f;

            // 확대, 축소 / Animation
            _mapImageRect.DOSizeDelta(size, time);
            Vector2 halfSize = (size - _mapStandardScale) * new Vector2(0.5f, -0.5f);

            if (isCloseUp) {
                _mapImageRect.DOLocalMove(_mapStandardPosition  + halfSize, time);
            } else {
                _mapImageRect.DOLocalMove(_mapStandardPosition, time);
            }
        }

        void IChapterView_UI.SetActiveMap(bool isActive) {
            if (isActive) {
                _mapImageRect.gameObject.SetActive(true);
                _mapButton.gameObject.SetActive(false);
            } else {
                _mapImageRect.gameObject.SetActive(false);
                _mapButton.gameObject.SetActive(true);
            }
        }
        #endregion
    }
}
