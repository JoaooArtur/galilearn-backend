using Core.Application.Messaging;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class CreateSubjectRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
    }
}
