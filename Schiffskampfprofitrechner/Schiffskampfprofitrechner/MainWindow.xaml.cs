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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void cBoxFaction_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Faction curFaction = new Faction(cBoxFaction.Text);
            InitializeFaction(curFaction);
            for (int i = 1; i < curFaction.Ships.Count; i++)
            {
                Label lbl = (Label)FindName(("lblShip" + i));
                lbl.Content = curFaction.Ships[i].Name;
            }
        }
    }
}
