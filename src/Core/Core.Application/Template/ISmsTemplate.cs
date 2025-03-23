//namespace Core.Application.Template
//{
//    public interface ISmsTemplate { Commands.Sms GenerateSms(List<string> destinations); }

//    public class SmsTemplate : ISmsTemplate
//    {
//        protected SmsTemplate(string body, Dictionary<string, string> parameters = null)
//        {
//            Body = GenerateBody(parameters, body);
//        }

//        private static string Body { get; set; }

//        public Commands.Sms GenerateSms(List<string> destinations)
//            => new(destinations.Select(x => new Dto.Destination(x)).ToList(), Body);

//        public string GenerateBody(Dictionary<string, string> parameters, string body)
//        {
//            var generatedBody = body;
//            foreach (var parameter in parameters)
//                generatedBody = generatedBody.Replace(parameter.Key, parameter.Value);
//            return generatedBody;
//        }
//    }
//}
