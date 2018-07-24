using Destructurama.Attributed;
using MessagePack;
using System;

namespace Company.Common.Data
{
    [Serializable]
    [MessagePackObject]
    public class RegisterRequest
    {
        [Key(nameof(Name))]
        public string Name { get; set; }

        [Key(nameof(Email))]
        public string Email { get; set; }

        [Key(nameof(Password))]
        [NotLogged]
        public string Password { get; set; }

        [Key(nameof(DateOfBirth))]
        public DateTime? DateOfBirth { get; set; }
    }
}
