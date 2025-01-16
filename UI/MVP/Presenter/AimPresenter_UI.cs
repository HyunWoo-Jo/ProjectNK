
using System;
using System.Runtime.CompilerServices;
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

            // ���ѵ� ���������� �̵��ǵ��� ����
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
            // ���� ���� ����
            float yHalf = size.y * 0.5f;
            float xHalf = size.x * 0.5f;
            _model.Bottom = -1 * (yHalf - (size.y * 0.1f)); // ȭ�� ���� �ۼ�Ʈ�� �̻� ������ �������� �ʵ��� ����
            _model.Top = yHalf - (size.y * 0.02f);
            _model.Right = xHalf - (size.x * 0.02f);
            _model.Left = -1 * (xHalf - (size.x * 0.02f));
        }
        internal Vector2 Position => _model.Position;
        #endregion
    }
}
