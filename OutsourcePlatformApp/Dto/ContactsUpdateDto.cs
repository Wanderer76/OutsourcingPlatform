using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OutsourcePlatformApp.Dto
{
    public class ContactsUpdateDto
    {
        public int Id { get; set; }
        public string? About { get; set; }
        public string? VkNickname { get; set; }
        public string? GithubUrl { get; set; }
        public string? Messager { get; set; }
        public string? Behance { get; set; }
        public string? LinkedIn { get; set; }
        public string? Dribble { get; set; }
    }
}