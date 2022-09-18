# Ara

Ara is an imperative, statically typed programming language that compiles to LLVM IR.

## TODO

- [ ] Arrays
- [ ] Strings
- [ ] Structs
- [ ] Fix up `for` code generation
- [ ] Escape analysis
- [ ] CI/CD
- [ ] Proper CLI w/ options
- [ ] Precompiled parser for macOS, Linux, and Windows
- [ ] Native AOT
- [ ] .NET 7
- [ ] Windows support?

## Overview

### Parsing

The parser is generated from a [Tree Sitter](https://tree-sitter.github.io/tree-sitter/) grammar. The compiler uses [P/Invoke](https://docs.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke) to call into the parser.

See: [grammar.js](parser/grammar.js) and [Parser.cs](compiler/Ara/Parsing/Parser.cs).

### AST Transformation

Transforms the parse tree into an abstract syntax tree.

See: [AstTransformer.cs](compiler/Ara/Ast/AstTransformer.cs).

### Semantic Analysis

Types are resolved and checked.

See: [TypeResolver.cs](compiler/Ara/Ast/Semantics/TypeResolver.cs) and [TypeChecker.cs](compiler/Ara/Ast/Semantics/TypeChecker.cs).

### Code Generation

The backend walks the AST and emits LLVM IR.

See: [CodeGenerator.cs](compiler/Ara/CodeGen/CodeGenerator.cs).

## Getting Started

### Submodules

```
$ git submodule init
$ git submodule update
```

### Parser

```
$ cd parser
$ npm install
$ cd tree-sitter
$ make
$ cd ..
$ make
$ cd ..
```

### Compiler

```
$ cd compiler
$ dotnet run
```