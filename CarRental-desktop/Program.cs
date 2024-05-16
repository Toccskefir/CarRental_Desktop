using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarRental_desktop
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Contains("--stat"))
            {
                Statisztika stat = new Statisztika();
                try
                {
                    stat.LoadCars();
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(1);
                }
                
                Console.WriteLine("20.000 Ft-nál olcsóbb napidíjú autók száma: " + stat.CheaperThan20k());

                if (stat.MoreExpensiveThan26k())
                {
                    Console.WriteLine("Van az adatok között 26.000 Ft-nál drágább napidíjú autó");
                } else
                {
                    Console.WriteLine("Nincs az adatok között 26.000 Ft-nál drágább napidíjú autó");
                }

                Car mostExpensiveCar = stat.MostExpensive();
                Console.WriteLine($"Legdrágább napidíjú autó: {mostExpensiveCar.License_plate_number} - {mostExpensiveCar.Brand} – {mostExpensiveCar.Model} – {mostExpensiveCar.Daily_cost} Ft");

                Console.WriteLine("Autók száma:");
                foreach(KeyValuePair<string, int> item in stat.BrandGroups())
                {
                    Console.WriteLine($"\t{item.Key}: {item.Value}");
                }

                Console.Write("Adjon meg egy rendszámot: ");
                string licensePlate = Console.ReadLine();
                Console.WriteLine(stat.SelectedCarDailyCost(licensePlate));
            }
            else
            {
                new Application().Run(new MainWindow());
            }
        }
    }
}
