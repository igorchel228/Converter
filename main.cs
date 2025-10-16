using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitConverter
{
    public enum Category
    {
        Length,
        Weight,
        Temperature,
        Volume,
        Speed
    }

    public class Unit
    {
        public string Name { get; set; }
        public double ToBase { get; set; }
        public Category Category { get; set; }

        public Unit(string name, double toBase, Category category)
        {
            Name = name;
            ToBase = toBase;
            Category = category;
        }
    }

    public class Converter
    {
        private List<Unit> units;

        public Converter()
        {
            units = new List<Unit>
            {
                new Unit("meter", 1, Category.Length),
                new Unit("kilometer", 1000, Category.Length),
                new Unit("centimeter", 0.01, Category.Length),
                new Unit("mile", 1609.34, Category.Length),
                new Unit("foot", 0.3048, Category.Length),
                new Unit("inch", 0.0254, Category.Length),

                new Unit("kilogram", 1, Category.Weight),
                new Unit("gram", 0.001, Category.Weight),
                new Unit("pound", 0.453592, Category.Weight),
                new Unit("ounce", 0.0283495, Category.Weight),
                new Unit("ton", 1000, Category.Weight),

                new Unit("celsius", 1, Category.Temperature),
                new Unit("fahrenheit", 1, Category.Temperature),
                new Unit("kelvin", 1, Category.Temperature),

                new Unit("liter", 1, Category.Volume),
                new Unit("milliliter", 0.001, Category.Volume),
                new Unit("gallon", 3.78541, Category.Volume),
                new Unit("cup", 0.236588, Category.Volume),

                new Unit("mps", 1, Category.Speed),
                new Unit("kph", 0.277778, Category.Speed),
                new Unit("mph", 0.44704, Category.Speed)
            };
        }

        public double Convert(double value, string fromUnit, string toUnit)
        {
            Unit from = units.Find(u => u.Name.Equals(fromUnit, StringComparison.OrdinalIgnoreCase));
            Unit to = units.Find(u => u.Name.Equals(toUnit, StringComparison.OrdinalIgnoreCase));

            if (from == null || to == null)
            {
                throw new ArgumentException("Invalid unit");
            }

            if (from.Category != to.Category)
            {
                throw new ArgumentException("Cannot convert between different categories");
            }

            if (from.Category == Category.Temperature)
            {
                return ConvertTemperature(value, fromUnit, toUnit);
            }

            double baseValue = value * from.ToBase;
            return baseValue / to.ToBase;
        }

        private double ConvertTemperature(double value, string fromUnit, string toUnit)
        {
            fromUnit = fromUnit.ToLower();
            toUnit = toUnit.ToLower();

            double celsius = 0;

            if (fromUnit == "celsius")
            {
                celsius = value;
            }
            else if (fromUnit == "fahrenheit")
            {
                celsius = (value - 32) * 5.0 / 9.0;
            }
            else if (fromUnit == "kelvin")
            {
                celsius = value - 273.15;
            }

            if (toUnit == "celsius")
            {
                return celsius;
            }
            else if (toUnit == "fahrenheit")
            {
                return (celsius * 9.0 / 5.0) + 32;
            }
            else if (toUnit == "kelvin")
            {
                return celsius + 273.15;
            }

            return 0;
        }

        public List<string> GetUnitsForCategory(Category category)
        {
            return units.Where(u => u.Category == category).Select(u => u.Name).ToList();
        }

        public List<string> GetAllUnits()
        {
            return units.Select(u => u.Name).ToList();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Converter converter = new Converter();

            while (true)
            {
                Console.WriteLine("\n=== Unit Converter ===");
                Console.WriteLine("1. Length");
                Console.WriteLine("2. Weight");
                Console.WriteLine("3. Temperature");
                Console.WriteLine("4. Volume");
                Console.WriteLine("5. Speed");
                Console.WriteLine("6. Custom Conversion");
                Console.WriteLine("7. Exit");

                Console.Write("\nEnter choice: ");
                string choice = Console.ReadLine();

                if (choice == "7")
                {
                    break;
                }

                Category category;
                switch (choice)
                {
                    case "1":
                        category = Category.Length;
                        break;
                    case "2":
                        category = Category.Weight;
                        break;
                    case "3":
                        category = Category.Temperature;
                        break;
                    case "4":
                        category = Category.Volume;
                        break;
                    case "5":
                        category = Category.Speed;
                        break;
                    case "6":
                        PerformCustomConversion(converter);
                        continue;
                    default:
                        Console.WriteLine("Invalid choice");
                        continue;
                }

                PerformCategoryConversion(converter, category);
            }
        }

        static void PerformCategoryConversion(Converter converter, Category category)
        {
            List<string> units = converter.GetUnitsForCategory(category);

            Console.WriteLine("\nAvailable units:");
            for (int i = 0; i < units.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {units[i]}");
            }

            Console.Write("\nFrom unit number: ");
            int fromIndex = int.Parse(Console.ReadLine()) - 1;

            Console.Write("To unit number: ");
            int toIndex = int.Parse(Console.ReadLine()) - 1;

            if (fromIndex < 0 || fromIndex >= units.Count || toIndex < 0 || toIndex >= units.Count)
            {
                Console.WriteLine("Invalid unit selection");
                return;
            }

            Console.Write("Value to convert: ");
            double value = double.Parse(Console.ReadLine());

            try
            {
                double result = converter.Convert(value, units[fromIndex], units[toIndex]);
                Console.WriteLine($"\nResult: {value} {units[fromIndex]} = {result:F4} {units[toIndex]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void PerformCustomConversion(Converter converter)
        {
            Console.Write("\nFrom unit: ");
            string fromUnit = Console.ReadLine();

            Console.Write("To unit: ");
            string toUnit = Console.ReadLine();

            Console.Write("Value: ");
            double value = double.Parse(Console.ReadLine());

            try
            {
                double result = converter.Convert(value, fromUnit, toUnit);
                Console.WriteLine($"\nResult: {value} {fromUnit} = {result:F4} {toUnit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
