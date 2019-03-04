using Microsoft.EntityFrameworkCore;
using Project0.DataAccess;
using System;
using System.Collections.Generic;

namespace Project0.Library.DAORepositories
{
    public class AddressRepo : IRepository<Address>
    {
        private readonly project0Context Context;

        AddressRepo(project0Context dbcontext)
        {
            Context = dbcontext;
        }

        public void AddT(Address obj)
        {
            if (obj is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null address");
            }
            else
            {
                if (GetTById(obj.Id) != null) //if given address is already in db
                {
                    throw new ArgumentOutOfRangeException("Address with given id already exists.");
                }
                else
                {
                    try
                    {
                        Context.Address.Add(obj); //add to local context
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

        public void UpdateT(Address obj)
        {
            if (obj is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null address");
            }
            else
            {
                //.Id is never null, no null check

                var existingAdd = GetTById(obj.Id);
                if (existingAdd != null) //if given address is actually in db
                {
                    //Context.Entry(existingAdd).CurrentValues.SetValues(obj); 
                    //would update values for any type of obj in context, allowing generics

                    //update local values
                    existingAdd.City = obj.City;
                    existingAdd.Country = obj.Country;
                    existingAdd.State = obj.State;
                    existingAdd.Street = obj.Street;
                    existingAdd.Zipcode = obj.Zipcode;

                    existingAdd.Customer = obj.Customer;
                    existingAdd.Location = obj.Location;
                    existingAdd.Orders = obj.Orders;

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Address with given id does not exist");
                }
            }
        }

        public void DeleteT(Address obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException("Cannot delete null address");
            }
            else
            {
                if (GetTById(obj.Id) != null) //if given address is already in db
                {
                    try
                    {
                        Context.Address.Remove(obj); //remove from local context
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
                    throw new ArgumentOutOfRangeException("Address with given id does not exist.");
                }
            
            }
        }


        public IEnumerable<Address> GetAllT()
        {           
            return Context.Address; //implicit upcasting to IEnumerable<>
        }


        public Address GetTById(int id)
        {
            return Context.Address.Find(id); //may return null, if it doesn't exist in db
        }


    }
}
