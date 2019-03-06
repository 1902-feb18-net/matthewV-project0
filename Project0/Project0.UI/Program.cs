using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; //for serialization of dictionary
using Project0.DataAccess;
using Project0.Library.DAORepositories;
using Project0.Library.Models;
using Project0.Library.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace Project0.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var saveLocation = "../../../../json.txt";
            string input;
            List<PizzaStore> stores;
            List<Orders> orders;
            List<Library.Models.Customer> customers;
            //List<Inventory> storeInventory;
            var dataSource = new List<PizzaStore>();  //for initializing repo

            var optionsBuilder = new DbContextOptionsBuilder<project0Context>();
            optionsBuilder.UseSqlServer(secretConfiguration.ConnectionString); //pass options builder what sql server to use and login info
            var options = optionsBuilder.Options;
            var dbContext = new project0Context(options);

            var storeRepository = new PizzaStoreRepository(dbContext);
            var orderRepository = new OrderRepo(dbContext);
            var customerRepository = new CustomerRepo(dbContext);
            //var inventoryRepository = new InventoryRepo(dbContext);

            //testing serialization worked with multiple stores
            //storeRepository.AddStore(new PizzaStore());
            //storeRepository.AddStore(new PizzaStore());

            while (true) //loop until exit command given
            {
                Console.WriteLine("p:\tDisplay or modify pizza stores.");
                Console.WriteLine("a:\tTo add pizza store.");
                Console.WriteLine("c:\tDisplay or add customers.");
                //Console.WriteLine("o:\tTo place order");
                Console.WriteLine("s:\tSave data to disk.");
                //Console.WriteLine("l:\tLoad data from disk.");
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
                            while (true)
                            {
                                //display chosen stores info
                                var store = stores[storeNum - 1];
                                var storeString = $"ID: {store.Id}, Name: \"{store.Name}\"";
                                //storeString += ", at Location: " + store.Location.ToString();

                                Console.WriteLine(storeString);
                                Console.WriteLine();

                                Console.WriteLine("o:\tDisplay orders.");
                                Console.WriteLine("i:\tDisplay or modify inventory");
                                Console.Write("Enter valid menu option, or b to go back: ");

                                input = Console.ReadLine();
                                if (input == "b")
                                {
                                    break;
                                }
                                else if (input == "o")
                                {
                                    DisplayOrdersOfStore(store.Id);
                                }
                                else if (input == "i")
                                {
                                    if (store.Inventory == null)
                                    {
                                        Console.WriteLine("No inventory.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Inventory: ");
                                        foreach (var item in store.Inventory)
                                        {
                                            Console.WriteLine($"Item Name: \"{item.Key}\", " +
                                                 $"Item Quantity: \"{item.Value}\"");
                                        }
                                    }

                                    while (true)
                                    {
                                        Console.WriteLine("a:\tAdd inventory item.");
                                        Console.Write("Enter valid menu option, or b to go back: ");
                                        input = Console.ReadLine();

                                        if (input == "a")
                                        {
                                            Dictionary<string, int> inv = store.Inventory;
                                            AddInventoryItem(ref inv);
                                            store.Inventory = inv;
                                            storeRepository.UpdateStore(store);
                                        }
                                        else if (input == "b")
                                        {
                                            break;
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Invalid input \"{input}\".");
                            Console.WriteLine();
                        }

                    }
                }
                else if (input == "a")
                {
                    AddNewPizzaStore();
                }
                else if (input == "c")
                {
                    while (true)
                    {
                        DisplayAddOrModifyCustomers();

                        input = Console.ReadLine();
                        if (input == "b")
                        {
                            break;
                        }
                        else if (input == "a")
                        {
                            AddNewCustomer();
                        }
                        else if (int.TryParse(input, out var custNum)
                                && custNum > 0 && custNum <= customers.Count) //if input is a valid store number
                        {
                            while (true)
                            {
                                //display chosen stores info
                                var cust = customers[custNum - 1];
                                var custString = $"ID: {cust.Id}, Name: \"{cust.FirstName} {cust.LastName}\"";

                                Console.WriteLine(custString);

                                Console.WriteLine("c:\tModify customer");
                                Console.Write("Enter valid menu option, or b to go back: ");

                                input = Console.ReadLine();
                                if (input == "b")
                                {
                                    break;
                                }
                                else if(input == "c")
                                {
                                    ////Modify Customer!!!!!!!
                                }

                            }
                        }
                    }
                }
                //else if (input == "o")
                //{

                //}
                else if (input == "s")  //use serializer to save data to file
                {
                    SaveToFile();
                }
                //else if (input == "l") //loading data from file
                //{
                //    LoadFromFile();
                //}
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



            void DisplayAddOrModifyCustomers()
            {
                customers = customerRepository.GetAllCustomer().ToList();
                Console.WriteLine();
                if (customers.Count == 0)
                {
                    Console.WriteLine("No customers.");
                }
                else
                {
                    for (var i = 1; i <= customers.Count; i++)
                    {
                        var customer = customers[i - 1]; //indexing starts at 0
                        var custString = $"{i}: \"{customer.FirstName} {customer.LastName}\" ";
                        //storeString += $", at Location: {store.Location.ToString()}";

                        Console.WriteLine(custString);
                    }
                }

                Console.WriteLine("a:\tAdd customer.");
                Console.WriteLine();
                Console.WriteLine("Enter valid menu option, or \"b\" to go back: ");

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
                    //storeString += $", at Location: {store.Location.ToString()}";

                    Console.WriteLine(storeString);
                }

                Console.WriteLine();
                Console.WriteLine("Enter valid menu option, or \"b\" to go back: ");
            }



            void DisplayOrdersOfStore(int id)
            {
                orders = orderRepository.GetOrdersOfLocation(id).ToList();
                Console.WriteLine();
                if (orders.Count == 0)
                {
                    Console.WriteLine("No orders.");
                }

                for (var i = 1; i <= orders.Count; i++)
                {
                    var order = orders[i - 1]; //indexing starts at 0
                    var ordString = $"{i}: Store\\: \"{order.StoreId}\", Customer\\: " +
                        $"\"{order.CustomerId}\", Time\\: \"{order.OrderTime}";

                    Console.WriteLine(ordString);
                }

            }



            PizzaStore AddNewPizzaStore()
            {
                Console.WriteLine();

                string name = null;
                //Library.Models.Address address = null;
                Dictionary<string, int> inventory = null;

                while (true)
                {
                    Console.Write("Would you like to use the default name? (y/n)");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
                        break;
                    }
                    else if (input == "n")
                    {
                        Console.Write("Enter the new pizza restaurant's name: ");
                        name = Console.ReadLine();
                        break;
                    }
                }

                //while (true)
                //{
                //    Console.Write("Would you like to use a default location? (y/n)");
                //    input = Console.ReadLine();

                //    if (input == "y")
                //    {
                //        break;
                //    }
                //    else if (input == "n")
                //    {
                //        address = NewAddress();
                //        break;
                //    }
                //}

                while (true)
                {
                    Console.Write("Would you like to use the default inventory? (y/n)");
                    input = Console.ReadLine();

                    if (input == "y")
                    {
                        break;
                    }
                    else if (input == "n")
                    {
                        inventory = NewInventory();
                        break;
                    }
                }

                //PizzaStore restaurant = new PizzaStore(name, inventory, address);
                PizzaStore restaurant = new PizzaStore(name, inventory);

                storeRepository.AddStore(restaurant);
                return restaurant;
            }

            void AddNewCustomer()
            {
                Console.WriteLine();

                Console.Write("Enter the new customer's firstname: ");
                string firstname = Console.ReadLine();
                Console.Write("Enter the new customer's lastname: ");
                string lastname = Console.ReadLine();

                PizzaStore store; 

                while (true)
                {
                    Console.Write("Would you like to use the default store? (y/n)");
                    input = Console.ReadLine();

                    if (input == "y")
                    {
                        store = new PizzaStore();
                        break;
                    }
                    else if (input == "n")
                    {
                        while (true)
                        {
                            Console.Write("Would you like to use a new store? (y/n)");
                            input = Console.ReadLine();
                            if (input == "y")
                            {
                                store = AddNewPizzaStore();
                                break;
                            }
                            else if (input == "n")
                            {
                                Console.Write("Enter existing store id: ");
                                input = Console.ReadLine();

                                store = storeRepository.GetStoreById(Convert.ToInt32(input));
                                break;
                            }
                        }

                        break;
                    }
                }

                Library.Models.Customer cust = new Library.Models.Customer(firstname, lastname, store);

                customerRepository.AddCustomer(cust);

            }


            void AddInventoryItem(ref Dictionary<string, int> inv)
            {
                Console.WriteLine("Enter the new item's name: ");
                string name = Console.ReadLine();
                Console.WriteLine("Enter the new item's amount: ");
                int amount = Convert.ToInt32(Console.ReadLine());
                inv.Add(name, amount);
                Console.WriteLine("Added");
            }

            Dictionary<string, int> NewInventory()
            {
                Dictionary<string, int> inventory = new Dictionary<string, int>();

                while (true)
                {
                    Console.Write("Would you like to add an inventory item? (y/n)");
                    input = Console.ReadLine();

                    if (input == "y")
                    {
                        try
                        {
                            AddInventoryItem(ref inventory);
                        }
                        catch (ArgumentException e)
                        {
                            //log it

                            Console.WriteLine(e.Message);
                            return inventory;
                        }
                    }
                    else if (input == "n")
                    {
                        break;
                    }
                }

                return inventory;
            }








            //Library.Models.Address NewAddress()
            //{
            //    string street, city, state, zipcode, country;
            //    Console.Write("Enter the new pizza restaurant's location - street: ");
            //    street = Console.ReadLine();
            //    Console.Write("Enter the new pizza restaurant's location - city: ");
            //    city = Console.ReadLine();
            //    Console.Write("Enter the new pizza restaurant's location - state: ");
            //    state = Console.ReadLine();
            //    Console.Write("Enter the new pizza restaurant's location - zipcode: ");
            //    zipcode = Console.ReadLine();
            //    Console.Write("Enter the new pizza restaurant's location - country: ");
            //    country = Console.ReadLine();
            //    try
            //    {
            //        return new Library.Models.Address(street, city, state, zipcode, country);
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    return null;
            //}

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

            //void LoadFromFile()
            //{
            //    Console.WriteLine();
            //    try
            //    {
            //        var deserialized = new List<PizzaStore>();
            //        deserialized = JsonConvert.DeserializeObject<List<PizzaStore>>(File.ReadAllText(saveLocation));
            //        if (deserialized != null)
            //        {
            //            deserialized.ToList();
            //        }

            //        Console.WriteLine("Loading Success.");
            //        var allStores = storeRepository.GetAllStores().ToList();

            //        foreach (var item in allStores) //delete current repo one restaraunt at a time
            //        {
            //            //try
            //            //{
            //                storeRepository.DeleteStore(item); //throws error!
            //            //}
            //            //catch (ArgumentOutOfRangeException e)
            //            //{
            //            //    Console.WriteLine(e.Message);
            //            //}
            //        }

            //        if (deserialized != null)
            //        {
            //            foreach (var item in deserialized) //replace with repo data from file
            //            {
            //                storeRepository.AddStore(item);
            //            }
            //        }
            //    }
            //    catch (FileNotFoundException) //if no save file
            //    {
            //        Console.WriteLine("No saved data found.");
            //    }
            //    catch (SecurityException ex)
            //    {
            //        Console.WriteLine($"Error while loading: {ex.Message}");
            //    }
            //    catch (IOException ex)
            //    {
            //        Console.WriteLine($"Error while loading: {ex.Message}");
            //    }
            //    //other exceptions?
            //}
        }

    }
}
