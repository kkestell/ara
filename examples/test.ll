%struct.list = type { i64, [0 x i32] }

define dso_local i32 @list_get(ptr %"ptr_list", i32 noundef %"i") {
  %"ptr_list_array" = getelementptr inbounds %struct.list, ptr %"ptr_list", i32 0, i32 1
  %"el" = getelementptr inbounds [0 x i32], ptr %"ptr_list_array", i32 0, i32 %"i"
  %"value" = load i32, ptr %"el", align 4
  ret i32 %"value"
}

define dso_local void @list_set(ptr %"ptr_list", i32 noundef %"i", i32 noundef %"value") {
  %"ptr_list_array" = getelementptr inbounds %struct.list, ptr %"ptr_list", i32 0, i32 1
  %"el" = getelementptr inbounds [0 x i32], ptr %"ptr_list_array", i32 0, i32 %"i"
  store i32 %"value", ptr %"el", align 4
  ret void
}

; define dso_local ptr @list_new(i64 noundef %"size") {
;   call void @GC_init()

;   %"struct_size_ptr" = getelementptr %struct.list, ptr null, i32 0, i32 1
;   %"struct_size" = ptrtoint %struct.list* %"struct_size_ptr" to i64

;   %"ptr_list" = call noalias ptr @GC_malloc(i64 noundef %"struct_size")

;   %"ptr_list_size" = getelementptr inbounds %struct.list, ptr %"ptr_list", i32 0, i32 0
;   store i64 %"size", ptr %"ptr_list_size", align 8
  
;   %"ptr_list_array" = getelementptr inbounds %struct.list, ptr %"ptr_list", i32 0, i32 1

;   %"size_in_bytes" = mul i64 %"size", 8

;   %"tmp" = call noalias ptr @GC_malloc(i64 noundef %"size_in_bytes")
;   store ptr %"tmp", ptr %"ptr_list_array", align 8

;   ; %"el0" = getelementptr inbounds [0 x i32], ptr %"ptr_list_array", i32 0, i32 0
;   ; store i32 3, ptr %"el0", align 8

;   ; %"el1" = getelementptr inbounds [0 x i32], ptr %"ptr_list_array", i32 0, i32 1
;   ; store i32 4, ptr %"el1", align 8

;   ret ptr %"ptr_list"
; }

define dso_local i32 @main() {
  ;%"ptr_list" = call noalias ptr @list_new(i32 50)

  ; %"ptr_list_size" = getelementptr inbounds %struct.list, ptr %"ptr_list", i32 0, i32 0
  ; %"size" = load i32, ptr %"ptr_list_size", align 8

  ;call void @list_set(ptr %"ptr_list", i32 0, i32 5)
  ;%"val" = call i32 @list_get(ptr %"ptr_list", i32 5)

  ret i32 0;%"val"
}

declare noalias ptr @GC_malloc(i64 noundef)
declare void @GC_init()
