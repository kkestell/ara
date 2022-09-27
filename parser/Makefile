ifeq ($(OS),Windows_NT)
	shared_library = bin/Ara.Parsing.Windows-x86_64.dll
else
    UNAME_S := $(shell uname -s)
    ifeq ($(UNAME_S),Linux)
        shared_library = bin/Ara.Parsing.Linux-x86_64.so
    endif
    ifeq ($(UNAME_S),Darwin)
        shared_library = bin/Ara.Parsing.OSX-x86_64.dylib
    endif
endif

all: $(shared_library)

$(shared_library): generate grammar.js
	mkdir -p bin
	clang -O3 -fPIC -shared \
		-I tree-sitter/lib/include \
		-I tree-sitter/lib/src \
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
