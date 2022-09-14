define i32 @fib (i32 %n) {
entry:
  %"0" = icmp eq i32 %"n", 0
  br i1 %"0", label %"if", label %"endif"
if:
  ret i32 0
endif:
  %"2" = icmp eq i32 %"n", 1
  br i1 %"2", label %"if.0", label %"endif.0"
if.0:
  ret i32 1
endif.0:
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