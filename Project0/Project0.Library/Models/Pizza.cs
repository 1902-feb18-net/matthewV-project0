using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class Pizza //Allows on-the-fly creation of Pizzas with other ingredients and prices, not just the given child classes. 
    {
        [DataMember]
        public HashSet<string> Items = new HashSet<string> { "Dough", "Cheese" };  //Set of items a pizza can be made out of. Assumed one of each.
                                                                                   //Every pizza should have dough and cheese. 
        [DataMember]
        public decimal Price { get; set; } = 10.00m; //Decimal is recommended type for money values. Default price is $10.00

    }

    //alternatively, could've added readonly to items, with each child inheriting from Pizza and their own items added in constructor, like so
    public class PepperoniPizza : Pizza
    {
        [DataMember]
        public static decimal DefaultPepperoniPrice { get; set; } = 15.00m;

        PepperoniPizza()
        {
            Items.Add("Pepperoni");  
            Price = DefaultPepperoniPrice;
        }

        PepperoniPizza(decimal setPrice)
        {
            Items.Add("Pepperoni");  
            Price = setPrice; 
        }
    }

    public class SpinachPizza : Pizza
    {
        [DataMember]
        public static decimal DefaultSpinachPrice { get; set; } = 15.00m;

        SpinachPizza()
        {
            Items.Add("Spinach");
            Price = DefaultSpinachPrice;
        }

        SpinachPizza(decimal setPrice)
        {
            Items.Add("Spinach");
            Price = setPrice;
        }
    }

    public class SausagePizza : Pizza
    {
        [DataMember]
        public static decimal DefaultSausagePrice { get; set; } = 15.00m;

        SausagePizza()
        {
            Items.Add("Sausage");
            Price = DefaultSausagePrice;
        }

        SausagePizza(decimal setPrice)
        {
            Items.Add("Sausage");
            Price = setPrice;
        }
    }

    public class PineapplePizza : Pizza
    {
        [DataMember]
        public static decimal DefaultPineapplePrice { get; set; } = 15.00m;

        PineapplePizza()
        {
            Items.Add("Pineapple");
            Price = DefaultPineapplePrice;
        }

        PineapplePizza(decimal setPrice)
        {
            Items.Add("Pineapple");
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
