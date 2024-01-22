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

        [property: JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Deadline { get; set; }

        public string? CompanyName { get; set; }
        public List<OrderVacancyDto> OrderVacancies { get; set; }
        public List<CategoryDto>? OrderCategories { get; set; }
        public List<SkillDto>? OrderSkills { get; set; }
    }

    public class OrderVacancyDto
    {
        public int Id { get; set; }

        public int MaxWorkers { get; set; }

        // public bool IsResponded { get; set; }
        // public bool IsAccepted { get; set; }
        // public bool IsCompleted { get; set; }
        public OrderRoleDto OrderRole { get; set; }
        public List<ExecutorResponseDto>? Responses { get; set; }
    }

    public class OrderRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}