# Ara

Ara is an imperative, statically typed programming language. The Ara compiler is written in C# and emits LLVM IR.

## Getting Started

### Clone Repository

```sh
$ git clone https://github.com/kkestell/ara
$ cd ara
$ git submodule init
$ git submodule update
```

### Build Parser

```sh
$ cd parser
$ npm install
$ cd tree-sitter
$ make
$ cd ..
$ make
$ cd ..
```

### Build Compiler

```sh
$ cd compiler
$ dotnet run
```

### Compile an Example
```sh
$ cd examples
$ ../compiler/Ara/bin/Debug/net6.0/Ara fib.ara
$ ./fib
$ echo $?
55
```