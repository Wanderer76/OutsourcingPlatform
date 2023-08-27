using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutsourcePlatformApp.Models
{
    [Table("Responses")]
    public class Response
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public int ExecutorId { get; set; }
        [Required] public Order Order { get; set; }
        public bool? IsAccept { get; set; }

        public bool IsCompleted { get; set; } = false;

        public Response()
        {
        }

        public Response(int executorId, Order order)
        {
            ExecutorId = executorId;
            Order = order;
        }
    }
}