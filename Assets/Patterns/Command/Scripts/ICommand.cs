using System.Collections.Generic;
using UnityEngine;

namespace Joymg.Patterns.Command
{
    public interface ICommand 
    {
        #region Methods
        void Execute(Map map, List<Entity> entities);

        void Undo(Map map);

        void Redo(Map map);
        #endregion

    }
}
