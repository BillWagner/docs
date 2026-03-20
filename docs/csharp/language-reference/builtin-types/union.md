---
title: "Union types"
description: Learn about union types in C#. Unions express values from a closed set of types with exhaustive pattern matching support.
ms.date: 03/20/2026
f1_keywords:
  - "union_CSharpKeyword"
helpviewer_keywords:
  - "union keyword [C#]"
  - "union type [C#]"
  - "case type [C#]"
ai-usage: ai-assisted
---
# Union types (C# reference)

A *union type* represents a value that can be one of several *case types*. Unions provide implicit conversions from each case type, exhaustive pattern matching, and enhanced nullability tracking. You declare a union type with the `union` keyword:

:::code language="csharp" source="snippets/unions/BasicUnion.cs" id="BasicDeclaration":::

This declaration creates a `Pet` union with three case types: `Cat`, `Dog`, and `Bird`. You can assign any case type value to a `Pet` variable, and the compiler ensures that `switch` expressions cover all case types.

[!INCLUDE[csharp-version-note](../includes/initial-version.md)]

> [!IMPORTANT]
> In .NET 10 Preview 2, the `UnionAttribute` and `IUnion` interface aren't included in the runtime. You must declare them yourself to use union types. See [Union attribute, interface, and lowering](#union-attribute-interface-and-lowering) for the required declarations.

## Union declarations

A union declaration specifies a name and a list of case types:

```csharp
public union Pet(Cat, Dog, Bird);
```

The compiler lowers a union declaration to a `struct` with the `[Union]` attribute, a public constructor for each case type, and a `Value` property. You don't write this lowered form directly—the `union` keyword generates it for you. See [Union attribute, interface, and lowering](#union-attribute-interface-and-lowering) for the generated code.

### Case types

Case types can be any type that converts to `object`, including classes, structs, interfaces, type parameters, nullable types, and other unions. The following examples show different case type possibilities:

:::code language="csharp" source="snippets/unions/BasicUnion.cs" id="CaseTypes":::
:::code language="csharp" source="snippets/unions/GenericUnion.cs" id="GenericUnion":::
:::code language="csharp" source="snippets/unions/ValueTypeCases.cs" id="ValueTypeCases":::

When a case type is a value type (like `int`), the value is boxed when stored in the union's `Value` property. Unions store their contents as a single `object?` reference.

### Body members

A union declaration can include a body with additional members, just like a struct:

:::code language="csharp" source="snippets/unions/BodyMembers.cs" id="BodyMembers":::

Instance fields, auto-properties, and field-like events aren't permitted in union declarations. You also can't declare public constructors with a single parameter, because the compiler generates those as union creation members.

## Union conversions

An implicit *union conversion* exists from each case type to the union type. You don't need to call a constructor explicitly:

:::code language="csharp" source="snippets/unions/BasicUnion.cs" id="BasicConversion":::

Union conversions work by calling the corresponding generated constructor. If a user-defined implicit conversion operator exists for the same type, the user-defined operator takes priority over the union conversion. See the [language specification](~/_csharplang/proposals/unions.md) for details on conversion priority.

A union conversion to a nullable union struct (`T?`) also works when `T` is a union type:

:::code language="csharp" source="snippets/unions/NullHandling.cs" id="NullableUnionExample":::

## Union matching

When you pattern match on a union type, patterns apply to the union's `Value` property — not the union value itself. This "unwrapping" behavior means the union is transparent to pattern matching:

:::code language="csharp" source="snippets/unions/BasicUnion.cs" id="PatternMatching":::

Two patterns are exceptions to this rule: the `var` pattern and the discard `_` pattern apply to the union value itself, not its `Value` property. Use `var` to capture the union value when `GetPet()` returns a `Pet?` (`Nullable<Pet>`):

```csharp
if (GetPet() is var pet) { /* pet is the Pet? value returned from GetPet */ }
```

In logical patterns, each branch follows the unwrapping rule individually. The following pattern tests that the `Pet?` isn't null *and* its `Value` isn't null:

```csharp
GetPet() switch
{
    var pet and not null => ..., // 'var pet' captures the Pet?; 'not null' checks Value
}
```

> [!NOTE]
> Because patterns apply to `Value`, a pattern like `pet is Pet` typically doesn't match, since `Pet` is tested against the *contents* of the union, not the union itself.

### Null matching

For struct unions, the `null` pattern checks whether `Value` is null:

:::code language="csharp" source="snippets/unions/NullHandling.cs" id="NullHandling":::

For class-based unions, `null` succeeds when either the union reference itself is null or its `Value` property is null:

```csharp
Result<string>? result = null;
if (result is null) { /* true — the reference is null */ }

Result<string> empty = new Result<string>((string?)null);
if (empty is null) { /* true — Value is null */ }
```

For nullable union struct types (`Pet?`), `null` succeeds when the nullable wrapper has no value or when the underlying union's `Value` is null.

## Union exhaustiveness

A `switch` expression is exhaustive when it handles all case types of a union. The compiler warns only if a case type isn't handled. A catch-all arm for any type isn't needed:

