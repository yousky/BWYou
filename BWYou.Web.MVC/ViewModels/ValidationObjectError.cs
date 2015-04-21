
namespace BWYou.Web.MVC.ViewModels
{
    public class ValidationObjectError
    {
        public string ObjectName { get; set; }
        public string ErrorMessage { get; set; }

        public ValidationObjectError(string ObjectName, string ErrorMessage)
        {
            this.ObjectName = ObjectName;
            this.ErrorMessage = ErrorMessage;
        }
    }
}
