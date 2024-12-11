using Joymg.Patterns.Command;
using System.Collections.Generic;
using UnityEngine;

// Author : Joy
namespace Joymg.Patterns.Command
{

    public class CommandInputHandler
    {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public ICommand GetInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                return new MoveCommand(Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                return new MoveCommand(Direction.Right);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                return new MoveCommand(Direction.Down);
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                return new MoveCommand(Direction.Left);
            }
            return null;

        }
        #endregion
    }

}
