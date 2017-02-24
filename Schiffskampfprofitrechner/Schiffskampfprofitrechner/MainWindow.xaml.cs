using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Controls;

namespace SKPR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Faction Imperium = new Faction("Imperium");
        private Faction Rebellion = new Faction("Rebellion");

        public void InitializeFaction(Faction faction)
        {
            string line;
            List<Ship> allShips = new List<Ship> { };

            StreamReader shipReadout = new StreamReader("data/ShipReadouts.txt");
            while ((line = shipReadout.ReadLine()) != null)
            {
                string[] splitLine = line.Split('.');
                string shipName = splitLine[0];
                int[] shipRes = splitLine[1].Split(',').Select(int.Parse).ToArray();
                string shipFaction = splitLine[2];
                if (shipFaction == faction.Name)
                {
                    Ship newShip = new Ship(shipName, shipRes, shipFaction);
                    allShips.Add(newShip);
                }
            }
            shipReadout.Close();
            faction.Ships.AddRange(allShips);
        }

        public void UpdateText(Faction faction)
        {
            for (int i = 1; i <= faction.Ships.Count; i++)
            {
                Label lbl = (Label)FindName(("lblShip" + i));
                Console.WriteLine("FACTION: " + faction.Name + " SHIP: " + faction.Ships[i - 1].Name);
                lbl.Content = faction.Ships[i - 1].Name;
            }
        }

        public Faction ReturnFaction(string faction)
        {
            switch (faction)
            {
                // SelectedItem.ToString() for ComboBox returns Control Type + actual Item. I'm too lazy to strip.
                case "System.Windows.Controls.ComboBoxItem: Imperium":
                    return Imperium;
                case "System.Windows.Controls.ComboBoxItem: Rebellion":
                    return Rebellion;
            }
            return Imperium;
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeFaction(Imperium);
            InitializeFaction(Rebellion);
        }

        private void cBoxFaction_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateText(ReturnFaction(cBoxFaction.SelectedItem.ToString()));
        }
    }
}
