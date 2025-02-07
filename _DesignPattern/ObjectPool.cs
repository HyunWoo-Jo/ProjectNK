using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace N.DesignPattern
{
    public interface IObjectPool {
        void RepayItem(GameObject item, int index);

    }
    public class ObjectPool<T> : IObjectPool where T : MonoBehaviour
    {
        private GameObject _ownerObj;
        private GameObject _itemObj;
        private Queue<T> _item_que;
        private List<T> index_T_list;
        int index = 0;

        private ObjectPool() {
        }

        public void Dipose() {
            _ownerObj = null;
            _itemObj = null;
            while (_item_que.Count > 0) {
                T item = _item_que.Dequeue();
                GameObject.Destroy(item.gameObject);
            }
        }
        public static ObjectPool<T> Instance(GameObject itemObj, int capacity = 10) {
            GameObject parentObj = new() {
                isStatic = true,
                name = itemObj.name + "_objectPool"
            };
            ObjectPool<T> pool = new ObjectPool<T>();
            pool._ownerObj = parentObj;
            pool._itemObj = itemObj;
            pool._item_que = new Queue<T>();
            pool.index_T_list = new List<T>();
            Enumerable.Range(0, capacity).ToList().ForEach(_ => pool.CreateItem());
            return pool;
        }

        private void CreateItem() {
            GameObject obj = GameObject.Instantiate(_itemObj);
            T t = obj.GetComponent<T>();
            obj.AddComponent<ObjectPoolItem>().Init(this, index++);
            _item_que.Enqueue(t);
            index_T_list.Add(t);
            obj.SetActive(false);
            obj.transform.SetParent(_ownerObj.transform);
        }

        public T BorrowItem() {
            
            if (_item_que.Count <= 0) {
                CreateItem();   
            }
            return _item_que.Dequeue();
        }

        public void RepayItem(GameObject item, int index) {
            item.gameObject.transform.SetParent(_ownerObj.transform);
            item.gameObject.SetActive(false);
            _item_que.Enqueue(index_T_list[index]);
        }

    }
}
