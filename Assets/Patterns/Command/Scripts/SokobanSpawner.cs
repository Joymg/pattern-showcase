using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Author : Joy
namespace Joymg.Patterns.Command
{
    [Serializable]
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
        private Vector3 Tile_Map_Center_Offset = new Vector3(0.5f, 0.5f, 0f);
        #endregion

        #region Fields
        public List<Vector3> spawnPoints = new();
        public List<Entity> entities = new();
        [SerializeField] private SpawnMode spawnMode;
        [SerializeField] private SpawnType spawnType;

        #endregion

        #region Unity Methods
        #endregion

        #region Methods
        public void Init(Map map, char[] ids)
        {
            //spawnType = SpawnType.Contiguous;
            spawnType = (SpawnType)Random.Range(0, Enum.GetNames(typeof(SpawnType)).Length);
            switch (spawnType)
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
            //Todo: use random tile
            int index = Random.Range(0, entry.TileBases.Length + 1);

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Entity entity = UnityEngine.Object.Instantiate(prefab, spawnPoints[i] + Tile_Map_Center_Offset, Quaternion.identity);
                entity.Init(new Coordinates((int)spawnPoints[i].x, (int)spawnPoints[i].y));
                entities.Add(entity);
            }
        }
        #endregion

    }
}
