using System;
using Systems.Base;
using Assets.Scripts.Libraries.RSG;
using Core;
using DG.Tweening;
using UnityEngine;
using View.Entity;
using Object = UnityEngine.Object;

namespace Systems
{
    public class TowersSystem : ISystem, IFixedUpdateSystem
    {
        public void FixedUpdate()
        {
            foreach (var tower in Game.World.Towers)
            {
                if(!tower.CanShoot)
                    continue;
                
                var monster = GetNearestMonster(tower);
                if (monster)
                {
                    if (tower.FollowingProjectile)
                        TryShootWithFollow(tower, monster);
                    else
                        TryShootWithPredicate(tower, monster);
                }
            }
        }

        private MonsterView GetNearestMonster(TowerView tower)
        {
            return Game.World.Monsters.MinBy(m => m.transform.position.GetDistance2DTo(tower.transform.position));
        }

        private void TryShootWithFollow(TowerView tower, MonsterView monster)
        {
            if (tower.transform.position.GetDistance2DTo(monster.transform.position) < tower.Range)
            {
                Promise.Resolved()
                    .Then(() => Rotate(tower, monster.transform.position))
                    .Then(() => FollowShoot(tower, monster))
                    .Then(() => Hit(tower, monster));
            }
        }

        private void TryShootWithPredicate(TowerView tower, MonsterView monster)
        {
            var prc = Game.World.Systems.GetSystem<PredictorSystem>().PredicateCrossingByBisectionMethod(monster.Route, monster, tower);
            if (prc != null)
            {
                var toPos = monster.Route.GetPosByPercent(prc.Value);
                if(tower.transform.position.GetDistance2DTo(toPos) < tower.Range)
                {
                    Promise.Resolved()
                        .Then(() => Rotate(tower, toPos))
                        .Then(() => Shoot(tower, toPos, tower.GetShootMoveTime(toPos)))
                        .Then(() => Hit(tower, monster));
                }
            }
        }

        private IPromise Rotate(TowerView tower, Vector3 toPos)
        {
            var promise = new Promise();
            
            tower.GizmosPos = toPos;
            
            var rotateData = tower.GetRotateData(toPos);
            
            tower.SetShootDelay(rotateData.time + tower.ShootInterval);
            tower.RotateTo(rotateData.rotate, rotateData.time, promise.Resolve);
            
            return promise;
        }

        private IPromise Shoot(TowerView tower, Vector3 toPos, float time)
        {
            if (!tower)
                return Promise.Rejected(null);

            return Object.Instantiate(tower.Projectile).ShootTo(tower, toPos, time);
        }

        private IPromise FollowShoot(TowerView tower, MonsterView monster)
        {
            if (!monster || !tower)
                return Promise.Rejected(null);
            
            return Object.Instantiate(tower.Projectile).FollowTo(tower, monster.transform);
        }

        private void Hit(TowerView tower, MonsterView monster)
        {
            if (tower && monster)
                monster.Hp -= tower.Damage;
        }
    }
}