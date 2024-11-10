using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Author : Joy
namespace Joymg.Patterns.Command
{
    [CreateAssetMenu(fileName ="SokobanTileHashmap", menuName = "Patterns/Command/SokobanTileHashMap")]
    public class SokobanHashmap : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public TileType TileType;
            public TileBase[] TileBases;
        }

        public List<Entry> Entries = new List<Entry>();

        public static implicit operator List<Entry>(SokobanHashmap hashmap) => hashmap.Entries;


    }

}
