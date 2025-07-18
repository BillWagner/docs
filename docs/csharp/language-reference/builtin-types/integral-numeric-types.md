---
title: "Integral numeric types"
titleSuffix: ""
description: "Learn the range, storage size, and uses for each of the integral numeric types."
ms.date: 06/17/2022
f1_keywords:
  - "byte_CSharpKeyword"
  - "sbyte_CSharpKeyword"
  - "short_CSharpKeyword"
  - "ushort_CSharpKeyword"
  - "int_CSharpKeyword"
  - "uint_CSharpKeyword"
  - "long_CSharpKeyword"
  - "ulong_CSharpKeyword"
  - "nint_CSharpKeyword"
  - "nuint_CSharpKeyword"
helpviewer_keywords: 
  - "integral types, C#"
  - "Visual C#, integral types"
  - "types [C#], integral types"
  - "ranges of integral types [C#]"
  - "byte keyword [C#]"
  - "sbyte keyword [C#]"
  - "short keyword [C#]"
  - "ushort keyword [C#]"
  - "int keyword [C#]"
  - "uint keyword [C#]"
  - "long keyword [C#]"
  - "ulong keyword [C#]"
  - "nint data type [C#]"
  - "nuint data type [C#]"
---
# Integral numeric types  (C# reference)

The *integral numeric types* represent integer numbers. All integral numeric types are [value types](value-types.md). They're also [simple types](value-types.md#built-in-value-types) and can be initialized with [literals](#integer-literals). All integral numeric types support [arithmetic](../operators/arithmetic-operators.md), [bitwise logical](../operators/bitwise-and-shift-operators.md), [comparison](../operators/comparison-operators.md), and [equality](../operators/equality-operators.md) operators.

## Characteristics of the integral types

C# supports the following predefined integral types:

|C# type/keyword|Range|Size|.NET type|
|----------|-----------|----------|-------------|
|`sbyte`|-128 to 127|Signed 8-bit integer|<xref:System.SByte?displayProperty=nameWithType>|
|`byte`|0 to 255|Unsigned 8-bit integer|<xref:System.Byte?displayProperty=nameWithType>|
|`short`|-32,768 to 32,767|Signed 16-bit integer|<xref:System.Int16?displayProperty=nameWithType>|
|`ushort`|0 to 65,535|Unsigned 16-bit integer|<xref:System.UInt16?displayProperty=nameWithType>|
|`int`|-2,147,483,648 to 2,147,483,647|Signed 32-bit integer|<xref:System.Int32?displayProperty=nameWithType>|
|`uint`|0 to 4,294,967,295|Unsigned 32-bit integer|<xref:System.UInt32?displayProperty=nameWithType>|
|`long`|-9,223,372,036,854,775,808 to 9,223,372,036,854,775,807|Signed 64-bit integer|<xref:System.Int64?displayProperty=nameWithType>|
|`ulong`|0 to 18,446,744,073,709,551,615|Unsigned 64-bit integer|<xref:System.UInt64?displayProperty=nameWithType>|
|`nint`|Depends on platform (computed at runtime)|Signed 32-bit or 64-bit integer|<xref:System.IntPtr?displayProperty=nameWithType>|
|`nuint`|Depends on platform (computed at runtime)|Unsigned 32-bit or 64-bit integer|<xref:System.UIntPtr?displayProperty=nameWithType>|

In all of the table rows except the last two, each C# type keyword from the leftmost column is an alias for the corresponding .NET type. The keyword and .NET type name are interchangeable. For example, the following declarations declare variables of the same type:

```csharp
int a = 123;
System.Int32 b = 123;
```

The `nint` and `nuint` types in the last two rows of the table are native-sized integers. You can use the `nint` and `nuint` contextual keywords to define *native-sized integers*. These are 32-bit integers when running in a 32-bit process, or 64-bit integers when running in a 64-bit process. They can be used for interop scenarios, low-level libraries, and to optimize performance in scenarios where integer math is used extensively.

The native-sized integer types are represented internally as the .NET types <xref:System.IntPtr?displayProperty=nameWithType> and <xref:System.UIntPtr?displayProperty=nameWithType>. Starting in C# 11, the `nint` and `nuint` types are aliases for the underlying types.

The default value of each integral type is zero, `0`.

Each of the integral types has `MinValue` and `MaxValue` properties that provide the minimum and maximum value of that type. These properties are compile-time constants except for the case of the native-sized types (`nint` and `nuint`). The `MinValue` and `MaxValue` properties are calculated at runtime for native-sized types. The sizes of those types depend on the process settings.

Use the <xref:System.Numerics.BigInteger?displayProperty=nameWithType> structure to represent a signed integer with no upper or lower bounds.

## Integer literals

Integer literals can be

- *decimal*: without any prefix
- *hexadecimal*: with the `0x` or `0X` prefix
- *binary*: with the `0b` or `0B` prefix

The following code demonstrates an example of each:

