using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utmdev.DynamicSearch;
using Utmdev.DynamicSearch.Attributes;
using static Utmdev.DynamicSearch.Attributes.Enums;

namespace Playground
{
    class Program
    {

        /// <summary>
        /// Search related model
        /// </summary>
        class Car
        {
            public string Model { get; set; }
            public string Owner { get; set; }
            public int Year { get; set; }
            public string Color { get; set; }
            public string Location { get; set; }
            public string EngineNumber { get; set; }
            public double Price { get; set; }

            public override string ToString()
                => $"Model = {Model}," +
                $" Owner = {Owner}," +
                $" Year = {Year}, " +
                $" Color = {Color}," +
                $" Location = {Location}" +
                $" Engine number = {EngineNumber}," +
                $" Price = {Price}";
        }

        class CarSearchModel : SearchModel
        {
            // Define order by options
            public enum OrderCarBy
            {
                None,
                Year,
                Price,
            }

            #region Search related properties

            [Compare(CompareType.Contains)]
            public string Model { get; set; }

            [Compare(CompareType.IsEqual)]
            public string Owner { get; set; }

            [Compare(CompareType.IsLessOrEqual)]
            public int Year { get; set; }

            [Compare(CompareType.IsEqual)]
            public string Color { get; set; }

            [Compare(CompareType.Contains)]
            public string Location { get; set; }

            [Compare(CompareType.IsEqual)]
            public string EngineNumber { get; set; }

            [DefaultValue(OrderCarBy.None)]
            public OrderCarBy OrderBy { get; set; }

            #endregion
        }

        static void Main(string[] args)
        {
            var cars = GetCars();

            IQueryable<Car> results;

            // Search cars by year range
            var searchModel = new CarSearchModel
            {
                Owner = "John Doe",
                Year = 2015,
                OrderBy = CarSearchModel.OrderCarBy.Price
            };

            var searchQuery = new SearchQuery<Car, CarSearchModel>();
            searchQuery.Source = cars.AsQueryable();

            results = searchQuery.Run(searchModel);

            // Show results
            if (results.Any())
            {
                foreach (var item in results)
                    Console.WriteLine(item);
            }
        }

        private static List<Car> GetCars()
        {
            return new List<Car>
            {
                // Bmw
                new Car
                {
                    Owner = "John Doe",
                    Model = "Bmw X3",
                    Color = "Black",
                    Year = 2010,
                    EngineNumber = "FF4563",
                    Price = 15330,
                },
                // Mercedes
                new Car
                {
                    Owner = "John Doe",
                    Model = "Mercedes E220",
                    Color = "Gray",
                    Year = 2009,
                    EngineNumber = "MS2344",
                    Price = 8900,
                },
                // Audi
                new Car
                {
                    Owner = "Ian Carlson",
                    Model = "Audi TT",
                    Color = "Red",
                    Year = 2004,
                    EngineNumber = "MS2343",
                    Price = 6000,
                },
                // Ferrari
                new Car
                {
                    Owner = "Mark Forster",
                    Model = "Ferrari GT",
                    Color = "Yellow",
                    Year = 1995,
                    Price = 154000,
                },
                // Nissan
                new Car
                {
                    Owner = "Donald Trump",
                    Model = "Nissan Quasqai",
                    Color = "Blue",
                    Year = 2015,
                    Price = 18000,
                },
            };
        }
    }
}
