using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Project0.DataAccess.Repositories
{
    public class InventoryRepo : IInventoryRepo
    {
        private readonly project0Context Context;

        InventoryRepo(project0Context dbcontext)
        {
            Context = dbcontext;
        }

        public void AddT(Inventory inventory)
        {
            if (inventory is null)
            {
                //log it!

                throw new ArgumentNullException("Cannot add null inventory");
            }
            else
            {
                if (GetTById(inventory.Id) != null) //if given inventory is already in db
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

        public void UpdateT(Inventory inventory)
        {
            if (inventory is null)
            {
                //log it!
                throw new ArgumentNullException("Cannot update null inventory");
            }
            else
            {
                //.Id is never null, no null check

                var existingInv = GetTById(inventory.Id);
                if (existingInv != null) //if given inventory is actually in db
                {
                    //update local values
                    existingInv.IngredientsId = inventory.IngredientsId;
                    existingInv.Quantity = inventory.Quantity;
                    existingInv.StoreId = existingInv.StoreId;

                    Context.SaveChanges(); //update db's values
                }
                else
                {
                    //log it!
                    throw new ArgumentOutOfRangeException("Inventory with given id does not exist");
                }
            }
        }

        public void DeleteT(Inventory inv)
        {
            if (inv is null)
            {
                throw new ArgumentNullException("Cannot delete null inventory");
            }
            else
            {
                if (GetTById(inv.Id) != null) //if given inventory is already in db
                {
                    try
                    {
                        Context.Inventory.Remove(inv); //remove from local context
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
                    throw new ArgumentOutOfRangeException("Inventory with given id does not exist.");
                }

            }
        }


        public IEnumerable<Inventory> GetAllT()
        {
            return Context.Inventory; //implicit upcasting to IEnumerable<>
        }


        public Inventory GetTById(int id)
        {
            return Context.Inventory.Find(id); //may return null, if it doesn't exist in db
        }


    }
}
