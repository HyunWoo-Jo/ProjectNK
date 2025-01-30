
using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;
using N.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using N.Data;
using N.DesignPattern;
namespace N.Test
{

    [TestFixture]
    public class UI_Aim_Test
    {
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

    }
    [TestFixture]
    public class UI_SeleteBottomPortrait_Test {
        private DataManager _dataManager;

        [SetUp]
        public void Init() {
            GameObject dataManagerObj = new();
            _dataManager = dataManagerObj.AddComponent<DataManager>();
        }

        [TearDown]
        public void Dispose() {
            GameObject.DestroyImmediate(_dataManager.gameObject);
        }

        private class SelecteBottomPortraitTest : ISelecteBottomPortraitView_UI {
            public SelecteBottomPortraitPresenter_UI Presenter { get; private set; }
            public List<string> _ammoText_list { get; private set; }
            public List<float> _hpRatio_list { get; private set; }
            public List<float> _shieldRatio_list { get; private set; }
            public List<Sprite> _portraitSprite_list { get; private set; }
            public List<EventTrigger.Entry> _buttonHandler_list { get; private set; }
            public List<(bool IsActive, bool IsSelected)> _activePortraitState_list { get; private set; } // �߰�

            public SelecteBottomPortraitTest() {
                Presenter = new SelecteBottomPortraitPresenter_UI();
                Presenter.Init(this);
                _ammoText_list = new List<string>();
                _hpRatio_list = new List<float>();
                _shieldRatio_list = new List<float>();
                _portraitSprite_list = new List<Sprite>();
                _buttonHandler_list = new List<EventTrigger.Entry>();
                _activePortraitState_list = new List<(bool IsActive, bool IsSelected)>(); // �ʱ�ȭ
            }

            void ISelecteBottomPortraitView_UI.UpdateAmmoText(int index, string text) {
                if (_ammoText_list.Count <= index) {
                    _ammoText_list.Add(text);
                } else {
                    _ammoText_list[index] = text;
                }
            }

            void ISelecteBottomPortraitView_UI.UpdateHp(int index, float amount) {
                if (_hpRatio_list.Count <= index) {
                    _hpRatio_list.Add(amount);
                } else {
                    _hpRatio_list[index] = amount;
                }
            }

            void ISelecteBottomPortraitView_UI.UpdateShield(int index, float amount) {
                if (_shieldRatio_list.Count <= index) {
                    _shieldRatio_list.Add(amount);
                } else {
                    _shieldRatio_list[index] = amount;
                }
            }

            void ISelecteBottomPortraitView_UI.UpdatePortrait(int index, Sprite portraitSprite) {
                if (_portraitSprite_list.Count <= index) {
                    _portraitSprite_list.Add(portraitSprite);
                } else {
                    _portraitSprite_list[index] = portraitSprite;
                }
            }

            void ISelecteBottomPortraitView_UI.AddButtonHandler(int index, EventTrigger.Entry entry) {
                if (_buttonHandler_list.Count <= index) {
                    _buttonHandler_list.Add(entry);
                } else {
                    _buttonHandler_list[index] = entry;
                }
            }


            void ISelecteBottomPortraitView_UI.UpdatePortraitAnimation(int index, bool isUp) {
                if (_activePortraitState_list.Count <= index) {
                    _activePortraitState_list.Add((isUp, _activePortraitState_list.Count > index && _activePortraitState_list[index].IsSelected));
                } else {
                    _activePortraitState_list[index] = (isUp, _activePortraitState_list[index].IsSelected);
                }
            }

            void ISelecteBottomPortraitView_UI.UpdateReloadingActive(int index, bool isActive) {
                if (_activePortraitState_list.Count <= index) {
                    _activePortraitState_list.Add((isActive, _activePortraitState_list.Count > index && _activePortraitState_list[index].IsSelected));
                } else {
                    _activePortraitState_list[index] = (isActive, _activePortraitState_list[index].IsSelected);
                }
            }

            void ISelecteBottomPortraitView_UI.UpdateReloadingAmount(int index, float amount) {
                if (_hpRatio_list.Count <= index) {
                    _hpRatio_list.Add(amount);
                } else {
                    _hpRatio_list[index] = amount;
                }
            }

        }
        [Test]
        public void SelecteBottomPortraitPresenterTest() {
            SelecteBottomPortraitTest testView = new();
            int count = 4;
            float canvasWidth = 1440f;

            // slot ������Ʈ ���� �� �Ҵ�
            var slotDataList = new List<PortraitSlot_UI_Data>();
            var slotPrefab = _dataManager.LoadAssetSync<GameObject>("Slot_Potrait.prefab");
            TestUtils.AssertWithDebug(slotPrefab != null, "prefab load ����");
            for (int i = 0; i < count; i++) {
                GameObject slotObj = GameObject.Instantiate(slotPrefab);
                slotDataList.Add(slotObj.GetComponent<PortraitSlot_UI_Data>());
            }

            // ���� ��ġ �� �ڵ鷯 �ʱ�ȭ
            testView.Presenter.ButtonInit(count, canvasWidth, slotDataList, index => { }, isEnter => { });

            // ���� ������ �׽�Ʈ
            TestUtils.AssertWithDebug(testView._buttonHandler_list.Count == count, $"�ڵ鷯 ���� ����ġ: {testView._buttonHandler_list.Count}");
            TestUtils.AssertWithDebug(slotDataList[0].transform.localPosition.x == -504f, "���� ��ġ �ʱ�ȭ ����");
            TestUtils.AssertWithDebug(slotDataList[3].transform.localPosition.x == 504f, "���� ��ġ �ʱ�ȭ ����");

            // ������Ʈ ���� �׽�Ʈ
            testView.Presenter.UpdateHp(0, 100, 50);
            TestUtils.AssertWithDebug(testView._hpRatio_list[0] == 0.5f, $"HP ������Ʈ ����: {testView._hpRatio_list[0]}");

            testView.Presenter.UpdateAmmoText(0, 100, 75);
            TestUtils.AssertWithDebug(testView._ammoText_list[0] == "75/100", $"ź�� UI ������Ʈ ����: {testView._ammoText_list[0]}");

            testView.Presenter.UpdateShield(0, 200, 50);
            TestUtils.AssertWithDebug(testView._shieldRatio_list[0] == 0.25f, $"���� ������Ʈ ����: {testView._shieldRatio_list[0]}");


            // �׽�Ʈ: �ʻ�ȭ �ִϸ��̼� �� ���ε� ���� ������Ʈ �׽�Ʈ
            testView.Presenter.UpdatePortaitAnimation(0, true);
            testView.Presenter.UpdateReloadingActive(0, true);
            testView.Presenter.UpdateReloadingAmount(0, 0.7f);
            Assert.AreEqual((true, testView._activePortraitState_list[0].IsSelected), testView._activePortraitState_list[0]);
            Assert.AreEqual(0.7f, testView._hpRatio_list[0]);

        }
    }
}
