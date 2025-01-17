using UnityEngine;
using N.Data;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UIElements;
namespace N.Game
{
    public enum CombatLogicClassName {
        StandardCombatLogic,

    }

    public abstract class CombatLogic : MonoBehaviour
    {
        protected InGameData _gameData;
        public abstract void WorkCombat();
        public abstract void InitData();
        public void Init(InGameData data) {
            _gameData = data;
        }
    }

    public class StandardCombatLogic : CombatLogic {
        private List<Character> _character_list = new();
        private int _curCharacterIndex = -1;
        public override void InitData() {
            foreach(var characterObj in _gameData.characterObj_list) {
                Character character = characterObj.GetComponent<Character>();
                character.CreateWeapon();
                _character_list.Add(character);
            }
        }

        public override void WorkCombat() {
            if(_curCharacterIndex != _gameData.currentCharacterIndex) {
                _curCharacterIndex = _gameData.currentCharacterIndex;
                for(int i =0;i< _character_list.Count; i++) {
                    if (i == _curCharacterIndex) continue;
                    CharacterState state = _gameData.playState == PlayState.Hide ? CharacterState.Hide : CharacterState.AI;
                    _character_list[i].ChangeState(state);
                }
                // 캐릭터가 변경되면 ui 업데이트
                Character character = _character_list[_curCharacterIndex];
                _gameData.aimView.ReloadAmmo(character.GetMaxAmmo, character.GetAmmo);
            }
            if (Input.GetMouseButtonDown(0)) {
                Character character = _character_list[_curCharacterIndex];
                character.ChangeState(CharacterState.Standing);
            }
            if (Input.GetMouseButtonUp(0)) {
                Character character = _character_list[_curCharacterIndex];
                character.ChangeState(CharacterState.Sitting);
            }
            foreach (Character character in _character_list) {
                character.Work();
            }
        }

        
    }
}
