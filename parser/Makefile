all: bin/parser.so

bin/parser.so: generate grammar.js
	mkdir -p bin
	cc -O3 -shared -I tree-sitter/lib/include -I ./src src/shim.c src/parser.c tree-sitter/libtree-sitter.a -o bin/parser.so

.PHONY: clean test
clean:
	rm -rf bin/parser.so src/grammar.json src/node-types.json src/parser.c src/tree_sitter/

test: generate
	./node_modules/.bin/tree-sitter test

generate:
	./node_modules/.bin/tree-sitter generate --no-bindings
