using System;
using Core;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using View.Entity.Base;
using View.Route;

namespace View.Entity
{
    public class TowerView : EntityView, IRoutePredictor
    {
        [NonSerialized] public Vector3 GizmosPos;
        
        [SerializeField] private float m_range;
        public float Range => m_range;
        
        [SerializeField] private float m_rotateSpeed;
        public float RotateSpeed => m_rotateSpeed;
        
        [SerializeField] private float m_shootInterval;
        public float ShootInterval => m_shootInterval;
        
        [SerializeField] protected ProjectileView m_projectile;
        public ProjectileView Projectile => m_projectile;
        
        [SerializeField] protected Transform m_shootPoint;
        public Vector3 StartShootPoint => m_shootPoint.position;
        
        [SerializeField] protected int m_damage;
        public int Damage => m_damage;
        
        [SerializeField] protected bool m_followingProjectile;
        public bool FollowingProjectile => m_followingProjectile;
        
        private float m_lastShotTime = -1;
        
        
        public float Speed => m_projectile.Speed;
        public bool CanShoot => m_lastShotTime < 0 || m_lastShotTime + ShootInterval < Time.time;

        
        public void SetShootDelay(float delay)
        {
            m_lastShotTime = Time.time + delay;
        }

        public void RotateTo(float rotateY, float time, Action onComplete)
        {
            transform.DORotate(new Vector3(0, rotateY, 0), time)
                .SetEase(Ease.OutBack)
                .OnComplete(onComplete.Invoke);
        }

        public float GetMoveTimeToPoint(BaseRouteView route, float prc)
        {
            var toPos = route.GetPosByPercent(prc);
            return GetRotateData(toPos).time + GetShootMoveTime(toPos);
        }

        public (float time, float rotate) GetRotateData(Vector3 toPos)
        {
            var direction = toPos - transform.position;
            
            var fromRotate = transform.rotation.eulerAngles.y;
            var toRotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            var needRotate = (toRotate - fromRotate).NearestRotate();
            
            return (Mathf.Ceil(Mathf.Abs(needRotate) / RotateSpeed) * Time.fixedDeltaTime, toRotate);
        }
        
        public float GetShootMoveTime(Vector3 toPos)
        {
            return (StartShootPoint - toPos).Set(y: 0).magnitude / m_projectile.Speed;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (GizmosPos != default)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(StartShootPoint, GizmosPos);
                Gizmos.DrawWireSphere(GizmosPos, 1f);
            }
            
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position.Set(y: 0), new Vector3(0, 1, 0), Range);
        }
#endif
    }
}