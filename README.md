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
false for if return true
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
