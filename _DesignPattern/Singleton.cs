using UnityEngine;

namespace N.DesignPattern
{
    [DefaultExecutionOrder(-100)] 
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance { get { return _instance; } }

        protected virtual void Awake() {
            if(_instance == null) {
                _instance = this.GetComponent<T>();
                DontDestroyOnLoad(this);
            } else {
                Destroy(this.gameObject);
            }
        }

    }
}
