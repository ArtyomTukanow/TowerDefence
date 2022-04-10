using System;
using Core;
using UnityEngine;

namespace View.Route
{
    public class LineRouteView : BaseRouteView
    {
        private Vector3 startPos;
        private Vector3 endPos;
        private Vector3 deltaPos;
        private float routeLength;

        public override float RouteLength => routeLength;
        
        protected void Awake() => Recalculate();
        private void OnValidate() => Recalculate();

        private void Recalculate()
        {
            startPos = spawner.transform.position;
            endPos = destroyer.position;
            routeLength = (endPos - startPos).magnitude;
            
            deltaPos = endPos - startPos;
        }

        public override Vector3 GetPosByPercent(float percent) => startPos + deltaPos * percent;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPos.Set(y: 0), endPos.Set(y: 0));
        }
    }
}