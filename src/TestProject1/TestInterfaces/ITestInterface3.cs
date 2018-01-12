namespace TestProject1.TestInterfaces
{
    /// <summary>
    /// ITestInterface3
    /// </summary>
    public interface ITestInterface3
    {
        /// <summary>
        /// TestMethodWithoutParams1
        /// </summary>
        /// <returns>ITestInterface1</returns>
        ITestInterface1 TestMethodWithoutParams1();

        /// <summary>
        /// TestProperty1 (ITestInterface2)
        /// </summary>
        ITestInterface2 TestProperty1 { get; set; }
    }
}