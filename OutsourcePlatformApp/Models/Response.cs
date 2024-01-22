using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Responses")]
    public class Response : BaseEntity
    {
        [Required] public int ExecutorId { get; set; }
        [Required] public OrderVacancy OrderVacancy { get; set; }
        [Required] public int OrderVacancyId { get; set; }
        public bool? IsAccept { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsResourceUploaded { get; set; }

        public Response()
        {
            
        }
        public Response(int executorId, OrderVacancy order)
        {
            ExecutorId = executorId;
            OrderVacancy = order;
            OrderVacancyId = order.Id;
        }
    }
}