using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models.Chat;
using Shared.Entities;


namespace OutsourcePlatformApp.Models
{
    [Table("Executors")]
    public class Executor : BaseEntity
    {
        [DataType(DataType.Date)] public DateOnly Birthdate { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Education>? Educations { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Skill>? Skills { get; set; }
//        public List<ChatRoom>? ChatRooms { get; set; } = new();
    }
}