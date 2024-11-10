using UnityEngine;

namespace Joymg.Patterns.Command
{
    public interface ICommand 
    {
        #region Methods
        void Execute();

        void Undo();

        void Redo();
        #endregion

    }
}
