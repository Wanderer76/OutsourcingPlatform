using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutsourcePlatformApp.Models
{
    [Table("Contacts")]
    public class Contact
    {
        [ForeignKey("User")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        public string? About { get; set; }
        public string? VkNickname { get; set; }
        public string? GithubUrl { get; set; }
        public string? Messager { get; set; }
        public string? Behance { get; set; }
        public string? LinkedIn { get; set; }
        public string? Dribble { get; set; }

        public User? User { get; set; }

        public Contact(string? about, string? vkNickname, string? githubUrl, string? messager, string? behance = null,
            string? linkedIn = null, string? dribble = null)
        {
            About = about;
            VkNickname = vkNickname;
            GithubUrl = githubUrl;
            Messager = messager;
            Behance = behance;
            LinkedIn = linkedIn;
            Dribble = dribble;
        }
    }
}