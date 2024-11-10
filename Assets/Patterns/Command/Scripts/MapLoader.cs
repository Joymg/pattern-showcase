using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

// Author : Joy

namespace Joymg.Patterns.Command
{
    public class MapLoader : MonoBehaviour
    {
        #region Enums

        #endregion

        #region Consts

        private struct Entry
        {
            public char character;
            public TileType tileType;

            public Entry(char character, TileType tileType)
            {
                this.character = character;
                this.tileType = tileType;
            }
        }

        #endregion

        #region Fields

        #endregion

        #region Unity Methods

        public string[] levels;
        [Header("TileMaps")] public Tilemap backgroundTilemap;
        public Tilemap staticTilemap;
        public Tilemap movablesTilemap;

        [FormerlySerializedAs("entityPrefab")] [Header("Entities")] [SerializeField]
        private Entity playerPrefab;

        [SerializeField] private Entity boxPrefab;
        [SerializeField] private Entity wallPrefab;
        [Header("Hashmap")] public SokobanHashmap tileHashmap;

        private Vector3Int _playerStartingPosition;

        private List<Entry> characterHashmap = new List<Entry>()
        {
            new Entry(' ', TileType.Floor),
            new Entry('#', TileType.Wall),
            new Entry('$', TileType.Box),
            new Entry('.', TileType.Goal),
            new Entry('@', TileType.Player),
        };

        private SokobanSpawner _wallSpawner;
        private SokobanSpawner _boxSpawner;

        #endregion

        #region Methods

        public void Init()
        {
            levels = SokobanParser.ParseLevel();
            _wallSpawner = new SokobanSpawner();
            _boxSpawner = new SokobanSpawner();
        }

        public void LoadLevel(int levelIndex)
        {
            string level = levels[levelIndex];

            string[] levelParts = level.Split('\n');
            int heigth = levelParts.Length;
            int width = levelParts[0].Length;

            _wallSpawner.Init(level, new char[]{'#'});
            _boxSpawner.Init(level, new char[]{'$','*'});

            for (int i = 0; i < heigth; i++)
            {
                for (int j = 0; j < levelParts[i].Length; j++)
                {
                    Vector3Int position = new Vector3Int(j, i);
                    backgroundTilemap.SetTile(position,
                        tileHashmap.Entries.First(entry => entry.TileType == TileType.Floor).TileBases[1]);
                    char currentChar = levelParts[i][j];
                    TileType tileType = characterHashmap.FirstOrDefault(entry => entry.character == currentChar)
                        .tileType;
                    switch (currentChar)
                    {
                        case '#':
                        case '.':
                            staticTilemap.SetTile(position,
                                tileHashmap.Entries.First(entry => entry.TileType == tileType).TileBases[2]);
                            break;
                        case '@':
                            _playerStartingPosition = position;
                            break;
                        case '$':
                            //movablesTilemap.SetTile(position, tileHashmap.Entries.First(entry => entry.TileType == tileType).TileBases[0]);
                            break;
                        case '*':
                            staticTilemap.SetTile(position,
                                tileHashmap.Entries.First(entry => entry.TileType == TileType.Goal).TileBases[0]);
                            //movablesTilemap.SetTile(position, tileHashmap.Entries.First(entry => entry.TileType == TileType.Player).TileBases[0]);
                            break;
                        case '+':
                            staticTilemap.SetTile(position,
                                tileHashmap.Entries.First(entry => entry.TileType == TileType.Goal).TileBases[0]);
                            _playerStartingPosition = position;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public Player InstantiatePlayer()
        {
            Player player = Instantiate((Player)playerPrefab, backgroundTilemap.transform.parent);
            Vector3 offset = Vector2.one * 0.5f;
            player.transform.SetPositionAndRotation(_playerStartingPosition + transform.position + offset,
                Quaternion.identity);
            return player;
        }

        public void InstantiateWalls()
        {
            _wallSpawner.InstantiateElements(wallPrefab,
                tileHashmap.Entries.FirstOrDefault(entry => entry.TileType == TileType.Wall));
        }

        public void InstantiateBoxes()
        {
            _boxSpawner.InstantiateElements(boxPrefab,
                tileHashmap.Entries.FirstOrDefault(entry => entry.TileType == TileType.Box));
        }

        public IEnumerator PlaceElements()
        {
            int count = _wallSpawner.entities.Count - 1;
            for (int i = 0; i < count; i++)
            {
                StartCoroutine(_wallSpawner.entities[i].SetIntoPosition(_wallSpawner.spawnPoints[i].z / -15f));
            }

            yield return StartCoroutine(_wallSpawner.entities[count]
                .SetIntoPosition(_wallSpawner.spawnPoints[count].z / -15f));

            count = _boxSpawner.entities.Count - 1;
            for (int i = 0; i < count; i++)
            {
                StartCoroutine(_boxSpawner.entities[i].SetIntoPosition(_boxSpawner.spawnPoints[i].z / -15f));
            }

            yield return StartCoroutine(_boxSpawner.entities[count]
                .SetIntoPosition(_boxSpawner.spawnPoints[count].z / -15f));
        }

        #endregion
    }
}