namespace DefibrillatorService.Models
{
    public class DefibrillatorLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public double DistanceKm { get; set; }
    }
}
