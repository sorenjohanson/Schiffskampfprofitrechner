using System.Windows;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System;

namespace SKPR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Create Factions with single constructor
        private Faction Imperium = new Faction("Imperium");
        private Faction Rebellion = new Faction("Rebellion");

        public void InitializeFaction(Faction faction)
        {
            // Empty string is needed here to prevent NullReferenceException
            string line;

            // StreamReader to read textfile. Each ship has data stored on one line in text file.
            StreamReader shipReadout = new StreamReader("data/ShipReadouts.txt");
            while ((line = shipReadout.ReadLine()) != null)
            {
                // Data structure:
                // Ship Name.Ship Resources.Ship Faction
                // We split according to data structure, convert as needed.
                string[] splitLine = line.Split('.');
                string shipName = splitLine[0];
                int[] shipRes = splitLine[1].Split(',').Select(int.Parse).ToArray();
                string shipFaction = splitLine[2];
                if (shipFaction == faction.Name)
                {
                    // add this new ship to faction.Ships
                    faction.Ships.Add(new Ship(shipName, shipRes, shipFaction));
                }
            }
            // Close StreamReader
            shipReadout.Close();
        }

        public void UpdateText(Faction faction)
        {
            // This updates all labels (named lblShip1 to lblShip14) to include Ship name.
            for (int i = 1; i <= faction.Ships.Count; i++)
            {
                Label lbl = (Label)FindName(("lblShip" + i));
                lbl.Content = faction.Ships[i - 1].Name;
            }
        }

        public Faction ReturnFaction(string faction)
        {
            // Returns Faction based on selected CBoxItem.
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

        public List<int> CalculateResources(string[] resources)
        {
            // resources array references TextBox elements, as they always end with resource name.
            List<int> resWin = new List<int> { };
            foreach (string res in resources)
            {
                // Late to the party workaround because this is primarily for Salvage + other Res,
                // .. but Credits needs to be in array.
                if (res != "Creds")
                {
                    // Loop through each individual resource, adding together the same resource
                    // .. from both Salvage and other gained Resources.
                    TextBox TFtxt = (TextBox)FindName(("TF" + res));
                    TextBox Restxt = (TextBox)FindName(("Res" + res));

                    // Add to returned List<int>
                    resWin.Add(int.Parse(TFtxt.Text) + int.Parse(Restxt.Text));
                }
                else if (res == "Creds")
                    resWin.Add(0);
            }
            return resWin;
        }

        public List<int> CalculateShipLosses(Faction faction)
        {
            // There are only a total of five resources.
            // We have to define the List already to make use of shipLoss.Insert() proper
            List<int> shipLoss = new List<int> { 0, 0, 0, 0, 0 };
            // There are 14 TextBoxes, txtShip1-txtShip14, so we iterate through all
            for (int i = 0; i < 14; i++)
            {
                // Find TextBox and the amount.
                TextBox shipBox = (TextBox)FindName(("txtShip" + (i + 1)));
                int shipAmount = int.Parse(shipBox.Text);
                //Console.WriteLine("Amount: " + shipAmount.ToString());
                if (shipAmount > 0)
                {
                    // We take resource value from current ship, iterate through all (total of 5)
                    // .. and insert the multiplied value at the proper spot
                    Ship currentShip = faction.Ships[i];
                    int[] shipRes = currentShip.Resources; // - At this point, resources are more than doubled.
                    for (int r = 0; r < shipRes.Count(); r++)
                    {
                        int currentRes = shipRes[r];
                        //Console.WriteLine("Current Resource: " + currentRes.ToString() + "inserted at range: " + r.ToString());
                        shipLoss.Insert(r, (currentRes * shipAmount));
                    }
                }
            }
            return shipLoss;
        }

        public MainWindow()
        {
            // Initializes both factions, then updates labels.
            InitializeComponent();
            InitializeFaction(Imperium);
            InitializeFaction(Rebellion);
            UpdateText(ReturnFaction(cBoxFaction.SelectedItem.ToString()));
        }

        private void cBoxFaction_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Updates labels.
            UpdateText(ReturnFaction(cBoxFaction.SelectedItem.ToString()));
        }

        private void btnBerechnen_Click(object sender, RoutedEventArgs e)
        {
            // Since Labels and TextBoxes have a Resource Identifier, we use resouces string[] to 
            // identify all relevant labels + textboxes and set them to be visible.
            string[] resources = { "Dura", "Tiba", "Kris", "Creds", "EZ" };
            Faction currentFaction = ReturnFaction(cBoxFaction.SelectedItem.ToString());
            lblProfit.Visibility = Visibility.Visible; // This is a message label that tells end user the program is working.
            List<int> shipLosses = CalculateShipLosses(currentFaction);
            List<int> resProfit = CalculateResources(resources);
            
            // At this final stage, we take the profit from gained resources
            // .. and subtract that from ship losses.
            // Final result will be total profit.
            for (int i = 0; i < 5; i++)
            {
                int currentRes = resProfit[i];
                int currentShipLoss = shipLosses[i];
                TextBox currentResBox = (TextBox)FindName((resources[i] + "Win"));
                currentResBox.Text = (currentRes - currentShipLoss).ToString();
                if (int.Parse(currentResBox.Text) >= 0)
                    currentResBox.Foreground = Brushes.Green;
                else
                    currentResBox.Foreground = Brushes.Red;
            }
            // Reveal labels and textboxes, hide message
            lblProfit.Visibility = Visibility.Hidden;
            foreach (string res in resources)
            {
                Label lbl = (Label)FindName(("lbl" + res + "Win"));
                TextBox winBox = (TextBox)FindName((res + "Win"));
                lbl.Visibility = Visibility.Visible;
                winBox.Visibility = Visibility.Visible;
            }
        }
    }
}
