using Microsoft.EntityFrameworkCore;
using Project0.DataAccess;
using System;
using System.Collections.Generic;

namespace Project0.Library.DAORepositories
{
    public class PizzaRepo : IRepository<Pizza>
    {
        private readonly project0Context Context;

        public PizzaRepo(project0Context dbcontext)
        {
            Context = dbcontext;
        }

        public void AddT(Pizza pizza)
        {
            if (pizza is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null pizza");
            }
            else
            {
                if (GetTById(pizza.Id) != null) //if given pizza is already in db
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

        public void UpdateT(Pizza pizza)
        {
            if (pizza is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null pizza");
            }
            else
            {
                //.Id is never null, no null check

                var existingPiz = GetTById(pizza.Id);
                if (existingPiz != null) //if given pizza is actually in db
                {
                    //update local values
                    existingPiz.Price = pizza.Price;

                    existingPiz.OrderItems = pizza.OrderItems;
                    existingPiz.PizzaIngredients = pizza.PizzaIngredients;

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Pizza with given id does not exist");
                }
            }
        }

        public void DeleteT(Pizza pizza)
        {
            if (pizza is null)
            {
                throw new ArgumentNullException("Cannot delete null pizza");
            }
            else
            {
                if (GetTById(pizza.Id) != null) //if given pizza is already in db
                {
                    try
                    {
                        Context.Pizza.Remove(pizza); //remove from local context
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
                    throw new ArgumentOutOfRangeException("Pizza with given id does not exist.");
                }

            }
        }


        public IEnumerable<Pizza> GetAllT()
        {
            return Context.Pizza; //implicit upcasting to IEnumerable<>
        }


        public Pizza GetTById(int id)
        {
            return Context.Pizza.Find(id); //may return null, if it doesn't exist in db
        }

    }
}
