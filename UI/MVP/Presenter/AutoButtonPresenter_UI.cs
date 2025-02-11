
using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.EventSystems;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class AutoButtonPresenter_UI : Presenter_UI<AutoButtonModel_UI, IAutoButtonView_UI> {
        // Your logic here
        #region internal
        internal void InitButton(Action downAction, bool isDown) {
            EventTrigger.Entry entry = new();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(e => { 
                downAction?.Invoke();
                _model.isDown = !_model.isDown;
                _view.UpdateUI(isDown);
            });
            _view.AddButtonHandler(entry, this.GetType().Name + "." + nameof(InitButton));
            _model.isDown = isDown;
        }
        #endregion
    }
}
