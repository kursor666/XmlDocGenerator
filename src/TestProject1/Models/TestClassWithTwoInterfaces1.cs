using System.Runtime.CompilerServices;
using Microsoft.Build.Framework;
using TestProject1.TestInterfaces;

namespace TestProject1.Models
{
    /// <summary>
    /// TestClassWithTwoInterfaces1 : ITestInterface1, ITestInterface2
    /// </summary>
    public class TestClassWithTwoInterfaces1 : ITestInterface1, ITestInterface2
    {
        public void TestMethodWithOneParam1(int param1)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return "";
        }

        public virtual void TestMethodWithTwoParams1(int param1, string param2)
        {
            throw new System.NotImplementedException();
        }

        void ITestInterface1.TestMethodWithoutParams1()
        {
            throw new System.NotImplementedException();
        }

        public string TestMethodWithOneParam1(ITestInterface1 param1)
        {
            throw new System.NotImplementedException();
        }

        public ITestInterface1 TestPropertyTestInterface1 { get; set; }


        [Required]
        public int TestProperty1 { get; set; }

        string ITestInterface2.TestMethodWithoutParams1()
        {
            throw new System.NotImplementedException();
        }
    }
}