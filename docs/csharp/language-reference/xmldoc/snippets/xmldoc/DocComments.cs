using System;

//-----------------------------------------------------------------------------
namespace XmlTags
{
    //<ClassExample>
    /// <summary>
    /// You may have some primary information about this class.
    /// </summary>
    /// <remarks>
    /// You may have some additional information about this class.
    /// </remarks>
    public class SomeClass
    {
        /// <returns>Returns zero.</returns>
        public static int GetZero() => 0;

        /// <param name="Int1">Used to indicate status.</param>
        /// <param name="Float1">Used to specify context.</param>
        public static void DoWork(int Int1, float Float1)
        {
        }

        /// <summary>DoMoreWork is a method in the TestClass class.
        /// The <paramref name="int1"/> parameter takes a number.
        /// </summary>
        public static void DoMoreWork(int int1)
        {
        }

        /// <summary>DoWork is a method in the TestClass class.
        /// <para>
        ///  Here's how you could make a second paragraph in a description. 
        /// <see cref="System.Console.WriteLine(System.String)"/> for information about output statements.
        /// </para>
        /// <seealso cref="TestClass.Main"/>
        /// </summary>
        /// <exception cref="System.Exception">Thrown when...</exception>
        public void DoSomething()
        {
            try
            {
            }
            catch (InvalidOperationException)
            {
            }
        }
        private string _name;

        /// <value>The Name property represents the employee's name.</value>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        /// <summary>Here is an example of a bulleted list:
        /// <list type="bullet">
        /// <item>
        /// <description>Item 1.</description>
        /// </item>
        /// <item>
        /// <description>Item 2.</description>
        /// </item>
        /// </list>
        /// </summary>
        static void BulletListMethod()
        {
        }

        /// <summary><c>DoAdditionalWork</c> is a method in the <c>TestClass</c> class.
        /// </summary>
        public static void DoAdditionalWork(int Int1)
        {
        }

        /// <summary>
        /// Adds two integers and returns the result.
        /// </summary>
        /// <returns>
        /// The sum of two integers.
        /// </returns>
        /// <example>
        /// <code>
        /// int c = Math.Add(4, 5);
        /// if (c > 10)
        /// {
        ///     Console.WriteLine(c);
        /// }
        /// </code>
        /// </example>
        /// <exception cref="System.OverflowException">Thrown when one parameter is max
        /// and the other is greater than 0.</exception>
        /// See <see cref="Math.Add(double, double)"/> to add doubles.
        public static int Add(int a, int b)
        {
            if ((a == int.MaxValue && b > 0) || (b == int.MaxValue && a > 0))
                throw new System.OverflowException();

            return a + b;
        }
    }
    /// <summary>
    /// TestClass contains several cref examples.
    /// </summary>
    public class TestClass
    {
        /// <summary>
        /// This sample shows how to specify the <see cref="TestClass"/> constructor as a cref attribute.
        /// </summary>
        public TestClass()
        { }

        /// <summary>
        /// This sample shows how to specify the <see cref="TestClass(int)"/> constructor as a cref attribute.
        /// </summary>
        public TestClass(int value)
        { }

        /// <summary>
        /// The GetZero method.
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="GetZero"/> method.
        /// <code>
        /// class TestClass
        /// {
        ///     static int Main()
        ///     {
        ///         return GetZero();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static int GetZero()
        {
            return 0;
        }
    }
}
//</ClassExample>

namespace InheritDoc
{
    //<InheritDocTag>
    /// <summary>
    /// You may have some primary information about this class.
    /// </summary>
    public class MainClass
    {
    }

    ///<inheritdoc/>
    public class DerivedClass : MainClass
    {
    }

    /// <summary>
    /// You may have some primary information about this interface.
    /// </summary>
    public interface ITestInterface
    {
    }

    ///<inheritdoc cref="ITestInterface"/>
    public class ImplementingClass : ITestInterface
    {
    }

    public class InheritOnlyReturns
    {
        /// <summary>In this example, this summary is only visible for this method.</summary>
        /// <returns>A boolean</returns>
        public static bool MyParentMethod(bool x) { return x; }

        /// <inheritdoc cref="MyParentMethod" path="/returns"/>
        public static bool MyChildMethod() { return false; }
    }

    public class InheritAllButRemarks
    {
        /// <summary>In this example, this summary is visible on all the methods.</summary>
        /// <remarks>The remarks.</remarks>
        /// <returns>A boolean</returns>
        public static bool MyParentMethod(bool x) { return x; }

        /// <inheritdoc cref="MyParentMethod" path="//*[not(self::remarks)]"/>
        public static bool MyChildMethod() { return false; }
    }
    //</InheritDocTag>

    //<IncludeTag>
    /// <include file='xml_include_tag.xml' path='MyDocs/MyMembers[@name="test"]/*' />
    class Test
    {
        static void Main()
        {
        }
    }

    /// <include file='xml_include_tag.xml' path='MyDocs/MyMembers[@name="test2"]/*' />
    class Test2
    {
        public void Test()
        {
        }
    }
    //</IncludeTag>
}

//<GenericExample>
/// <summary>
/// GenericClass.
/// </summary>
/// <remarks>
/// This example shows how to specify the <see cref="GenericClass{T}"/> type as a cref attribute.
/// </remarks>
class GenericClass<T>
{
    // Fields and members.
}

public class ParamsAndParamRefs
{
    /// <summary>
    /// The GetGenericValue method.
    /// </summary>
    /// <remarks>
    /// This sample shows how to specify the <see cref="GetGenericValue"/> method as a cref attribute.
    /// </remarks>
    public static T GetGenericValue<T>(T para)
    {
        return para;
    }

    /// <summary>
    /// Creates a new array of arbitrary type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The element type of the array</typeparam>
    public static T[] mkArray<T>(int n)
    {
        return new T[n];
    }
}
//</GenericExample>
