define i32 @main () {
entry:
%"0" = call ptr @GC_malloc(i64 128)
ret i32 1
}
declare ptr @GC_malloc(i64 noundef)