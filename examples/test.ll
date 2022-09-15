define i32 @main () {
entry:
%"x" = alloca i32, align 4
store i32 1, ptr %"x"
store i32 2, ptr %"x"
%"1" = load i32, ptr %"x"
%"2" = icmp eq i32 %"1", 2
br i1 %"2", label %"if", label %"endif"
if:
ret i32 5
endif:
%"4" = load i32, ptr %"x"
ret i32 %"4"
}