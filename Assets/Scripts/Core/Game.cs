using System;
using UnityEngine;

namespace Core
{
    public class Game : MonoBehaviour
    {
        public static Game Instance { get; private set; }

        private World world;
        public static World World => Instance.world;

        public Game()
        {
            if(Instance)
                throw new Exception("Singleton exception! Game.Instance already exist");
            Instance = this;
        }

        private void Awake()
        {
            Instance.world = Instance.gameObject.AddComponent<World>();
            Instance.world.AddEntities();
            Instance.world.AddAllSystems();
        }
    }
}