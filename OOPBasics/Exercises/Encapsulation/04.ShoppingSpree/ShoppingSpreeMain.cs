﻿namespace _04.ShoppingSpree
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class ShoppingSpreeMain
    {
        public static void Main()
        {
            var people = new Dictionary<string, Person>();
            var products = new Dictionary<string, Product>();

            string personInfo = Console.ReadLine();
            string productInfo = Console.ReadLine();
            
            var personMatches = personInfo.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            var productMatches = productInfo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            AddPeople(personMatches, people);
            AddProducts(productMatches, products);

            string input = Console.ReadLine();
            while (input != "END")
            {
                string[] inputArgs = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                var person = people[inputArgs[0]];
                var product = products[inputArgs[1]];
                try
                {
                    person.BuyProduct(product);
                    Console.WriteLine($"{person.Name} bought {product.Name}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                input = Console.ReadLine();
            }

            foreach (var person in people)
            {
                Console.WriteLine(person.Value);
            }
        }

        private static void AddProducts(string[] productMatches, Dictionary<string, Product> products)
        {
            foreach (string match in productMatches)
            {
                var productCostPair = match.Split('=');
                try
                {
                    var product = new Product(productCostPair[0], decimal.Parse(productCostPair[1]));
                    products.Add(product.Name, product);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }
            }
        }

        private static void AddPeople(string[] personMatches, Dictionary<string, Person> people)
        {
            foreach (string match in personMatches)
            {
                var personMoneyPair = match.Split('=');
                try
                {
                    var person = new Person(personMoneyPair[0], decimal.Parse(personMoneyPair[1]));
                    people.Add(person.Name, person);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }
            }
        }
    }

    public class Product
    {
        private string name;
        private decimal price;

        public Product(string name, decimal price)
        {
            this.Name = name;
            this.Price = price;
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Name cannot be empty");
                }

                this.name = value;
            }
        }

        public decimal Price
        {
            get { return this.price; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"Money cannot be negative");
                }

                this.price = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class Person
    {
        private string name;
        private decimal money;
        private List<Product> products;

        public Person(string name, decimal money)
        {
            this.Name = name;
            this.Money = money;
            this.products = new List<Product>();
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Name cannot be empty");
                }

                this.name = value;
            }
        }

        public decimal Money
        {
            get { return this.money; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"Money cannot be negative");
                }

                this.money = value;
            }
        }

        public List<Product> Products => this.products;

        public void BuyProduct(Product product)
        {
            if (this.Money < product.Price)
            {
                throw new InvalidOperationException($"{this.Name} can't afford {product.Name}");
            }

            this.Products.Add(product);
            this.Money -= product.Price;
        }

        public override string ToString()
        {
            return this.Products.Count > 0
                ? $"{this.Name} - {string.Join(", ", this.Products)}"
                : $"{this.Name} - Nothing bought";
        }
    }
}
