using System.Collections.Generic;
using System.Text;
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

        public readonly Direction Direction;
        private List<Entity> affectedEntities;
        public List<Entity> AffectedEntities => affectedEntities;
        #endregion

        #region Methods

        public MoveCommand(Direction direction)
        {
            Direction = direction;
        }

        public MoveCommand(MoveCommand moveCommand)
        {
            Direction = moveCommand.Direction;
            affectedEntities = moveCommand.AffectedEntities;
        }

 
        public void Execute(Map map, List<Entity> entities)
        {
            affectedEntities = new List<Entity>(entities) ;
            SortControlledEntities(Direction);
            ExecuteMovements(map, Direction);
        }


        private void SortControlledEntities(Direction direction)
        {
            CoordinateDirectionComparer comparer = new CoordinateDirectionComparer(direction);
            affectedEntities.Sort((a, b) => comparer.Compare(a.Coordinates, b.Coordinates));
        }

        private void ExecuteMovements(Map map, Direction direction)
        {
            foreach (Entity entity in affectedEntities)
            {
                Map.Cell currentCell = map.GetCell(entity.Coordinates.Reverse());
                Coordinates pre = currentCell.coordinates;
                Map.Cell neighborCell = currentCell.GetNeighbour(direction);
                map.Swap(currentCell, neighborCell);
                entity.MoveTowards(direction.Collapse());
                //Debug.Log(map.ToString());
                //Debug.Log($"{entity.name}:{pre}->{entity.Coordinates}");
            }
        }

        public void Redo(Map map)
        {
            SortControlledEntities(Direction);
            ExecuteMovements(map, Direction);
        }

        public void Undo(Map map)
        {
            SortControlledEntities(Direction.Opposite());
            ExecuteMovements(map, Direction.Opposite());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < affectedEntities.Count; i++)
                sb.Append($"{affectedEntities[i].name},");

            sb.Remove(sb.Length-1,1);
            
            return $"Type: {this.GetType().Name}, Elements: {sb}";
        }

        #endregion
    }
}