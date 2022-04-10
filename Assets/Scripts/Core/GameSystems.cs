using System;
using System.Collections.Generic;
using Systems.Base;

namespace Systems
{
    public class GameSystems
    {
        private readonly Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();
        private readonly List<IFixedUpdateSystem> fixedUpdateSystems = new List<IFixedUpdateSystem>();
        
        public T GetSystem<T>() where T: ISystem
        {
            return systems.TryGetValue(typeof(T), out var result) ? (T)result : default;
        }
        
        public GameSystems AddSystem<T>() where T: ISystem, new()
        {
            var type = typeof(T);
            if (!systems.ContainsKey(type))
            {
                var system = new T();
                systems[type] = system;
                
                if(system is IFixedUpdateSystem fSystem)
                    fixedUpdateSystems.Add(fSystem);
            }
            return this;
        }

        public void FixedUpdate()
        {
            foreach (var system in fixedUpdateSystems)
                system.FixedUpdate();
        }
    }
}