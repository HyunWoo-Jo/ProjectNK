
using System;
using System.Runtime.CompilerServices;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class AimPresenter_UI : Presenter_UI<AimModel_UI, IAimView_UI> {
        // Your logic here
        #region internal

        internal void ChagneAddPosition(Vector2 addPos) {
            Vector2 pos = _model.Position + addPos;

            // 제한된 영역에서만 이동되도록 설정
            if (pos.y < _model.Bottom || pos.y > _model.Top) {
                addPos.y = 0;
            }
            if(pos.x > _model.Right || pos.x < _model.Left) {
                addPos.x = 0;
            }
            pos = _model.Position + addPos;

            _model.Direction = (pos - _model.Position).normalized;
            _model.Position = pos;
            _view.ChangeAimPos(pos);
        }
        internal void ChangePosition(Vector2 pos) {
            _model.Position = pos;
            _view.ChangeAimPos(pos);

        }
        internal void SetScreenSize(Vector2 size) {
            _model.ScreenSize = size;
            // 제한 구역 설정
            float yHalf = size.y * 0.5f;
            float xHalf = size.x * 0.5f;
            _model.Bottom = -1 * (yHalf - (size.y * 0.1f)); // 화면 비율 퍼센트지 이상 에임이 움직이지 않도록 설정
            _model.Top = yHalf - (size.y * 0.02f);
            _model.Right = xHalf - (size.x * 0.02f);
            _model.Left = -1 * (xHalf - (size.x * 0.02f));
        }
        
        internal void SetAmmo(int ammoCount) {
            _model.CurrentAmmo = ammoCount;
            _view.UpdateAmmoUI(_model.MaxAmmo, _model.CurrentAmmo);
        }
        internal void SetAmmo(int maxAmmo, int ammoCount) {
            _model.MaxAmmo = maxAmmo;
            _model.CurrentAmmo = ammoCount;
            _view.UpdateAmmoUI(_model.MaxAmmo, _model.CurrentAmmo);
        }

        internal Vector2 ScreenPosition() {
            Vector3 position = Position;
            Vector2 screenSize = ScreenSize;
            position.x += screenSize.x * 0.5f;
            position.y += screenSize.y * 0.5f;
            return position;
        }

        internal int AmmoCount => _model.CurrentAmmo;
        internal Vector2 Position => _model.Position;

        internal Vector2 ScreenSize => _model.ScreenSize;
        #endregion
    }
}
