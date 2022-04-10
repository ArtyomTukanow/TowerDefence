using Core;
using UnityEngine;

namespace View.Entity.Base
{
    public abstract class EntityView : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            if(Game.World)
                Game.World.AddEntity(this);
        }

        private void OnDisable()
        {
            if(Game.World)
                Game.World.RemoveEntity(this);
        }
    }
}