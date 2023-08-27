using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models.Chat;


namespace OutsourcePlatformApp.Models
{
    [Table("Executors")]
    public class Executor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExecutorId { get; set; }
        [DataType(DataType.Date)] public DateOnly Birthdate { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Education>? Educations { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Skill>? Skills { get; set; }
        public List<ChatRoom>? ChatRooms { get; set; } = new();
    }
}