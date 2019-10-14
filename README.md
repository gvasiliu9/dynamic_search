
# Dynamic search library
This library will help you to write LINQ queries dynamically. By using this library you will be able to search through various data sources. Search process is based on Expression Trees.

<a href="https://www.nuget.org/packages/Utmdev.DynamicSearch/" target="_blank"><img src="https://img.shields.io/nuget/v/Utmdev.DynamicSearch?style=for-the-badge"/></a>

![enter image description here](https://pineco.de/wp-content/uploads/2018/03/basic-eloquent-search-techniques-cover.png)

**Usage**

In this example we will execute a search query in a list of cars. First we need to create the car model.

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

Next step is to define our search model. Created search model will be used to execute the search query. 

! IMPORTANT:
Properties from search model should be identical to properties from model. Properties which are not search related should be excluded by using [Exclude] attribute. 

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

Search model attributes:

There are 3 supported attributes:

    [Exclude] - is used when a property does not exist in model.
    [DefaultValue] - is used for enums, when is needed to specify enum default value. For example: our search mecahnism has a OrderBy property which by default can have different values.
    [Compare(CompareType)] - is used to specify compare type for spcified value. For example (IsEqual, Contains, IsLessOrEqual, IsGreaterOrEqual, IsLess, IsGreater).

In order to run a search query we should specify our items source, in our example a list of cars:

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

Run search query:

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

Result:

	Model = Bmw X3, Owner = John Doe, Year = 2010,  Color = Black, Location =  Engine number = FF4563, Price = 15330
	Model = Mercedes E220, Owner = John Doe, Year = 2009,  Color = Gray, Location =  Engine number = MS2344, Price = 8900
