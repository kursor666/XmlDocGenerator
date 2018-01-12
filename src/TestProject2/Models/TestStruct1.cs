using System;

namespace TestProject2.Models
{
    /// <summary>
    /// TestStruct1
    /// </summary>
    public struct TestStruct1
    {
        public static int TestPublicStaticField;

        internal static int TestInternalStaticField;

        public static int TestPublicStaticProperty { get; set; }

        internal static int TestInternalStaticProperty { get; set; }

        public static int TestPublicStaticMethod()
        {
            return 0;
        }

        internal static int TestInternalStaticMethod()
        {
            return 0;
        }



        /// <summary>
        /// TestMethod1 
        /// </summary>
        public void TestMethod1()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TestMethod2
        /// </summary>
        /// <returns> int </returns>
        public int TestMethod2()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TestMethod3
        /// </summary>
        /// <param name="param1"> int </param>
        /// <returns> string </returns>
        public string TestMethod3(int param1)
        {
            throw new NotImplementedException();
        }
    }
}