namespace TestProject1.TestInterfaces
{
    /// <summary>
    /// ITestInterface2
    /// </summary>
    public interface ITestInterface2
    {
        /// <summary>
        /// TestMethodWithoutParams1
        /// </summary>
        /// <returns>string</returns>
        string TestMethodWithoutParams1();

        /// <summary>
        /// TestMethodWithOneParam1
        /// </summary>
        /// <param name="param1">ITestInterface1</param>
        /// <returns>string</returns>
        string TestMethodWithOneParam1(ITestInterface1 param1);

        /// <summary>
        /// Test property (ITestInterface1)
        /// </summary>
        ITestInterface1 TestPropertyTestInterface1 { get; set; }
    }
}