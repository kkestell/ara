CC = clang
CFLAGS = -O3 -shared

ifeq ($(OS),Windows_NT)
	shared_library = bin/Ara.Parsing.Windows-x86_64.dll
	CFLAGS += -Wl,"/DEF:exports.def"
else
	CFLAGS += -fPIC
    UNAME_S := $(shell uname -s)
    ifeq ($(UNAME_S),Linux)
        shared_library = bin/Ara.Parsing.Linux-x86_64.so
    endif
    ifeq ($(UNAME_S),Darwin)
        shared_library = bin/Ara.Parsing.OSX-x86_64.dylib
    endif
endif

all: $(shared_library)

$(shared_library): src/parser.c src/main.c tree-sitter/lib/src/lib.c
	mkdir -p bin
	$(CC) $(CFLAGS) \
		-I tree-sitter/lib/include \
		-I tree-sitter/lib/src \
		src/main.c \
		src/parser.c \
		tree-sitter/lib/src/lib.c \
		-o $(shared_library)

src/parser.c: grammar.js
	./node_modules/.bin/tree-sitter generate --no-bindings

.PHONY: clean test
clean:
	rm -rf bin

clean-ts:
	rm -rf bin src/grammar.json src/node-types.json src/tree_sitter

test: src/parser.c
	./node_modules/.bin/tree-sitter test

