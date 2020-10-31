# Assembler
## **Data types:**
| Name | Bytes | Bits | Unsinged range |
| --- |:---:|:---:|---:|
|Byte|1|8|255|
|Word|2|16|65535|
|Double Word|4|32|4294967295|
|Quad Word|8|64|...|
|Xmm Word|16|128|...|
|Ymm Word|32|256|...|
##  **Registers:**
## **Basic**
|Name|Size|Desc|
|---|---|---|
|rax|64|
|rbx|64|
|rcx|64|
|rdx|64|
|rsi|64|
|rdi|64|
|rbp|64|
|rsp|64|

|Oznaczenie|Bity|Należące bity|
|---|---|---|
|r** | 64 |1111 1111|
|e** | 32 |1111 0000|
|** | 16 |0011 0000|
|*l | 8 |0001 0000|

## **Additional**
|Name|Size|Desc|
|---|---|---|
|r8|64||
|...|...|...|
|r15|64|...|

|Oznaczenie|Bity|Należące bity|
|---|---|---|
|r* | 64 |1111 1111|
|r*d | 32 |1111 0000|
|r*w | 16 |0011 0000|
|r*b | 8 |0001 0000|

## **Segment pointers**
|Name|Size|Desc|
|---|---|---|
|cs|64||
|fs|64||
|gs|64||
|rip|64||