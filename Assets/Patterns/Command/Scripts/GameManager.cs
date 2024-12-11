using Codice.Client.BaseCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static GameManager Instance;

        public int level;

        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private List<Entity> _controlledEntities = new List<Entity>();
        [SerializeField] private Map _map;

        //Movement keys
        private CommandInputHandler _inputHandler;
        private ICommand upCommand;
        public Action<Direction> upExecute;
        private ICommand rightCommand;
        public Action<Direction> rightExecute;
        private ICommand downCommand;
        public Action<Direction> downExecute;
        private ICommand leftCommand;
        public Action<Direction> leftExecute;

        //Storing commans for easier undo and redo
        [SerializeField] private Stack<ICommand> undoCommands = new();
        [SerializeField] private Stack<ICommand> redoCommands = new();

        //On replay control is taken from player
        private bool isReplaying = false;
        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance != this)
                Instance = this;
        }
        private IEnumerator Start()
        {
            _inputHandler = new CommandInputHandler();
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

            yield return StartCoroutine(_mapLoader.PlaceElements());

            Entity player = _mapLoader.InstantiatePlayer();

            _map.entities.Add(player);
            _controlledEntities.Add(player);
            yield return null;
        }

        private void Update()
        {
            if (isReplaying)
                return;

            ICommand inputCommand = _inputHandler.GetInput();

            /*if (inputCommand != null)
                if (CheckValidMovement((inputCommand as MoveCommand).Direction, out List<Entity> movingEntities))
                {
                    ExecuteCommand(inputCommand, movingEntities);
                }*/
            if (Input.GetKeyDown(KeyCode.W) && CheckValidMovement(Direction.Up, out List<Entity> movingEntities))
            {
                upExecute?.Invoke(Direction.Up);
                ExecuteCommand(upCommand, movingEntities);
            }
            else if (Input.GetKeyDown(KeyCode.D) && CheckValidMovement(Direction.Right, out movingEntities))
            {
                rightExecute?.Invoke(Direction.Right);
                ExecuteCommand(rightCommand, movingEntities);
            }
            else if (Input.GetKeyDown(KeyCode.S) && CheckValidMovement(Direction.Down, out movingEntities))
            {
                downExecute?.Invoke(Direction.Down);
                ExecuteCommand(downCommand, movingEntities);
            }
            else if (Input.GetKeyDown(KeyCode.A) && CheckValidMovement(Direction.Left, out movingEntities))
            {
                leftExecute?.Invoke(Direction.Left);
                ExecuteCommand(leftCommand, movingEntities);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Undo();
            }
            //Redo with r
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Redo();
            }

            if (_map.CheckWin()) { Debug.Log("YOU WON!!"); }

            //Start replay
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Replay());

                isReplaying = true;
            }
        }

        private bool CheckValidMovement(Direction direction, out List<Entity> movingEntities)
        {

            SortControlledEntities(_controlledEntities, direction);

            if (!_map.TryMovingEntitiesInDirection(_controlledEntities, direction, out movingEntities))
                return false;

            if (movingEntities.Count <= 0)
                return false;

            return true;
        }

        private void SortControlledEntities(List<Entity> entities, Direction direction)
        {
            CoordinateDirectionComparer comparer = new CoordinateDirectionComparer(direction);
            entities.Sort((a, b) => comparer.Compare(a.Coordinates, b.Coordinates));
        }

        private void ExecuteCommand(ICommand command, List<Entity> movingEntities)
        {
            string pre = _map.ToString();
            //Execure command
            command.Execute(_map, movingEntities);
            string post = _map.ToString();

            Debug.Log($"{pre}\n\n{post}");

            //Add to Undo Stack
            if (command is MoveCommand moveCommand)
            {
                command = new MoveCommand(moveCommand);
            }
            undoCommands.Push(command);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = undoCommands.Count - 1; i >= 0; i--)
            {
                stringBuilder.AppendLine($"Movement {i}: {undoCommands.ElementAt(i) as MoveCommand}");
            }
            Debug.Log(stringBuilder);

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
