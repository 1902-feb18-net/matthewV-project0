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
            var defaultStore = newCustomer.Store;

            //Assert
            Assert.NotNull(defaultStore);
        }


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

        [Fact]
        public void PizzaStoreCannotHaveNullName()
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => newPizzaStore.Name = null);
        }

        [Fact]
        public void PizzaStoreCannotHaveEmptyName()
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();

            //Act and Assert
            Assert.Throws<ArgumentException>(() => newPizzaStore.Name = "");
        }


        [Fact]
        public void PizzaStoreCannotHaveNullInventoryItemName()
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => newPizzaStore.AddInventoryItem(null, 1));
        }

        [Fact]
        public void PizzaStoreCannotHaveTwoInventoryItemWIthSameName()
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();

            //Act
            newPizzaStore.AddInventoryItem("", 1);

            //Assert
            Assert.Throws<ArgumentException>(() => newPizzaStore.AddInventoryItem("", 1));
        }


        //Order tests
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void PlacedOrderShouldDecreaseStoreInventoryByPositiveAmount(int amount) //* inventory decreases when orders are accepted
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();
            Dictionary<Pizza, int> order = new Dictionary<Pizza, int>() { { cheesePizza, amount } };
            var originalInventory = new Dictionary<string, int>(newPizzaStore.Inventory);

            //Act
            newPizzaStore.PlacedOrder(newCustomer, order);
            var decreasedInventory = new Dictionary<string, int>(newPizzaStore.Inventory);

            foreach (var item in cheesePizza.Items)
            {
                Assert.Equal(originalInventory[item.Key], decreasedInventory[item.Key] + (amount * item.Value));
            }
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(0)]
        public void PlacedOrderShouldRejectNegativeAmountOrZero(int amount)
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();
            Dictionary<Pizza, int> order = new Dictionary<Pizza, int>() { { cheesePizza, amount } };

            //Act and Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => newPizzaStore.PlacedOrder(newCustomer, order));

        }

        [Theory]
        [InlineData(35)]
        [InlineData(100)]
        public void PlacedOrderShouldRejectAmountGreaterThanInventory(int amount) //* rejects orders that cannot be fulfilled with remaining inventory
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();
            Dictionary<Pizza, int> order = new Dictionary<Pizza, int>() { { cheesePizza, amount } };

            //Act and Assert
            Assert.Throws<ArgumentException>(() => newPizzaStore.PlacedOrder(newCustomer, order));
        }

        // Customer cannot place more than one order from the same location within two hours!
        [Fact]
        public void CustomerCannotPlaceMoreTwoOrdersFromSameLocationWithinTwoHours()
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();
            Dictionary<Pizza, int> order = new Dictionary<Pizza, int>() { { cheesePizza, 1 } };
            Dictionary<Pizza, int> order2 = new Dictionary<Pizza, int>() { { cheesePizza, 1 } };

            //Act
            newPizzaStore.PlacedOrder(newCustomer, order);

            //Assert
            Assert.Throws<Exception>(() => newPizzaStore.PlacedOrder(newCustomer, order));

        }

        [Fact]
        public void NewOrderObjShouldRejectNullStore()
        {
            //Arrange
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();
            Dictionary<Pizza, int> orderItem = new Dictionary<Pizza, int>() { { cheesePizza, 1 } };

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => new Order(null, newCustomer, orderItem, DateTime.Now));
        }

        [Fact]
        public void NewOrderObjShouldRejectNullCustomer()
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Pizza cheesePizza = new Pizza();
            Dictionary<Pizza, int> orderItem = new Dictionary<Pizza, int>() { { cheesePizza, 1 } };

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => new Order(newPizzaStore, null, orderItem, DateTime.Now));
        }

        [Fact]
        public void NewDeliveryOrderShouldRejectNullStore()
        {
            //Arrange
            Customer newCustomer = new Customer();
            Pizza cheesePizza = new Pizza();
            Dictionary<Pizza, int> orderItem = new Dictionary<Pizza, int>() { { cheesePizza, 1 } };
            Address address = new Address();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => new Order(null, newCustomer,orderItem, DateTime.Now));
        }

        [Fact]
        public void NewDeliveryOrderShouldRejectNullCustomer()
        {
            //Arrange
            PizzaStore newPizzaStore = new PizzaStore();
            Pizza cheesePizza = new Pizza();
            Dictionary<Pizza, int> orderItem = new Dictionary<Pizza, int>() { { cheesePizza, 1 } };
            Address address = new Address();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => new Order(newPizzaStore, null, orderItem, DateTime.Now));
        }

        [Fact]
        public void CustomerShouldRejectSettingNullStore()
        {
            //Arrange
            Customer newCustomer = new Customer();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => newCustomer.Store = null);
        }

        [Fact]
        public void AddressShouldRejectSettingNullStreet()
        {
            //Arrange
            Address address = new Address();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => address.Street = null);
        }

        [Fact]
        public void AddressShouldRejectSettingNullCountry()
        {
            //Arrange
            Address address = new Address();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => address.Country = null);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-1)]
        [InlineData(0)]
        public void NewPizzaPriceCannotBeNegativeAmountOrZero(int amount)
        {
            //Arrange
            Pizza cheesePizza = new Pizza();

            //Act and Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cheesePizza.Price = amount);
        }



    }
}

