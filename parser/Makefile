all: bin/parser.so

bin/parser.so: grammar.js
	mkdir -p bin
	./node_modules/.bin/tree-sitter generate --no-bindings
	clang -O3 -fPIC -shared -I tree-sitter/lib/include -I ./src src/shim.c src/parser.c tree-sitter/libtree-sitter.a -o bin/parser.so
