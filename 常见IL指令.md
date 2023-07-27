# 常见IL指令

## IL操作的数据类型

**数字类型**的有：`int32`, `int64`, `native int`, `F`,`float32`, `float64`  以及对应的拓展 `int8`,  `int16`

对于是否是有符号数字，是通过指令去区分的，在栈上不保存相关信息

**布尔类型**占用一个字节，所有位为0表示false，至少1位为1表示true

**字符类型**占用两个字节，采用`UTF-16`编码，相当于宽字符

**对象引用**由指令newobj或者newarr创建，可以作为参数，局部变量，返回值，存储在数组，或者对象的字段来使用

**运行时指针**有非托管指针和托管指针，可进行指针+整数、指针-整数，指针-指针操作

## IL代码的基本格式

**opcode + operand**

其中**opcode**为1或2字节，表示特定的功能

**operand**表示操作数，如果opcode需要一定的参数，则是由operand提供，如果不需要参数，则operand为空。因此operand的长度是由opcode决定的

**operand**允许出现的类型有：int8, uint8, int16, uint16, int32, uint32, int64, uint64, float32, float64, 都是小端序存储

## IL 基本指令

注意，以下的所有Token都是32位整数，例：typeTok就是32位int

### add

`add`

…, value1, value2 → …, result 

不检查溢出, 栈顶两元素相加

### and

`and`

…, value1, value2 → …, result 

栈顶两整数相与

### or

`or`

…, value1, value2 → …, result 

栈顶两整数相或

### box

`box <typeTok>`

…, val → …, obj 

装箱操作，通常出现在一个类型转为object类型的时候

将栈顶一个值视作typeTok表示的类型，然后包装成object放回栈顶

```c#
private static void Main(string[] args)
{
	float test_f = 1.234f;
	object holy_shit = test_f;
    float unbox_one = (float)holy_shit;
	Console.WriteLine("Hello World!" + holy_shit.ToString());
	Console.ReadLine();
}
```

```
.method private hidebysig static void
    Main(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 2
    .locals init (
      [0] float32 test_f,
      [1] object holy_shit,
      [2] float32 unbox_one
    )

    // [12 9 - 12 10]
    IL_0000: nop

    // [13 13 - 13 35]
    IL_0001: ldc.r4       1.234
    IL_0006: stloc.0      // test_f

    // [14 13 - 14 39]
    IL_0007: ldloc.0      // test_f
    IL_0008: box          [mscorlib]System.Single
    IL_000d: stloc.1      // holy_shit

    // [15 13 - 15 48]
    IL_000e: ldloc.1      // holy_shit
    IL_000f: unbox.any    [mscorlib]System.Single
    IL_0014: stloc.2      // unbox_one

    // [16 13 - 16 70]
    IL_0015: ldstr        "Hello World!"
    IL_001a: ldloc.1      // holy_shit
    IL_001b: callvirt     instance string [mscorlib]System.Object::ToString()
    IL_0020: call         string [mscorlib]System.String::Concat(string, string)
    IL_0025: call         void [mscorlib]System.Console::WriteLine(string)
    IL_002a: nop

    // [17 13 - 17 32]
    IL_002b: call         string [mscorlib]System.Console::ReadLine()
    IL_0030: pop

    // [18 9 - 18 10]
    IL_0031: ret

  }
```

### call

`call <methodTok>`

…, arg0, arg1 … argN → …, retVal (not always returned) 

调用指定methodTok对应的函数