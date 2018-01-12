using System;
using System.Collections.Generic;
using TestProject1.Models;

namespace TestProject2.Models
{
    /// <summary>
    /// TestClassWithInheritance1:TestClassWithTwoInterfaces1
    /// </summary>
    public class TestClassWithInheritance1 : TestClassWithTwoInterfaces1
    {
        public override string ToString()
        {
            return "";
        }

        /// <summary>
        /// константааааа
        /// </summary>
        public const int SomeConst = 228;


        /// <summary>
        /// Test Return List Method
        /// </summary>
        /// <param name="list">input param list</param>
        /// <param name="floatNumber"> float param epta</param>
        /// <returns>Ienum returns</returns>
        public IEnumerable<List<TestClassWithInheritance1>> TestReturnListMethod(List<TestClassWithOneInterface1> list, float floatNumber )
        {
            var bs = SomeConst;
            return null;
        }

        /// <summary>
        /// <seealso cref="TestMethodWithTwoParams1"/>
        /// An enumeration containing the available robot actions. The available actions are:
        /// <list type="table">
        /// <listheader>
        /// <term>Action</term>
        /// <term>Description</term>
        /// <term>Power Consumption</term>
        /// </listheader>
        /// <item>
        /// <term>Forward</term>
        /// <term>Move <see cref="TestMethodWithTwoParams1"/> in a straight line.</term>
        /// <term>50W</term>
        /// </item>
        /// <item>
        /// <term><paramref name="str"/></term>
        /// <term>Move backwards in a straight line.</term>
        /// <term>50W</term>
        /// </item>
        /// <item>
        /// <term>RotateLeft</term>
        /// <term>Rotate to the left.</term>
        /// <term>30W</term>
        /// </item>
        /// <item>
        /// <term>RotateRight</term>
        /// <term>Rotate to the right.</term>
        /// <term>30W</term>
        /// </item>
        /// <item>
        /// <term>Dig</term>
        /// <term>Tells the robot to dig and obtain a soil sample.</term>
        /// <term>800W</term>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="str">lol</param>
        /// <param name="int64">kek</param>
        public TestClassWithInheritance1(string str, Int64 int64)
        {

        }

        public TestClassWithInheritance1()
        {

        }

        /// <summary>
        /// overriding TestMethodWithTwoParams1 <![CDATA[kbsfg;jabng>?<param name="<M<<dmffgljnafsg"></param>]]>
        /// </summary>
        /// <param name="param1"> int </param>
        /// <param name="param2"> string </param>
        public override void TestMethodWithTwoParams1(int param1, string param2)
        {
            throw new NotImplementedException();
        }

        public void TestOverrideMethod(int param1)
        {
            
        }

        public void TestOverrideMethod()
        {
            
        }

        public void TestOverrideMethod(string param)
        {

        }

        public void TestOverrideMethod(string param1, int param2)
        {

        }

        public TestNestedClass1 TestGenericMethod<T>() where T : TestNestedInheritanceClass1, ITestInterface1
        {
            return null;
        }

        public class TestNestedClass1
        {
            public class TestNestedInPublicClass1 { }

            protected class TestNestedInProtectedClass1 { }

            internal class TestNestedInInternalClass1 { }

            private class TestNestedInPrivateClass1 { }
        }

        protected class TestNestedProtectedClass1 { }

        internal class TestNestedInternalClass1 { }

        private class TestNestedPrivateClass1 { }

        public class TestNestedInheritanceClass1 : TestNestedClass1
        {

        }

        private int TestPrivateField;

        protected int TestProtectedField;

        internal int TestInternalField;

        public int TestPublicField;

    }

    internal class TestInternalClass
    {

    }

    class TestPrivateClass
    {

    }

}