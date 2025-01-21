
using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;
using N.UI;
namespace N.Test
{

    [TestFixture]
    public class UI_Test
    {
        #region Aim UI MVP 
        private class AimUITest : IAimView_UI {
            public AimPresenter_UI Presenter { get; private set; }
            public Vector2 Position { get; private set; } = Vector2.zero;

            public AimUITest() {
                Presenter = new AimPresenter_UI();
                Presenter.Init(this);
            }

            void IAimView_UI.ChangeAimPos(Vector2 pos) {
                Position = pos;
            }

            void IAimView_UI.UpdateAmmoUI(int maxAmmo, int curAmmo) {
                // 테스트에 필요 없음
            }
        }

        [Test]
        public void AimMovementTest() {
            AimUITest aimUITest = new();

            // 화면 크기 설정
            aimUITest.Presenter.SetScreenSize(new Vector2(1000, 1000));
            TestUtils.AssertWithDebug(aimUITest.Presenter.ScreenSize == new Vector2(1000, 1000), "화면 크기 설정이 올바르지 않습니다.");

            // 위치 변경 테스트
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, 100));
            TestUtils.AssertWithDebug(aimUITest.Position != Vector2.zero, "Aim이 이동하지 않았습니다.");

            // 위치 초기화 및 하단 제한 테스트
            aimUITest.Presenter.ChangePosition(Vector2.zero);
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, -390));
            TestUtils.AssertWithDebug((Mathf.Abs(aimUITest.Position.y + 390) <= Mathf.Epsilon), "Aim이 하단 제한에서 이동하지 않았습니다. y : " + aimUITest.Position.y);

            // 하단 제한 초과 여부 테스트
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, -11));
            TestUtils.AssertWithDebug(!(aimUITest.Position.y < -400), "Aim이 하단 제한을 초과했습니다. y : " + aimUITest.Position.y);
        }

        [Test]
        public void AmmoTest() {
            AimUITest aimUITest = new();

            // 탄약 수량 설정
            aimUITest.Presenter.SetAmmo(10, 5);
            TestUtils.AssertWithDebug(aimUITest.Presenter.AmmoCount == 5, "현재 탄약 수가 올바르지 않습니다. Ammo : " + aimUITest.Presenter.AmmoCount);

            // 최대 탄약 수 변경
            aimUITest.Presenter.SetAmmo(20, 15);
            TestUtils.AssertWithDebug(aimUITest.Presenter.AmmoCount == 15, "탄약 수가 올바르게 업데이트되지 않았습니다. Ammo : " + aimUITest.Presenter.AmmoCount);
        }

        [Test]
        public void ScreenBoundaryTest() {
            AimUITest aimUITest = new();

            // 화면 크기 설정
            aimUITest.Presenter.SetScreenSize(new Vector2(500, 500));
            TestUtils.AssertWithDebug(aimUITest.Presenter.ScreenSize == new Vector2(500, 500), "화면 크기 설정이 올바르지 않습니다.");
            // 제한 영역 확인
            aimUITest.Presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.Presenter.ChagneAddPosition(new Vector2(-500, 0));
            TestUtils.AssertWithDebug(aimUITest.Position.x >= -400, "화면 왼쪽 제한을 초과했습니다. x :" + aimUITest.Position.x);

            aimUITest.Presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.Presenter.ChagneAddPosition(new Vector2(500, 0));
            TestUtils.AssertWithDebug(aimUITest.Position.x <= 400, "화면 오른쪽 제한을 초과했습니다. x : " + aimUITest.Position.x);

            aimUITest.Presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, -500));
            TestUtils.AssertWithDebug(aimUITest.Position.y >= -270, "화면 하단 제한을 초과했습니다. y : " + aimUITest.Position.y);

            aimUITest.Presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, 500));
            TestUtils.AssertWithDebug(aimUITest.Position.y <= 290, "화면 상단 제한을 초과했습니다. y : " + aimUITest.Position.x);
        }
        #endregion
    }
}
