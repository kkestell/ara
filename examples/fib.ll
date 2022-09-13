define i32 @main (i32 %n) {
%"0" = alloca i32, align 4
%"1" = add i32 5, 4
%"2" = mul i32 3, 2
%"3" = sdiv i32 %"2", 1
%"4" = sub i32 %"1", %"3"
store i32 %"4", ptr %"0"
%"x" = load i32, ptr %"0"
%"6" = alloca float, align 4
%"7" = fadd float 0x40091EB860000000, 0x400DAE1480000000
store float %"7", ptr %"6"
%"y" = load float, ptr %"6"
ret i32 0
}