using DataAccessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp2
{
    class Repository
    {
        private readonly project0Context Context;

        public Repository(project0Context dbcontext)
        {
            Context = dbcontext;
        }


        public IEnumerable<Customer> GetAllCustomers()
        {
            return Context.Customer; //implicit upcasting from DbSet
        }


        public Customer GetCustomerById(int id)
        {
            return Context.Customer.Find(id); //may return null, if it doesn't exist in db
        }



        public IEnumerable<Customer> GetCustomersByFirstName(string name)
        {
            if (name is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot search null FirstName"); //First Name is NOT NULL
            }
            else
            {
                return Context.Customer.Where(cust => cust.FirstName == name);
            }
        }

        public IEnumerable<Customer> GetCustomersByFullName(string name)
        {
            if (name is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot search null Full Name"); //First and last name are NOT NULL
            }
            else
            {
                return Context.Customer.Where(cust => (cust.FirstName + cust.LastName) == name);
            }

        }

        public void Add(Customer customer)
        {
            if (customer is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null customer");
            }
            else
            {
                if (GetCustomerById(customer.Id) != null) //if given customer is already in db
                {
                    throw new ArgumentOutOfRangeException("Customer with given id already exists.");
                }
                else
                {
                    try
                    {
                        Context.Customer.Add(customer); //add to local context
                        Context.SaveChanges();  //run context.SaveChanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }

        public IEnumerable<Ingredients> GetAllIngredients()
        {
            return Context.Ingredients; //implicit upcasting to IEnumerable<>
        }


        public Ingredients GetIngredientById(int id)
        {
            return Context.Ingredients.Find(id); //may return null, if it doesn't exist in db
        }

        public void Add(Ingredients obj)
        {
            if (obj is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null Ingredients");
            }
            else
            {
                if (GetIngredientById(obj.Id) != null) //if given address is already in db
                {
                    throw new ArgumentOutOfRangeException("Ingredients with given id already exists.");
                }
                else
                {
                    try
                    {
                        Context.Ingredients.Add(obj); //add to local context
                        Context.SaveChanges();  //run context.SaveChanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }

        public IEnumerable<Inventory> GetAllInventory()
        {
            return Context.Inventory; //implicit upcasting to IEnumerable<>
        }


        public Inventory GetInventoryById(int id)
        {
            return Context.Inventory.Find(id); //may return null, if it doesn't exist in db
        }

        public void Add(Inventory inventory)
        {
            if (inventory is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null inventory");
            }
            else
            {
                if (GetInventoryById(inventory.Id) != null) //if given inventory is already in db
                {
                    throw new ArgumentOutOfRangeException("Inventory with given id already exists.");
                }
                else
                {
                    try
                    {
                        Context.Inventory.Add(inventory); //add to local context
                        Context.SaveChanges();  //run context.SaveChanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }

        public IEnumerable<Location> GetAllLocation()
        {
            return Context.Location.Include(o => o.Inventory);// //implicit upcasting to IEnumerable<>
        }


        public Location GetLocationById(int id)
        {
            return Context.Location.Find(id); //may return null, if it doesn't exist in db
        }

        public void Add(Location obj)
        {
            if (obj is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null Location");
            }
            else
            {
                if (GetLocationById(obj.Id) != null) //if given Location is already in db
                {
                    throw new ArgumentOutOfRangeException("Location with given id already exists.");
                }
                else
                {
                    try
                    {
                        Context.Location.Add(obj); //add to local context
                        Context.SaveChanges();  //run context.SaveChanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }

        public IEnumerable<OrderItems> GetAllOrderItems()
        {
            return Context.OrderItems; //implicit upcasting to IEnumerable<>
        }


        public OrderItems GetOrderItemsById(int id)
        {
            return Context.OrderItems.Find(id); //may return null, if it doesn't exist in db
        }


        public void Add(OrderItems obj)
        {
            if (obj is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null OrderItems");
            }
            else
            {
                if (GetOrderItemsById(obj.Id) != null) //if given OrderItems is already in db
                {
                    throw new ArgumentOutOfRangeException("OrderItems with given id already exists.");
                }
                else
                {
                    try
                    {
                        Context.OrderItems.Add(obj); //add to local context
                        Context.SaveChanges();  //run context.SaveChanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }

        public IEnumerable<Orders> GetAllOrders()
        {
            return Context.Orders; //implicit upcasting to IEnumerable<>
        }

        public Orders GetOrdersById(int id)
        {
            return Context.Orders.Find(id); //may return null, if it doesn't exist in db
        }

        public IEnumerable<Orders> GetOrdersOfCustomer(int custId)
        {
            return Context.Orders.Where(ord => ord.CustomerId == custId);
        }

        public IEnumerable<Orders> GetOrdersOfLocation(int storeId)
        {
            return Context.Orders.Where(ord => ord.StoreId == storeId);
        }

        public IEnumerable<Orders> GetOrdersSortedEarliest()
        {
            return Context.Orders.OrderBy(o => o.OrderTime);
        }

        public IEnumerable<Orders> GetOrdersSortedLatest()
        {
            return Context.Orders.OrderByDescending(o => o.OrderTime);
        }


        public List<(decimal, Orders)> GetOrdersSortedExpensive()
        {
            var ordersinfo = Context.Orders.Include(x => x.OrderItems)
               .ThenInclude(y => y.Pizza);

            SortedList<decimal, Orders> mySL = new SortedList<decimal, Orders>(new DuplicateKeyComparer<decimal>());

            foreach (Orders ord in ordersinfo)
            {
                decimal orderprice = 0;
                foreach (OrderItems oi in ord.OrderItems)
                {
                    orderprice += oi.Quantity * oi.Pizza.Price; //calculate total order price                   
                }
                mySL.Add(orderprice, ord); //sorts them while adding, lowest first
            }

            List<(decimal, Orders)> sortedOrds = new List<(decimal, Orders)>();

            foreach (var order in mySL.Reverse()) //reverse to add highest prices first
            {
                sortedOrds.Add((order.Key, order.Value));
            }

            return sortedOrds;
        }


        public List<(decimal, Orders)> GetOrdersSortedCheapest()
        {
            var ordersinfo = Context.Orders.Include(x => x.OrderItems)
                .ThenInclude(y => y.Pizza);

            //didn't allow duplicates. Added comparer to sort equal values
            SortedList<decimal, Orders> mySL = new SortedList<decimal, Orders>(new DuplicateKeyComparer<decimal>());

            foreach (Orders ord in ordersinfo)
            {
                decimal orderprice = 0;
                foreach(OrderItems oi in ord.OrderItems)
                {
                    orderprice += oi.Quantity * oi.Pizza.Price; //calculate total order price                   
                }
                mySL.Add(orderprice, ord);
            }

            List<(decimal, Orders)> reversedOrds = new List<(decimal, Orders)>();

            foreach (var order in mySL)
            {
                reversedOrds.Add((order.Key, order.Value));
            }

            return reversedOrds;
        }

        public (int, int) GetOrdersStatistics()
        {
            var ordersinfo = Context.Orders;

            var mySL = new SortedList<decimal, int>(new DuplicateKeyComparer<decimal>());

            foreach (Orders ord in ordersinfo)
            {
                if (mySL.ContainsKey(ord.CustomerId)) //implicit cast to decimal
                {
                    mySL[ord.CustomerId] += 1; //increment ordercount for customer
                }
                else
                {
                    mySL.Add(ord.CustomerId, 1); //customer has one order
                }
            }

            return (Decimal.ToInt32(mySL.Last().Key), mySL.Last().Value);
        }

        public List<OrderItems> SuggestOrder(int custId)
        {
            var order = GetOrdersOfCustomer(custId).Last(); //find an order by the customer

            var ois = GetAllOrderItems().Where(o => o.OrderId == order.Id); //get all items in the order

            return ois.ToList();
        }

        public void Add(Orders order)
        {
            if (order is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null order");
            }
            else
            {
                if (GetOrdersById(order.Id) != null) //if given order is already in db
                {
                    throw new ArgumentOutOfRangeException("Order with given id already exists.");
                }
                else
                {
                    try
                    {
                        //need to check 2hr time constraint for customer at this store
                        foreach (var orders in GetAllOrders().Where(o => o.CustomerId == order.CustomerId & o.StoreId == order.StoreId).ToList() )
                        {
                            if (order.OrderTime.Subtract(orders.OrderTime) <= new TimeSpan(2, 0, 0))
                            {
                                //log it

                                throw new Exception("A customer cannot place another order at a pizza store within two hours.");
                            }
                        }
                        

                        Context.Orders.Add(order); //add to local context
                        Context.SaveChanges();  //run context.SaveChanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }

        public IEnumerable<PizzaIngredients> GetAllPizzaIngredients()
        {
            return Context.PizzaIngredients; //implicit upcasting to IEnumerable<>
        }

        public PizzaIngredients GetPizzaIngredientsById(int id)
        {
            return Context.PizzaIngredients.Find(id); //may return null, if it doesn't exist in db
        }

        public void Add(PizzaIngredients PizzaIngredient)
        {
            if (PizzaIngredient is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null PizzaIngredient");
            }
            else
            {
                if (GetPizzaIngredientsById(PizzaIngredient.Id) != null) //if given PizzaIngredient is already in db
                {
                    throw new ArgumentOutOfRangeException("PizzaIngredient with given id already exists.");
                }
                else
                {
                    try
                    {
                        Context.PizzaIngredients.Add(PizzaIngredient); //add to local context
                        Context.SaveChanges();  //run context.savechanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }


        public IEnumerable<Pizza> GetAllPizza()
        {
            return Context.Pizza; //implicit upcasting to IEnumerable<>
        }


        public Pizza GetPizzaById(int id)
        {
            return Context.Pizza.Find(id); //may return null, if it doesn't exist in db
        }


        public void Add(Pizza pizza)
        {
            if (pizza is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null pizza");
            }
            else
            {
                if (GetPizzaById(pizza.Id) != null) //if given pizza is already in db
                {
                    throw new ArgumentOutOfRangeException("Pizza with given id already exists.");
                }
                else
                {
                    try
                    {
                        Context.Pizza.Add(pizza); //add to local context
                        Context.SaveChanges();  //run context.SaveChanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }


        public void Update(Customer customer)
        {
            if (customer is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null customer");
            }
            else
            {
                //customer.Id is never null, no null check

                var existingCust = GetCustomerById(customer.Id);
                if (existingCust != null) //if given customer is actually in db
                {
                    //update local values
                    Context.Entry(existingCust).CurrentValues.SetValues(customer);


                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Customer with given id does not exist");
                }
            }
        }

        public void Update(Location store)
        {
            if (store is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null store");
            }
            else
            {
                //customer.Id is never null, no null check

                var existingStore = GetLocationById(store.Id);
                if (existingStore != null) //if given customer is actually in db
                {
                    //update local values
                    Context.Entry(existingStore).CurrentValues.SetValues(store);

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Customer with given id does not exist");
                }
            }
        }

        //public void Delete(Customer customer)
        //{
        //    if (customer is null)
        //    {
        //        throw new ArgumentNullException("Cannot delete null customer");
        //    }
        //    else
        //    {
        //        if (GetTById(customer.Id) != null) //if given customer is already in db
        //        {
        //            try
        //            {
        //                Context.Customer.Remove(customer); //remove from local context
        //                Context.SaveChanges();  //run context.SaveChanges() to run the appropriate delete, removing it from db
        //            }
        //            catch (DbUpdateException)
        //            {
        //                //log it!

        //                throw;
        //            }

        //        }
        //        else
        //        {
        //            throw new ArgumentOutOfRangeException("Customer with given id does not exist.");
        //        }

        //    }
        //}


        public class DuplicateKeyComparer<TKey> :
             IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;   // Handle equality as being greater
                else
                    return result;
            }
        }
    }
}
