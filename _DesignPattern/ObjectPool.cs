using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace N.DesignPattern
{
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {

        private GameObject _itemObj;
        private Queue<T> _item_que;

        public static ObjectPool<T> Instance(GameObject itemObj, int capacity = 10) {
            GameObject parentObj = new() {
                isStatic = true,
                name = itemObj.name + "_objectPool"
            };
            ObjectPool<T> t = parentObj.AddComponent<ObjectPool<T>>();
          
            t._itemObj = itemObj;
            t._item_que = new Queue<T>();
            Enumerable.Range(0, capacity).ToList().ForEach(_ => t.CreateItem());
            return t;
        }

        private void CreateItem() {
            GameObject obj = Instantiate(_itemObj);
            T t = obj.GetComponent<T>();
            obj.AddComponent<ObjectPoolItem<T>>().Init(this, t);
            _item_que.Enqueue(t);
        }

        public T BorrowItem() {
            
            if (_item_que.Count <= 0) {
                CreateItem();   
            }
            return _item_que.Dequeue();
        }

        public void RepayItem(T item) {
            item.gameObject.transform.SetParent(this.transform);
            item.gameObject.SetActive(false);
            _item_que.Enqueue(item);
        }

    }
}
