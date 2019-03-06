using DataAccessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        //create logger factory for all instances of context, using nlogger
        //public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new NLogLoggerProvider() }); 
        public static readonly LoggerFactory ConsoleLogger = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        static void Main(string[] args)
        {
            //var saveLocation = "../../../../json.txt";
            string input; //for reading console input
            //for displaying the returned objects
            List<Location> stores;
            List<Orders> orders;
            List<Customer> customers;
            List<Pizza> pizzas;

            //create database context
            var optionsBuilder = new DbContextOptionsBuilder<project0Context>();
            optionsBuilder.UseSqlServer(secretConfiguration.ConnectionString); //pass options builder what sql server to use and login info
            //optionsBuilder.UseLoggerFactory(MyLoggerFactory); //for logging 
            //optionsBuilder.UseLoggerFactory(ConsoleLogger); //for logging to console
            var options = optionsBuilder.Options;
            var dbContext = new project0Context(options);

            Repository repo = new Repository(dbContext); //initialize repo with context

            while (true) //loop until exit command given
            {
                Console.WriteLine("p:\tDisplay or modify pizza stores.");
                Console.WriteLine("a:\tTo add pizza store.");
                Console.WriteLine("c:\tDisplay, add or modify customers.");
                Console.WriteLine("o:\tDisplay or place orders.");
                Console.WriteLine("i:\tDisplay or add pizza.");
                Console.WriteLine("n:\tSearch customer by name.");
                Console.WriteLine("s:\tSuggest order for customer.");
                Console.WriteLine("d\tDisplay order statistics");
                //Console.WriteLine("s:\tSave data to disk.");
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
                                var storeString = $"ID: {store.Id}, Name: \"{store.LocationName}\"";

                                Console.WriteLine(storeString);
                                Console.WriteLine();

                                Console.WriteLine("o:\tDisplay orders");
                                Console.WriteLine("r:\tRename store");
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
                                else if (input == "r")
                                {
                                    Console.WriteLine("Enter store's new name: ");
                                    store.LocationName = Console.ReadLine();
                                    repo.Update(store);
                                }
                                else if (input == "i")
                                {

                                    while (true)
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
                                                Console.WriteLine($"Item Id: \"{item.IngredientsId}\", " +
                                                     $"Item Name: \"{repo.GetIngredientById(item.IngredientsId).Name}\", " +
                                                     $"Item Quantity: \"{item.Quantity}\"");
                                            }
                                        }
                                        Console.WriteLine("a:\tAdd inventory item");
                                        Console.Write("Enter valid menu option, or b to go back: ");
                                        input = Console.ReadLine();

                                        if (input == "a")
                                        {
                                            AddInventoryItem(ref store);
                                        }
                                        else if (input == "b")
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
                        DisplayCustomers();

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
                                //display chosen customer's info
                                var cust = customers[custNum - 1];
                                var custString = $"ID: {cust.Id}, Name: \"{cust.FirstName} {cust.LastName}\", Email: \"{cust.Email}\"";

                                Console.WriteLine(custString);

                                Console.WriteLine("c:\tModify customer name");
                                Console.WriteLine("e:\tModify customer email");
                                Console.WriteLine("o:\tDisplay orders of customer");
                                Console.Write("Enter valid menu option, or b to go back: ");

                                input = Console.ReadLine();
                                if (input == "b")
                                {
                                    break;
                                }
                                else if (input == "o")
                                {
                                    DisplayOrdersOfCustomer(cust.Id);
                                }
                                else if (input == "c")
                                {
                                    Console.Write("Enter the customer's new firstname: ");
                                    cust.FirstName = Console.ReadLine();
                                    Console.Write("Enter the customer's new lastname: ");
                                    cust.LastName = Console.ReadLine();

                                    repo.Update(cust);
                                }
                                else if (input == "e")
                                {
                                    Console.Write("Enter the customer's new email: ");
                                    cust.Email = Console.ReadLine();

                                    repo.Update(cust);
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($"Invalid input \"{input}\".");
                                    Console.WriteLine();
                                }

                            }
                        }
                    }
                }
                else if (input == "o")
                {
                    while (true)
                    {
                        DisplayOrders();

                        input = Console.ReadLine();
                        if (input == "b")
                        {
                            break;
                        }
                        else if (input == "d")
                        {
                            Console.WriteLine("1:\tSorted Earliest");
                            Console.WriteLine("2:\tSorted Latest");
                            Console.WriteLine("3:\tSorted Cheapest");
                            Console.WriteLine("4:\tSorted Expensive");
                            Console.Write("Enter valid menu option, or b to go back: ");
                            input = Console.ReadLine();
                            if (input == "1")
                            {
                                DisplayOrdersInList(new List<Orders>(repo.GetOrdersSortedEarliest()));
                                Console.Write("Press Enter to continue: ");
                                Console.ReadLine();
                            }
                            else if (input == "2")
                            {
                                DisplayOrdersInList(new List<Orders>(repo.GetOrdersSortedLatest()));
                                Console.Write("Press Enter to continue: ");
                                Console.ReadLine();
                            }
                            else if (input == "3")
                            {
                                DisplayPricedOrders(repo.GetOrdersSortedCheapest());
                                Console.Write("Press Enter to continue: ");
                                Console.ReadLine();
                            }
                            else if (input == "4")
                            {
                                DisplayPricedOrders(repo.GetOrdersSortedExpensive());
                                Console.Write("Press Enter to continue: ");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Invalid input \"{input}\".");
                                Console.WriteLine();
                            }

                        }
                        else if (input == "p")
                        {
                            //add order
                            PlaceOrder();
                        }
                        else if (int.TryParse(input, out var orderNum)
                              && orderNum > 0 && orderNum <= orders.Count) //if input is a valid store number
                        {
                            var order = orders[orderNum - 1];
                            var cust = repo.GetCustomerById(order.CustomerId);
                            var store = repo.GetLocationById(order.StoreId);
                            var ordString = $"Id: {order.Id}, Customer: \"{cust.FirstName} {cust.LastName}\", " +
                                $"Store: \"{store.LocationName}\", Time: {order.OrderTime}";
                            Console.WriteLine(ordString);

                            while (true)
                            {
                                Console.Write("Enter b to go back: ");
                                input = Console.ReadLine();
                                if (input == "b")
                                {
                                    break;
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
                else if (input == "i")
                {
                    while (true)
                    {
                        DisplayPizzas();

                        input = Console.ReadLine();
                        if (input == "b")
                        {
                            break;
                        }
                        else if (input == "a")
                        {
                            AddPizza();
                        }
                        else if (int.TryParse(input, out var pizzaNum)
                                && pizzaNum > 0 && pizzaNum <= pizzas.Count) //if input is a valid pizza number
                        {
                            //display chosen pizza's info
                            var pizza = pizzas[pizzaNum - 1];

                            //get ingredients information as well!?
                            var pizString = $"ID: {pizza.Id}, Price: \"{pizza.Price}\"";
                            Console.WriteLine(pizString);

                            Console.Write("Enter b to go back: ");

                            input = Console.ReadLine();
                            if (input == "b")
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

                    }
                }
                else if (input == "n")
                {
                    Console.WriteLine("Enter customer's first name: ");
                    var custs = new List<Customer>(repo.GetCustomersByFirstName(Console.ReadLine()));

                    if (custs.Count == 0)
                    {
                        Console.WriteLine("No match found");
                    }
                    else
                    {
                        foreach (var cust in custs)
                        {
                            Console.WriteLine("ID: " + cust.Id + ", Name: " + cust.FirstName + " " + cust.LastName + ", Email: " + cust.Email);
                        }
                        Console.WriteLine();
                    }

                }
                //else if (input == "s")  //use serializer to save data to file
                //{
                //    SaveToFile();
                //}
                else if (input == "s")
                {
                    bool err = false;
                    while (!err)
                    {
                        Console.WriteLine("Enter customer's id: ");
                        input = Console.ReadLine();
                        int custId;
                        while (!int.TryParse(input, out custId))
                        {
                            Console.WriteLine("Enter customer's id: ");
                            input = Console.ReadLine();
                        }
                        List<OrderItems> suggested = repo.SuggestOrder(custId);
                        foreach (var item in suggested)
                        {
                            Console.WriteLine($"PizzaID: {item.PizzaId}, Quantity: {item.Quantity}");
                        }

                        //while (true)
                        //{
                        Console.WriteLine("Would you like to order this? (y/n)");
                        input = Console.ReadLine();
                        if (input == "y")
                        {
                            Console.WriteLine("Enter new order's store id: ");
                            input = Console.ReadLine();
                            int storeId;
                            while (!int.TryParse(input, out storeId))
                            {
                                Console.WriteLine("Enter new order's store id: ");
                                input = Console.ReadLine();
                            }
                            Orders ord = new Orders();
                            ord.CustomerId = custId;
                            ord.OrderTime = DateTime.Now;
                            ord.StoreId = storeId;

                            try
                            {
                                repo.Add(ord);
                            }
                            catch (DbUpdateException)
                            {
                                Console.WriteLine("There was an error updating the database - add order");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                break;
                            }

                            foreach (var item in suggested)
                            {
                                OrderItems newItem = new OrderItems();
                                newItem.OrderId = ord.Id;
                                newItem.PizzaId = item.PizzaId;
                                newItem.Quantity = item.Quantity;
                                repo.Add(newItem);
                            }
                        }
                        else if (input == "n")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Invalid input \"{input}\".");
                            Console.WriteLine();
                        }
                        //}
                    }
                }
                else if (input == "d")
                {
                    DisplayOrderStatistics();
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





            void DisplayOrderStatistics()
            {
                (int, int) buyCust = repo.GetOrdersStatistics();
                Console.WriteLine($"Customer ID: {buyCust.Item1} has the most orders, at {buyCust.Item2} total orders.");
                Console.WriteLine();
            }



            void DisplayPizzas()
            {
                pizzas = new List<Pizza>(repo.GetAllPizza());
                Console.WriteLine();

                if (pizzas.Count == 0)
                {
                    Console.WriteLine("No pizzas.");
                }
                else
                {
                    for (var i = 1; i <= pizzas.Count; i++)
                    {
                        var pizza = pizzas[i - 1]; //indexing starts at 0
                        var pizString = $"{i}: ID: {pizza.Id}, Price: {pizza.Price} ";

                        Console.WriteLine(pizString);
                    }

                }

                Console.WriteLine();
                Console.WriteLine("Enter valid menu option, or \"b\" to go back: ");

            }

            void AddPizza()
            {
                Pizza pizza = new Pizza();
                bool read = false;
                while (!read)
                {
                    try
                    {
                        Console.WriteLine("Enter pizza price: ");
                        pizza.Price = Convert.ToDecimal(Console.ReadLine());
                        read = true;
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                repo.Add(pizza);

                while (true)
                {
                    Console.WriteLine("Would you like to add an ingredient? (y/n)");
                    input = Console.ReadLine();
                    if (input == "n")
                    { break; }
                    else if (input == "y")
                    {
                        Ingredients ingredient = new Ingredients();
                        Console.WriteLine("Enter new ingredient name: ");
                        ingredient.Name = Console.ReadLine();
                        repo.Add(ingredient);

                        PizzaIngredients pizzaingredient = new PizzaIngredients();
                        pizzaingredient.PizzaId = pizza.Id;
                        pizzaingredient.IngredientsId = ingredient.Id;

                        Console.WriteLine("Enter amount of ingredient on pizza: ");
                        pizzaingredient.Quantity = Convert.ToInt32(Console.ReadLine());
                        repo.Add(pizzaingredient);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Invalid input \"{input}\".");
                        Console.WriteLine();
                    }

                }
            }

            void PlaceOrder()
            {
                Orders order = new Orders();
                Console.WriteLine("Enter Customer ID");
                order.CustomerId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter Store ID");
                order.StoreId = Convert.ToInt32(Console.ReadLine());
                order.OrderTime = DateTime.Now;

                //check time constraint 
                try
                {
                    repo.Add(order);
                    AddOrderItems(ref order);
                }
                catch (DbUpdateException)
                {
                    Console.WriteLine("There was an error updating the database.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            void AddOrderItems(ref Orders order)
            {
                //have to add one item to order
                OrderItems item = new OrderItems
                {
                    OrderId = order.Id
                };
                Console.WriteLine("Enter Pizza ID");
                item.PizzaId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter Quantity");
                item.Quantity = Convert.ToInt32(Console.ReadLine());

                repo.Add(item);

                while (true)
                {
                    Console.WriteLine("Would you like to add another item? (y/n)");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
                        OrderItems itemN = new OrderItems
                        {
                            OrderId = order.Id
                        };
                        Console.WriteLine("Enter Pizza ID");
                        itemN.PizzaId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter Quantity");
                        itemN.Quantity = Convert.ToInt32(Console.ReadLine());

                        repo.Add(itemN);
                    }
                    else if (input == "n")
                    {
                        break;
                    }
                }

            }

            void DisplayOrders()
            {
                orders = new List<Orders>(repo.GetAllOrders());
                Console.WriteLine();

                if (orders.Count == 0)
                {
                    Console.WriteLine("No orders.");
                    Console.WriteLine();
                }
                else
                {
                    for (var i = 1; i <= orders.Count; i++)
                    {
                        var order = orders[i - 1]; //indexing starts at 0
                        var ordString = $"{i}: ID: {order.Id}, Time: {order.OrderTime} ";

                        Console.WriteLine(ordString);
                    }

                }

                Console.WriteLine("d:\tDisplay orders sorted");
                Console.WriteLine("p:\tPlace order");
                Console.WriteLine();
                Console.WriteLine("Enter valid menu option, or \"b\" to go back: ");

            }

            void DisplayPricedOrders(List<(decimal, Orders)> sortedOrd)
            {
                Console.WriteLine();

                if (sortedOrd.Count == 0)
                {
                    Console.WriteLine("No orders.");
                    Console.WriteLine();
                }
                else
                {
                    for (var i = 1; i <= sortedOrd.Count; i++)
                    {
                        var order = sortedOrd[i - 1]; //indexing starts at 0
                        var ordString = $"ID: {order.Item2.Id}, Time: {order.Item2.OrderTime}, Total Price: {order.Item1} ";

                        Console.WriteLine(ordString);
                    }
                }
            }

            void DisplayOrdersInList(List<Orders> orderList)
            {
                Console.WriteLine();

                if (orderList.Count == 0)
                {
                    Console.WriteLine("No orders.");
                    Console.WriteLine();
                }
                else
                {
                    for (var i = 1; i <= orderList.Count; i++)
                    {
                        var order = orderList[i - 1]; //indexing starts at 0
                        var ordString = $"ID: {order.Id}, Time: {order.OrderTime} ";

                        Console.WriteLine(ordString);
                    }
                }
            }

            void DisplayCustomers()
            {
                customers = new List<Customer>(repo.GetAllCustomers());
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

                        Console.WriteLine(custString);
                    }
                }

                Console.WriteLine("a:\tAdd customer");
                Console.WriteLine();
                Console.WriteLine("Enter valid menu option, or \"b\" to go back: ");

            }



            void DisplayOrModifyStores()
            {
                stores = new List<Location>(repo.GetAllLocation());
                Console.WriteLine();
                if (stores.Count == 0)
                {
                    Console.WriteLine("No pizza stores.");
                }

                for (var i = 1; i <= stores.Count; i++)
                {
                    var store = stores[i - 1]; //indexing starts at 0
                    var storeString = $"{i}: \"{store.LocationName}\"";
                    //storeString += $", at Location: {store.Location.ToString()}";

                    Console.WriteLine(storeString);
                }

                Console.WriteLine();
                Console.WriteLine("Enter valid menu option, or \"b\" to go back: ");
            }



            void DisplayOrdersOfStore(int id)
            {
                orders = new List<Orders>(repo.GetOrdersOfLocation(id));
                Console.WriteLine();
                if (orders.Count == 0)
                {
                    Console.WriteLine("No orders.");
                    Console.WriteLine();
                }

                for (var i = 1; i <= orders.Count; i++)
                {
                    var order = orders[i - 1]; //indexing starts at 0
                    var ordString = $"StoreId: {order.StoreId}, CustomerId: " +
                        $"{order.CustomerId}, Time: {order.OrderTime}";

                    Console.WriteLine(ordString);
                    Console.WriteLine();
                }

            }

            void DisplayOrdersOfCustomer(int id)
            {
                orders = new List<Orders>(repo.GetOrdersOfCustomer(id));
                Console.WriteLine();
                if (orders.Count == 0)
                {
                    Console.WriteLine("No orders.");
                    Console.WriteLine();
                }

                for (var i = 1; i <= orders.Count; i++)
                {
                    var order = orders[i - 1]; //indexing starts at 0
                    var ordString = $"StoreId: {order.StoreId}, CustomerId: " +
                        $"{order.CustomerId}, Time: {order.OrderTime}";

                    Console.WriteLine(ordString);
                    Console.WriteLine();
                }

            }


            Location AddNewPizzaStore()
            {
                Console.WriteLine();

                Location restaurant = new Location();

                while (true)
                {
                    Console.Write("Would you like to use the default name? (y/n)");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
                        restaurant.LocationName = "Matthew's Pizzeria";
                        break;
                    }
                    else if (input == "n")
                    {
                        Console.Write("Enter the new pizza restaurant's name: ");
                        restaurant.LocationName = Console.ReadLine();
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Invalid input \"{input}\".");
                        Console.WriteLine();
                    }
                }

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
                        NewInventory(ref restaurant);
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Invalid input \"{input}\".");
                        Console.WriteLine();
                    }
                }


                repo.Add(restaurant);
                return restaurant;
            }

            void AddNewCustomer()
            {
                Console.WriteLine();

                Console.Write("Enter the new customer's firstname: ");
                string firstname = Console.ReadLine();
                Console.Write("Enter the new customer's lastname: ");
                string lastname = Console.ReadLine();
                Console.Write("Enter the new customer's email: ");
                string email = Console.ReadLine();

                Location store;

                //while (true)
                //{
                //Console.Write("Would you like to use the default store? (y/n)");
                //input = Console.ReadLine();

                //if (input == "y")
                //{
                //    store = new Location();
                //    break;
                //}
                //else if (input == "n")
                //{
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

                        store = repo.GetLocationById(Convert.ToInt32(input));
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Invalid input \"{input}\".");
                        Console.WriteLine();
                    }
                }

                //break;
                //}
                //}

                Customer cust = new Customer
                {
                    FirstName = firstname,
                    LastName = lastname,
                    Email = email
                };

                repo.Add(cust);

            }


            void AddInventoryItem(ref Location store)
            {
                try
                {
                    Inventory inv = new Inventory();
                    inv.StoreId = store.Id;

                    Ingredients ing = new Ingredients();
                    Console.WriteLine("Enter the new item's name: ");
                    ing.Name = Console.ReadLine();
                    repo.Add(ing);

                    inv.IngredientsId = ing.Id;
                    Console.WriteLine("Enter the new item's amount: ");
                    inv.Quantity = Convert.ToInt32(Console.ReadLine());

                    repo.Add(inv);

                    Console.WriteLine("Added");
                }
                catch (ArgumentException e)
                {
                    //log it

                    Console.WriteLine(e.Message);
                }
            }

            void NewInventory(ref Location store)
            {
                while (true)
                {
                    Console.Write("Would you like to add an inventory item? (y/n)");
                    input = Console.ReadLine();

                    if (input == "y")
                    {
                        AddInventoryItem(ref store);
                    }
                    else if (input == "n")
                    {
                        Console.WriteLine("Using default inventory");

                        //replace this with function call to repo
                        Inventory inv = new Inventory();
                        inv.StoreId = store.Id;
                        Ingredients ing = new Ingredients();
                        ing.Name = "Cheese";
                        repo.Add(ing);
                        inv.IngredientsId = ing.Id;
                        inv.Quantity = 5;
                        repo.Add(inv);

                        Inventory inv2 = new Inventory();
                        inv2.StoreId = store.Id;
                        Ingredients ing2 = new Ingredients();
                        ing2.Name = "Dough";
                        repo.Add(ing2);
                        inv.IngredientsId = ing2.Id;
                        inv2.Quantity = 5;
                        repo.Add(inv2);
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

            //void SaveToFile()
            //{
            //    Console.WriteLine();

            //    stores = new List<Location>(repo.GetAllLocation()); //get list of all pizza stores

            //    try
            //    {
            //        var serialized = JsonConvert.SerializeObject(stores, Formatting.Indented); //serialize to the file                  
            //        File.WriteAllTextAsync(saveLocation, serialized);
            //        Console.WriteLine("Saving Success.");
            //    }
            //    catch (SecurityException ex)
            //    {
            //        Console.WriteLine($"Error while saving: {ex.Message}");
            //    }
            //    catch (IOException ex)
            //    {
            //        Console.WriteLine($"Error while saving: {ex.Message}");
            //    }
            //    //other exceptions
            //}


        }



    }
}
