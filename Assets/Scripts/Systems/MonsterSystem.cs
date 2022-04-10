using System.Collections.Generic;
using Systems.Base;
using Core;
using UnityEngine;
using View.Entity;

namespace Systems
{
    public class MonsterSystem : ISystem, IFixedUpdateSystem
    {
        public void FixedUpdate()
        {
            List<MonsterView> needToKillMonsters = null;
            
            foreach (var monster in Game.World.Monsters)
            {
                monster.AddPosition(monster.PercentPerSecond * Time.fixedDeltaTime);

                if (monster.IsOnEndOfPath)
                {
                    needToKillMonsters ??= new List<MonsterView>();
                    needToKillMonsters.Add(monster);
                }
            }

            if (needToKillMonsters != null)
                foreach (var monster in needToKillMonsters)
                    Object.Destroy(monster.gameObject);
        }
    }
}