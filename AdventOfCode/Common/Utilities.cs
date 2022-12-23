using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public class EntitySpace<Entity>
    {
        public List<Entity> Entities { get; set; } = new();
        public Dictionary<int, List<Entity>> X_Entities { get; set; } = new();
        public Dictionary<int, List<Entity>> Y_Entities { get; set; } = new();
    }
    public interface Entity
    {
        public int ID { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
    public enum Direction { North, South, East, West };
}
