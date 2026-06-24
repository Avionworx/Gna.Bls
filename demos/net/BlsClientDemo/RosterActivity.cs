using System.Text.Json.Serialization;

namespace BlsClientDemo
{
    //For list of all properties look in the Model definition...
    //In the example we use default Gna Model
    public class RosterActivity
    {
        [JsonPropertyName("ActivityId")]
        public Guid ActivityId { get; set; }

        //use JsonPropertyName attribute to match your data model 
        // STD is the coresponding property name in the Gna Model
        [JsonPropertyName("STD")]
        public DateTime? Start { get; set; }

        // STD is the coresponding property name in the Gna Model
        [JsonPropertyName("STA")]
        public DateTime? End { get; set; }

        // If your property name is matching property name in the model, no need to use JsonPropertyName attribute
        public int? FlightNumber { get; set; }
        public string? CrewNumer { get; set; }

        public string Rank { get; set; } = "CAP";
        public string ActingRank { get; set; } = "CAP";

        public DateTime EmploymentDate { get; set; } = new DateTime(2020, 1, 1);
        public DateTime BirthDate { get; set; } = new DateTime(1980, 1, 1);
        public bool IsActive { get; set; } = true;

        public bool IsDuty { get; set; } = true;

        public int FlightDeckCount { get; set; } = 2;

        public string? Code { get; set; }

        public double EmploymentPercentage { get; set; } = 100;

        public string HomeBaseCode { get; set; } = "MMX";

        public string AirlineCode { get; set; } = "AWX";

        public string DepAirportCode { get; set; } = "MMX";

        public string ServiceTypeCode { get; set; } = "J";

        public string ArrAirportCode { get; set; } = "MMX";

        public string Registration { get; set; } = "SE-DUU";

        public string AcTypeCode { get; set; } = "221";

        public string AcVersion { get; set; } = "221";

        public string AcConfiguration { get; set; } = "Y135";
    }


}
