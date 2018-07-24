using System;

namespace Company.Api.Rest.Data
{
    [Serializable]
    public class RegisterRequestDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
