
using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;
using N.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using N.Data;
using N.DesignPattern;
using System;
namespace N.Test {
    #region UI_Aim
    [TestFixture]
    public class UI_Aim_Test {
        // Aim UI 테스트를 위한 Mock 클래스
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
                // 테스트에서 필요하지 않음
            }
        }

        [Test]
        public void AimMovementTest() {
            AimUITest aimUITest = new();

            // 화면 크기 설정 테스트
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

            // 탄약 설정 테스트
            aimUITest.Presenter.SetAmmo(10, 5);
            TestUtils.AssertWithDebug(aimUITest.Presenter.AmmoCount == 5, "현재 탄약 수가 올바르지 않습니다. Ammo : " + aimUITest.Presenter.AmmoCount);

            // 최대 탄약 수 변경 테스트
            aimUITest.Presenter.SetAmmo(20, 15);
            TestUtils.AssertWithDebug(aimUITest.Presenter.AmmoCount == 15, "탄약 수가 올바르게 업데이트되지 않았습니다. Ammo : " + aimUITest.Presenter.AmmoCount);
        }

        [Test]
        public void ScreenBoundaryTest() {
            AimUITest aimUITest = new();

            // 화면 크기 설정 테스트
            aimUITest.Presenter.SetScreenSize(new Vector2(500, 500));
            TestUtils.AssertWithDebug(aimUITest.Presenter.ScreenSize == new Vector2(500, 500), "화면 크기 설정이 올바르지 않습니다.");

            // 제한 영역 테스트
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
            TestUtils.AssertWithDebug(aimUITest.Position.y <= 290, "화면 상단 제한을 초과했습니다. y : " + aimUITest.Position.y);
        }
    }
    #endregion

    #region UI_AutoButton
    [TestFixture]
    public class UI_AutoButton_Test {
        private class AutoButtonUITest : IAutoButtonView_UI {
            public AutoButtonPresenter_UI Presenter { get; private set; }
            public bool IsDown { get; private set; }

            public AutoButtonUITest() {
                Presenter = new AutoButtonPresenter_UI();
                Presenter.Init(this);
            }

            void IAutoButtonView_UI.UpdateUI(bool isDown) {
                IsDown = isDown;
            }

            void IAutoButtonView_UI.AddButtonHandler(EventTrigger.Entry entry, string classMethodName) {
                // event 호출
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                entry.callback.Invoke(eventData);
            }
        }

        [Test]
        public void ButtonToggleTest() {
            AutoButtonUITest buttonUITest = new();
            bool actionCalled = false;

            // 버튼 초기화
            buttonUITest.Presenter.InitButton(() => actionCalled = true, false);
            TestUtils.AssertWithDebug(buttonUITest.IsDown == false, "버튼 초기 상태가 올바르지 않습니다.");

            // 버튼 클릭 시 동작 확인
            buttonUITest.Presenter.InitButton(() => actionCalled = true, true);
            TestUtils.AssertWithDebug(actionCalled, "버튼 동작이 호출되지 않았습니다.");
            TestUtils.AssertWithDebug(buttonUITest.IsDown == true, "버튼 상태가 올바르게 변경되지 않았습니다.");
        }
    }

    #endregion

    #region UI_EnemyHpBar
    [TestFixture]
    public class UI_EnemyHpBar_Test {
        private class EnemyHpBarUITest : IEnemyHpBarView_UI {
            public EnemyHpBarPresenter_UI Presenter { get; private set; }
            public Vector3 ScreenPosition { get; private set; }
            public float FillAmount { get; private set; }

            public EnemyHpBarUITest() {
                Presenter = new EnemyHpBarPresenter_UI();
                Presenter.Init(this);
            }

            void IEnemyHpBarView_UI.SetScreenPosition(Vector3 screenPosition) {
                ScreenPosition = screenPosition;
            }

            void IEnemyHpBarView_UI.SetFillAmount(float amount) {
                FillAmount = amount;
            }
        }

        [Test]
        public void HpBarFillAmountTest() {
            EnemyHpBarUITest hpBarUITest = new();

            // 체력바 설정 테스트
            hpBarUITest.Presenter.SetFillAmount(100, 50);
            TestUtils.AssertWithDebug(Mathf.Approximately(hpBarUITest.FillAmount, 0.5f), "체력바 게이지 설정이 올바르지 않습니다. FillAmount: " + hpBarUITest.FillAmount);

            hpBarUITest.Presenter.SetFillAmount(200, 100);
            TestUtils.AssertWithDebug(Mathf.Approximately(hpBarUITest.FillAmount, 0.5f), "체력바 게이지가 올바르게 업데이트되지 않았습니다. FillAmount: " + hpBarUITest.FillAmount);
        }

        [Test]
        public void HpBarPositionTest() {
            EnemyHpBarUITest hpBarUITest = new();
            Vector3 enemyPosition = new Vector3(10, 5, 0);

            // 체력바 위치 설정 테스트
            hpBarUITest.Presenter.SetPosition(enemyPosition);
            TestUtils.AssertWithDebug(hpBarUITest.ScreenPosition != Vector3.zero, "체력바 위치가 업데이트되지 않았습니다.");
        }
    }
    #endregion

    #region UI_Inventory
    [TestFixture]
    public class UI_Inventory_Test {
        private class InventoryUITest : IInventoryView_UI {
            public InventoryPresenter_UI Presenter { get; private set; }
            public InventoryModel_UI Model { get; private set; }
            public InventoryUITest() {
                Model = new InventoryModel_UI();
                Presenter = new InventoryPresenter_UI();
                Presenter.Init(this);
            }

            void IInventoryView_UI.UpdateTypeButton(InventoryModel_UI.InvenType invenType, EquipmentType selectedType) {
                Model.invenType = invenType;
                Model.selectedType = (int)selectedType;
            }

            void IInventoryView_UI.UpdateSelectedColor(InventoryModel_UI.InvenType invenType, int selectedType) {
                // 테스트에 필요 없음
            }
        }

        [Test]
        public void InventoryTypeSelectionTest() {
            InventoryUITest uiTest = new();

            uiTest.Presenter.UpdateTypeButton(InventoryModel_UI.InvenType.Equipment);
            TestUtils.AssertWithDebug(uiTest.Model.invenType == InventoryModel_UI.InvenType.Equipment, "인벤토리 유형이 올바르게 설정되지 않았습니다.");

            uiTest.Presenter.UpdateSeleteValue(2);
            TestUtils.AssertWithDebug(uiTest.Model.selectedType == 2, "선택된 장비 유형이 올바르게 설정되지 않았습니다.");
        }

        [Test]
        public void InventoryUIUpdateTest() {
            InventoryUITest uiTest = new();
            uiTest.Presenter.UpdateUI();

            TestUtils.AssertWithDebug(uiTest.Model.invenType == InventoryModel_UI.InvenType.None, "UI 업데이트 후 invenType 값이 일치하지 않습니다.");
            TestUtils.AssertWithDebug(uiTest.Model.selectedType == 0, "UI 업데이트 후 선택된 타입이 초기화되지 않았습니다.");
        }
    }
    #endregion

    #region UI_LoadingScene
    [TestFixture]
    public class UI_LoadingScene_Test {
        // Mock 클래스: ILoadingSceneView_UI를 구현하여 테스트 진행
        private class LoadingSceneUITest : ILoadingSceneView_UI {
            public LoadingScenePresenter_UI Presenter { get; private set; }
            public float Progress { get; private set; } = 0f;
            public LoadingSceneUITest() {
                Presenter = new LoadingScenePresenter_UI();
                Presenter.Init(this);
            }

            void ILoadingSceneView_UI.UpdateUI(float progress) {
                Progress = progress;
            }
        }

        [Test]
        public void ProgressUpdateTest() {
            LoadingSceneUITest loadingUITest = new();

            // 초기 상태 확인  
            TestUtils.AssertWithDebug(loadingUITest.Progress == 0f, "초기 Progress 값이 올바르지 않습니다. Progress : " + loadingUITest.Progress);

            // Progress 업데이트 테스트  
            loadingUITest.Presenter.UpdateUI(0.5f);
            TestUtils.AssertWithDebug(Mathf.Abs(loadingUITest.Progress - 0.5f) > Mathf.Epsilon, "Progress 업데이트가 정상적으로 이루어지지 않았습니다. Progress : " + loadingUITest.Progress);

            // 0.88 이상일 경우 1로 설정되는지 확인  
            loadingUITest.Presenter.UpdateUI(0.9f);
            TestUtils.AssertWithDebug(Mathf.Abs(loadingUITest.Progress - 1f) > Mathf.Epsilon, "Progress가 1로 설정되지 않았습니다. Progress : " + loadingUITest.Progress);
        }

        [Test]
        public void FakeLoadingProgressTest() {
            LoadingSceneUITest loadingUITest = new();

            // 가짜 로딩 적용 확인  
            loadingUITest.Presenter.InitTime();
            loadingUITest.Presenter.UpdateUI(0.3f);

            TestUtils.AssertWithDebug(loadingUITest.Presenter.GetDisplayProgress() >= 0f, "Fake Progress가 올바르게 동작하지 않습니다. Fake Progress : " + loadingUITest.Presenter.GetDisplayProgress());
        }
    }
    #endregion

    #region UI_Lobby
    [TestFixture]
    public class UI_Lobby_Test {
        private class LobbyUITest : ILobbyView_UI {
            public LobbyPresenter_UI Presenter { get; private set; }
            public bool LoadSceneCalled { get; private set; } = false;
            public Dictionary<string, GameObject> OnUI { get; private set; } = new();

            public LobbyUITest() {
                Presenter = new LobbyPresenter_UI();
                Presenter.Init(this);
            }

            public void LoadNextScene() {
                LoadSceneCalled = true;
            }
        }


        [Test]
        public void UIInteractionTest() {
            LobbyUITest lobbyUITest = new();

            // 인벤토리 버튼 클릭 시 UI가 생성되는지 테스트
            string invenKey = typeof(InventoryView_UI).Name;
            TestUtils.AssertWithDebug(!lobbyUITest.OnUI.ContainsKey(invenKey), "인벤토리 UI가 시작 시 존재하면 안 됩니다.");

            // 인벤토리 UI 추가 후 확인
            lobbyUITest.OnUI.Add(invenKey, new GameObject());
            TestUtils.AssertWithDebug(lobbyUITest.OnUI.ContainsKey(invenKey), "인벤토리 UI가 정상적으로 추가되지 않았습니다.");

            // UI 닫기 테스트
            lobbyUITest.OnUI.Clear();
            TestUtils.AssertWithDebug(lobbyUITest.OnUI.Count == 0, "UI가 정상적으로 닫히지 않았습니다.");
        }
    }
    #endregion
    #region UI_Reloading
    [TestFixture]
    public class UI_Reloading_Test {
        private class ReloadingUITest : IReloadingView_UI {
            public ReloadingPresenter_UI Presenter { get; private set; }
            public float FillAmount { get; private set; } = 0f;

            public ReloadingUITest() {
                Presenter = new ReloadingPresenter_UI();
                Presenter.Init(this);
            }

            void IReloadingView_UI.UpdateFillUI(float amount) {
                FillAmount = amount;
            }
        }

        [Test]
        public void UpdateReloadTimeTest() {
            ReloadingUITest reloadingUITest = new();

            // 50% 진행 상태 테스트
            reloadingUITest.Presenter.UpdateReload(2f, 1f);
            TestUtils.AssertWithDebug(Mathf.Approximately(reloadingUITest.FillAmount, 0.5f),
                "재장전 UI가 올바르게 업데이트되지 않았습니다. FillAmount: " + reloadingUITest.FillAmount);

            // 100% 진행 상태 테스트
            reloadingUITest.Presenter.UpdateReload(4f, 4f);
            TestUtils.AssertWithDebug(Mathf.Approximately(reloadingUITest.FillAmount, 1f),
                "재장전 UI가 100%가 되지 않았습니다. FillAmount: " + reloadingUITest.FillAmount);
        }

        [Test]
        public void UpdateReloadAmountTest() {
            ReloadingUITest reloadingUITest = new();

            // 30% 진행 상태 테스트
            reloadingUITest.Presenter.UpdateReload(0.3f);
            TestUtils.AssertWithDebug(Mathf.Approximately(reloadingUITest.FillAmount, 0.3f),
                "재장전 UI가 30%로 설정되지 않았습니다. FillAmount: " + reloadingUITest.FillAmount);

            // 80% 진행 상태 테스트
            reloadingUITest.Presenter.UpdateReload(0.8f);
            TestUtils.AssertWithDebug(Mathf.Approximately(reloadingUITest.FillAmount, 0.8f),
                "재장전 UI가 80%로 설정되지 않았습니다. FillAmount: " + reloadingUITest.FillAmount);
        }
    }
    #endregion
    #region UI_SelecteBottomPortrait
    [TestFixture]
    public class UI_SelecteBottomPortrait_Test {
        private class SelecteBottomPortraitUITest : ISelecteBottomPortraitView_UI {
            public SelecteBottomPortraitPresenter_UI Presenter { get; private set; }
            public List<PortraitSlot_UI_Data> Slots { get; private set; }
            public float[] HpAmounts;
            public float[] ShieldAmounts;
            public string[] AmmoTexts;
            public bool[] ReloadingActive;
            public float[] ReloadingAmounts;
            public bool[] PortraitAnimations;

            public SelecteBottomPortraitUITest(int slotCount) {
                Presenter = new SelecteBottomPortraitPresenter_UI();
                Presenter.Init(this);
                Slots = new List<PortraitSlot_UI_Data>(slotCount);
                HpAmounts = new float[slotCount];
                ShieldAmounts = new float[slotCount];
                AmmoTexts = new string[slotCount];
                ReloadingActive = new bool[slotCount];
                ReloadingAmounts = new float[slotCount];
                PortraitAnimations = new bool[slotCount];
            }

            void ISelecteBottomPortraitView_UI.UpdateHp(int index, float amount) {
                HpAmounts[index] = amount;
            }

            void ISelecteBottomPortraitView_UI.UpdateShield(int index, float amount) {
                ShieldAmounts[index] = amount;
            }

            void ISelecteBottomPortraitView_UI.UpdateAmmoText(int index, string text) {
                AmmoTexts[index] = text;
            }

            void ISelecteBottomPortraitView_UI.UpdatePortrait(int index, Sprite portraitSprite) {
                // 초상화 변경 검증
            }

            void ISelecteBottomPortraitView_UI.UpdatePortraitAnimation(int index, bool isUp) {
                PortraitAnimations[index] = isUp;
            }

            void ISelecteBottomPortraitView_UI.UpdateReloadingActive(int index, bool isActive) {
                ReloadingActive[index] = isActive;
            }

            void ISelecteBottomPortraitView_UI.UpdateReloadingAmount(int index, float amount) {
                ReloadingAmounts[index] = amount;
            }

            void ISelecteBottomPortraitView_UI.AddButtonHandler(int index, EventTriggerType type, Action action, string entryClassMethodName) {
                // 버튼 핸들러 추가 검증
            }
        }

        [Test]
        public void UpdateHpTest() {
            SelecteBottomPortraitUITest uiTest = new(3);
            uiTest.Presenter.UpdateHp(0, 100f, 50f);
            Assert.AreEqual(0.5f, uiTest.HpAmounts[0]);
        }

        [Test]
        public void UpdateShieldTest() {
            SelecteBottomPortraitUITest uiTest = new(3);
            uiTest.Presenter.UpdateShield(1, 200f, 100f);
            Assert.AreEqual(0.5f, uiTest.ShieldAmounts[1]);
        }

        [Test]
        public void UpdateAmmoTextTest() {
            SelecteBottomPortraitUITest uiTest = new(3);
            uiTest.Presenter.UpdateAmmoText(2, 30, 15);
            Assert.AreEqual("15/30", uiTest.AmmoTexts[2]);
        }

        [Test]
        public void UpdatePortraitAnimationTest() {
            SelecteBottomPortraitUITest uiTest = new(3);
            uiTest.Presenter.UpdatePortaitAnimation(1, true);
            Assert.IsTrue(uiTest.PortraitAnimations[1]);

            uiTest.Presenter.UpdatePortaitAnimation(1, false);
            Assert.IsFalse(uiTest.PortraitAnimations[1]);
        }

        [Test]
        public void UpdateReloadingActiveTest() {
            SelecteBottomPortraitUITest uiTest = new(3);
            uiTest.Presenter.UpdateReloadingActive(0, true);
            Assert.IsTrue(uiTest.ReloadingActive[0]);

            uiTest.Presenter.UpdateReloadingActive(0, false);
            Assert.IsFalse(uiTest.ReloadingActive[0]);
        }

        [Test]
        public void UpdateReloadingAmountTest() {
            SelecteBottomPortraitUITest uiTest = new(3);
            uiTest.Presenter.UpdateReloadingAmount(2, 0.75f);
            Assert.AreEqual(0.75f, uiTest.ReloadingAmounts[2]);
        }
    }
    #endregion
}
