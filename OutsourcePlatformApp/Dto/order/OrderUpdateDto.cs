using Newtonsoft.Json;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Utils;

namespace OutsourcePlatformApp.Dto
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public int? Price { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? MaxWorkers { get; set; }

        [property: JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly? Deadline { get; set; }
        public string? CompanyName { get; set; }
        public List<SkillDto>? OrderSkills { get; set; }
        public List<CategoryDto>? OrderCategories { get; set; }
    }
}