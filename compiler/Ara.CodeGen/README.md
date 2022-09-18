# Ara.CodeGen

## Examples

### Simplest Executable

```c#
module = new Module();

var type = new FunctionType(IrType.Int32);
var func = module.AddFunction("main", type);

var block = func.AddBlock();

builder = block.IrBuilder();
builder.Return(new IntValue(42));

Console.WriteLine(module.Emit());
```

#### IR

```llvm
define i32 @main () {
entry:
ret i32 42
}
```

#### Compile and Run

```shell
$ llc -filetype=obj -opaque-pointers test.ll -o test.o
$ clang test.o -o test
$ ./test
$ echo $?
42
```