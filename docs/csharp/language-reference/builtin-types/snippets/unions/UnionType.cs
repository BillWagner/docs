// Required until these types are added to the .NET runtime:
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class UnionAttribute : Attribute;

    public interface IUnion
    {
        object? Value { get; }
    }
}

// <CaseTypes>
public record class Cat(string Name);
public record class Dog(string Name);
public record class Bird(string Name);
// </CaseTypes>

// <BasicDeclaration>
public union Pet(Cat, Dog, Bird);
// </BasicDeclaration>

// <GenericUnion>
public record class None;
public record class Some<T>(T Value);
public union Option<T>(None, Some<T>);
// </GenericUnion>

// <ValueTypeCases>
public union IntOrString(int, string);
// </ValueTypeCases>

// <BodyMembers>
public union OneOrMore<T>(T, IEnumerable<T>)
{
    public IEnumerable<T> AsEnumerable() => Value switch
    {
        T single => [single],
        IEnumerable<T> multiple => multiple,
        _ => []
    };
}
// </BodyMembers>

// <ManualBasicPattern>
[System.Runtime.CompilerServices.Union]
public struct Shape : System.Runtime.CompilerServices.IUnion
{
    private readonly object? _value;

    public Shape(Circle value) { _value = value; }
    public Shape(Rectangle value) { _value = value; }

    public object? Value => _value;
}

public record class Circle(double Radius);
public record class Rectangle(double Width, double Height);
// </ManualBasicPattern>

// <NonBoxingAccessPattern>
[System.Runtime.CompilerServices.Union]
public struct IntOrBool : System.Runtime.CompilerServices.IUnion
{
    private readonly int _intValue;
    private readonly bool _boolValue;
    private readonly byte _tag; // 0 = none, 1 = int, 2 = bool

    public IntOrBool(int? value)
    {
        if (value.HasValue)
        {
            _intValue = value.Value;
            _tag = 1;
        }
    }

    public IntOrBool(bool? value)
    {
        if (value.HasValue)
        {
            _boolValue = value.Value;
            _tag = 2;
        }
    }

    public object? Value => _tag switch
    {
        1 => _intValue,
        2 => _boolValue,
        _ => null
    };

    public bool HasValue => _tag != 0;

    public bool TryGetValue(out int value)
    {
        value = _intValue;
        return _tag == 1;
    }

    public bool TryGetValue(out bool value)
    {
        value = _boolValue;
        return _tag == 2;
    }
}
// </NonBoxingAccessPattern>

// <ClassUnion>
[System.Runtime.CompilerServices.Union]
public class Result<T> : System.Runtime.CompilerServices.IUnion
{
    private readonly object? _value;

    public Result(T? value) { _value = value; }
    public Result(Exception? value) { _value = value; }

    public object? Value => _value;
}
// </ClassUnion>

// Uncomment when union member providers are available in the compiler:
// <MemberProvider>
// [System.Runtime.CompilerServices.Union]
// public record class Outcome<T> : Outcome<T>.IUnionMembers
// {
//     private readonly object? _value;
//
//     private Outcome(object? value) => _value = value;
//
//     public interface IUnionMembers
//     {
//         static Outcome<T> Create(T? value) => new(value);
//         static Outcome<T> Create(Exception? value) => new(value);
//         object? Value { get; }
//     }
//
//     object? IUnionMembers.Value => _value;
// }
// </MemberProvider>

class Program
{
    static void Main()
    {
        BasicConversion();
        PatternMatching();
        GenericUnionExample();
        ValueTypeCasesExample();
        BodyMembersExample();
        NullHandling();
        ManualUnionExample();
        NonBoxingExample();
        ClassUnionExample();
        NullableUnionExample();
    }

    // <BasicConversion>
    static void BasicConversion()
    {
        Pet pet = new Dog("Rex");
        Console.WriteLine(pet.Value); // output: Dog { Name = Rex }

        Pet pet2 = new Cat("Whiskers");
        Console.WriteLine(pet2.Value); // output: Cat { Name = Whiskers }
    }
    // </BasicConversion>

