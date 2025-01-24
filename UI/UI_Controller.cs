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
        private Dictionary<string, object> _ui_dic = new();

        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_mainCanvas);
#endif
        }

        public View InstantiateUI<View>(bool isActive = true) where View : MonoBehaviour{
            string typeName = typeof(View).Name;
            // Key 할당
            string key = "";
            switch (typeName) {
                case "AimView_UI":
                key = "StandardAim_UI.prefab";
                break;
                case "ReloadingView_UI":
                key = "Reload_UI.prefab";
                break;
                case "SelecteBottomPortraitView_UI":
                key = "SelectBottomPortrait_UI.prefab";
                break;
            }
            if (key != string.Empty) { // key가 있으면 생성 할당
                View view = InstanceUI<View>(key, isActive);
                _ui_dic.Add(typeName, view);
                return view;
            }
            return null;
        }

        public View GetUIView<View>() where View : MonoBehaviour{
            string typeName = typeof(View).Name;
            return _ui_dic[typeName] as View;

        }
      
        private View InstanceUI<View>(string key, bool isAtive) {
            GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>(key);
            GameObject uiObj = Instantiate(prefab);
            uiObj.transform.SetParent(_mainCanvas.transform);
            uiObj.transform.localPosition = Vector3.zero; 
            uiObj.transform.localScale = Vector3.one;
            uiObj.gameObject.SetActive(isAtive);
            return uiObj.GetComponent<View>();
        }



    }
}
