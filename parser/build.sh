#!/bin/bash

rm -rf {obj,bin}
mkdir -p {obj,bin}
clang -c -o obj/lib.o -Itree-sitter/include tree-sitter/src/lib.c
clang -c -o obj/parser.o -Itree-sitter/include src/parser.c
clang -c -o obj/shim.o -Itree-sitter/include src/shim.c
clang -v -shared -z '/DEF:AraParser.def' --target=x86_64-pc-windows-msvc -o bin/AraParser.dll obj/lib.o obj/parser.o obj/shim.o
clang -v -shared --target=x86_64-linux-gnu -o bin/AraParser.dll obj/lib.o obj/parser.o obj/shim.o