    // <PatternMatching>
    static void PatternMatching()
    {
        Pet pet = new Dog("Rex");

        var name = pet switch
        {
            Dog d => d.Name,
            Cat c => c.Name,
            Bird b => b.Name,
        };
        Console.WriteLine(name); // output: Rex
    }
    // </PatternMatching>

    // <GenericUnionExample>
    static void GenericUnionExample()
    {
        Option<int> some = new Some<int>(42);
        Option<int> none = new None();

        var result = some switch
        {
            Some<int> s => $"Has value: {s.Value}",
            None => "No value",
        };
        Console.WriteLine(result); // output: Has value: 42

        var result2 = none switch
        {
            Some<int> s => $"Has value: {s.Value}",
            None => "No value",
        };
        Console.WriteLine(result2); // output: No value
    }
    // </GenericUnionExample>

    // <ValueTypeCasesExample>
    static void ValueTypeCasesExample()
    {
        IntOrString val1 = 42;
        IntOrString val2 = "hello";

        Console.WriteLine(Describe(val1)); // output: int: 42
        Console.WriteLine(Describe(val2)); // output: string: hello

        static string Describe(IntOrString value) => value switch
        {
            int i => $"int: {i}",
            string s => $"string: {s}",
            null => "null",
        };
    }
    // </ValueTypeCasesExample>

    // <BodyMembersExample>
    static void BodyMembersExample()
    {
        OneOrMore<string> single = "hello";
        OneOrMore<string> multiple = new[] { "a", "b", "c" }.AsEnumerable();

        Console.WriteLine(string.Join(", ", single.AsEnumerable())); // output: hello
        Console.WriteLine(string.Join(", ", multiple.AsEnumerable())); // output: a, b, c
    }
    // </BodyMembersExample>

    // <NullHandling>
    static void NullHandling()
    {
        Pet pet = default;
        Console.WriteLine(pet.Value is null); // output: True

        var description = pet switch
        {
            Dog d => d.Name,
            Cat c => c.Name,
            Bird b => b.Name,
            null => "no pet",
        };
        Console.WriteLine(description); // output: no pet
    }
    // </NullHandling>

    // <ManualUnionExample>
    static void ManualUnionExample()
    {
        Shape shape = new Shape(new Circle(5.0));

        var area = shape switch
        {
            Circle c => Math.PI * c.Radius * c.Radius,
            Rectangle r => r.Width * r.Height,
        };
        Console.WriteLine($"{area:F2}"); // output: 78.54
    }
    // </ManualUnionExample>

    // <NonBoxingExample>
    static void NonBoxingExample()
    {
        IntOrBool val = new IntOrBool((int?)42);

        var description = val switch
        {
            int i => $"int: {i}",
            bool b => $"bool: {b}",
        };
        Console.WriteLine(description); // output: int: 42
    }
    // </NonBoxingExample>

    // <ClassUnionExample>
    static void ClassUnionExample()
    {
        Result<string> ok = new Result<string>("success");
        Result<string> err = new Result<string>(new InvalidOperationException("failed"));

        Console.WriteLine(Describe(ok));  // output: OK: success
        Console.WriteLine(Describe(err)); // output: Error: failed

        static string Describe(Result<string> result) => result switch
        {
            string s => $"OK: {s}",
            Exception e => $"Error: {e.Message}",
            null => "null",
        };
    }
    // </ClassUnionExample>

    // <NullableUnionExample>
    static void NullableUnionExample()
    {
        Pet? maybePet = new Dog("Buddy");
        Pet? noPet = null;

        Console.WriteLine(Describe(maybePet)); // output: Dog: Buddy
        Console.WriteLine(Describe(noPet));    // output: no pet

        static string Describe(Pet? pet) => pet switch
        {
            Dog d => d.Name,
            Cat c => c.Name,
            Bird b => b.Name,
            null => "no pet",
        };
    }
    // </NullableUnionExample>

    // Uncomment when union member providers are available in the compiler:
    // <MemberProviderExample>
    // static void MemberProviderExample()
    // {
    //     Outcome<string> ok = "success";
    //     var msg = ok switch
    //     {
    //         string s => $"OK: {s}",
    //         Exception e => $"Error: {e.Message}",
    //     };
    //     Console.WriteLine(msg);
    // }
    // </MemberProviderExample>
}
