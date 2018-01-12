using System;
using System.Xml.Serialization;
using TestProject1.TestInterfaces;

namespace TestProject1.Models
{
    



    /// <inheritdoc />
    /// <summary>
    /// TestClassWithOneInterface1:ITestInterface1
    /// </summary>
    [Serializable]
    public class TestClassWithOneInterface1:ITestInterface1
    {
        /// <summary>
        /// TestMethodWithOneParam1
        /// </summary>
        /// <param name="param1"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void TestMethodWithOneParam1(int param1)
        {
            throw new System.NotImplementedException();
        }

        public void TestMethod(int fff) { }

        /// <inheritdoc />
        /// <summary>
        /// TestMethodWithTwoParams1
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public void TestMethodWithTwoParams1(int param1, string param2)
        {
            throw new System.NotImplementedException();
        }

        public void TestMethodWithoutParams1()
        {
            throw new System.NotImplementedException();
        }

        public int TestProperty1 { get; set; }

        public string TestMethod1()
        {
            throw new System.NotImplementedException();
        }
    }
}