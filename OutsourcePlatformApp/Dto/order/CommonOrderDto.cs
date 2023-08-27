using OutsourcePlatformApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutsourcePlatformApp.Dto
{
    public class CommonOrderDto
    {
        public int OrderId { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxWorkers { get; set; }
        public string CompanyName { get; set; }
        public int? ResponseCount { get; set; }
        public bool IsResponsed { get; set; }
        public bool IsCompleted { get; set; }
        public DateOnly Deadline { get; set; }
        public OrderSkillsDto? OrderSkills { get; set; }
        public OrderCategoriesDto? OrderCategories { get; set; }

        public CommonOrderDto(int orderId, int price, string name, string description, int maxWorkers, string companyName, int? responseCount, DateOnly deadline,bool isResponsed,bool isCompleted,OrderSkillsDto? orderSkills, OrderCategoriesDto? orderCategories)
        {
            OrderId = orderId;
            Price = price;
            Name = name;
            Description = description;
            MaxWorkers = maxWorkers;
            CompanyName = companyName;
            ResponseCount = responseCount;
            Deadline = deadline;
            IsResponsed = isResponsed;
            OrderSkills = orderSkills;
            OrderCategories = orderCategories;
            IsCompleted = isCompleted;
        }
    }
}
