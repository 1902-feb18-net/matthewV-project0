using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class Pizza //Allows on-the-fly creation of Pizzas with other ingredients and prices, not just the given child classes. 
    {
        [DataMember]
        public Dictionary<string, int> Items = new Dictionary<string, int> { { "Dough", 1 }, { "Cheese", 1 } };
        //Set of items a pizza can be made out of. Every pizza should have at least dough and cheese! Assumed one of each.

        private decimal _price = 10.00m; //decimal is recommended type for money values. Default price is $10.00
        [DataMember]
        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Price cannot be 0 or less.");
                }
                _price = value;
            }
        }

        public Pizza() { }

        public Pizza(decimal price)
        {
            Price = price;
        }

    }

    //alternatively, could've added readonly to items, with each child inheriting from Pizza and their own items added in constructor, like so
    public class PepperoniPizza : Pizza
    {
        [DataMember]
        public static decimal DefaultPepperoniPrice { get; set; } = 15.00m;

        PepperoniPizza()
        {
            Items.Add("Pepperoni", 1);
            Price = DefaultPepperoniPrice;
        }

        PepperoniPizza(decimal setPrice)
        {
            Items.Add("Pepperoni", 1);
            Price = setPrice;
        }
    }

    public class SpinachPizza : Pizza
    {
        [DataMember]
        public static decimal DefaultSpinachPrice { get; set; } = 15.00m;

        SpinachPizza()
        {
            Items.Add("Spinach", 1);
            Price = DefaultSpinachPrice;
        }

        SpinachPizza(decimal setPrice)
        {
            Items.Add("Spinach", 1);
            Price = setPrice;
        }
    }

    public class SausagePizza : Pizza
    {
        [DataMember]
        public static decimal DefaultSausagePrice { get; set; } = 15.00m;

        SausagePizza()
        {
            Items.Add("Sausage", 1);
            Price = DefaultSausagePrice;
        }

        SausagePizza(decimal setPrice)
        {
            Items.Add("Sausage", 1);
            Price = setPrice;
        }
    }

    public class PineapplePizza : Pizza
    {
        [DataMember]
        public static decimal DefaultPineapplePrice { get; set; } = 15.00m;

        PineapplePizza()
        {
            Items.Add("Pineapple", 1);
            Price = DefaultPineapplePrice;
        }

        PineapplePizza(decimal setPrice)
        {
            Items.Add("Pineapple", 1);
            Price = setPrice;
        }
    }


    ////original product struct idea
    //public enum Components //all components a pizza can be made of. 
    //{
    //    Dough,
    //    Pepperoni,
    //    Pineapple,
    //    Spinach,
    //    Sausage
    //}

    //public struct CheesePizza
    //{
    //    public static Components Cheese; 
    //    public static Components Dough;
    //    //the components of a cheese pizza don't change, but the price can
    //    public double Price { get; set; }

    //    CheesePizza(double price)
    //    {
    //        Price = price;
    //    }

    //}



}
