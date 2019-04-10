using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DefibrillatorService.Models
{
    public class Point
    {
        [XmlElement("lng")]
        public double Longitude { get; set; }
        [XmlElement("lat")]
        public double Latitude { get; set; }
    }

    public enum ItemType
    {
        EAD
    }

    public class Polygon
    {
        public string Name { get; set; }
        public UInt32 SRID { get; set; }
        public double ClusterDistance { get; set; }
        public Point[] Points { get; set; }
        public ItemType[] ItemTypes { get; set; }
    }

    public enum LocationTypesMode
    {
        EXCLUDE
    }

    public class LocationType
    {
        public string Item { get; set; }
    }

    public class LocationTypes
    {
        public LocationTypesMode Mode { get; set; }
        public bool IsMobile { get; set; }
        public LocationType LocationType { get; set; }
    }

    public class LimitingCriteria
    {
        public string[] IncludeItemTypes { get; set; }
        public LocationTypes LocationTypes { get; set; }
    }

    public class MapSearchCriteria
    {
        public Polygon[] Polygons { get; set; }
        public LimitingCriteria LimitingCriteria { get; set; }
    }
}
