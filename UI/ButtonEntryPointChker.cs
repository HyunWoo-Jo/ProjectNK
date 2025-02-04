using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using N.Utills;
namespace N.UI
{
#if UNITY_EDITOR
    /// <summary>
    /// 버튼 진입점을 Inspector에서 확인하기 위한 클래스
    /// </summary>
    public class ButtonEntryPointChker : MonoBehaviour
    {
        [ReadOnlyAttribute][SerializeField] private List<EntryPointName> entryPointName_list = new List<EntryPointName>();

        [Serializable]
        public struct EntryPointName {
            public EventTriggerType triggerType;
            public string className;
            public string methodName;
        }

        public void AddEntry(string methodName, EventTriggerType type) {
            string[] strs = methodName.Split('.');
            // strs 0 className, 1 methodName
            entryPointName_list.Add(new EntryPointName{triggerType = type,className = strs[0], methodName = strs[1] });
        }

    }

#endif
}
