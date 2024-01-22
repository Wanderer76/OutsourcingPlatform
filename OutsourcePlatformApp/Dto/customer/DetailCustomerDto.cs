using System.ComponentModel.DataAnnotations;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class DetailCustomerDto
    {
        public string INN { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public List<OrderUpdateDto>? Orders { get; set; }

        public DetailCustomerDto(Customer customer, IEnumerable<Order> orders)
        {
            INN = customer.INN;
            CompanyName = customer.CompanyName;
            Address = customer.Address;
            Orders = orders.Select(order =>
                new OrderUpdateDto
                {
                    Id = order.Id,
                    Price = order.Price,
                    Name = order.Name,
                    CompanyName = order.CompanyName,
                    Deadline = order.Deadline.ToDateTime(TimeOnly.MinValue),
                    Description = order.Description,
                    OrderVacancies = order.OrderVacancies.Select(vacancy => new OrderVacancyDto
                    {
                        MaxWorkers = vacancy.MaxWorkers,
                        OrderRole = new OrderRoleDto { Id = vacancy.OrderRole.Id, Name = vacancy.OrderRole.Name }
                    }).ToList(),
                    OrderCategories = order.OrderCategories
                        .Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList(),
                    OrderSkills = order.OrderSkills.Select(c => new SkillDto { Id = c.Id, Name = c.Name }).ToList()
                }).ToList();
        }
    }
}