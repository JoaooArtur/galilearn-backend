using Microsoft.AspNetCore.Mvc;

namespace WebBff.Endpoints.Students.Requests
{
    /// <summary>
    /// Represents the request for sign up a Student.
    /// </summary>
    public sealed class SignUpRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
