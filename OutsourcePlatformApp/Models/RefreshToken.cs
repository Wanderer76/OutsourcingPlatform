using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutsourcePlatformApp.Models;

[Table("RefreshTokens")]
public class RefreshToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Token { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly Expires { get; set; }

}