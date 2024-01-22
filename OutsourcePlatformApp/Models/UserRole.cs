using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("UserRoles")]
    public class UserRole : BaseEntity
    {
        [MaxLength(25)]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}