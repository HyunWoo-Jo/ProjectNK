using UnityEngine;
using N.Data;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.TextCore.Text;
namespace N.Game
{
    public enum CombatLogicClassName {
        StandardCombatLogic,

    }

    public abstract class CombatLogic : MonoBehaviour, ILogic {
        protected InGameData _gameData;
        protected int _curCharacterIndex = -1;
        public abstract void Work();
        public abstract void Instance();
        public virtual void ChangeSlot(int index) {
            _curCharacterIndex = index;
        }
        public void Init(InGameData data) {
            _gameData = data;
        }

        
    }

    public class StandardCombatLogic : CombatLogic {
        private List<Character> _character_list = new();
        public override void Instance() {
            foreach(var characterObj in _gameData.characterObj_list) {
                Character character = characterObj.GetComponent<Character>();
                character.CreateWeapon();
                character.CreateAI<CharacterStandardAI>();
                _character_list.Add(character);
            }
        }
        public override void Work() {
       
            Character character = _character_list[_curCharacterIndex];

            // Hide 상태 컨트롤
            if (_curCharacterIndex != _gameData.currentCharacterIndex) {
                _curCharacterIndex = _gameData.currentCharacterIndex;
                for(int i =0;i< _character_list.Count; i++) {
                    if (i == _curCharacterIndex) continue;
                    CharacterState state = _gameData.playState == PlayState.Hide ? CharacterState.Hide : CharacterState.AI;
                    _character_list[i].ChangeState(state);
                }       
            }
            // 마우스 클릭
            if (Input.GetMouseButtonDown(0) && !InputLogic.IsLockClick) {
                character.ChangeState(CharacterState.Standing);
            }
            // 마우스 UP
            if (Input.GetMouseButtonUp(0)) {
                character.ChangeState(CharacterState.Sitting);
            }
            // 모든 캐릭터 Work
            foreach (Character charac in _character_list) {
                charac.Work();
            }
        }
        /// <summary>
        /// Slot이 변경될때 호출
        /// </summary>
        /// <param name="index"></param>
        public override void ChangeSlot(int index) {
            if (_curCharacterIndex != -1) {
                _character_list[_curCharacterIndex].ChangeState(CharacterState.AI);
                _character_list[index].ChangeState(CharacterState.Sitting);
            }
            base.ChangeSlot(index);
        }


    }
}
