using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEditor.VersionControl;
namespace N.Utills
{
    public static class AsyncUtills
    {
        public static async UniTask WaitIsNull(object obj) {
            while (obj == null) {
                await UniTask.Delay(10);
            }
        }
    }
}
