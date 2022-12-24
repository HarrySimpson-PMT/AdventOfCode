namespace AdventOfCode.Common
{
    public class EntitySpace<IEntity>
    {
        public List<IEntity> Entities { get; set; } = new();
        public Dictionary<int, List<IEntity>> X_Entities { get; set; } = new();
        public Dictionary<int, List<IEntity>> Y_Entities { get; set; } = new();
    }
    public interface IEntity
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
    public abstract class Entity : IEntity
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public abstract char GetIcon();        
        public Entity(int x, int y, int id)
        {
            ID = id;
            X = x;
            Y = y;
        }
    }
    public enum Direction { North, South, East, West };
}
