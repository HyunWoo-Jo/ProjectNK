using UnityEngine;
using N.Data;
using N.DesignPattern;
namespace N.Game
{
    public enum CharacterState {
        Standing,
        Sitting,
        AI,
    }
    public class Character : MonoBehaviour
    {
        private CharacterState _state;
        private string _name;
        private CharacterAI _ai;
        private Weapon _weapon;

        private GameObject _model;
        private Sprite _portrait;
        

        internal void Work() {
            if (_state.Equals(CharacterState.Standing)) {
                _weapon.Shot();
            } else if (_state.Equals(CharacterState.Sitting)) {
                _weapon.Reloading();
            } else if (_state.Equals(CharacterState.AI)) {
                _ai.Work();
            }

        }
    }
}
