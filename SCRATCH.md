# Scratch

```
$ brew install llvm bdw-gc
$ sudo ln -s /usr/lib/llvm-15/bin/llc /usr/local/bin/llc
$ sudo ln -s /usr/lib/llvm-15/bin/clang /usr/local/bin/clang
```

Compiler Explorer flags to emit IR

```
-S -emit-llvm -g0 -O0 -Xclang -disable-llvm-passes -fno-discard-value-names -fno-merge-all-constants -fno-objc-arc
```
