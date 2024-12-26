using UnityEngine;
using N.Data;
namespace N.Game
{
    public static class CameraFuntion 
    {
        /// <summary>
        /// Camera Setting 속도로 target을 추적하는 기능
        /// </summary>
        /// <param name="cameraTr"></param>
        /// <param name="targetTr"></param>
        internal static void EaseInOutLerpTarget(Transform cameraTr, Transform targetTr) {
            float t = Settings.CameraLerpSpeed * Time.deltaTime; t = t * t * (3f - 2f * t); // Smoothstep 함수
            cameraTr.position = Vector3.Lerp(cameraTr.position, targetTr.position, t); 
        }
        /// <summary>
        /// Center를 look기준으로 Distance Proportional 이동
        /// </summary>
        /// <param name="cameraTr"> 회전을 적용할 타겟 </param>
        /// <param name="target"> 이동할 target</param>
        /// <param name="look"> 바라보는 곳</param>
        /// <param name="centerPos"> 기준 점</param>
        /// <param name="distance"> 이동할 거리 </param>
        /// <param name="offset">추가할 offset</param>
        internal static void DistanceProportional(Transform cameraTr, Transform target, Transform look, Vector3 centerPos, float distance, Vector3 offset) {
            Vector3 direction = (look.position - centerPos).normalized; // front 
            Vector3 pos = centerPos + (-direction * distance) + offset; // position

            Vector3 right = Vector3.Cross(Vector3.up, direction);
            Vector3 up = Vector3.Cross(direction, right);

            target.position = pos;
            cameraTr.rotation = Quaternion.LookRotation(direction, up);
        }
    }
}
