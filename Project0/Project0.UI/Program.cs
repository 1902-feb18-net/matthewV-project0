using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; //for serialization of dictionary
using Project0.DataAccess;
using Project0.Library.Models;
using Project0.Library.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
//using System.Xml.Serialization; //doesn't serialize dictionaries

namespace Project0.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var saveLocation = "../../../../json.txt";
            string input;
            List<PizzaStore> stores;
            var dataSource = new List<PizzaStore>();  //for initializing repo
            var storeRepository = new PizzaStoreRepository(dataSource);

            var optionsBuilder = new DbContextOptionsBuilder<project0Context>();
            optionsBuilder.UseSqlServer(secretConfiguration.ConnectionString); //pass options builder what sql server to use and login info
            var options = optionsBuilder.Options;

            //testing serialization worked with multiple stores
            storeRepository.AddStore(new PizzaStore());
            storeRepository.AddStore(new PizzaStore());

            while (true) //loop until exit command given
            {
                Console.WriteLine("p:\tDisplay or modify pizza stores.");
                Console.WriteLine("s:\tSave data to disk.");
                Console.WriteLine("l:\tLoad data from disk.");
                Console.Write("Enter valid menu option, or \"exit\" to quit: ");
                input = Console.ReadLine(); //read command from console

                if (input == "p") //still need other use cases
                {
                    while (true)
                    {
                        DisplayOrModifyStores();
                        input = Console.ReadLine();

                        if (input == "b")
                        {
                            break;
                        }
                        else if (int.TryParse(input, out var storeNum)
                                && storeNum > 0 && storeNum <= stores.Count) //if input is a valid store number
                        {
                            //display chosen stores info
                            var store = stores[storeNum - 1];
                            var storeString = $"\"{store.Name}\"";
                            storeString += ", at Location: " + store.Location.ToString();
                            Console.WriteLine(storeString);
                            Console.WriteLine();

                            //further operations!

                            break;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Invalid input \"{input}\".");
                            Console.WriteLine();
                        }

                    }
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
                    Console.WriteLine();
                }
            }

            void DisplayOrModifyStores()
            {
                stores = storeRepository.GetStores().ToList();
                Console.WriteLine();
                if (stores.Count == 0)
                {
                    Console.WriteLine("No pizza stores.");
                }

                for (var i = 1; i <= stores.Count; i++)
                {
                    var store = stores[i - 1]; //indexing starts at 0
                    var storeString = $"{i}: \"{store.Name}\"";
                    storeString += $", at Location: {store.Location.ToString()}";

                    Console.WriteLine(storeString);
                }

                Console.WriteLine();
                Console.WriteLine("Enter valid menu option, or \"b\" to go back: ");
            }


            void SaveToFile()
            {
                Console.WriteLine();

                stores = storeRepository.GetStores().ToList(); //get list of all pizza stores

                try
                {
                    var serialized = JsonConvert.SerializeObject(stores, Formatting.Indented); //serialize to the file                  
                    File.WriteAllTextAsync(saveLocation, serialized);
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
                //other exceptions
            }

            void LoadFromFile()
            {
                Console.WriteLine();
                //List<PizzaStore> stores;
                try
                {
                    var deserialized = new List<PizzaStore>();
                    deserialized = JsonConvert.DeserializeObject<List<PizzaStore>>(File.ReadAllText(saveLocation));

                    //using (var stream = new FileStream("data.xml", FileMode.Open)) //reads from data.xml in current directory
                    //{
                    //    stores = (List<PizzaStore>)serializer.Deserialize(stream);
                    //}
                    Console.WriteLine("Loading Success.");
                    foreach (var item in storeRepository.GetStores()) //delete current repo one restaraunt at a time
                    {
                        storeRepository.DeleteStore(item.Id);
                    }
                    foreach (var item in deserialized) //replace with repo data from file
                    {
                        storeRepository.AddStore(item);
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
