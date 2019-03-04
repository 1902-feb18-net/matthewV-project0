using Microsoft.EntityFrameworkCore;
using Project0.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project0.Library.DAORepositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly project0Context Context;

        CustomerRepo(project0Context dbcontext)
        {
            Context = dbcontext;
        }

        public void AddT(Customer customer)
        {
            if (customer is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null customer");
            }
            else
            {
                if (GetTById(customer.Id) != null) //if given customer is already in db
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

        public void UpdateT(Customer customer)
        {
            if (customer is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null customer");
            }
            else
            {
                //customer.Id is never null, no null check

                var existingCust = GetTById(customer.Id);
                if (existingCust != null) //if given customer is actually in db
                {
                    //update local values
                    existingCust.AddressId = customer.AddressId;
                    existingCust.Email = customer.Email;
                    existingCust.FirstName = customer.FirstName;
                    existingCust.LastName = customer.LastName;
                    existingCust.StoreId = customer.StoreId;

                    existingCust.Orders = customer.Orders;
                    

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Customer with given id does not exist");
                }
            }
        }

        public void DeleteT(Customer customer)
        {
            if (customer is null)
            {
                throw new ArgumentNullException("Cannot delete null customer");
            }
            else
            {
                if (GetTById(customer.Id) != null) //if given customer is already in db
                {
                    try
                    {
                        Context.Customer.Remove(customer); //remove from local context
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
                    throw new ArgumentOutOfRangeException("Customer with given id does not exist.");
                }
            
            }
        }


        public IEnumerable<Customer> GetAllT()
        {
            return Context.Customer; //implicit upcasting to IEnumerable<>
        }


        public Customer GetTById(int id)
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


    }
}
