using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Author : Joy
namespace Joymg.Patterns.Command
{
    public class SokobanSpawner
    {
        #region Enums
        private enum SpawnType
        {
            AtOnce,
            Linear,
            Snake,
            Contiguous
        }
        #endregion

        #region Consts
        private const float Placing_Time = 1.5f;
        #endregion

        #region Fields
        public List<Vector3> spawnPoints = new();
        public List<Entity> entities = new();
        private SpawnMode spawnMode;

        #endregion

        #region Unity Methods
        #endregion

        #region Methods
        public void Init(Map map, char[] ids)
        {
            SpawnType mode = SpawnType.Snake;
            //SpawnType mode = (SpawnType)Random.Range(0, Enum.GetNames(typeof(SpawnType)).Length-1);
            switch (mode)
            {
                case SpawnType.AtOnce:
                    spawnMode = new AtOnceSpawnMode(map, ids);
                    break;
                case SpawnType.Linear:
                    spawnMode = new LinesSpawnMode(map, ids);
                    break;
                case SpawnType.Snake:
                    spawnMode = new SnakeSpawnMode(map, ids);
                    break;
                case SpawnType.Contiguous:
                    spawnMode = new ContiguousSpawnMode(map, ids);
                    break;
            }

            spawnPoints = spawnMode.GenerateOrder();
        }

        public void InstantiateElements(Entity prefab, SokobanHashmap.Entry entry)
        {
            int index = Random.Range(0, entry.TileBases.Length + 1);

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Entity entity = UnityEngine.Object.Instantiate(prefab, spawnPoints[i], Quaternion.identity);
                entities.Add(entity);
            }
        }
    #endregion

    }
}
