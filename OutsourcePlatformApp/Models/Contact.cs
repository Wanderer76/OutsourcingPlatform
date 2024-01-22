using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Contacts")]
    public class Contact : BaseEntity
    {
        public string? About { get; set; }
        public List<ContactLink> ContactLinks { get; set; }
        public User? User { get; set; }
        public int? UserId { get; set; }

    }
}