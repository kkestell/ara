# Windows

## Parser

Install the following:

* [LLVM 15](https://github.com/llvm/llvm-project/releases/tag/llvmorg-15.0.0)
* [MSYS2](https://www.msys2.org/)
* [Node](https://nodejs.org/en/download/)

> NOTE: If you only want to compile the parser, you can completely ignore Node. It's only required if you want to make changes to the grammar (`parser/grammar.js`).

## Compiling

Fire up an MSYS2 shell.

Install `make` using `pacman`, the MSYS2 package manager:

```
$ pacman -S make
```

Ensure that Node and LLVM are in your `PATH`:

```
$ tail -n1 ~/.bash_profile
PATH="/c/Program Files/nodejs:/c/Program Files/LLVM/bin:${PATH}"
```

In the parser directory...

```
$ cd ara/parser
```

Install Node dependencies:

```
$ npm install
```

And then build the parser:

```
$ make
```

The output should look something like:

```
./node_modules/.bin/tree-sitter generate --no-bindings
mkdir -p bin
clang -O3 -shared \
        -I tree-sitter/lib/include \
        -I tree-sitter/lib/src \
        src/main.c \
        src/parser.c \
        tree-sitter/lib/src/lib.c \
        -o bin/Ara.Parsing.Windows-x86_64.dll

...

Creating library bin/Ara.Parsing.Windows-x86_64.lib and object bin/Ara.Parsing.Windows-x86_64.exp
```

Finally, open `compiler/Ara/Ara.sln` and build the solution.

## Changing the Grammar

Ara uses [Tree Sitter](https://tree-sitter.github.io/tree-sitter/) for parsing.

Fire up an MSYS2 shell.

Ensure that Node is in your `PATH`:

```
$ tail -n1 ~/.bash_profile
PATH="/c/Program Files/nodejs:/c/Program Files/LLVM/bin:${PATH}"
```

In the parser directory...

```
$ cd ara/parser
```

Install Node dependencies:

```
$ npm install
```

And then regenerate the parser:

```
$ ./node_modules/.bin/tree-sitter generate --no-bindings
```

You can also run `make`, which will detect changes to `grammar.js` and call `tree-sitter generate` before compiling the shared library.

## Tests

```
$ make test
```