using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OutsourcePlatformApp.Dto
{
    public class ContactsUpdateDto
    {
        public int Id { get; set; }
        public string? About { get; set; }
        public List<ContactDto> Contacts { get; set; }
    }

    public class ContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public ContactDto()
        {
        }

        public ContactDto(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}