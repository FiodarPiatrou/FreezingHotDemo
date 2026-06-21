using Entities.Interfaces;
using UnityEngine;

namespace Entities
{
    public abstract class EntityDeath : EntityComponent
    {
        private void Start()
        {
            Owner.GetEntityComponent<IEntityHealth>().OnDeath += Death;
        }

        protected abstract void Death();
      

    }
}
