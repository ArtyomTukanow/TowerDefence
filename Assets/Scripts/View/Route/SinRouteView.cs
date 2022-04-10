using Core;
using UnityEngine;

namespace View.Route
{
    public class SinRouteView : BaseRouteView
    {
        private Vector3 startPos;
        private Vector3 endPos;
        private Vector3 deltaPos;
        private Vector3 perpendicular;
        private float routeLength;
        
        [SerializeField] private float amplitude = 1;
        [SerializeField] private int periods = 10;

        public override float RouteLength => routeLength;

        protected void Awake() => Recalculate();
        private void OnValidate() => Recalculate();

        private void Recalculate()
        {
            startPos = spawner.transform.position;
            endPos = destroyer.position;
            
            deltaPos = endPos - startPos;

            perpendicular = Vector3.Cross(deltaPos, Vector3.up).normalized * amplitude;
            
            //todo длина синусоиды вычисляется иначе. Но грубым образом можно так.
            routeLength = (endPos - startPos).magnitude + Mathf.Abs(amplitude * periods) * 2;
        }

        public override Vector3 GetPosByPercent(float percent)
        {
            return startPos + deltaPos * percent + perpendicular * Mathf.Sin(percent * periods * Mathf.PI);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var wasPos = GetPosByPercent(0).Set(y: 0);
            for (var i = 2; i <= 100; i ++)
            {
                var newPos = GetPosByPercent((float) i / 100).Set(y: 0);
                Gizmos.DrawLine(wasPos, newPos);
                wasPos = newPos;
            }
        }
    }
}