```csharp
var decimalLiteral = 42;
var hexLiteral = 0x2A;
var binaryLiteral = 0b_0010_1010;
```

The preceding example also shows the use of `_` as a *digit separator*. You can use the digit separator with all kinds of numeric literals.

The type of an integer literal is determined by its suffix as follows:

- If the literal has no suffix, its type is the first of the following types in which its value can be represented: `int`, `uint`, `long`, `ulong`.

  > [!NOTE]
  > Literals are interpreted as positive values. For example, the literal `0xFF_FF_FF_FF` represents the number `4294967295` of the `uint` type, though it has the same bit representation as the number `-1` of the `int` type. If you need a value of a certain type, cast a literal to that type. Use the `unchecked` operator, if a literal value cannot be represented in the target type. For example, `unchecked((int)0xFF_FF_FF_FF)` produces `-1`.

- If the literal is suffixed by `U` or `u`, its type is the first of the following types in which its value can be represented: `uint`, `ulong`.
- If the literal is suffixed by `L` or `l`, its type is the first of the following types in which its value can be represented: `long`, `ulong`.

  > [!NOTE]
  > You can use the lowercase letter `l` as a suffix. However, this generates a compiler warning because the letter `l` can be confused with the digit `1`. Use `L` for clarity.

- If the literal is suffixed by `UL`, `Ul`, `uL`, `ul`, `LU`, `Lu`, `lU`, or `lu`, its type is `ulong`.

If the value represented by an integer literal exceeds <xref:System.UInt64.MaxValue?displayProperty=nameWithType>, a compiler error [CS1021](../../misc/cs1021.md) occurs.

If the determined type of an integer literal is `int` and the value represented by the literal is within the range of the destination type, the value can be implicitly converted to `sbyte`, `byte`, `short`, `ushort`, `uint`, `ulong`, `nint` or `nuint`:

```csharp
byte a = 17;
byte b = 300;   // CS0031: Constant value '300' cannot be converted to a 'byte'
```

As the preceding example shows, if the literal's value isn't within the range of the destination type, a compiler error [CS0031](../../misc/cs0031.md) occurs.

You can also use a cast to convert the value represented by an integer literal to the type other than the determined type of the literal:

```csharp
var signedByte = (sbyte)42;
var longVariable = (long)42;
```

## Conversions

You can convert any integral numeric type to any other integral numeric type. If the destination type can store all values of the source type, the conversion is implicit. Otherwise, you need to use a [cast expression](../operators/type-testing-and-cast.md#cast-expression) to perform an explicit conversion. For more information, see [Built-in numeric conversions](numeric-conversions.md).

## Native sized integers

Native sized integer types have special behavior because the storage is determined by the natural integer size on the target machine.

- To get the size of a native-sized integer at run time, you can use `sizeof()`. However, the code must be compiled in an unsafe context. For example:

  :::code language="csharp" source="snippets/shared/NativeIntegerTypes.cs" id="SizeOf":::

  You can also get the equivalent value from the static <xref:System.IntPtr.Size?displayProperty=nameWithType> and <xref:System.UIntPtr.Size?displayProperty=nameWithType> properties.
- To get the minimum and maximum values of native-sized integers at run time, use `MinValue` and `MaxValue` as static properties with the `nint` and `nuint` keywords, as in the following example:

  :::code language="csharp" source="snippets/shared/NativeIntegerTypes.cs" id="MinMax":::

- While the full range of `nint` and `nuint` may be larger, compile-time constants are restricted to a 32-bit range:
  - For `nint`: <xref:System.Int32.MinValue?displayProperty=nameWithType> to <xref:System.Int32.MaxValue?displayProperty=nameWithType>.
  - For `nuint`: <xref:System.UInt32.MinValue?displayProperty=nameWithType> to <xref:System.UInt32.MaxValue?displayProperty=nameWithType>.
- The compiler provides implicit and explicit conversions to other numeric types. For more information, see [Built-in numeric conversions](numeric-conversions.md).
- There's no direct syntax for native-sized integer literals. There's no suffix to indicate that a literal is a native-sized integer, such as `L` to indicate a `long`. You can use implicit or explicit casts of other integer values instead. For example:

  ```csharp
  nint a = 42
  nint a = (nint)42;
  ```

## C# language specification

For more information, see the following sections of the [C# language specification](~/_csharpstandard/standard/README.md):

- [Integral types](~/_csharpstandard/standard/types.md#836-integral-types)
- [Integer literals](~/_csharpstandard/standard/lexical-structure.md#6453-integer-literals)
- [Native sized integral types](~/_csharplang/proposals/csharp-9.0/native-integers.md)
- [C# 11 - Numeric `IntPtr` and `UIntPtr`](~/_csharplang/proposals/csharp-11.0/numeric-intptr.md)

## See also

- [Value types](value-types.md)
- [Floating-point types](floating-point-numeric-types.md)
- [Standard numeric format strings](../../../standard/base-types/standard-numeric-format-strings.md)
- [Numerics in .NET](../../../standard/numerics.md)
