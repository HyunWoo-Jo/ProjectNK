
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
                // �׽�Ʈ�� �ʿ� ����
            }
        }

        [Test]
        public void AimMovementTest() {
            AimUITest aimUITest = new();

            // ȭ�� ũ�� ����
            aimUITest.Presenter.SetScreenSize(new Vector2(1000, 1000));
            TestUtils.AssertWithDebug(aimUITest.Presenter.ScreenSize == new Vector2(1000, 1000), "ȭ�� ũ�� ������ �ùٸ��� �ʽ��ϴ�.");

            // ��ġ ���� �׽�Ʈ
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, 100));
            TestUtils.AssertWithDebug(aimUITest.Position != Vector2.zero, "Aim�� �̵����� �ʾҽ��ϴ�.");

            // ��ġ �ʱ�ȭ �� �ϴ� ���� �׽�Ʈ
            aimUITest.Presenter.ChangePosition(Vector2.zero);
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, -390));
            TestUtils.AssertWithDebug((Mathf.Abs(aimUITest.Position.y + 390) <= Mathf.Epsilon), "Aim�� �ϴ� ���ѿ��� �̵����� �ʾҽ��ϴ�. y : " + aimUITest.Position.y);

            // �ϴ� ���� �ʰ� ���� �׽�Ʈ
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, -11));
            TestUtils.AssertWithDebug(!(aimUITest.Position.y < -400), "Aim�� �ϴ� ������ �ʰ��߽��ϴ�. y : " + aimUITest.Position.y);
        }

        [Test]
        public void AmmoTest() {
            AimUITest aimUITest = new();

            // ź�� ���� ����
            aimUITest.Presenter.SetAmmo(10, 5);
            TestUtils.AssertWithDebug(aimUITest.Presenter.AmmoCount == 5, "���� ź�� ���� �ùٸ��� �ʽ��ϴ�. Ammo : " + aimUITest.Presenter.AmmoCount);

            // �ִ� ź�� �� ����
            aimUITest.Presenter.SetAmmo(20, 15);
            TestUtils.AssertWithDebug(aimUITest.Presenter.AmmoCount == 15, "ź�� ���� �ùٸ��� ������Ʈ���� �ʾҽ��ϴ�. Ammo : " + aimUITest.Presenter.AmmoCount);
        }

        [Test]
        public void ScreenBoundaryTest() {
            AimUITest aimUITest = new();

            // ȭ�� ũ�� ����
            aimUITest.Presenter.SetScreenSize(new Vector2(500, 500));
            TestUtils.AssertWithDebug(aimUITest.Presenter.ScreenSize == new Vector2(500, 500), "ȭ�� ũ�� ������ �ùٸ��� �ʽ��ϴ�.");
            // ���� ���� Ȯ��
            aimUITest.Presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.Presenter.ChagneAddPosition(new Vector2(-500, 0));
            TestUtils.AssertWithDebug(aimUITest.Position.x >= -400, "ȭ�� ���� ������ �ʰ��߽��ϴ�. x :" + aimUITest.Position.x);

            aimUITest.Presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.Presenter.ChagneAddPosition(new Vector2(500, 0));
            TestUtils.AssertWithDebug(aimUITest.Position.x <= 400, "ȭ�� ������ ������ �ʰ��߽��ϴ�. x : " + aimUITest.Position.x);

            aimUITest.Presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, -500));
            TestUtils.AssertWithDebug(aimUITest.Position.y >= -270, "ȭ�� �ϴ� ������ �ʰ��߽��ϴ�. y : " + aimUITest.Position.y);

            aimUITest.Presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.Presenter.ChagneAddPosition(new Vector2(0, 500));
            TestUtils.AssertWithDebug(aimUITest.Position.y <= 290, "ȭ�� ��� ������ �ʰ��߽��ϴ�. y : " + aimUITest.Position.x);
        }
        #endregion
    }
}
