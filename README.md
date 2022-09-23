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

### Docker

```sh
$ docker build --tag=ara .
$ docker run -it -h=docker --rm ara /bin/bash
```

#### Run an Example

```sh
root@docker:/ara/examples# ara fib.ara
root@docker:/ara/examples# ./fib 
root@docker:/ara/examples# echo $?
55
```

## Misc.

```
$ brew install llvm bdw-gc
$ sudo ln -s /usr/lib/llvm-15/bin/llc /usr/local/bin/llc
$ sudo ln -s /usr/lib/llvm-15/bin/clang /usr/local/bin/clang
```
