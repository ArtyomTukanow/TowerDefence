using Systems.Base;
using UnityEngine;
using View.Route;

namespace Systems
{
    public class PredictorSystem : ISystem
    {
        private const int MAX_ITERATIONS = 8;
        private const float DELTA_TIME = 0.1f;
        
        public float? PredicateCrossingByBisectionMethod(BaseRouteView route, IRouteOnPathPredictor monster, IRoutePredictor tower)
        {
            var startPoint = monster.CurrentPosition;
            var endPoint = 1f;


            if (!IsPointsValid(startPoint, endPoint))
                return null;

            return NextIteration(startPoint, endPoint);

            float NextIteration(float start, float end, int iteration = 0)
            {
                var middle = start + (end - start) / 2;
                
                if (iteration > MAX_ITERATIONS)
                    return middle;
                
                iteration++;
                
                var fMiddle = GetDeltaTime(middle);

                if (Mathf.Abs(fMiddle) < DELTA_TIME)
                    return middle;
                
                if(fMiddle < 0)
                    return NextIteration(middle, end, iteration + 1);
                else
                    return NextIteration(start, middle, iteration + 1);
            }

            bool IsPointsValid(float startPoint, float endPoint) => GetDeltaTime(startPoint) <= 0 && GetDeltaTime(endPoint) >= 0;
            float GetDeltaTime(float prc) => monster.GetMoveTimeToPoint(route, prc) - tower.GetMoveTimeToPoint(route, prc);
        }
    }
}