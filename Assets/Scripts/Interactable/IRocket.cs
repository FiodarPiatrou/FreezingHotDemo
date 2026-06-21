using System;

namespace Entities
{
    public interface IRocket
    {
        event Action OnLaunch;
    }
}