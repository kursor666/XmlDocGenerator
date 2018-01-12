using System;

namespace TestProject2.Models
{
    /// <summary>
    /// TestStructWithInterface1
    /// </summary>
    public struct TestStructWithInterface1:ITestInterface1
    {
        public TestEnum1 TestEnum1 { get; set; }

        public TestStruct1 TestStruct1 { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// inheritdoc TestMethod
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="param1">inheritdoc TestEnum1</param>
        /// <param name="param2">inheritdoc TestStruct1</param>
        public void TestMethod(TestEnum1 param1, TestStruct1 param2)
        {
            throw new System.NotImplementedException();
        }

    }
}