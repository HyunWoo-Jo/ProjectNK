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
    public class UI_Controller : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private GameObject _parentCanvas_prefab;
        private List<GameObject> instantCanvas_list = new();
        private Dictionary<string, object> _ui_dic = new();
        private Dictionary<string, string> _key_dic = new();
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_mainCanvas);
            Assert.IsNotNull(_parentCanvas_prefab);
#endif
            AddKey();

        }

        public GameObject InstantiateParentCanvas(int order) {
            GameObject canvasObj = GameObject.Instantiate(_parentCanvas_prefab);
            canvasObj.transform.SetParent(_mainCanvas.transform);
            instantCanvas_list.Add(canvasObj);
            Canvas canvas = canvasObj.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = order;

            return canvasObj;
        }
        public void RemoveParentCanvas(GameObject canvas) {
            instantCanvas_list.Remove(canvas);
            Destroy(canvas);
        }

        public View InstantiateUI<View>(int order, bool isActive = true) where View : MonoBehaviour{
            string typeName = typeof(View).Name;
            // Key 할당
            string key = GetKey(typeName);
            if (key != string.Empty) { // key가 있으면 생성 할당
                View view = InstanceUI<View>(key, isActive, order);
                _ui_dic.Add(typeName, view);
                return view;
            }
            return null;
        }

        public View GetUIView<View>() where View : MonoBehaviour{
            string typeName = typeof(View).Name;
            return _ui_dic[typeName] as View;

        }
      
        private View InstanceUI<View>(string key, bool isAtive, int order) {
            GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>(key);
            GameObject uiObj = Instantiate(prefab);
            uiObj.transform.SetParent(_mainCanvas.transform);
            uiObj.transform.localPosition = Vector3.zero; 
            uiObj.transform.localScale = Vector3.one;
            uiObj.gameObject.SetActive(isAtive);
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
            _key_dic.Add("AimView_UI", "StandardAim_UI.prefab");
            _key_dic.Add("ReloadingView_UI", "Reload_UI.prefab");
            _key_dic.Add("SelecteBottomPortraitView_UI", "SelectBottomPortrait_UI.prefab");
            _key_dic.Add("EnemyHpBarView_UI", "EnemyHPBar_UI.prefab");
        }

        


    }
}
