	.section	__TEXT,__text,regular,pure_instructions
	.build_version macos, 11, 0
	.globl	_main                           ## -- Begin function main
	.p2align	4, 0x90
_main:                                  ## @main
	.cfi_startproc
## %bb.0:
	subq	$24, %rsp
	.cfi_def_cfa_offset 32
	movl	$0, 20(%rsp)
	movl	$8, %edi
	callq	_malloc
	movq	%rax, 8(%rsp)
	movq	8(%rsp), %rax
	movq	%rax, (%rsp)                    ## 8-byte Spill
	movl	$1, (%rax)
	movl	$16, %edi
	callq	_malloc
	movq	%rax, %rcx
	movq	(%rsp), %rax                    ## 8-byte Reload
	movq	%rcx, 4(%rax)
	movl	$1, 4(%rax)
	movl	$2, 8(%rax)
	movl	24(%rax), %eax
	addq	$24, %rsp
	retq
	.cfi_endproc
                                        ## -- End function
.subsections_via_symbols
