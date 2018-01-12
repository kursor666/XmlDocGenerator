using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpYaml.Serialization;
using XmlDocGenerator.ProjectManager;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.TypeItems;

namespace XmlDocGenerator.YamlManager
{
    public class YamlController
    {
        private StringTypeItems _stringTypeItems;
        private readonly string _folderPath;
        private string _fileName;

        public YamlController(Options options)
        {
            _folderPath = options.MetaPath;
        }

        public void Serialize(ObjectTypeItems objectTypesItems)
        {
            //DeleteCash();
            // BAD: 
            var converter = new ReflectionObjectModelToYamlConverter(objectTypesItems);
            _stringTypeItems = converter.GetStringTypesHash();
            _fileName = $"{objectTypesItems.AssemblyName}.yaml";
            var text = new Serializer().Serialize(_stringTypeItems);
            Save(text);
        }

        public StringTypeItems GetOneTypeItems()
        {
            var types = Deserialize().ToList();
            var result = new StringTypeItems();
            foreach (var hash in types)
            {
                result.Types = result.Types?.Concat(hash.Types).ToList();
                result.Constructors = result.Constructors?.Concat(hash.Constructors).ToList();
                result.Events = result.Events?.Concat(hash.Events).ToList();
                result.Fields = result.Fields?.Concat(hash.Fields).ToList();
                result.Methods = result.Methods?.Concat(hash.Methods).ToList();
                result.Properties = result.Properties?.Concat(hash.Properties).ToList();
                result.Namespaces = result.Namespaces?.Concat(hash.Namespaces).ToList();
                result.Attributes = result.Attributes?.Concat(hash.Attributes).ToList();
                result.Delegates = result.Delegates.Concat(hash.Delegates).ToList();
            }
            return result;
        }

        public ICollection<StringTypeItems> Deserialize()
        {
            var files = GetYamlFilesFromFolder(_folderPath);
            var serializer = new Serializer();
            return files.Select(file => serializer.Deserialize<StringTypeItems>(Read(file))).ToList();
        }

        #region FileMethods

        private void Save(string text)
        {
            using (var fstream = new FileStream($"{_folderPath}{_fileName}", FileMode.Create))
            {
                var array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
            }
        }

        private ICollection<string> GetYamlFilesFromFolder(string folderPath)
        {
            var files = new List<string>();
            if (!Directory.Exists(folderPath)) return files;
            files.AddRange(Directory.GetFiles(folderPath, "*.yml"));
            files.AddRange(Directory.GetFiles(folderPath, "*.yaml"));
            return files;
        }

        private string Read(string filePath)
        {
            string textFromFile;
            using (var fstream = File.OpenRead(filePath))
            {
                var array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                textFromFile = System.Text.Encoding.Default.GetString(array);
            }
            return textFromFile;
        }

        public void DeleteCash()
        {
            foreach (var file in GetYamlFilesFromFolder(_folderPath).ToList())
            {
                File.Delete(file);
            }
        }

        #endregion

    }


}
