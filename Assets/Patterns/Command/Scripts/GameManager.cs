using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Joymg.Patterns.Command
{
    public class GameManager : MonoBehaviour
    {
        #region Enums
        #endregion

        #region Consts
        private const float REPLAY_PAUSE_TIMER = 0.5f;
        #endregion

        #region Fields
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private Player _player;

        //Movement keys
        private ICommand upCommand;
        private ICommand rightCommand;
        private ICommand downCommand;
        private ICommand leftCommand;

        //Storing commans for easier undo and redo
        private Stack<ICommand> undoCommands = new();
        private Stack<ICommand> redoCommands = new();

        //On replay control is taken from player
        private bool isReplaying = false;
        #endregion

        #region Unity Methods
        private IEnumerator Start()
        {


            _mapLoader.Init();
            _mapLoader.LoadLevel(2);

            _mapLoader.InstantiateWalls();
            _mapLoader.InstantiateBoxes();
            yield return StartCoroutine( _mapLoader.PlaceElements());

            _player = _mapLoader.InstantiatePlayer();
            upCommand = new MoveCommand(_player, Direction.Up);
            rightCommand = new MoveCommand(_player, Direction.Right);
            downCommand = new MoveCommand(_player, Direction.Down);
            leftCommand = new MoveCommand(_player, Direction.Left);
            yield return null;
        }

        private void Update()
        {
            if (isReplaying)
                return;

            if (Input.GetKeyDown(KeyCode.W))
            {
                ExecuteCommand(upCommand);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                ExecuteCommand(rightCommand);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                ExecuteCommand(downCommand);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                ExecuteCommand(leftCommand);
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                Undo();
            }
            //Redo with r
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Redo();

            }

            //Start replay
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Replay());

                isReplaying = true;
            }
        }

        private void ExecuteCommand(ICommand command)
        {
            //Execure command
            command.Execute();

            //Add to Undo Stack
            undoCommands.Push(command);

            //After modifiying redo is not possible
            redoCommands.Clear();
        }

        private void Undo()
        {
            if (undoCommands.Count <= 0)
                return;

            ICommand lastCommand = undoCommands.Pop();

            lastCommand.Undo();

            //after undo a command it is stored in case of redo
            redoCommands.Push(lastCommand);
        }

        private void Redo()
        {
            if (redoCommands.Count <= 0)
                return;

            ICommand undoneCommand = redoCommands.Pop();

            undoneCommand.Redo();

            //redone commands are inserted back into de undo stack
            undoCommands.Push(undoneCommand);
        }

        private IEnumerator Replay()
        {
            _player.transform.position = Vector3.zero;

            yield return new WaitForSeconds(REPLAY_PAUSE_TIMER);

            List<ICommand> replay = undoCommands.ToList();

            for (int i = replay.Count - 1; i >= 0; i--)
            {
                ICommand currentCommand = replay[i];

                currentCommand.Execute();

                yield return new WaitForSeconds(REPLAY_PAUSE_TIMER);
            }

            isReplaying = false;

        }
        #endregion

        #region Methods
        #endregion

    }
}
