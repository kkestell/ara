CC = clang
CFLAGS = -O3 -c

ifeq ($(OS),Windows_NT)
	static_library = bin/libara.lib
else
	static_library = bin/libara.a
endif

all: $(static_library)

$(static_library): src/io.c
	mkdir -p bin
	$(CC) $(CFLAGS) \
		src/io.c \
		-o bin/io.o
	ar rcs $(static_library) bin/io.o

.PHONY: clean
clean:
	rm -rf bin
