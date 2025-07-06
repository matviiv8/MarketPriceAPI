using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MarketPriceAPI.Application.DTOs.Requests.HistoricalPrices
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Periodicity
    {
        minute,
        hour,
        day
    }

    public class HistoricalPriceRequestDTO
    {

        [FromQuery(Name = "interval")]
        public int Interval { get; set; } = 1;

        [FromQuery(Name = "periodicity")]
        public Periodicity Periodicity { get; set; } = Periodicity.minute;

        [FromQuery(Name = "startDate")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [FromQuery(Name = "endDate")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now;
    }
}
