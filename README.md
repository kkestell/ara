# Ara

Ara is an imperative, statically typed programming language. The Ara compiler is written in C# and emits LLVM IR.

## Getting Started

### Clone Repository

```
$ git clone https://github.com/kkestell/ara
$ cd ara
$ git submodule init
$ git submodule update
```

### Docker

```
$ docker build --tag=ara .
$ docker run -it \
  --mount type=bind,source="$(pwd)/examples",target=/ara/examples \
  -h=docker --rm ara /bin/bash
```

Or

```
$ ./docker.sh
```

#### Run an Example

```
root@docker:/ara/examples# ara fib.ara
      AST      0.06 ms
Semantics      0.04 ms
 Code Gen      0.04 ms
     LLVM     30.96 ms
root@docker:/ara/examples# ./fib 
root@docker:/ara/examples# echo $?
55
```
