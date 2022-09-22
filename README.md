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
root@docker:/ara/examples# time ara fib.ara

real    0m0.050s
user    0m0.038s
sys     0m0.023s
root@docker:/ara/examples# ./fib 
root@docker:/ara/examples# echo $?
55
```

## Misc.

```
$ brew install llvm bdw-gc
$ sudo ln -s /usr/local/Cellar/llvm/15.0.0/bin/llc /usr/local/bin/llc
$ sudo ln -s /usr/local/Cellar/llvm/15.0.0/bin/clang /usr/local/bin/clang
```
