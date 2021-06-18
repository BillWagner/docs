---
title: "How to use the XML documentation features - C# programming guide"
description: Learn how to use XML documentation features. See code examples and view additional available resources.
ms.date: 06/01/2018
ms.topic: how-to
---
# How to use the XML documentation features

The following sample provides a basic overview of a type that has been documented.

## Example

```csharp
// If compiling from the command line, compile with: -doc:YourFileName.xml

/// <summary>
/// Class level summary documentation goes here.
/// </summary>
/// <remarks>
/// Longer comments can be associated with a type or member through
/// the remarks tag.
/// </remarks>
public class TestClass : TestInterface
{
    /// <summary>
    /// Store for the Name property.
    /// </summary>
    private string _name = null;

    /// <summary>
    /// The class constructor.
    /// </summary>
    public TestClass()
    {
        // TODO: Add Constructor Logic here.
    }

    /// <summary>
    /// Name property.
    /// </summary>
    /// <value>
    /// A value tag is used to describe the property value.
    /// </value>
    public string Name
    {
        get
        {
            if (_name == null)
            {
                throw new System.Exception("Name is null");
            }
            return _name;
        }
    }

    /// <summary>
    /// Description for SomeMethod.
    /// </summary>
    /// <param name="s"> Parameter description for s goes here.</param>
    /// <seealso cref="System.String">
    /// You can use the cref attribute on any tag to reference a type or member
    /// and the compiler will check that the reference exists.
    /// </seealso>
    public void SomeMethod(string s)
    {
    }

    /// <summary>
    /// Some other method.
    /// </summary>
    /// <returns>
    /// Return values are described through the returns tag.
    /// </returns>
    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public int SomeOtherMethod()
    {
        return 0;
    }

    public int InterfaceMethod(int n)
    {
        return n * n;
    }

    /// <summary>
    /// The entry point for the application.
    /// </summary>
    /// <param name="args"> A list of command line arguments.</param>
    static int Main(System.String[] args)
    {
        // TODO: Add code to start application here.
        return 0;
    }
}

/// <summary>
/// Documentation that describes the interface goes here.
/// </summary>
/// <remarks>
/// Details about the interface go here.
/// </remarks>
interface TestInterface
{
    /// <summary>
    /// Documentation that describes the method goes here.
    /// </summary>
    /// <param name="n">
    /// Parameter n requires an integer argument.
    /// </param>
    /// <returns>
    /// The method returns an integer.
    /// </returns>
    int InterfaceMethod(int n);
}
```

The example generates an *.xml* file with the following contents.

```xml
<?xml version="1.0"?>
<doc>
    <assembly>
        <name>xmlsample</name>
    </assembly>
    <members>
        <member name="T:TestClass">
            <summary>
            Class level summary documentation goes here.
            </summary>
            <remarks>
            Longer comments can be associated with a type or member through
            the remarks tag.
            </remarks>
        </member>
        <member name="F:TestClass._name">
            <summary>
            Store for the Name property.
            </summary>
        </member>
        <member name="M:TestClass.#ctor">
            <summary>
            The class constructor.
            </summary>
        </member>
        <member name="P:TestClass.Name">
            <summary>
            Name property.
            </summary>
            <value>
            A value tag is used to describe the property value.
            </value>
        </member>
        <member name="M:TestClass.SomeMethod(System.String)">
            <summary>
            Description for SomeMethod.
            </summary>
            <param name="s"> Parameter description for s goes here.</param>
            <seealso cref="T:System.String">
            You can use the cref attribute on any tag to reference a type or member
            and the compiler will check that the reference exists.
            </seealso>
        </member>
        <member name="M:TestClass.SomeOtherMethod">
            <summary>
            Some other method.
            </summary>
            <returns>
            Return values are described through the returns tag.
            </returns>
            <seealso cref="M:TestClass.SomeMethod(System.String)">
            Notice the use of the cref attribute to reference a specific method.
            </seealso>
        </member>
        <member name="M:TestClass.Main(System.String[])">
            <summary>
            The entry point for the application.
            </summary>
            <param name="args"> A list of command line arguments.</param>
        </member>
        <member name="T:TestInterface">
            <summary>
            Documentation that describes the interface goes here.
            </summary>
            <remarks>
            Details about the interface go here.
            </remarks>
        </member>
        <member name="M:TestInterface.InterfaceMethod(System.Int32)">
            <summary>
            Documentation that describes the method goes here.
            </summary>
            <param name="n">
            Parameter n requires an integer argument.
            </param>
            <returns>
            The method returns an integer.
            </returns>
        </member>
    </members>
</doc>
```

## Robust programming

XML documentation starts with `///`. When you create a new project, the wizards put some starter `///` lines in for you. The processing of these comments has some restrictions:

