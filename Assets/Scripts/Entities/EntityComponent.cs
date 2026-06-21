using System;
using Entities.Interfaces;
using UnityEngine;

namespace Entities
{
    public class EntityComponent : MonoBehaviour
    {
        protected IEntity Owner { get; private set; }

        public void Initialize(IEntity owner)
        {
            Owner = owner;
        }

    }
}
