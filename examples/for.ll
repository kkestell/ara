define i32 @sum (i32 %n) {
entry:
%"sum" = alloca i32, align 4
store i32 0, ptr %"sum"
%"1" = icmp sgt i32 %"n", 0
%"delta" = alloca i32, align 4
br i1 %"1", label %"if", label %"else"
if:
store i32 1, ptr %"delta"
br label %"endif"
else:
store i32 -1, ptr %"delta"
br label %"endif"
endif:
%"7" = alloca i32, align 4
store i32 0, ptr %"7"
br label %"for"
for:
%"i" = load i32, ptr %"7"
%"10" = load i32, ptr %"sum"
%"11" = add i32 %"10", %"i"
store i32 %"11", ptr %"sum"
%"12" = load i32, ptr %"7"
%"13" = load i32, ptr %"delta"
%"14" = add i32 %"12", %"13"
store i32 %"14", ptr %"7"
%"16" = load i32, ptr %"7"
br i1 %"1", label %"if.0", label %"else.0"
if.0:
%"18" = icmp slt i32 %"16", %"n"
br i1 %"18", label %"for", label %"endfor"
br label %"endif.0"
else.0:
%"21" = icmp sgt i32 %"16", %"n"
br i1 %"21", label %"for", label %"endfor"
br label %"endif.0"
endif.0:
br label %"endfor"
endfor:
%"25" = load i32, ptr %"sum"
ret i32 %"25"
}
define i32 @main () {
entry:
%"0" = call i32 @sum(i32 100)
%"s" = alloca i32, align 4
store i32 %"0", ptr %"s"
%"2" = load i32, ptr %"s"
%"3" = icmp eq i32 %"2", 4950
br i1 %"3", label %"if", label %"endif"
if:
ret i32 1
endif:
ret i32 0
}