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

        public DetailCustomerDto(Customer customer)
        {
            INN = customer.INN;
            CompanyName = customer.CompanyName;
            Address = customer.Address;
            if (customer.Orders != null)
            {
                Orders = customer.Orders.Select(order =>
                    new OrderUpdateDto
                    {
                        Price = order.Price, Name = order.Name, CompanyName = order.CompanyName,
                        Deadline = order.Deadline, Description = order.Description, MaxWorkers = order.MaxWorkers,
                        OrderCategories = order.OrderCategories
                            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList(),
                        OrderSkills = order.OrderSkills.Select(s => new SkillDto { Id = s.Id, Name = s.Name }).ToList()
                    }).ToList();
            }
        }
    }
}