using Shared.Entities;

namespace OutsourcePlatformApp.Models;

public class OrderVacancy : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int MaxWorkers { get; set; }
    public OrderRole OrderRole { get; set; }
    public int OrderRoleId { get; set; }
    public List<Response>? Responses { get; set; }
}