- The documentation must be well-formed XML. If the XML is not well-formed, a warning is generated and the documentation file will contain a comment that says that an error was encountered.

- Developers are free to create their own set of tags. There is a [recommended set of tags](recommended-tags.md). Some of the recommended tags have special meanings:

  - The `<param>` tag is used to describe parameters. If used, the compiler verifies that the parameter exists and that all parameters are described in the documentation. If the verification fails, the compiler issues a warning.

  - The `cref` attribute can be attached to any tag to reference a code element. The compiler verifies that this code element exists. If the verification fails, the compiler issues a warning. The compiler respects any `using` statements when it looks for a type described in the `cref` attribute.

  - The `<summary>` tag is used by IntelliSense inside Visual Studio to display additional information about a type or member.

    > [!NOTE]
    > The XML file does not provide full information about the type and members (for example, it does not contain any type information). To get full information about a type or member, use the documentation file together with reflection on the actual type or member.

## Walkthrough

Let's walk through documenting a very basic math library to make it easy for new developers to understand/contribute and for third-party developers to use.

Here's code for the simple math library:

:::code language="csharp" source="./snippets/xmldoc/sample-library.cs":::

The sample library supports four major arithmetic operations (`add`, `subtract`, `multiply`, and `divide`) on `int` and `double` data types.

Now you want to be able to create an API reference document from your code for third-party developers who use your library but don't have access to the source code.
As mentioned earlier XML documentation tags can be used to achieve this. You will now be introduced to the standard XML tags the C# compiler supports.

## \<summary>

The `<summary>` tag adds brief information about a type or member.
I'll demonstrate its use by adding it to the `Math` class definition and the first `Add` method. Feel free to apply it to the rest of your code.

:::code language="csharp" source="./snippets/xmldoc/summary-tag.cs":::

The `<summary>` tag is important, and we recommend that you include it because its content is the primary source of type or member information in IntelliSense or an API reference document.

## \<remarks>

The `<remarks>` tag supplements the information about types or members that the `<summary>` tag provides. In this example, you'll just add it to the class.

:::code language="csharp" source="./snippets/xmldoc/remarks-tag.cs":::

## \<returns>

The `<returns>` tag describes the return value of a method declaration.
As before, the following example illustrates the `<returns>` tag on the first `Add` method. You can do the same on other methods.

:::code language="csharp" source="./snippets/xmldoc/returns-tag.cs" id="ReturnTag":::

## \<value>

The `<value>` tag is similar to the `<returns>` tag, except that you use it for properties.
Assuming your `Math` library had a static property called `PI`, here's how you'd use this tag:

:::code language="csharp" source="./snippets/xmldoc/value-tag.cs":::

## \<example>

You use the `<example>` tag to include an example in your XML documentation.
This involves using the child `<code>` tag.

:::code language="csharp" source="./snippets/xmldoc/example-tag.cs" ID="ExampleTag":::

The `code` tag preserves line breaks and indentation for longer examples.

## \<para>

You use the `<para>` tag to format the content within its parent tag. `<para>` is usually used inside a tag, such as `<remarks>` or `<returns>`, to divide text into paragraphs.
You can format the contents of the `<remarks>` tag for your class definition.

:::code language="csharp" source="./snippets/xmldoc/para-tag.cs":::

## \<c>

Still on the topic of formatting, you use the `<c>` tag for marking part of text as code.
It's like the `<code>` tag but inline. It's useful when you want to show a quick code example as part of a tag's content.
Let's update the documentation for the `Math` class.

:::code language="csharp" source="./snippets/xmldoc/c-tag.cs":::

## \<exception>

By using the `<exception>` tag, you let your developers know that a method can throw specific exceptions.
Looking at your `Math` library, you can see that both `Add` methods throw an exception if a certain condition is met. Not so obvious, though,
is that integer `Divide` method throws as well if the `b` parameter is zero. Now add exception documentation to this method.

:::code language="csharp" source="./snippets/xmldoc/exception-tag.cs":::

The `cref` attribute represents a reference to an exception that is available from the current compilation environment.
This can be any type defined in the project or a referenced assembly. The compiler will issue a warning if its value cannot be resolved.

## \<see>

The `<see>` tag lets you create a clickable link to a documentation page for another code element. In our next example, we'll create a clickable link between the two `Add` methods.

:::code language="csharp" source="./snippets/xmldoc/see-tag.cs":::

The `cref` is a **required** attribute that represents a reference to a type or its member that is available from the current compilation environment.
This can be any type defined in the project or a referenced assembly.

## \<seealso>

You use the `<seealso>` tag in the same way you do the `<see>` tag. The only difference is that its content is typically placed in a "See Also" section. Here we'll add a `seealso` tag on the integer `Add` method to reference other methods in the class that accept integer parameters:

:::code language="csharp" source="./snippets/xmldoc/seealso-tag.cs":::

