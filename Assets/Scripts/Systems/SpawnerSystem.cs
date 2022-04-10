using Systems.Base;
using Core;

namespace Systems
{
    public class SpawnerSystem : ISystem, IFixedUpdateSystem
    {
        public void FixedUpdate()
        {
            foreach (var spawner in Game.World.Spawners)
                if (spawner.CanSpawn)
                    spawner.OnSpawn();
        }
    }
}