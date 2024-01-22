using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models.Chat;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Customers")]
    public class Customer : BaseEntity
    {
        [Required] public string INN { get; set; }
        [Required] public string CompanyName { get; set; }

        public string Address { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        //public List<Order>? Orders { get; set; }
       // public List<ChatRoom>? ChatRooms { get; set; } = new();
    }
}