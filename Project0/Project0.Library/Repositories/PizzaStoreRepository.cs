using Project0.DataAccess;
using Project0.Library.DAORepositories;
using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Project0.Library.Repositories
{
    [DataContract]
    public class PizzaStoreRepository
    {
        /// <summary>
        /// A repository managing data access for pizza restaurants.
        /// </summary>
        public LocationRepo locationRepo;

        public PizzaStoreRepository(project0Context db)
        {
            locationRepo = new LocationRepo(db) ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Get a restaurant by ID.
        /// </summary>
        /// <param name="id">The ID of the restaurant</param>
        /// <returns>The restaurant</returns>
        public PizzaStore GetStoreById(int id)
        {
            return Mapper.Map(locationRepo.GetTById(id));
        }

        /// <summary>
        /// Get all restaurants with deferred execution.
        /// </summary>
        /// <returns>The collection of restaurants</returns>
        public IEnumerable<PizzaStore> GetStores(string search = null)
        {
            if (search == null) //no parameter defaults to returning each item
            {
                foreach (var item in locationRepo.GetAllT())
                {
                    yield return Mapper.Map(item);
                }
            }
            else
            {   //or return each pizza restaurant with the given item in inventory (doesn't check quantity)
                PizzaStore store;
                foreach (var item in locationRepo.GetAllT())
                {
                    store = Mapper.Map(item);
                    if (store.Inventory.ContainsKey(search))
                    {
                        yield return store;
                    }
                }
            }
        }

        /// <summary>
        /// Add a pizza store restaurant.
        /// </summary>
        /// <param name="restaurant">The restaurant object</param>
        public void AddStore(PizzaStore restaurant)
        {
            Location store = Mapper.Map(restaurant);

            if (locationRepo.GetTById(store.Id) != null)
            {
                throw new InvalidOperationException($"Pizza Store with ID {store.Id} already exists.");
            }
            locationRepo.AddT(store);
        }


        /// <summary>
        /// Get all restaurants.
        /// </summary>
        /// <returns>The collection of restaurants</returns>
        public ICollection<PizzaStore> GetAllStores()
        {
            return new List<PizzaStore>(Mapper.Map(locationRepo.GetAllT()) );
        }




        /// <summary>
        /// Delete a pizza restaurant by ID. 
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant</param>
        public void DeleteStore(PizzaStore rest)
        {
            Location store = Mapper.Map(rest);
            locationRepo.DeleteT(store);          
        }

        /// <summary>
        /// Update a pizza store restaurant.
        /// </summary>
        /// <param name="restaurant">The restaurant with changed values</param>
        /// <remarks>Have to make sure pizza store obj arg has all the same fields as original (except ones changing)</remarks>
        public void UpdateStore(PizzaStore restaurant)
        {
            locationRepo.UpdateT(Mapper.Map(restaurant));
        }

        //public void UpdateStore(PizzaStore restaurant)
        //{
        //    DeleteStore(restaurant);
        //    AddStore(restaurant);
        //}

    }
}
