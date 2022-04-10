using System.Collections.Generic;
using System.Linq;
using Systems;
using UnityEngine;
using View.Entity;
using View.Entity.Base;

namespace Core
{
    public class World : MonoBehaviour
    {
        private readonly HashSet<EntityView> m_entities = new HashSet<EntityView>();
        
        private readonly HashSet<TowerView> m_towers = new HashSet<TowerView>();
        private readonly HashSet<MonsterView> m_monsters = new HashSet<MonsterView>();
        private readonly HashSet<SpawnerView> m_spawners = new HashSet<SpawnerView>();
        
        public IEnumerable<TowerView> Towers => m_towers;
        public IEnumerable<MonsterView> Monsters => m_monsters;
        public IEnumerable<SpawnerView> Spawners => m_spawners;
        
        public GameSystems Systems { get; private set; }

        public IEnumerable<T> GetEntities<T>() where T : EntityView => m_entities.OfType<T>();
        public T GetEntity<T>() where T : EntityView => m_entities.FirstOrDefault(v => v is T) as T;

        public void AddEntity(EntityView view)
        {
            if(m_entities.Contains(view))
                return;
            
            m_entities.Add(view);

            switch (view)
            {
                case TowerView tower:
                    m_towers.Add(tower);
                    break;
                case MonsterView monster:
                    m_monsters.Add(monster);
                    break;
                case SpawnerView spawner:
                    m_spawners.Add(spawner);
                    break;
            }
        }

        public void RemoveEntity(EntityView view)
        {
            m_entities.Remove(view);

            switch (view)
            {
                case TowerView tower:
                    m_towers.Remove(tower);
                    break;
                case MonsterView monster:
                    m_monsters.Remove(monster);
                    break;
                case SpawnerView spawner:
                    m_spawners.Remove(spawner);
                    break;
            }
        }
        
        

        private void FixedUpdate()
        {
            Systems.FixedUpdate();
        }


        public void AddAllSystems()
        {
            Systems = new GameSystems()
                .AddSystem<SpawnerSystem>()
                .AddSystem<TowersSystem>()
                .AddSystem<MonsterSystem>()
                .AddSystem<PredictorSystem>();
        }

        public void AddEntities()
        {
            foreach (var view in FindObjectsOfType<EntityView>())
                AddEntity(view);
        }
    }
}