using System;
using Xunit;
using Project0.Library.Models;
using System.Collections.Generic;

namespace Project0.Testing
{
    public class UnitTest1
    {
        //Customer tests
        [Fact]
        public void NewCustomerShouldHaveADefaultStore() //* customer has a default store location
        {
            //Arrange
            Customer newCustomer = new Customer();

            //Act
            var defaultStore = newCustomer.DefaultStore;

            //Assert
            Assert.NotNull(defaultStore);
        }


        // Customer cannot place more than one order from the same location within two hours!



        //Pizza Store Location tests
        [Fact]
        public void NewPizzaStoreShouldHaveAnInventory() //* store location has an inventory
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();

            //Act
            var defaultInventory = newPizzaStore.Inventory;

            //Assert
            Assert.NotNull(defaultInventory); 
        }

        [Fact]
        public void NewPizzaStoreShouldHaveTheInitialInventory() 
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Dictionary<string, int> initialInventory = PizzaStore.InitialInventory;

            //Act
            var defaultInventory = newPizzaStore.Inventory;

            //Assert
            Assert.True(DictionaryComparison.DictionaryEquals<string, int>(defaultInventory, initialInventory)); //Comparing equal 
        }

        [Fact]
        public void NewPizzaStoreShouldHaveAnOrderHistory() //* store location has an order history
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();

            //Act
            var defaultOrderHistory = newPizzaStore.OrderHistory;

            //Assert
            Assert.NotNull(defaultOrderHistory);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void NewOrderShouldDecreaseStoreInventoryByPositiveAmount(int amount) //* inventory decreases when orders are accepted
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();
            var originalInventory = new Dictionary<string, int> (newPizzaStore.Inventory);

            //Act
            newPizzaStore.PlacedOrder(newCustomer, cheesePizza, amount);
            var decreasedInventory = new Dictionary<string, int>(newPizzaStore.Inventory);

            foreach (var item in cheesePizza.Items)
            {
                Assert.Equal(originalInventory[item], decreasedInventory[item] + amount);
            }
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(0)]
        public void NewOrderShouldRejectNegativeAmountOrZero(int amount) 
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();

            //Act and Assert
            Assert.Throws<ArgumentOutOfRangeException>( () => newPizzaStore.PlacedOrder(newCustomer, cheesePizza, amount));

        }

        [Theory]
        [InlineData(35)]
        [InlineData(100)]
        public void NewOrderShouldRejectAmountGreaterThanInventory(int amount) //* rejects orders that cannot be fulfilled with remaining inventory
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();

            //Act
            Assert.Throws<ArgumentException>(() => newPizzaStore.PlacedOrder(newCustomer, cheesePizza, amount));

        }

        
        //* relationship between products sold and inventory required must have some complexity for at least one product!


        ////Order tests
        //[Fact]
        //public void NewOrderShouldHaveAStoreLocation() //* order has a store location
        //{
        //    //Arrange
        //    Order newOrder = new Order();

        //    //Act
        //    var defaultStore = newOrder.Store;

        //    //Assert
        //    Assert.NotNull(defaultStore);
        //}

        //[Fact]
        //public void NewOrderShouldHaveACustomer() //* order has a customer
        //{
        //    //Arrange
        //    Order newOrder = new Order();

        //    //Act
        //    var defaultCustomer = newOrder.Customer;

        //    //Assert
        //    Assert.NotNull(defaultCustomer);
        //}


        //order has an order time. Order time is a DateTime, 
        //which cannot be null, so no Assert.NotNull(orderTime) test


        //check additional business rules!

    }
}
