﻿namespace _07.Google
{
    public class Company : INameable
    {
        public Company(string name, string department, decimal salary)
        {
            this.Name = name;
            this.Department = department;
            this.Salary = salary;
        }

        public string Name { get; set; }

        public string Department { get; set; }

        public decimal Salary { get; set; }

        public override string ToString()
        {
            return $"{this.Name} {this.Department} {this.Salary:.00}";
        }
    }
}