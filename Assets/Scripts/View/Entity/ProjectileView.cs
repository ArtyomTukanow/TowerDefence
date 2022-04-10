using Assets.Scripts.Libraries.RSG;
using DG.Tweening;
using UnityEngine;
using View.Entity.Base;

namespace View.Entity
{
    public class ProjectileView : EntityView
    {
        [SerializeField] private float m_speed;
        public float Speed => m_speed;
        public float SpeedPerFrame => m_speed * Time.fixedDeltaTime;

        private Transform m_target;
        private Promise m_followPromise;
        private TowerView m_tower;
        
        void FixedUpdate()
        {
            if (m_followPromise == null)
                return;
            
            if (!m_target)
            {
                if(m_tower)
                    m_tower.GizmosPos = default;
                
                m_followPromise.Reject();
                m_followPromise = null;
                return;
            }

            if(m_tower)
                m_tower.GizmosPos = m_target.position;
            
            var translation = m_target.position - transform.position;
        
            if (translation.magnitude > SpeedPerFrame)
                translation = translation.normalized * SpeedPerFrame;
        
            transform.Translate(translation);

            if (transform.position.Equals(m_target.position))
            {
                if(m_tower)
                    m_tower.GizmosPos = default;
                
                m_followPromise.Resolve();
                m_followPromise = null;
            }
        }

        public IPromise ShootTo(TowerView tower, Vector3 toPos, float time)
        {
            m_tower = tower;
            var promise = new Promise();
            transform.position = tower.StartShootPoint;
            
            DOTween.Sequence()
                .Join(transform.DOMoveX(toPos.x, time).SetEase(Ease.Linear))
                .Join(transform.DOMoveZ(toPos.z, time).SetEase(Ease.Linear))
                .Join(transform.DOMoveY(toPos.y, time).SetEase(Ease.InSine))
                .OnComplete(() =>
                {
                    if (this && gameObject)
                    {
                        Destroy(gameObject);
                        promise.Resolve();

                        if(m_tower)
                            m_tower.GizmosPos = default;
                    }
                    else
                    {
                        promise.Reject();
                    }
                });

            return promise;
        }

        public IPromise FollowTo(TowerView tower, Transform target)
        {
            m_tower = tower;
            m_followPromise = new Promise();
            transform.position = tower.StartShootPoint;
            m_target = target;
            
            return m_followPromise.Finally(() =>
            {
                if(gameObject)
                    Destroy(gameObject);
            });
        }
    }
}