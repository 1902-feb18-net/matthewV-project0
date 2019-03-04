using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Project0.DataAccess.Repositories
{
    public class OrderRepo : IOrdersRepo
    {
        private readonly project0Context Context;

        OrderRepo(project0Context dbcontext)
        {
            Context = dbcontext;
        }

        public void AddT(Orders order)
        {
            if (order is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null order");
            }
            else
            {
                if (GetTById(order.Id) != null) //if given order is already in db
                {
                    throw new ArgumentOutOfRangeException("Order with given id already exists.");
                }
                else
                {
                    try
                    {
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

        public void UpdateT(Orders order)
        {
            if (order is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null order");
            }
            else
            {
                //order.Id is never null, no null check

                var existingOrd = GetTById(order.Id);
                if (existingOrd != null) //if given order is actually in db
                {
                    //update local values
                    existingOrd.AddressId = order.AddressId;
                    existingOrd.CustomerId = order.CustomerId;
                    existingOrd.OrderTime = order.OrderTime;
                    existingOrd.StoreId = order.StoreId;

                    existingOrd.OrderItems = order.OrderItems;

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Order with given id does not exist");
                }
            }
        }

        public void DeleteT(Orders order)
        {
            if (order is null)
            {
                throw new ArgumentNullException("Cannot delete null order");
            }
            else
            {
                if (GetTById(order.Id) != null) //if given order is already in db
                {
                    try
                    {
                        Context.Orders.Remove(order); //remove from local context
                        Context.SaveChanges();  //run context.SaveChanges() to run the appropriate delete, removing it from db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Order with given id does not exist");
                }
            }
        }

        public IEnumerable<Orders> GetAllT()
        {
            return Context.Orders; //implicit upcasting to IEnumerable<>
        }

        public Orders GetTById(int id)
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

        //public IEnumerable<Orders> GetOrdersSortedExpensive()
        //{

        //    var included = Context.Orders
        //        .Include(o => o.OrderItems)
        //            .ThenInclude(i => i.Pizza);
        //            //.OrderBy(o => o.OrderItems.Pizza.price *  o.OrderItems.quantity);
      
        //}


        public IEnumerable<Orders> GetOrdersStatistics()
        {
            throw new System.NotImplementedException();
        }

        public Orders SuggestOrder(int custId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Orders> GetOrdersSortedCheapest()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Orders> GetOrdersSortedExpensive()
        {
            throw new NotImplementedException();
        }
    }
}
