namespace TestProject1.TestInterfaces
{
    /// <summary>
    /// The <see cref="TestProject1.TestInterfaces"/> namespace contains iterfaces for ....
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc { }

    /// <summary>
    /// ITestInterface1
    /// </summary>
    public interface ITestInterface1
    {
        /// <summary>
        /// TestMethodWithOneParam1 (int)
        /// </summary>
        /// <param name="param1"> Test int param</param>
        void TestMethodWithOneParam1(int param1);

        /// <summary>
        /// TestMethodWithTwoParams1 (int, string)
        /// </summary>
        /// <param name="param1">Test int param</param>
        /// <param name="param2">Test string param</param>
        void TestMethodWithTwoParams1(int param1, string param2);

        /// <summary>
        /// TestMethodWithoutParams1
        /// </summary>
        void TestMethodWithoutParams1();

        /// <summary>
        /// Test int property
        /// </summary>
        int TestProperty1 { get; set; }
    }
}