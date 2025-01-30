
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
            public List<(bool IsActive, bool IsSelected)> _activePortraitState_list { get; private set; } // 추가

            public SelecteBottomPortraitTest() {
                Presenter = new SelecteBottomPortraitPresenter_UI();
                Presenter.Init(this);
                _ammoText_list = new List<string>();
                _hpRatio_list = new List<float>();
                _shieldRatio_list = new List<float>();
                _portraitSprite_list = new List<Sprite>();
                _buttonHandler_list = new List<EventTrigger.Entry>();
                _activePortraitState_list = new List<(bool IsActive, bool IsSelected)>(); // 초기화
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

            // slot 오브젝트 생성 및 할당
            var slotDataList = new List<PortraitSlot_UI_Data>();
            var slotPrefab = _dataManager.LoadAssetSync<GameObject>("Slot_Potrait.prefab");
            TestUtils.AssertWithDebug(slotPrefab != null, "prefab load 오류");
            for (int i = 0; i < count; i++) {
                GameObject slotObj = GameObject.Instantiate(slotPrefab);
                slotDataList.Add(slotObj.GetComponent<PortraitSlot_UI_Data>());
            }

            // 슬롯 위치 및 핸들러 초기화
            testView.Presenter.ButtonInit(count, canvasWidth, slotDataList, index => { }, isEnter => { });

            // 슬롯 데이터 테스트
            TestUtils.AssertWithDebug(testView._buttonHandler_list.Count == count, $"핸들러 개수 불일치: {testView._buttonHandler_list.Count}");
            TestUtils.AssertWithDebug(slotDataList[0].transform.localPosition.x == -504f, "슬롯 위치 초기화 오류");
            TestUtils.AssertWithDebug(slotDataList[3].transform.localPosition.x == 504f, "슬롯 위치 초기화 오류");

            // 업데이트 로직 테스트
            testView.Presenter.UpdateHp(0, 100, 50);
            TestUtils.AssertWithDebug(testView._hpRatio_list[0] == 0.5f, $"HP 업데이트 오류: {testView._hpRatio_list[0]}");

            testView.Presenter.UpdateAmmoText(0, 100, 75);
            TestUtils.AssertWithDebug(testView._ammoText_list[0] == "75/100", $"탄약 UI 업데이트 오류: {testView._ammoText_list[0]}");

            testView.Presenter.UpdateShield(0, 200, 50);
            TestUtils.AssertWithDebug(testView._shieldRatio_list[0] == 0.25f, $"쉴드 업데이트 오류: {testView._shieldRatio_list[0]}");


            // 테스트: 초상화 애니메이션 및 리로드 상태 업데이트 테스트
            testView.Presenter.UpdatePortaitAnimation(0, true);
            testView.Presenter.UpdateReloadingActive(0, true);
            testView.Presenter.UpdateReloadingAmount(0, 0.7f);
            Assert.AreEqual((true, testView._activePortraitState_list[0].IsSelected), testView._activePortraitState_list[0]);
            Assert.AreEqual(0.7f, testView._hpRatio_list[0]);

        }
    }
}
