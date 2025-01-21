
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
            public List<string> AmmoTexts { get; private set; }
            public List<float> HpRatios { get; private set; }
            public List<float> ShieldRatios { get; private set; }
            public List<Sprite> PortraitSprites { get; private set; }
            public List<EventTrigger.Entry> ButtonHandlers { get; private set; }

            public SelecteBottomPortraitTest() {
                Presenter = new SelecteBottomPortraitPresenter_UI();
                Presenter.Init(this);
                AmmoTexts = new List<string>();
                HpRatios = new List<float>();
                ShieldRatios = new List<float>();
                PortraitSprites = new List<Sprite>();
                ButtonHandlers = new List<EventTrigger.Entry>();
            }

            void ISelecteBottomPortraitView_UI.UpdateAmmoText(int index, string text) {
                if (AmmoTexts.Count <= index) {
                    AmmoTexts.Add(text);
                } else {
                    AmmoTexts[index] = text;
                }
            }

            void ISelecteBottomPortraitView_UI.UpdateHp(int index, float amount) {
                if (HpRatios.Count <= index) {
                    HpRatios.Add(amount);
                } else {
                    HpRatios[index] = amount;
                }
            }

            void ISelecteBottomPortraitView_UI.UpdateShield(int index, float amount) {
                if (ShieldRatios.Count <= index) {
                    ShieldRatios.Add(amount);
                } else {
                    ShieldRatios[index] = amount;
                }
            }

            void ISelecteBottomPortraitView_UI.UpdatePortrait(int index, Sprite portraitSprite) {
                if (PortraitSprites.Count <= index) {
                    PortraitSprites.Add(portraitSprite);
                } else {
                    PortraitSprites[index] = portraitSprite;
                }
            }

            void ISelecteBottomPortraitView_UI.AddButtonHandler(int index, EventTrigger.Entry entry) {
                if (ButtonHandlers.Count <= index) {
                    ButtonHandlers.Add(entry);
                } else {
                    ButtonHandlers[index] = entry;
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
            testView.Presenter.ButtonInit(count, canvasWidth, slotDataList, index => { });

            // 슬롯 데이터 테스트
            TestUtils.AssertWithDebug(testView.ButtonHandlers.Count == count, $"핸들러 개수 불일치: {testView.ButtonHandlers.Count}");
            TestUtils.AssertWithDebug(slotDataList[0].transform.localPosition.x == -504f, "슬롯 위치 초기화 오류");
            TestUtils.AssertWithDebug(slotDataList[3].transform.localPosition.x == 504f, "슬롯 위치 초기화 오류");

            // 업데이트 로직 테스트
            testView.Presenter.UpdateHp(0, 100, 50);
            TestUtils.AssertWithDebug(testView.HpRatios[0] == 0.5f, $"HP 업데이트 오류: {testView.HpRatios[0]}");

            testView.Presenter.UpdateAmmoText(0, 100, 75);
            TestUtils.AssertWithDebug(testView.AmmoTexts[0] == "75/100", $"탄약 UI 업데이트 오류: {testView.AmmoTexts[0]}");

            testView.Presenter.UpdateShield(0, 200, 50);
            TestUtils.AssertWithDebug(testView.ShieldRatios[0] == 0.25f, $"쉴드 업데이트 오류: {testView.ShieldRatios[0]}");
        }
    }
}
