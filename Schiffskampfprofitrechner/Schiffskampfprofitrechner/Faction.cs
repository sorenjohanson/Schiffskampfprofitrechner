using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SKPR
{
    public class Faction
    {
        public string Name { get; set; }
        public List<Ship> Ships { get; set; }
        public BitmapImage Logo { get; set; }

        public Faction(string name)
        {
            Name = name;
            Ships = new List<Ship> { };
        }

        public Faction(string name, List<Ship> ships, BitmapImage logo)
        {
            Name = name;
            Ships = ships;
            Logo = logo;
        }
    }
}
