using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_desktop
{
    internal class Statisztika
    {
        private List<Car> cars;
        private MySqlConnection connection;

        internal List<Car> Cars { get => cars; set => cars = value; }

        public Statisztika()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.Port = 3306;
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "carrental";
            connection = new MySqlConnection(builder.ConnectionString);
        }

        private void OpenConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
            } catch (MySqlException ex)
            {
                throw new Exception("Hiba történt a kapcsolódás során: " + ex.Message);
            }
        }

        private void CloseConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Hiba történt a kapcsolódás során: " + ex.Message);
            }
        }

        public void LoadCars()
        {
            cars = new List<Car>();

            OpenConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, license_plate_number, brand, model, daily_cost FROM cars";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string license_plate_number = reader.GetString("license_plate_number");
                    string brand = reader.GetString("brand");
                    string model = reader.GetString("model");
                    int daily_cost = reader.GetInt32("daily_cost");
                    Car currentCar = new Car(id, license_plate_number, brand, model, daily_cost);
                    cars.Add(currentCar);
                }
            }
            CloseConnection();
        }

        public bool Delete(int id)
        {
            OpenConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM cars WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);

            int affectedRows = command.ExecuteNonQuery();
            CloseConnection();
            return affectedRows == 1;
        }

        public int CheaperThan20k()
        {
            int count = 0;
            foreach (Car item in Cars)
            {
                if (item.Daily_cost < 20000)
                {
                    count++;
                }
            }
            return count;
        }

        public bool MoreExpensiveThan26k()
        {
            foreach (Car item in Cars)
            {
                if (item.Daily_cost > 26000)
                {
                    return true;
                }
            }
            return false;
        }

        public Car MostExpensive()
        {
            Car mostExpensiveCar = Cars[0];
            foreach(Car item in Cars)
            {
                if (item.Daily_cost > mostExpensiveCar.Daily_cost)
                {
                    mostExpensiveCar = item;
                }
            }
            return mostExpensiveCar;
        }

        public Dictionary<string, int> BrandGroups()
        {
            Dictionary<string, int> carsByBrand = new Dictionary<string, int>();
            foreach (Car item in Cars)
            {
                if (carsByBrand.ContainsKey(item.Brand))
                {
                    carsByBrand[item.Brand]++;
                } else
                {
                    carsByBrand.Add(item.Brand, 1);
                }
            }
            return carsByBrand;
        }

        public string SelectedCarDailyCost(string license_plate_number)
        {
            foreach(Car item in Cars)
            {
                if (item.License_plate_number == license_plate_number)
                {
                    if (item.Daily_cost > 25000)
                    {
                        return "A megadott autó napidíja nagyobb mint 25.000 Ft";
                    } else
                    {
                        return "A megadott autónapidíja nem nagyobb mint 25.000 Ft";
                    }
                }
            }
            return "Nincs ilyen autó";
        }
    }
}
