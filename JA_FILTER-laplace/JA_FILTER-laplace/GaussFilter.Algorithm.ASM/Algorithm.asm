.data 
; amount of bytes in one pixel
BYTE_IN_PIXEL_Q qword 4
.code
; ==============================================================
; Main procedure used for applying laplace filter on image row
; ### PARAMETERS ###
;	int startingSubpixelIndex		RCX => R10
;	int subpixelToFilter			RDX => R11
;	byte* original,					R8						[byte array of original image]
;	byte* filtered,					R9						[byte array of filtered image]
;	int* mask,		    			STACK [rbp+48] 			[laplace mask table pointer]
;	int subpixelWidth    			STACK [rbp+56] 			[subpixelWidth]
; ### USED REGISTERS ###
; rax
; rbx - temp value for subpixel 
; rcx
; rdx
; r12 - loop counter
; r13 - mask subpixel index
; r14 -
; r15 -
; ==============================================================

laplace proc
; prepare for stack deallocation
push rbp
mov rbp, rsp
push rax
push rbx
push rcx
push rdx
push r12
push r13
push r14
push r15
; main code
PREPAREVARIABLES:
mov r10, rcx
mov r11, rdx
; run main loop
xor r12, r12
MAINLOOP:
xor rbx, rbx
xor rcx, rcx
xor rdx, rdx
; Set sub pixel
		; add 1st subpixels from mask
		xor r13, r13
		xor rcx, rcx
		mov ecx, dword ptr[rbp+56]
		sub r13, rcx					
		sub r13, BYTE_IN_PIXEL_Q
		add r13, r8  					; add original image starting index
		add r13, r10 					; add starting subpixel index
		add r13, r12 					; add loop index
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx] 		; multiply by mask value
		add ebx, eax
		; add 2nd subpixels from mask
		add r13, BYTE_IN_PIXEL_Q
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx+4] 		; multiply by mask value
		add ebx, eax
	; add 3rd subpixels from mask
		add r13, BYTE_IN_PIXEL_Q
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx+8] 		; multiply by mask value
		add ebx, eax
	; add 4th subpixels from mask
		sub r13, BYTE_IN_PIXEL_Q
		sub r13, BYTE_IN_PIXEL_Q
		xor rcx, rcx
		mov ecx, dword ptr[rbp+56]
		add r13, rcx
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx+12] 	; multiply by mask value
		add ebx, eax
	; add 5th subpixels from mask
		add r13, BYTE_IN_PIXEL_Q
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx+16] 	; multiply by mask value
		add ebx, eax
	; add 6th subpixels from mask
		add r13, BYTE_IN_PIXEL_Q
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx+20] 	; multiply by mask value
		add ebx, eax
	; add 7th subpixels from mask
		sub r13, BYTE_IN_PIXEL_Q
		sub r13, BYTE_IN_PIXEL_Q
		xor rcx, rcx
		mov ecx, dword ptr[rbp+56]
		add r13, rcx
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx+24] 	; multiply by mask value
		add ebx, eax
	; add 8th subpixels from mask
		add r13, BYTE_IN_PIXEL_Q
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx+28] 	; multiply by mask value
		add ebx, eax
	; add 9th subpixels from mask
		add r13, BYTE_IN_PIXEL_Q
		xor eax, eax
		mov al, byte ptr[r13] 			; get corresponding image subpixel
		mov rcx, qword ptr[rbp+48] 		; get mask value 
		imul eax, dword ptr[rcx+32] 	; multiply by mask value
		add ebx, eax

	; check max/min for subpixels

	cmp ebx, 255	
	jg SET_MAX		
	cmp ebx, 0
	jl SET_MIN
	jmp SET

	SET_MAX:
	mov ebx, 255
	jmp SET

	SET_MIN:
	mov ebx, 0
	jmp SET

	SET:
	mov rax, r9
	add rax, r12			
	add rax, r10			 
	mov byte ptr[rax], bl
	; set subpixels


	inc r12 				; inrement loop counter
	cmp r12d, r11d			; check with subpixed width of image
	jz CLEARSTACK			
	jmp MAINLOOP			

CLEARSTACK:
; clear stack
	pop r15
	pop r14
	pop r13
	pop r12
	pop rdx
	pop rcx
	pop rbx
	pop rax
	pop rbp
	ret
laplace endp 
end

