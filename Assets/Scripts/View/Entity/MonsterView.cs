using UnityEngine;
using View.Entity.Base;
using View.Route;

namespace View.Entity
{
    public class MonsterView : EntityView, IRouteOnPathPredictor
    {
        [SerializeField] private float m_speed;
        public float Speed => m_speed;
        
        [SerializeField] private int m_hp;
        public int Hp
        {
            get => m_hp;
            set
            {
                m_hp = value;
                CheckByHp();
            }
        }
        
        public float CurrentPosition { get; private set; } = 0f;
        public float PercentPerSecond => Speed / Route.RouteLength;
        
        public bool IsOnEndOfPath => CurrentPosition >= 1;

        public BaseRouteView Route { get; private set; }

        public void Init(BaseRouteView route)
        {
            Route = route;
            CurrentPosition = default;
            UpdateTransformPosition();
        }

        public void AddPosition(float prc)
        {
            CurrentPosition += Mathf.Min(Mathf.Max(prc, 0), 1);
            UpdateTransformPosition();
        }

        public void SetPosition(float prc)
        {
            CurrentPosition = Mathf.Min(Mathf.Max(prc, 0), 1);
            UpdateTransformPosition();
        }

        private void UpdateTransformPosition()
        {
            transform.position = Route.GetPosByPercent(CurrentPosition);
        }

        public float GetMoveTimeToPoint(BaseRouteView route, float prc)
        {
            var deltaPrc = prc - CurrentPosition;
            return deltaPrc / PercentPerSecond;
        }

        private void CheckByHp()
        {
            if (Hp <= 0)
                Destroy(gameObject);
        }

        private void OnValidate()
        {
            CheckByHp();
        }
    }
}