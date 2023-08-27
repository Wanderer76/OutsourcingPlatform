using System.ComponentModel.DataAnnotations;
using OutsourcePlatformApp.Utils;
using System.Text.Json.Serialization;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class ExecutorUpdateDto
    {
        [DataType(DataType.Date)] 
        [property: JsonConverter(typeof(DateOnlyConverter))] public DateOnly BirthDate { get; set; }
        public string? City { get; set; }

        public ExecutorUpdateDto(Executor executor)
        {
            BirthDate = executor.Birthdate;
            City = executor.City;
        }
    }
}