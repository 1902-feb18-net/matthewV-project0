using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Project0.DataAccess.Repositories
{
    public class OrderItemsRepo : IRepository<OrderItems>
    {
        private readonly project0Context Context;

        OrderItemsRepo(project0Context dbcontext)
        {
            Context = dbcontext;
        }

        public void AddT(OrderItems obj)
        {
            if (obj is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null OrderItems");
            }
            else
            {
                if (GetTById(obj.Id) != null) //if given OrderItems is already in db
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

        public void UpdateT(OrderItems obj)
        {
            if (obj is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null OrderItems");
            }
            else
            {
                //.Id is never null, no null check

                var existingOI = GetTById(obj.Id);
                if (existingOI != null) //if given OrderItems is actually in db
                {
                    //update local values
                    existingOI.OrderId = obj.OrderId;
                    existingOI.PizzaId = obj.PizzaId;
                    existingOI.Quantity = obj.Quantity;

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("OrderItems with given id does not exist");
                }
            }
        }

        public void DeleteT(OrderItems obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException("Cannot delete null OrderItems");
            }
            else
            {
                if (GetTById(obj.Id) != null) //if given OrderItems is already in db
                {
                    try
                    {
                        Context.OrderItems.Remove(obj); //remove from local context
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
                    throw new ArgumentOutOfRangeException("OrderItems with given id does not exist.");
                }

            }
        }


        public IEnumerable<OrderItems> GetAllT()
        {
            return Context.OrderItems; //implicit upcasting to IEnumerable<>
        }


        public OrderItems GetTById(int id)
        {
            return Context.OrderItems.Find(id); //may return null, if it doesn't exist in db
        }
    }
}
