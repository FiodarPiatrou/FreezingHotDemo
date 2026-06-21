using System;

namespace Managers.Interface
{
    public interface ITimer
    {
        public float TimeRemaining { get;  }
        
        public event Action OnTimeChanged;
    }
}