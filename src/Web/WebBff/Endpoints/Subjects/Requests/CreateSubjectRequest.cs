using Core.Application.Messaging;
using Newtonsoft.Json;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class CreateSubjectRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonRequired]
        public int Index { get; set; }
    }
}
