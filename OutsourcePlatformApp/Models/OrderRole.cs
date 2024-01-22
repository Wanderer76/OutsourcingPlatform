using Shared.Entities;

namespace OutsourcePlatformApp.Models;

public class OrderRole : BaseEntity
{
    public string Name { get; set; }
    public List<OrderVacancy> OrderVacancies { get; set; }
}