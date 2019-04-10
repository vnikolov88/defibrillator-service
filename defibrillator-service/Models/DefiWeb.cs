using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DefibrillatorService.Models
{
    public class DefiWebLocationDescription
    {
        public string LanguageCode { get; set; }
        public string Text { get; set; }
    }

    public class DefiWebLocationPoint
    {
        [JsonProperty("lng")]
        public double Longitude { get; set; }
        [JsonProperty("lat")]
        public double Latitude { get; set; }
    }

    public class DefiWebLocationDetail
    {
        public DefiWebLocationPoint Position { get; set; }
        public DefiWebLocationDescription Description { get; set; }
        public string[] Equipment { get; set; }
    }

    public class DefiWebLocationAddress
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
    }

    public class DefiWebLocation
    {
        public string LocationType { get; set; }
        public DefiWebLocationDetail Detail { get; set; }
        public DefiWebLocationAddress Address { get; set; }
    }

    public class DefiWeb
    {
        public DefiWebLocation[] Locations { get; set; }
    }
}
