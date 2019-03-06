using Microsoft.EntityFrameworkCore;
using Project0.DataAccess;
using System;
using System.Collections.Generic;

namespace Project0.Library.DAORepositories
{
    public class IngredientsRepo : IRepository<Ingredients>
    {
        private readonly project0Context Context;

        public IngredientsRepo(project0Context dbcontext)
        {
            Context = dbcontext;
        }

        public void AddT(Ingredients obj)
        {
            if (obj is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null Ingredients");
            }
            else
            {
                if (GetTById(obj.Id) != null) //if given address is already in db
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

        public void UpdateT(Ingredients obj)
        {
            if (obj is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null Ingredients");
            }
            else
            {
                //.Id is never null, no null check

                var existingIng = GetTById(obj.Id);
                if (existingIng != null) //if given Ingredients is actually in db
                {
                    //update local values
                    existingIng.Name = obj.Name;

                    existingIng.Inventory = obj.Inventory;
                    existingIng.PizzaIngredients = obj.PizzaIngredients;

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Ingredients with given id does not exist");
                }
            }
        }

        public void DeleteT(Ingredients obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException("Cannot delete null Ingredients");
            }
            else
            {
                if (GetTById(obj.Id) != null) //if given Ingredients is already in db
                {
                    try
                    {
                        Context.Ingredients.Remove(obj); //remove from local context
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
                    throw new ArgumentOutOfRangeException("Ingredients with given id does not exist.");
                }

            }
        }


        public IEnumerable<Ingredients> GetAllT()
        {
            return Context.Ingredients; //implicit upcasting to IEnumerable<>
        }


        public Ingredients GetTById(int id)
        {
            return Context.Ingredients.Find(id); //may return null, if it doesn't exist in db
        }
    }
}
