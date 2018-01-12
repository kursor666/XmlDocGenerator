using XmlDocGenerator.ProjectManager;

namespace XmlDocGenerator.Util
{
    public class BaseController
    {
        public event ProcessLogsHandler WriteLog = (string message) => {};

        protected void Write(string message)
        {
            WriteLog(message);
        }
    }
}