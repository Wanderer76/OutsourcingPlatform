using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Utils;

namespace OutsourcePlatformApp.Dto
{
    public class OrderCreationDto
    {
        [Range(0.0, double.MaxValue)] public int Price { get; set; }
        [MinLength(1)] public string Name { get; set; }
        [MinLength(1)] public string Description { get; set; }

        //Todo -1 неограниченое количество
        [Range(-1, int.MaxValue)] public int MaxWorkers { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly Deadline { get; set; }

        //[MinLength(0)] public ICollection<SkillDto>? OrderSkills { get; set; }
        [MinLength(0)] public ICollection<CategoryDto>? OrderCategories { get; set; }
        [MinLength(0)] public ICollection<SkillDto>? OrderSkills { get; set; }
        public List<OrderVacancyDto> OrderVacancies { get; set; }
    }
}