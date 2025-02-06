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
    /// ��ư �������� Inspector���� Ȯ���ϱ� ���� Ŭ����
    /// </summary>
    public class EditorButtonEntryPointChker : MonoBehaviour
    {
        [ReadOnlyAttribute][SerializeField] private List<EntryPointName> _entryPointName_list = new();

        [Serializable]
        public struct EntryPointName {
            public EventTriggerType triggerType;
            public string className;
            public string methodName;
        }

        public void AddEntry(string ownerClassMethodName, EventTriggerType type) {
            string[] strs = ownerClassMethodName.Split('.');
            // strs 0 className, 1 methodName
            _entryPointName_list.Add(new EntryPointName{triggerType = type, className = strs[0], methodName = strs[1] });
        }

    }

#endif
}
