define i32 @add (i32 %a, i32 %b) {
entry:
%"0" = add i32 %"a", %"b"
ret i32 %"0"
}
define i32 @main () {
entry:
%"0" = call i32 @add(i32 1, i32 2)
ret i32 %"0"
}