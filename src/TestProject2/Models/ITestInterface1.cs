namespace TestProject2.Models
{
    /// <summary>
    /// ITestInterface1
    /// </summary>
    public interface ITestInterface1
    {
        /// <summary>
        /// TestEnum1
        /// </summary>
        TestEnum1 TestEnum1 { get; set; }

        /// <summary>
        /// TestStruct1
        /// </summary>
        TestStruct1 TestStruct1 { get; set; }

        /// <summary>
        /// TestMethod
        /// </summary>
        /// <param name="param1">TestEnum1</param>
        /// <param name="param2">TestStruct1</param>
        void TestMethod(TestEnum1 param1, TestStruct1 param2);
    }
}