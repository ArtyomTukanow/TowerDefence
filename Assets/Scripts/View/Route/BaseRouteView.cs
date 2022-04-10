using UnityEngine;
using View.Entity;
using View.Entity.Base;

namespace View.Route
{
    public abstract class BaseRouteView : EntityView
    {
        [SerializeField] protected SpawnerView spawner;
        [SerializeField] protected Transform destroyer;
        
        public abstract float RouteLength { get; }

        public abstract Vector3 GetPosByPercent(float percent);
    }
}