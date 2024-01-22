using System.ComponentModel.DataAnnotations;

namespace OutsourcePlatformApp.Dto;

public class CommonCustomerDto
{
    public string Addres { get; set; }
    public string INN { get; set; }
    public string Fullname { get; set; }
    
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    public string VkNickname { get; set; }
    public string Messager { get; set; }
}