using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Project0.DataAccess.Repositories
{
    public class LocationRepo : IRepository<Location>
    {
        private readonly project0Context Context;

        LocationRepo(project0Context dbcontext)
        {
            Context = dbcontext;
        }

        public void AddT(Location obj)
        {
            if (obj is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null Location");
            }
            else
            {
                if (GetTById(obj.Id) != null) //if given Location is already in db
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

        public void UpdateT(Location obj)
        {
            if (obj is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null Location");
            }
            else
            {
                //.Id is never null, no null check

                var existingLoc = GetTById(obj.Id);
                if (existingLoc != null) //if given Location is actually in db
                {
                    //update local values
                    existingLoc.AddressId = obj.AddressId;
                    existingLoc.LocationName = obj.LocationName;

                    existingLoc.Customer = obj.Customer;
                    existingLoc.Inventory = obj.Inventory;
                    existingLoc.Orders = obj.Orders;

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Location with given id does not exist");
                }
            }
        }

        public void DeleteT(Location obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException("Cannot delete null Location");
            }
            else
            {
                if (GetTById(obj.Id) != null) //if given Location is already in db
                {
                    try
                    {
                        Context.Location.Remove(obj); //remove from local context
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
                    throw new ArgumentOutOfRangeException("Location with given id does not exist.");
                }

            }
        }


        public IEnumerable<Location> GetAllT()
        {
            return Context.Location; //implicit upcasting to IEnumerable<>
        }


        public Location GetTById(int id)
        {
            return Context.Location.Find(id); //may return null, if it doesn't exist in db
        }

    }
}
