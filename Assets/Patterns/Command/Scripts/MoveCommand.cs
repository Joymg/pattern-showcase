using System.Collections.Generic;
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
        private List<Entity> affectedEntities;

        #endregion

        #region Methods

        public MoveCommand(Direction direction)
        {
            _direction = direction;
        }

        public void Execute()
        {
            _player.MoveTowards(_direction.Collapse());
        }

        public void Execute(Map map, List<Entity> entities)
        {
            SortControlledEntities(entities, _direction);

            foreach (Entity entity in entities)
            {
                affectedEntities = map.CalculateMovingEntitiesInDirection(entity.Coordinates.Reverse(), _direction);
            }

            if (affectedEntities.Count <= 0)
                return;

            ExecuteMovements(map, _direction);
        }


        private void SortControlledEntities(List<Entity> entities, Direction direction)
        {
            CoordinateDirectionComparer comparer = new CoordinateDirectionComparer(direction);
            entities.Sort((a, b) => comparer.Compare(a.Coordinates, b.Coordinates));
        }

        private void ExecuteMovements(Map map, Direction direction)
        {
            foreach (Entity entity in affectedEntities)
            {
                Map.Cell currentCell = map.GetCell(entity.Coordinates.Reverse());
                Map.Cell neighborCell = currentCell.GetNeighbour(direction);
                map.Swap(currentCell, neighborCell);
                entity.MoveTowards(direction.Collapse());
            }
        }

        public void Redo(Map map)
        {
            SortControlledEntities(affectedEntities, _direction);
            ExecuteMovements(map, _direction);
        }

        public void Undo(Map map)
        {
            SortControlledEntities(affectedEntities, _direction.Opposite());
            ExecuteMovements(map, _direction.Opposite());
        }

        #endregion
    }
}