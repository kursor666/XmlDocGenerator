using System;

namespace TestProject2.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    [Test]
    public class TestGenericClass<T, U> where T:ITestInterface1 where U:new()
    {
        public TestGenericClass()
        {

        }


        public T TestGenericMethod1<TU>(TU parametr) where TU:TestClassWithInheritance1
        {
            return default(T);
        }


        [Test]
        internal string TestGenericClass_MyTestEvent()
        {
            throw new System.NotImplementedException();
        }

        [Test]
        protected string TestEventMethod()
        {
            return null;
        }


        public override string ToString()
        {
            return base.ToString();
        }

        public event MyTestDelegate MyTestEvent;

    }

    public delegate string MyTestDelegate();

}