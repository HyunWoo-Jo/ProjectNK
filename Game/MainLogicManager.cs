using UnityEngine;
using N.DesignPattern;
using System;
using System.Collections.Generic;
using N.Data;
namespace N.Game
{
    /// <summary>
    /// 메인 로직 셋팅을 넘겨주는 역할을 진행
    /// </summary>
    public class MainLogicManager : Singleton<MainLogicManager> {
        public EnemySpawnDataList spawnDataList;
        public CameraLogicClassName cameraLogicClassName = CameraLogicClassName.StandardGameCameraLogic;
        public CombatLogicClassName combatLogicClassName = CombatLogicClassName.StandardCombatLogic;
        public EnemyLogicClassName enemyLogicClassName = EnemyLogicClassName.StandardEnemyLogic;
        public List<InputLogicClassName> inputLogicClassName_list = new List<InputLogicClassName> { InputLogicClassName.InputCombatAimLogic };
        
        public List<string> characterName_list = new List<string> { "Lux", "Nami", "Nunu", "Ryze" };
        public PlayMainLogic curPlayMainLogic;
       
        protected override void Awake() {
            base.Awake();
            Application.targetFrameRate = Settings.targetFrame;
        }
        /// <summary>
        /// 함수명,클래스명으로 함수 호출
        /// </summary>
        /// <param name="mainLogic"></param>
        /// <param name="methodName"></param>
        /// <param name="className"></param>
        private void InvokeGenericMethod(PlayMainLogic mainLogic, string methodName, string className) {
            Type type = Type.GetType($"N.Game.{className}");
            var setCameraMethod = mainLogic.GetType().GetMethod(methodName).MakeGenericMethod(type);
            setCameraMethod.Invoke(mainLogic, null);
        }
        /// <summary>
        /// playlogic 초기화 함수 play씬에서 PlayMainLogic.cs 생성될떄 호출
        /// </summary>
        /// <param name="mainLogic"></param>
        internal void SendModules(PlayMainLogic mainLogic) {
            InvokeGenericMethod(mainLogic, "SetCamera", cameraLogicClassName.ToString());

            foreach(var inputClassName in inputLogicClassName_list) {
                InvokeGenericMethod(mainLogic, "SetInput", inputClassName.ToString());
            }

            InvokeGenericMethod(mainLogic, "SetCombat", combatLogicClassName.ToString());
            InvokeGenericMethod(mainLogic, "SetEnemy", enemyLogicClassName.ToString());

            mainLogic.SetCharacter(characterName_list);
        }


       
    }
}
