using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Project0.DataAccess.Repositories
{
    class PizzaIngredientRepo : IRepository<PizzaIngredients>
    {
        private readonly project0Context context;

        PizzaIngredientRepo(project0Context dbcontext)
        {
            context = dbcontext;
        }

        public void AddT(PizzaIngredients PizzaIngredient)
        {
            if (PizzaIngredient is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null PizzaIngredient");
            }
            else
            {
                if (GetTById(PizzaIngredient.Id) != null) //if given PizzaIngredient is already in db
                {
                    throw new ArgumentOutOfRangeException("PizzaIngredient with given id already exists.");
                }
                else
                {
                    try
                    {
                        context.PizzaIngredients.Add(PizzaIngredient); //add to local context
                        context.SaveChanges();  //run context.savechanges() to run the appropriate insert, adding it to db
                    }
                    catch (DbUpdateException)
                    {
                        //log it!

                        throw;
                    }
                }
            }
        }

        public void UpdateT(PizzaIngredients PizzaIngredient)
        {
            if (PizzaIngredient is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null PizzaIngredient");
            }
            else
            {
                //PizzaIngredient.id is never null, no null check

                var existingPI = GetTById(PizzaIngredient.Id);
                if (existingPI != null) //if given PizzaIngredient is actually in db
                {
                    //update local values
                    existingPI.IngredientsId = PizzaIngredient.IngredientsId;
                    existingPI.PizzaId = PizzaIngredient.PizzaId;
                    existingPI.Quantity = PizzaIngredient.Quantity;

                    context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("PizzaIngredient with given id does not exist");
                }
            }
        }

        public void DeleteT(PizzaIngredients PizzaIngredient)
        {
            if (PizzaIngredient is null)
            {
                throw new ArgumentNullException("Cannot delete null PizzaIngredient");
            }
            else
            {
                if (GetTById(PizzaIngredient.Id) != null) //if given PizzaIngredient is already in db
                {
                    try
                    {
                        context.PizzaIngredients.Remove(PizzaIngredient); //remove from local context
                        context.SaveChanges();  //run context.savechanges() to run the appropriate delete, removing it from db
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
                    throw new ArgumentOutOfRangeException("PizzaIngredient with given id does not exist");
                }
            }
        }


        public IEnumerable<PizzaIngredients> GetAllT()
        {
            return context.PizzaIngredients; //implicit upcasting to ienumerable<>
        }


        public PizzaIngredients GetTById(int id)
        {
            return context.PizzaIngredients.Find(id); //may return null, if it doesn't exist in db
        }
        
    }
}
