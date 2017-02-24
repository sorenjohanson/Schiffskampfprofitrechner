namespace SKPR
{
    public class Ship
    {
        public string Name { get; set; }
        public int[] Resources { get; set; }
        public string Faction { get; set; }

        public Ship(string name, int[] res, string faction)
        {
            Name = name;
            Resources = res;
            Faction = faction;
        }
    }
}