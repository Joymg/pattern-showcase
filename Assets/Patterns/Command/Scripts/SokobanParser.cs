using System.IO;
using UnityEngine;

// Author : Joy
namespace Joymg.Patterns.Command
{
    public class SokobanParser
    {
        private const string Levels_Path = "Command/Levels/";
        private const string Levels_File = "levels";
        #region Enums
        #endregion

        #region Consts

        private struct LevelList
        {
            public string[] levels;

            public static implicit operator string[](LevelList list) => list.levels;
        }
        #endregion

        #region Fields
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public static string[] ParseLevel()
        {
            TextAsset levelFile = Resources.Load<TextAsset>(Levels_Path + Levels_File);
            if (levelFile == null)
                return null;

            string[] levels = JsonUtility.FromJson<LevelList>(levelFile.ToString());
            return levels;
        }
        #endregion

    }


}