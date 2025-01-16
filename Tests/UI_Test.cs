
using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace N.UI
{

    [TestFixture]
    public class UI_Test
    {
        private class AimUITest : IAimView_UI {
            public AimPresenter_UI presenter = new AimPresenter_UI();
            public Vector2 pos = Vector2.zero;
            public AimUITest() {
                presenter.Init(this);
            }

            void IAimView_UI.ChangeAimPos(Vector2 pos) {
                this.pos = pos;
            }
        }
        [Test]
        public void AimTest() {
            AimUITest aimUITest = new AimUITest();
            aimUITest.presenter.SetScreenSize(new Vector2(1000, 1000));
            aimUITest.presenter.ChagneAddPosition(new Vector2(0, 100));
            Assert.AreNotEqual(aimUITest.pos, Vector2.zero, "aim doesn't move. standard");
            aimUITest.presenter.ChangePosition(new Vector2(0, 0));
            aimUITest.presenter.ChagneAddPosition(new Vector2(0, -390));
            Assert.IsFalse(Mathf.Abs((aimUITest.pos.y - 390)) <= Mathf.Epsilon , "aim doesn't move. bottom");
            aimUITest.presenter.ChagneAddPosition(new Vector2(0, -11));
            Assert.IsFalse(aimUITest.pos.y < -400, "aim is over limit area bottom");
        }

    }
}
