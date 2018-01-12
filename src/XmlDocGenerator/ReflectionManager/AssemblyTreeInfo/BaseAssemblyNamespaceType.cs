using System.Reflection;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo
{
    /// <summary>
    /// Базовый класс для моделей.
    /// </summary>
    public class BaseAssemblyNamespaceType
    {
        /// <summary>
        /// Свойство, показывающее из сборки этот тип, или он импортирован.
        /// </summary>
        public bool IsTargetType { get; set; } = false;

        /// <summary>
        /// Пространство имен данного типа.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Имя типа.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полное имя типа, включающее пространство имен.
        /// </summary>
        public string FullName { get; set; }

        public string XmlName { get; set; }

        public string Description { get; set; }

        public Module Module { get; set; }

        public string Form { get; set; }

        public string Version { get; set; }

        protected BaseAssemblyNamespaceType(Assembly assembly)
        {
            Version = assembly.FullName;
        }
    }
}