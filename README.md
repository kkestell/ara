# Ara

Ara is an imperative, statically typed programming language. Ara uses Tree Sitter for parsing. The compiler is written in C# and emits LLVM IR.

```
fn fib(n: int) -> int {
  if n == 0 {
    return 0
  }
  if n == 1 {
    return 1
  }
  return fib(n: n-2) + fib(n: n-1)
}

fn main() -> int {
  return fib(n: 10)
}
```

## Syntax

### Comments

Line comments start with `#` and end at the end of the line:

```
# This is a comment.
```

### Reserved Words

```
false for fn if return true
```

### Identifiers

Identifiers begin with a letter and may contain letters, numbers, and underscores.

### Blocks

Ara uses curly braces to define blocks. You can use a block anywhere a statement is allowed, like in control flow statements. Function bodies are also blocks.

## Values

### Booleans

A boolean value represents truth or falsehood. There are two boolean literals, `true` and `false`.

### Numbers

Ara has an integer type, `int`, and a floating point type, `float`.

## Variables

Variables are declared by specifying their name, an optional type, and an optional initialization value.

```
foo: int
```

This creates a new integer, `foo`, in the current scope.

Variables can also be initialized when they're created. In this case, their type can be inferred:

```
foo := 1
```

## Arrays

Stack allocated, static arrays can be declared by appending the array's size to a known type:

```
foos: int[10]
```

## Functions

Functions are declared using the `fn` keyword.

```
fn sum(a: int, b: int) -> int {
  return a + b
}
```

A function's return type can also be inferred.

```
fn product(a: int, b: int) {
  return a + b
}
```

### Calling Functions

When calling a function, argument names must be provided.

```
p := product(a: 1, b: 2)
```

## Control Flow

### If Statements

```
if x == 0 {
  # ...
}
```
