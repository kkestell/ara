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

## Misc.

```
$ brew install llvm@15
$ brew install bdw-gc
$ sudo ln -s /usr/local/Cellar/llvm/15.0.0/bin/llc /usr/local/bin/llc
$ sudo ln -s /usr/local/Cellar/llvm/15.0.0/bin/clang /usr/local/bin/clang
```
