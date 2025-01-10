using UnityEngine;

namespace N.Game
{
    public abstract class CharacterAI
    {
        private Character _owner;
        internal void Init(Character owner) {
            _owner = owner;
        }

        internal void Work() {

        }
    }
}
