using Project0.Library.Models;
using Project0.Library.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Serialization;

namespace Project0.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataSource = new List<PizzaStore>();  //for initializing repo
            var restaurantRepository = new PizzaStoreRepository(dataSource);
            var serializer = new XmlSerializer(typeof(List<PizzaStore>));  //need to be able to save/load from file.
            restaurantRepository.AddRestaurant(new PizzaStore());
            restaurantRepository.AddRestaurant(new PizzaStore());

            while (true) //loop until exit command given
            {
                Console.WriteLine("s:\tSave data to disk.");
                Console.WriteLine("l:\tLoad data from disk.");
                Console.WriteLine(); //extra white space for readability
                Console.Write("Enter valid menu option, or \"exit\" to quit: ");
                var input = Console.ReadLine(); //read command from console

                if (false) //still need other use cases
                {

                }
                else if (input == "s")  //use serializer to save data to file
                {
                    SaveToFile();                 
                }
                else if (input == "l") //loading data from file
                {
                    LoadFromFile();   
                }
                else if (input == "exit") //exits loop and therefore program
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invalid input \"{input}\".");
                }
            }

            void SaveToFile()
            {
                Console.WriteLine();
                var restaurants = restaurantRepository.GetRestaurants().ToList(); //get list of all pizza restaurants
                try
                {
                    using (var stream = new FileStream("data.xml", FileMode.Create)) //create/overwrite file data.xml in current directory
                    {
                        serializer.Serialize(stream, restaurants); //serialize to the file
                    }
                    Console.WriteLine("Saving Success.");
                }
                catch (SecurityException ex)
                {
                    Console.WriteLine($"Error while saving: {ex.Message}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error while saving: {ex.Message}");
                }
                //more exceptions possible!
            }

            void LoadFromFile()
            {
                Console.WriteLine();
                List<PizzaStore> restaurants;
                try
                {
                    using (var stream = new FileStream("data.xml", FileMode.Open)) //reads from data.xml in current directory
                    {
                        restaurants = (List<PizzaStore>)serializer.Deserialize(stream);
                    }
                    Console.WriteLine("Loading Success.");
                    foreach (var item in restaurantRepository.GetRestaurants()) //delete current repo one restaraunt at a time
                    {
                        restaurantRepository.DeleteRestaurant(item.Id);
                    }
                    foreach (var item in restaurants) //replace with repo data from file
                    {
                        restaurantRepository.AddRestaurant(item);
                    }
                }
                catch (FileNotFoundException) //if no save file
                {
                    Console.WriteLine("No saved data found.");
                }
                catch (SecurityException ex)
                {
                    Console.WriteLine($"Error while loading: {ex.Message}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error while loading: {ex.Message}");
                }
                //other exceptions?
            }
        }
 
    }
}