:::code language="csharp" source="snippets/unions/BasicUnion.cs" id="PatternMatching":::

If the null state of the union's `Value` property is "maybe null," you must also handle `null` to avoid a warning:

:::code language="csharp" source="snippets/unions/NullHandling.cs" id="NullHandling":::

## Nullability

The null state of a union's `Value` property is tracked with these rules:

- When you create a union value from a case type (through a constructor or union conversion), `Value` gets the null state of the incoming value.
- When the non-boxing access pattern's `HasValue` or `TryGetValue(...)` members query the union's contents, the null state of `Value` becomes "not null" on the `true` branch.

## Custom union types

Union declarations provide an opinionated, concise syntax. For scenarios where you need different storage strategies, interop support, or want to adapt existing types, you can create union types manually.

Any class or struct with a `[Union]` attribute is a *union type* if it follows the *basic union pattern*. The basic union pattern requires:

- A `[System.Runtime.CompilerServices.Union]` attribute on the type.
- One or more public constructors, each with a single by-value or `in` parameter. The parameter type of each constructor defines a *case type*.
- A public `Value` property of type `object?` (or `object`) with a `get` accessor.

All union members must be public. The compiler uses these members to implement union conversions, pattern matching, and exhaustiveness checks.

### Basic union pattern

The following example shows a manually declared union type:

:::code language="csharp" source="snippets/unions/ManualUnion.cs" id="ManualBasicPattern":::

:::code language="csharp" source="snippets/unions/ManualUnion.cs" id="ManualUnionExample":::

### Non-boxing access pattern

A manual union type can optionally implement the *non-boxing access pattern* to enable strongly typed access to value-type cases without boxing during pattern matching. This pattern requires:

- A `HasValue` property of type `bool` that returns `true` when `Value` isn't null.
- A `TryGetValue` method for each case type that returns `bool` and delivers the value through an `out` parameter.

:::code language="csharp" source="snippets/unions/NonBoxingAccess.cs" id="NonBoxingAccessPattern":::

:::code language="csharp" source="snippets/unions/NonBoxingAccess.cs" id="NonBoxingExample":::

The compiler prefers `TryGetValue` over the `Value` property when implementing pattern matching, which avoids boxing value types.

<!-- Union member providers are not yet available in .NET 10 Preview 2. Uncomment the following section when the feature ships. -->
<!--
### Union member providers

A union type can delegate its union members to a nested `IUnionMembers` interface. When this interface is present, the compiler looks for `Create` factory methods instead of constructors:

:::code language="csharp" source="snippets/unions/MemberProvider.cs" id="MemberProvider":::

Union member providers are useful when the union type needs a private constructor or when the creation logic requires a factory pattern, such as with `record class` union types.
-->

### Class-based union types

A class can also be a union type. This is useful when you need reference semantics or inheritance:

:::code language="csharp" source="snippets/unions/ClassUnion.cs" id="ClassUnion":::

:::code language="csharp" source="snippets/unions/ClassUnion.cs" id="ClassUnionExample":::

For class-based unions, the `null` pattern matches both a null reference and a null `Value`.

### Well-formedness

The compiler assumes that manual union types satisfy these behavioral rules:

- **Soundness**: `Value` always returns null or a value of one of the case types—never a value of a different type. For struct unions, `default` produces a `Value` of `null`.
- **Stability**: If a union value is created from a case type, `Value` matches that case type (or is null if the input was null).
- **Creation equivalence**: If a value is implicitly convertible to two different case types, both creation members produce the same observable behavior.
- **Access pattern consistency**: The `HasValue` and `TryGetValue` members, if present, behave equivalently to checking `Value` directly.

## Union attribute, interface, and lowering

The following attribute and interface support union types at compile time and runtime:

```csharp
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class UnionAttribute : Attribute;

    public interface IUnion
    {
        object? Value { get; }
    }
}
```

Union declarations generated by the compiler implement `IUnion` automatically. You can check for any union value at runtime using `IUnion`:

```csharp
if (value is IUnion { Value: null }) { /* the union's value is null */ }
```

The compiler lowers a `union` declaration to a struct that implements `IUnion`. For example, the `Pet` declaration:

```csharp
public union Pet(Cat, Dog, Bird);
```

becomes equivalent to:

```csharp
[Union] public struct Pet : IUnion
{
    public Pet(Cat value) => Value = value;
    public Pet(Dog value) => Value = value;
    public Pet(Bird value) => Value = value;
    public object? Value { get; }
}
```

> [!IMPORTANT]
> In .NET 10 Preview 2, these types aren't included in the runtime. You must declare them in your project to use union types. They'll be included in a future .NET preview.

## C# language specification

For more information, see the [Unions](~/_csharplang/proposals/unions.md) feature specification.

## See also

- [The C# type system](../../fundamentals/types/index.md)
- [Pattern matching](../operators/patterns.md)
- [Switch expression](../operators/switch-expression.md)
- [Value types](value-types.md)
- [Records](record.md)
