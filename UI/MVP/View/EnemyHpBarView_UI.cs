
using UnityEngine;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine.UI;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface IEnemyHpBarView_UI : IView_UI {
        // Your logic here
        internal void SetScreenPosition(Vector3 screenPosition);
        internal void SetFillAmount(float amount);
    }

    public class EnemyHpBarView_UI : View_UI<EnemyHpBarPresenter_UI,EnemyHpBarModel_UI> ,IEnemyHpBarView_UI
    {
        protected override void CreatePresenter() {
            _presenter = new EnemyHpBarPresenter_UI();
            _presenter.Init(this);  
        }

        // Your logic here
        [SerializeField] private Image fill_img;
        private void Awake() {
#if UNITY_EDITOR
            string scriptName = typeof(EnemyHpBarView_UI).Name;
            Assert.IsNotNull(fill_img, scriptName);
#endif
        }

        #region public
        public void SetPosition(Vector3 pos) {
            _presenter.SetPosition(pos);
        }
        public void SetImageFill(float hp, float curHp) {
            _presenter.SetFillAmount(hp, curHp);
        }
        #endregion

        #region internal
        void IEnemyHpBarView_UI.SetScreenPosition(Vector3 screenPosition) {
            this.gameObject.transform.localPosition = screenPosition;
        }

        void IEnemyHpBarView_UI.SetFillAmount(float amount) {
           fill_img.fillAmount = amount;
        }

        #endregion
    }
}
