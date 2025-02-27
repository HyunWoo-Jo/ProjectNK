using UnityEngine;
using N.Data;
using N.DesignPattern;
using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace N.UI
{
    /// <summary>
    /// UI 오브젝트를 생성하고 초기화 하며 저장함
    /// </summary>
    public class UI_Controller : Singleton<UI_Controller>
    {
        private Canvas _mainCanvas;
        [SerializeField] private GameObject _parentCanvas_prefab;
        private List<GameObject> instantCanvas_list = new();
        private Dictionary<string, string> _key_dic = new();
        
        protected override void Awake() {
            base.Awake();
#if UNITY_EDITOR
            Assert.IsNotNull(_parentCanvas_prefab);
#endif
            if (_key_dic.Count == 0) AddKey();
            FindMainCanvas();
        }
        
        /// <summary>
        /// Main Canvas 검색
        /// </summary>
        private void FindMainCanvas() {
            GameObject mainCanvasObj = GameObject.Find("MainCanvas");
            _mainCanvas =  mainCanvasObj.GetComponent<Canvas>();
        }

        public GameObject InstantiateParentCanvas(int order) {
            if (_mainCanvas == null) FindMainCanvas();
            GameObject canvasObj = GameObject.Instantiate(_parentCanvas_prefab);
            canvasObj.transform.SetParent(_mainCanvas.transform);
            instantCanvas_list.Add(canvasObj);
            Canvas canvas = canvasObj.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = order;
            // anchor 초기화
            RectTransform rt = canvasObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(1, 1);
            return canvasObj;
        }
        public void RemoveParentCanvas(GameObject canvas) {
            instantCanvas_list.Remove(canvas);
            Destroy(canvas);
        }
        public void RemoveUI(string typeName, GameObject obj){
            Destroy(obj);
            // Key 할당
            string key = GetKey(typeName);
            DataManager.Instance.ReleaseAsset(key);
        }

        public View InstantiateUI<View>(int order, bool isActive = true) where View : MonoBehaviour{
            if (_mainCanvas == null) FindMainCanvas();
            string typeName = typeof(View).Name;
            // Key 할당
            if (_key_dic.Count == 0) AddKey();
            string key = GetKey(typeName);
            if (!string.IsNullOrEmpty(key)) { // key가 있으면 생성 할당
                View view = InstanceUI<View>(key, isActive, order);
                return view;
            }
            return null;
        }
      
        private View InstanceUI<View>(string key, bool isAtive, int order) {
            GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>(key);
            GameObject uiObj = Instantiate(prefab);
            uiObj.transform.SetParent(_mainCanvas.transform);
            uiObj.transform.localPosition = Vector3.zero; 
            uiObj.transform.localScale = Vector3.one;
            uiObj.gameObject.SetActive(isAtive);
            // anchor 변경
            RectTransform rt = uiObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(1, 1);
            Canvas canvas = uiObj.GetComponent<Canvas>();
            
            if (canvas != null) {
                canvas.overrideSorting = true;
                canvas.sortingOrder = order;
            }
            return uiObj.GetComponent<View>();
        }
        

        private string GetKey(string typeName) {
            _key_dic.TryGetValue(typeName, out string key);
            return key;
        }
       

        private void AddKey() {
            _key_dic.Add(typeof(AimView_UI).Name, "StandardAim_UI.prefab");
            _key_dic.Add(typeof(ReloadingView_UI).Name, "Reload_UI.prefab");
            _key_dic.Add(typeof(SelecteBottomPortraitView_UI).Name, "SelectBottomPortrait_UI.prefab");
            _key_dic.Add(typeof(EnemyHpBarView_UI).Name, "EnemyHPBar_UI.prefab");
            _key_dic.Add(typeof(AutoButtonView_UI).Name, "AutoButton_UI.prefab");
            _key_dic.Add(typeof(InventoryView_UI).Name, "Inventory_UI.prefab");
        }




    }
}
