using UnityEngine;
using View.Entity.Base;
using View.Route;

namespace View.Entity
{
    public class SpawnerView : EntityView
    {
        [SerializeField] private float m_interval = 3;
        public float Interval => m_interval;
        
        [SerializeField] private MonsterView m_monsterPrefab;
        public MonsterView MonsterPrefab => m_monsterPrefab;
        
        [SerializeField] private BaseRouteView m_route;
        public BaseRouteView Route => m_route;
        
        private float m_lastSpawn = -1;
        
        public bool CanSpawn => m_lastSpawn < 0 || Time.time > m_lastSpawn + Interval;
        
        public void OnSpawn()
        {
            m_lastSpawn = Time.time;
            
            var newMonster = Instantiate(MonsterPrefab, transform.position, Quaternion.identity);
            newMonster.GetComponent<MonsterView>().Init(Route);
        }
    }
}