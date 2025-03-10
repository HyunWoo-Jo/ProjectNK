
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using DG.Tweening;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
     public interface IAimView_UI : IView_UI {
        // Your logic here
       internal void ChangeAimPos(Vector2 pos);
       internal void UpdateAmmoUI(int maxAmmo, int curAmmo);
    }

    public class AimView_UI : View_UI<AimPresenter_UI,AimModel_UI> ,IAimView_UI
    {
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private TMP_Text _ammoCountText;
       
        [SerializeField] private Color _countBgEmptyColor;
        [SerializeField] private Color _countBgGageZeroColor;
        [SerializeField] private Color _countBgGageOneColor;
        [SerializeField] private Image _countBgImage;

        [SerializeField] private Image _gageImage;
        protected override void CreatePresenter() {
            _presenter = new AimPresenter_UI();
            _presenter.Init(this);  
        }

        private void Awake() {
            // Assertion
#if UNITY_EDITOR
            string scriptName = typeof(AimView_UI).Name;
            Assert.IsNotNull(_rectTransform, scriptName);
            Assert.IsNotNull(_ammoCountText, scriptName);
            Assert.IsNotNull(_countBgImage, scriptName);
            Assert.IsNotNull(_gageImage, scriptName);
#endif
        }

        // Your logic here
        #region public

        public void AddPosition(Vector2 pos) {
            _presenter.ChagneAddPosition(pos);
        }
        public void SetPosition(Vector2 pos) {
            _presenter.ChangePosition(pos);
        }
        public void SetScreenSize(Vector2 screenSize) {
            _presenter.SetScreenSize(screenSize);
        }
        public Vector2 GetPosition() {
            return _presenter.Position;
        }
        public Vector2 ScreenPosition() {
            return _presenter.ScreenPosition();
        }
        public void SetAmmo(int count) {
            _presenter.SetAmmo(count);
        }

        public void ReloadAmmo(int maxAmmo, int loadAmmo) {
            _presenter.SetAmmo(maxAmmo, loadAmmo);
        }

        public void ShakeAim() {
            _rectTransform.DOShakePosition(0.02f, 10f);
        }

        #endregion

        #region internal
        void IAimView_UI.ChangeAimPos(Vector2 pos) { 
            _rectTransform.localPosition = pos;
        }


        void IAimView_UI.UpdateAmmoUI(int maxAmmo, int curAmmo) {
            _ammoCountText.text = curAmmo.ToString().PadLeft(3,'0');
            float ration = (float)curAmmo / (float)maxAmmo;
            _gageImage.fillAmount = ration;
            Color color;
            if (ration <= 0.2f) {
                color = _countBgEmptyColor;
            } else {
                color = Color.Lerp(_countBgGageZeroColor, _countBgGageOneColor, ration);
            }
            _countBgImage.color = color;

        }
        #endregion
    }
}
