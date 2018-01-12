using System;
using System.IO;
using XmlDocGenerator.MarkdownManager;
using XmlDocGenerator.ReflectionManager;
using XmlDocGenerator.Util;
using XmlDocGenerator.YamlManager;

namespace XmlDocGenerator.ProjectManager
{
    /// <summary>
    /// Класс, занимающийся управлением работой приложения, в зависимости от выбранного режима работы.
    /// </summary>
    public class ProjectController : BaseController
    {
        private bool _writeLogs;

        private BaseController _currentController;

        private YamlController _yamlController;

        /// <summary>
        /// Метод выполняющий опредленнные инструкции необходимые для генерации документации.
        /// </summary>
        /// <param name="generateMode">Набор опций, определяющих параметры генерации.</param>
        /// <returns></returns>
        public int Generate(GenerateMode generateMode)
        {
            try
            {
                if (!Directory.Exists(generateMode.OutPath)) Directory.CreateDirectory(generateMode.OutPath);
                if (!Directory.Exists(generateMode.TemplatesPath) ||
                    Directory.GetFiles(generateMode.TemplatesPath).Length == 0)
                    throw new FileNotFoundException();
                _currentController = new MarkdownController(generateMode);
                if (generateMode.Reneg)
                    ((MarkdownController)_currentController).DeleteCash();
                Meta(generateMode);
                ((MarkdownController)_currentController).SetTypeItems(_yamlController.GetOneTypeItems());
                ((MarkdownController)_currentController).CreateAndSave();
                if (generateMode.Clear)
                    _yamlController.DeleteCash();
                return 0;
            }
            catch (Exception e)
            {
                _writeLogs = true;
                VerboseMessage(e.Message);
                throw;
                return 1;
            }
        }

        /// <summary>
        /// Метод выполняющий опредленнные инструкции необходимые для сканирования сборки.
        /// </summary>
        /// <param name="scanMode">Набор опций, определяющих параметры сканирования.</param>
        /// <returns></returns>
        public int Scan(ScanMode scanMode)
        {
            try
            {
                if (!File.Exists(scanMode.DllPath)) throw new FileNotFoundException();
                if (!Directory.Exists(scanMode.MetaPath)) Directory.CreateDirectory(scanMode.MetaPath);
                _currentController = new ReflectionController(scanMode);
                Meta(scanMode);
                var objItems = ((ReflectionController) _currentController).GetObjectTypeItems();
                _yamlController.Serialize(objItems);
                return 0;
            }
            
            catch (Exception e)
            {
                _writeLogs = true;
                VerboseMessage(e.Message);
                return 1;
            }
        }

        private void Meta(Options options)
        {
            _yamlController = new YamlController(options);
            _writeLogs = options.Verbose;
            if (_currentController != null)
                _currentController.WriteLog += VerboseMessage;
        }

        /// <summary>
        /// Метод выводящий данные в консоль, а так же проверяющий необходимость вывода данных.
        /// </summary>
        /// <param name="mesage">Сообщение для вывода.</param>
        public void VerboseMessage(string mesage)
        {
            if (_writeLogs)
                Console.WriteLine(mesage);
        }

    }

    /// <summary>
    /// Делегат через который вызывается метод, в котором определяется необходимость вывода данных в консоль.
    /// </summary>
    /// <param name="message">Сообщение для вывода в консоль.</param>
    public delegate void ProcessLogsHandler(string message);
}