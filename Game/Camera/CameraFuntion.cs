using UnityEngine;
using N.Data;
namespace N.Game
{
    public static class CameraFuntion 
    {
        /// <summary>
        /// Camera Setting �ӵ��� target�� �����ϴ� ���
        /// </summary>
        /// <param name="cameraTr"></param>
        /// <param name="targetTr"></param>
        internal static void EaseInOutLerpTarget(Transform cameraTr, Transform targetTr) {
            float t = Settings.CameraLerpSpeed * Time.deltaTime; t = t * t * (3f - 2f * t); // Smoothstep �Լ�
            cameraTr.position = Vector3.Lerp(cameraTr.position, targetTr.position, t); 
        }
        /// <summary>
        /// Center�� look�������� Distance Proportional �̵�
        /// </summary>
        /// <param name="cameraTr"> ȸ���� ������ Ÿ�� </param>
        /// <param name="target"> �̵��� target</param>
        /// <param name="look"> �ٶ󺸴� ��</param>
        /// <param name="centerPos"> ���� ��</param>
        /// <param name="distance"> �̵��� �Ÿ� </param>
        /// <param name="offset">�߰��� offset</param>
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
