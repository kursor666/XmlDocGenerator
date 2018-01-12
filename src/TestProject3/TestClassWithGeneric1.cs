using System;
using TestProject1.Models;
using TestProject1.TestInterfaces;

namespace TestProject3
{
    /// <summary>
    /// TestClassWithGeneric1
    /// </summary>
    /// <typeparam name="TTestGeneric">where TTestGeneric: ITestGenericInterface1</typeparam>
    public class TestClassWithGeneric1<TTestGeneric> where TTestGeneric: ITestGenericInterface1
    {
        /// <summary>
        /// public TestClassWithGeneric1 ctor
        /// </summary>
        public TestClassWithGeneric1(TestClassWithTwoInterfaces1 testClassWithTwoInterfaces1,
            ITestInterface2 testInterface2)
        {
            _testClassWithTwoInterfaces1 = testClassWithTwoInterfaces1;
            _testInterface2 = testInterface2;
            TestPrivateMethod();
        }

        /// <summary>
        /// private TestPrivateMethod
        /// </summary>
        private void TestPrivateMethod()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// private TestClassWithTwoInterfaces1
        /// </summary>
        private TestClassWithTwoInterfaces1 _testClassWithTwoInterfaces1;

        /// <summary>
        /// private ITestInterface2
        /// </summary>
        private ITestInterface2 _testInterface2;

    }
}