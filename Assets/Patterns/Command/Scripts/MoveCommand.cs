using UnityEngine;

// Author : 
namespace Joymg.Patterns.Command
{
    [System.Serializable]
    public class MoveCommand : ICommand
    {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        private readonly Direction _direction;
        private Player _player;
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public MoveCommand(Player player, Direction direction)
        {
            _direction = direction;
            _player = player;
        }

        public void Execute()
        {
            _player.MoveTowards(_direction.Collapse());
        }

        public void Redo()
        {
            Execute();
        }

        public void Undo()
        {
            _player.MoveTowards(_direction.Opposite().Collapse());
        }
        #endregion

    }
}