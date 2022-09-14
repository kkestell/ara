define i32 @fib (i32 %n) {
entry:
%"0" = icmp eq i32 %"n", 0
br i1 %"0", label %"if.1", label %"if.2"
if.1:
ret i32 0
if.2:
%"2" = icmp eq i32 %"n", 1
br i1 %"2", label %"if.3", label %"if.4"
if.3:
ret i32 1
if.4:
%"4" = sub i32 %"n", 2
%"5" = call i32 @fib(i32 %"4")
%"6" = sub i32 %"n", 1
%"7" = call i32 @fib(i32 %"6")
%"8" = add i32 %"5", %"7"
ret i32 %"8"
}
define i32 @main () {
entry:
%"0" = call i32 @fib(i32 10)
ret i32 %"0"
}