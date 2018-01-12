using System.Reflection;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes
{
    public class Event:LocalAssemblyType
    {
        public EventInfo EventInfo { get; }

        public Event(EventInfo member, Assembly assembly) : base(member, assembly)
        {
            EventInfo = member;
            Form = "Event";
        }
    }
}