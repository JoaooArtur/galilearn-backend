using Microsoft.AspNetCore.Mvc;

namespace WebBff.Endpoints.Students.Requests
{
    /// <summary>
    /// Represents the request for sign up a Student.
    /// </summary>
    public sealed class SignUpRequest
    {
        public string Email { get; set; }
    }
}
