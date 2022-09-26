shared_library = bin/Ara.Parsing.$(shell uname -sm | sed -E 's/[ \/]/-/g').so

all: $(shared_library)

$(shared_library): generate grammar.js
	mkdir -p bin
	clang -O3 -fPIC -shared \
		-I tree-sitter/lib/include \
		src/main.c \
		src/parser.c \
		tree-sitter/lib/src/lib.c \
		-o $(shared_library)

.PHONY: clean test
clean:
	rm -rf bin src/grammar.json src/node-types.json src/parser.c src/tree_sitter

test: generate
	./node_modules/.bin/tree-sitter test

generate:
	./node_modules/.bin/tree-sitter generate --no-bindings
