
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
        // Aim UI �׽�Ʈ�� ���� Mock Ŭ����
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
                // �׽�Ʈ���� �ʿ����� ����
            }
        }

        [Test]
        public void AimMovementTest() {
            AimUITest aimUITest = new();

            // ȭ�� ũ�� ���� �׽�Ʈ
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

            // ź�� ���� �׽�Ʈ
            aimUITest.Presenter.SetAmmo(10, 5);
            TestUtils.AssertWithDebug(aimUITest.Presenter.AmmoCount == 5, "���� ź�� ���� �ùٸ��� �ʽ��ϴ�. Ammo : " + aimUITest.Presenter.AmmoCount);

            // �ִ� ź�� �� ���� �׽�Ʈ
            aimUITest.Presenter.SetAmmo(20, 15);
            TestUtils.AssertWithDebug(aimUITest.Presenter.AmmoCount == 15, "ź�� ���� �ùٸ��� ������Ʈ���� �ʾҽ��ϴ�. Ammo : " + aimUITest.Presenter.AmmoCount);
        }

        [Test]
        public void ScreenBoundaryTest() {
            AimUITest aimUITest = new();

            // ȭ�� ũ�� ���� �׽�Ʈ
            aimUITest.Presenter.SetScreenSize(new Vector2(500, 500));
            TestUtils.AssertWithDebug(aimUITest.Presenter.ScreenSize == new Vector2(500, 500), "ȭ�� ũ�� ������ �ùٸ��� �ʽ��ϴ�.");

            // ���� ���� �׽�Ʈ
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
            TestUtils.AssertWithDebug(aimUITest.Position.y <= 290, "ȭ�� ��� ������ �ʰ��߽��ϴ�. y : " + aimUITest.Position.y);
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
                // event ȣ��
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                entry.callback.Invoke(eventData);
            }
        }

        [Test]
        public void ButtonToggleTest() {
            AutoButtonUITest buttonUITest = new();
            bool actionCalled = false;

            // ��ư �ʱ�ȭ
            buttonUITest.Presenter.InitButton(() => actionCalled = true, false);
            TestUtils.AssertWithDebug(buttonUITest.IsDown == false, "��ư �ʱ� ���°� �ùٸ��� �ʽ��ϴ�.");

            // ��ư Ŭ�� �� ���� Ȯ��
            buttonUITest.Presenter.InitButton(() => actionCalled = true, true);
            TestUtils.AssertWithDebug(actionCalled, "��ư ������ ȣ����� �ʾҽ��ϴ�.");
            TestUtils.AssertWithDebug(buttonUITest.IsDown == true, "��ư ���°� �ùٸ��� ������� �ʾҽ��ϴ�.");
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

            // ü�¹� ���� �׽�Ʈ
            hpBarUITest.Presenter.SetFillAmount(100, 50);
            TestUtils.AssertWithDebug(Mathf.Approximately(hpBarUITest.FillAmount, 0.5f), "ü�¹� ������ ������ �ùٸ��� �ʽ��ϴ�. FillAmount: " + hpBarUITest.FillAmount);

            hpBarUITest.Presenter.SetFillAmount(200, 100);
            TestUtils.AssertWithDebug(Mathf.Approximately(hpBarUITest.FillAmount, 0.5f), "ü�¹� �������� �ùٸ��� ������Ʈ���� �ʾҽ��ϴ�. FillAmount: " + hpBarUITest.FillAmount);
        }

        [Test]
        public void HpBarPositionTest() {
            EnemyHpBarUITest hpBarUITest = new();
            Vector3 enemyPosition = new Vector3(10, 5, 0);

            // ü�¹� ��ġ ���� �׽�Ʈ
            hpBarUITest.Presenter.SetPosition(enemyPosition);
            TestUtils.AssertWithDebug(hpBarUITest.ScreenPosition != Vector3.zero, "ü�¹� ��ġ�� ������Ʈ���� �ʾҽ��ϴ�.");
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
                // �׽�Ʈ�� �ʿ� ����
            }
        }

        [Test]
        public void InventoryTypeSelectionTest() {
            InventoryUITest uiTest = new();

            uiTest.Presenter.UpdateTypeButton(InventoryModel_UI.InvenType.Equipment);
            TestUtils.AssertWithDebug(uiTest.Model.invenType == InventoryModel_UI.InvenType.Equipment, "�κ��丮 ������ �ùٸ��� �������� �ʾҽ��ϴ�.");

            uiTest.Presenter.UpdateSeleteValue(2);
            TestUtils.AssertWithDebug(uiTest.Model.selectedType == 2, "���õ� ��� ������ �ùٸ��� �������� �ʾҽ��ϴ�.");
        }

        [Test]
        public void InventoryUIUpdateTest() {
            InventoryUITest uiTest = new();
            uiTest.Presenter.UpdateUI();

            TestUtils.AssertWithDebug(uiTest.Model.invenType == InventoryModel_UI.InvenType.None, "UI ������Ʈ �� invenType ���� ��ġ���� �ʽ��ϴ�.");
            TestUtils.AssertWithDebug(uiTest.Model.selectedType == 0, "UI ������Ʈ �� ���õ� Ÿ���� �ʱ�ȭ���� �ʾҽ��ϴ�.");
        }
    }
    #endregion

    #region UI_LoadingScene
    [TestFixture]
    public class UI_LoadingScene_Test {
        // Mock Ŭ����: ILoadingSceneView_UI�� �����Ͽ� �׽�Ʈ ����
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

            // �ʱ� ���� Ȯ��  
            TestUtils.AssertWithDebug(loadingUITest.Progress == 0f, "�ʱ� Progress ���� �ùٸ��� �ʽ��ϴ�. Progress : " + loadingUITest.Progress);

            // Progress ������Ʈ �׽�Ʈ  
            loadingUITest.Presenter.UpdateUI(0.5f);
            TestUtils.AssertWithDebug(Mathf.Abs(loadingUITest.Progress - 0.5f) > Mathf.Epsilon, "Progress ������Ʈ�� ���������� �̷������ �ʾҽ��ϴ�. Progress : " + loadingUITest.Progress);

            // 0.88 �̻��� ��� 1�� �����Ǵ��� Ȯ��  
            loadingUITest.Presenter.UpdateUI(0.9f);
            TestUtils.AssertWithDebug(Mathf.Abs(loadingUITest.Progress - 1f) > Mathf.Epsilon, "Progress�� 1�� �������� �ʾҽ��ϴ�. Progress : " + loadingUITest.Progress);
        }

        [Test]
        public void FakeLoadingProgressTest() {
            LoadingSceneUITest loadingUITest = new();

            // ��¥ �ε� ���� Ȯ��  
            loadingUITest.Presenter.InitTime();
            loadingUITest.Presenter.UpdateUI(0.3f);

            TestUtils.AssertWithDebug(loadingUITest.Presenter.GetDisplayProgress() >= 0f, "Fake Progress�� �ùٸ��� �������� �ʽ��ϴ�. Fake Progress : " + loadingUITest.Presenter.GetDisplayProgress());
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

            // �κ��丮 ��ư Ŭ�� �� UI�� �����Ǵ��� �׽�Ʈ
            string invenKey = typeof(InventoryView_UI).Name;
            TestUtils.AssertWithDebug(!lobbyUITest.OnUI.ContainsKey(invenKey), "�κ��丮 UI�� ���� �� �����ϸ� �� �˴ϴ�.");

            // �κ��丮 UI �߰� �� Ȯ��
            lobbyUITest.OnUI.Add(invenKey, new GameObject());
            TestUtils.AssertWithDebug(lobbyUITest.OnUI.ContainsKey(invenKey), "�κ��丮 UI�� ���������� �߰����� �ʾҽ��ϴ�.");

            // UI �ݱ� �׽�Ʈ
            lobbyUITest.OnUI.Clear();
            TestUtils.AssertWithDebug(lobbyUITest.OnUI.Count == 0, "UI�� ���������� ������ �ʾҽ��ϴ�.");
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

            // 50% ���� ���� �׽�Ʈ
            reloadingUITest.Presenter.UpdateReload(2f, 1f);
            TestUtils.AssertWithDebug(Mathf.Approximately(reloadingUITest.FillAmount, 0.5f),
                "������ UI�� �ùٸ��� ������Ʈ���� �ʾҽ��ϴ�. FillAmount: " + reloadingUITest.FillAmount);

            // 100% ���� ���� �׽�Ʈ
            reloadingUITest.Presenter.UpdateReload(4f, 4f);
            TestUtils.AssertWithDebug(Mathf.Approximately(reloadingUITest.FillAmount, 1f),
                "������ UI�� 100%�� ���� �ʾҽ��ϴ�. FillAmount: " + reloadingUITest.FillAmount);
        }

        [Test]
        public void UpdateReloadAmountTest() {
            ReloadingUITest reloadingUITest = new();

            // 30% ���� ���� �׽�Ʈ
            reloadingUITest.Presenter.UpdateReload(0.3f);
            TestUtils.AssertWithDebug(Mathf.Approximately(reloadingUITest.FillAmount, 0.3f),
                "������ UI�� 30%�� �������� �ʾҽ��ϴ�. FillAmount: " + reloadingUITest.FillAmount);

            // 80% ���� ���� �׽�Ʈ
            reloadingUITest.Presenter.UpdateReload(0.8f);
            TestUtils.AssertWithDebug(Mathf.Approximately(reloadingUITest.FillAmount, 0.8f),
                "������ UI�� 80%�� �������� �ʾҽ��ϴ�. FillAmount: " + reloadingUITest.FillAmount);
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
                // �ʻ�ȭ ���� ����
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
                // ��ư �ڵ鷯 �߰� ����
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
