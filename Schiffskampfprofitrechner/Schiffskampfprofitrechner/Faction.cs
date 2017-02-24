using System.Collections.Generic;

namespace SKPR
{
    public class Faction
    {
        public string Name { get; set; }
        public List<Ship> Ships { get; set; }

        public Faction(string name)
        {
            Name = name;
            Ships = new List<Ship> { };
        }

        public Faction(string name, List<Ship> ships)
        {
            Name = name;
            Ships = ships;
        }
    }
}