The `cref` attribute represents a reference to a type or its member that is available from the current compilation environment.
This can be any type defined in the project or a referenced assembly.

## \<param>

You use the `<param>` tag to describe a method's parameters. Here's an example on the double `Add` method:
The parameter the tag describes is specified in the **required** `name` attribute.

:::code language="csharp" source="./snippets/xmldoc/param-tag.cs":::

## \<typeparam>

You use `<typeparam>` tag just like the `<param>` tag but for generic type or method declarations to describe a generic parameter.
Add a quick generic method to your `Math` class to check if one quantity is greater than another.

:::code language="csharp" source="./snippets/xmldoc/typeparam-tag.cs":::

## \<paramref>

Sometimes you might be in the middle of describing what a method does in what could be a `<summary>` tag, and you might want to make a reference to a parameter. The `<paramref>` tag is great for just this. Let's update the summary of our double based `Add` method. Like the `<param>` tag, the parameter name is specified in the **required** `name` attribute.

:::code language="csharp" source="./snippets/xmldoc/paramref-tag.cs":::

## \<typeparamref>

You use `<typeparamref>` tag just like the `<paramref>` tag but for generic type or method declarations to describe a generic parameter.
You can use the same generic method you previously created.

:::code language="csharp" source="./snippets/xmldoc/typeparamref-tag.cs":::

## \<list>

You use the `<list>` tag to format documentation information as an ordered list, unordered list, or table. Make an unordered list of every math operation your `Math` library supports.

:::code language="csharp" source="./snippets/xmldoc/list-tag.cs":::

You can make an ordered list or table by changing the `type` attribute to `number` or `table`, respectively.

## \<inheritdoc>

You can use the `<inheritdoc>` tag to inherit XML comments from base classes, interfaces, and similar methods. This eliminates unwanted copying and pasting of duplicate XML comments and automatically keeps XML comments synchronized.

:::code language="csharp" source="./snippets/xmldoc/inheritdoc-tag.cs":::

### Put it all together

If you've followed this tutorial and applied the tags to your code where necessary, your code should now look similar to the following:

:::code language="csharp" source="./snippets/xmldoc/tagged-library.cs":::

From your code, you can generate a detailed documentation website complete with clickable cross-references. But you're faced with another problem: your code has become hard to read.
There's so much information to sift through that this is going to be a nightmare for any developer who wants to contribute to this code.
Thankfully there's an XML tag that can help you deal with this:

## \<include>

The `<include>` tag lets you refer to comments in a separate XML file that describe the types and members in your source code, as opposed to placing documentation comments directly in your source code file.

Now you're going to move all your XML tags into a separate XML file named `docs.xml`. Feel free to name the file whatever you want.

:::code language="xml" source="./snippets/xmldoc/include.xml":::

In the above XML, each member's documentation comments appear directly inside a tag named after what they do. You can choose your own strategy.
Now that you have your XML comments in a separate file, let's see how your code can be made more readable by using the `<include>` tag:

:::code language="csharp" source="./snippets/xmldoc/include-tag.cs":::

And there you have it: our code is back to being readable, and no documentation information has been lost.

The `file` attribute represents the name of the XML file containing the documentation.

The `path` attribute represents an [XPath](../../../standard/data/xml/xpath-queries-and-namespaces.md) query to the `tag name` present in the specified `file`.

The `name` attribute represents the name specifier in the tag that precedes the comments.

The `id` attribute, which can be used in place of `name`, represents the ID for the tag that precedes the comments.

### User-defined tags

All the tags outlined above represent those that are recognized by the C# compiler. However, a user is free to define their own tags.
Tools like Sandcastle bring support for extra tags like [\<event>](https://ewsoftware.github.io/XMLCommentsGuide/html/81bf7ad3-45dc-452f-90d5-87ce2494a182.htm) and [\<note>](https://ewsoftware.github.io/XMLCommentsGuide/html/4302a60f-e4f4-4b8d-a451-5f453c4ebd46.htm),
and even support [documenting namespaces](https://ewsoftware.github.io/XMLCommentsGuide/html/BD91FAD4-188D-4697-A654-7C07FD47EF31.htm).
Custom or in-house documentation generation tools can also be used with the standard tags and multiple output formats from HTML to PDF can be supported.

## \<permission>

```xml
<permission cref="member">description</permission>
```

- cref = " `member`": A reference to a member or field that is available to be called from the current compilation environment. The compiler checks that the given code element exists and translates `member` to the canonical element name in the output XML. *member* must appear within double quotation marks (" ").
- `description`: A description of the access to the member.

The `<permission>` tag lets you document the access of a member. The <xref:System.Security.PermissionSet> class lets you specify access to a member.

:::code language="csharp" source="./snippets/xmldoc/DocComments.cs" ID="PermissionTag":::
