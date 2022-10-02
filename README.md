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
