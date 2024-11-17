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

        public int level;
        
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private List<Entity> _controlledEntities = new List<Entity>();
        [SerializeField] private Map _map;

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
            yield return CreateMap();
            upCommand = new MoveCommand(Direction.Up);
            rightCommand = new MoveCommand(Direction.Right);
            downCommand = new MoveCommand(Direction.Down);
            leftCommand = new MoveCommand(Direction.Left);
            yield return null;
        }

        private IEnumerator CreateMap()
        {
            _mapLoader.Init();
            _map = _mapLoader.LoadLevel(level);

            _mapLoader.InstantiateWalls();
            _map.entities.AddRange(_mapLoader.InstantiateBoxes());
            
            yield return StartCoroutine( _mapLoader.PlaceElements());

            Entity player = _mapLoader.InstantiatePlayer();
            
            _map.entities.Add(player);
            _controlledEntities.Add(player);
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
            command.Execute(_map, _controlledEntities);

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

            lastCommand.Undo(_map);

            //after undo a command it is stored in case of redo
            redoCommands.Push(lastCommand);
        }

        private void Redo()
        {
            if (redoCommands.Count <= 0)
                return;

            ICommand undoneCommand = redoCommands.Pop();

            undoneCommand.Redo(_map);

            //redone commands are inserted back into de undo stack
            undoCommands.Push(undoneCommand);
        }

        private IEnumerator Replay()
        {
            foreach (Entity entity in _map.entities)
            {
                Destroy(entity.gameObject);
            }
            
            yield return CreateMap();

            yield return new WaitForSeconds(REPLAY_PAUSE_TIMER);

            List<ICommand> replay = undoCommands.ToList();

            for (int i = replay.Count - 1; i >= 0; i--)
            {
                ICommand currentCommand = replay[i];

                currentCommand.Execute(_map, _controlledEntities);

                yield return new WaitForSeconds(REPLAY_PAUSE_TIMER);
            }

            isReplaying = false;

        }
        #endregion

        #region Methods
        #endregion

    }
}
