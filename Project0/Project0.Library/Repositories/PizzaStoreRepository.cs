using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Project0.Library.Repositories
{
    [DataContract]
    public class PizzaStoreRepository
    {
        /// <summary>
        /// A repository managing data access for pizza restaurants.
        /// </summary>
        [DataMember]
        private readonly ICollection<PizzaStore> _stores;

        /// <summary>
        /// Initializes a new pizza restaurant repository given a suitable data source.
        /// </summary>
        /// <param name="data">The data source</param>
        public PizzaStoreRepository(ICollection<PizzaStore> data)
        {
            _stores = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Get all restaurants with deferred execution.
        /// </summary>
        /// <returns>The collection of restaurants</returns>
        public IEnumerable<PizzaStore> GetStores(string search = null)
        {
            if (search == null) //no parameter defaults to returning each item
            {
                foreach (var item in _stores)
                {
                    yield return item;
                }
            }
            else
            {   //or return each pizza restaurant with the given item in inventory (doesn't check quantity)
                foreach (var item in _stores) 
                {
                    //foreach(InventoryItem inven in item.Inventory)
                    //{
                    //    if(inven.Name == search)
                    //    {
                    //        yield return item;
                    //    }
                    //}

                    if (item.Inventory.ContainsKey(search))
                    {
                        yield return item;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Get a restaurants by ID.
        /// </summary>
        /// <param name="id">The ID of the restaurant</param>
        /// <returns>The restaurant</returns>
        public PizzaStore GetStoreById(int id)
        {
            return _stores.First(r => r.Id == id);  //First will always return the one result, since Id unique
        }

        /// <summary>
        /// Add a pizza store restaurant.
        /// </summary>
        /// <param name="restaurant">The restaurant object</param>
        public void AddStore(PizzaStore restaurant)
        {
            if (_stores.Any(r => r.Id == restaurant.Id))
            {
                throw new InvalidOperationException($"Pizza Store with ID {restaurant.Id} already exists.");
            }
            _stores.Add(restaurant);
        }

        /// <summary>
        /// Delete a pizza restaurant by ID. 
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant</param>
        public void DeleteStore(int restaurantId)
        {
            _stores.Remove(_stores.First(r => r.Id == restaurantId));
        }

        /// <summary>
        /// Update a pizza store restaurant.
        /// </summary>
        /// <param name="restaurant">The restaurant with changed values</param>
        /// <remarks>Have to make sure pizza store obj arg has all the same fields as original (except ones changing)</remarks>
        public void UpdateStore(PizzaStore restaurant) 
        {
            DeleteStore(restaurant.Id);
            AddStore(restaurant);  
        }

    }
}
