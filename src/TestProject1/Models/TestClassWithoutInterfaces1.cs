using System;
using TestProject1.TestInterfaces;

namespace TestProject1.Models
{
    /// <summary>
    /// TestClassWithoutInterfaces1
    /// </summary>
    public class TestClassWithoutInterfaces1
    {
        /// <summary>
        /// TestMethodWithoutParams1
        /// </summary>
        public void TestMethodWithoutParams1()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TestMethodWithOneParam1
        /// </summary>
        /// <param name="param1">ITestInterface3</param>
        public void TestMethodWithOneParam1(ITestInterface3 param1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TestMethodWithOneParam2
        /// </summary>
        /// <param name="param1">ITestInterface1</param>
        /// <returns>ITestInterface3</returns>
        public ITestInterface3 TestMethodWithOneParam2(ITestInterface1 param1)
        {
            throw new NotImplementedException();
        }
    }
}