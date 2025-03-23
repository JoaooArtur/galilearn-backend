namespace Core.Application.Template
{
    public interface IEmailTemplate
    {
        string GetHtmlString();
    }

    public class EmailTemplate : IEmailTemplate
    {
        protected EmailTemplate(string path)
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            HtmlString = File.ReadAllText(FilePath);
        }

        private string FilePath { get; set; }
        private string HtmlString { get; set; }

        public string GetHtmlString()
            => HtmlString;
    }
}
