
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;
using N.Utills;
using N.Data;
using N.Game;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class ChapterPresenter_UI : Presenter_UI<ChapterModel_UI, IChapterView_UI> {
        // Your logic here
        #region internal
        
        internal void PreSceneButtonInit(EventTrigger et) {
            et.AddEventButton(EventTriggerType.PointerDown, () => {
                // 메인 씬 로드
                LoadManager.Instance.LoadAsync("MainLobbyScene", true);
            }, ClassName() + "." + nameof(PreSceneButtonInit));
        }
        internal void MapButtonInit(EventTrigger et) {
            et.AddEventButton(EventTriggerType.PointerDown, () => {
                _model.isMapActive = true;
                _view.SetActiveMap(true);
            }, ClassName() + "." + nameof(MapButtonInit));

            _model.isMapActive = true;
            _view.SetActiveMap(true);
        }
        internal void MapScaleButtonInit(EventTrigger et) {
            et.AddEventButton(EventTriggerType.PointerDown, () => {
                if (_model.isScaleUp) {
                    // 기본 사이즈로
                    _view.UpdateMapImageRect(new Vector2(400, 400), false);
                } else {
                    // 2배로
                    _view.UpdateMapImageRect(new Vector2(800, 800), true);
                }
                _model.isScaleUp = !_model.isScaleUp;
            }, ClassName() + "." + nameof(MapScaleButtonInit));
        }
        internal void MapCloseButtonInit(EventTrigger et) {
            et.AddEventButton(EventTriggerType.PointerDown, () => {
                _model.isMapActive = false;
                _view.SetActiveMap(false);
            }, ClassName() + "." + nameof(MapScaleButtonInit));
        }
        #endregion
    }
}
