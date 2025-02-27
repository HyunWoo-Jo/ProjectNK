
using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.EventSystems;
using NUnit.Framework;
using N.Utills;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface IAutoButtonView_UI : IView_UI {
        // Your logic here
        internal void UpdateUI(bool isDown);
        internal void AddButtonHandler(EventTrigger.Entry entry, string classMethodName);
    }

    public class AutoButtonView_UI : View_UI<AutoButtonPresenter_UI,AutoButtonModel_UI> ,IAutoButtonView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new AutoButtonPresenter_UI();
            _presenter.Init(this);  
        }
        [SerializeField] private EventTrigger _eventTrigger;
        // Your logic here
        #region public

        private void Awake() {
#if UNITY_EDITOR
            string scriptName = typeof(AutoButtonView_UI).Name;
            Assert.IsNotNull(_eventTrigger, scriptName);
#endif
        }
        public void InitButton(Action downAction, bool isAuto) {
            _presenter.InitButton(downAction, isAuto);
        }


        #endregion

        #region internal
        void IAutoButtonView_UI.UpdateUI(bool isDown) {
         
        }

        void IAutoButtonView_UI.AddButtonHandler(EventTrigger.Entry entry, string classMethodName) {
            _eventTrigger.AddEventButton(entry, classMethodName);
        }
        #endregion
    }
}
