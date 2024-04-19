# Ara

# Syntax

Ara's syntax is designed to be familiar to people coming from C-like languages while being a bit simpler and more streamlined.

Scripts are stored in plain text files with a .ara file extension. Ara compiles programs ahead-of-time, first to LLVM IR, and then to a native binary.

## Comments

Line comments start with `//` and end at the end of the line:

```
// This is a comment.
```

## Reserved words

One way to get a quick feel for a language’s style is to see what words it reserves. Here’s what Ara has:

```
as break continue elif else extern fn for if null return struct var while
```

## Identifiers

Naming rules are similar to other programming languages. Identifiers start with a letter or underscore and may contain letters, digits, and underscores. Case is sensitive.

```
hi
camelCase
PascalCase
_under_score
abc123
ALL_CAPS
```

## Newlines

Newlines (`\n`) are meaningful in Ara. They are used to separate statements:

```
// Two statements:
var x: i32 // Newline.
x = 0
```

### Semicolons

TODO

```
fn foo(): i32
{
    return 10
}

fn bar(): void
{
    return;
}
```


