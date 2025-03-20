namespace CarsAPI.Models
{
    /// <summary>
    /// Domain model
    /// </summary>
    public class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double Engine_Size { get; set; }
        public string Fuel_Type { get; set; }
        public string Transmission { get; set; }
        public int Mileage { get; set; }
        public int Doors { get; set; }
        public int Owner_Count { get; set; }
        public decimal Price { get; set; }
    }
}
