using Ara.Semantics;

namespace Ara.Tests;

public class IntegrationTests : TestBase
{
    [Test]
    public void Empty()
    {
        const string source =
            """
            fn main(): void
            {
            }
            """;

        Assert.DoesNotThrow(() => CompileAndExecute(source));
    }

    [Test]
    public void ReturnsZero()
    {
        const string source =
            """
            fn main(): i32
            {
                return 0
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(0));
    }
    
    [Test]
    public void ReturnVoid()
    {
        const string source =
            """
            fn foo(): void
            {
                return;
            }

            fn main(): i32
            {
                foo()
                return 0
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(0));
    }
    
    [Test]
    public void ReturnNull()
    {
        const string source =
            """
            fn foo(): ^void
            {
                return null
            }

            fn main(): i32
            {
                var bar: ^i32 = foo()
                if (bar == null)
                {
                    return 0
                }
                else
                {
                    return 1
                }
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(0));
    }

    [Test]
    public void VariableDeclaration()
    {
        const string source =
            """
            fn main(): i32
            {
                var x: i32 = 42
                return x
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(42));
    }

    [Test]
    public void ArrayArgument()
    {
        const string source =
            """
            fn foo(a: [1]i32): i32
            {
                return a[0]
            }

            fn main(): i32
            {
                var a: [1]i32 = [100]
                return foo(a)
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }

    [Test]
    public void FunctionCallWithMixedArguments()
    {
        const string source =
            """
            fn add(x: i32, y: ^i32, z: [2]i32): i32
            {
                return x + ^y + z[0] + z[1]
            }

            fn main(): i32
            {
                var a: i32 = 10
                var b: ^i32 = malloc(4) as ^i32
                b^ = 20
                var c: [2]i32 = [30, 40]
            
                return add(a, b, c)
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }
    
    [Test]
    public void ImplicitCastFromVoidPointer()
    {
        const string source =
            """
            fn main(): i32
            {
                var a: ^i32 = malloc(4) // equiv. to malloc(4) as ^i32
                a^ = 42
                return ^a
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(42));
    }

    [Test]
    public void PointerAssignment()
    {
        const string source =
            """
            fn main(): i32
            {
                var a: ^i32 = malloc(4) as ^i32
                a^ = 42
                var b: ^i32 = a
                
                return ^b
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(42));
    }

    [Test]
    public void ReturnPointer()
    {
        const string source =
            """
            fn allocate_i32(): ^i32
            {
                return malloc(4) as ^i32
            }

            fn main(): i32
            {
              var y: ^i32 = allocate_i32()
              y^ = 42
              return y^
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(42));
    }

    [Test]
    public void Malloc()
    {
        const string source =
            """
            fn main(): i32
            {
              var y: ^i32 = malloc(4) as ^i32
              y^ = 42
              return y^
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(42));
    }

    [Test]
    public void ArrayAccess()
    {
        const string source =
            """
            fn extract_second(arr: [5]i32): i32
            {
                return arr[1]
            }

            fn main(): i32
            {
                var x: [5]i32 = [1, 2, 3, 4, 5]
                return extract_second(x)
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(2));
    }

    [Test]
    public void PointerDereferencing()
    {
        const string source =
            """
            fn main(): i32
            {
                var val: ^i32 = malloc(4)
                val^ = 42
                val^ = val^ + 10
                return val^
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(52));
    }

    [Test]
    public void StructArgument()
    {
        const string source =
            """
            struct Pair
            {
                x: i32
                y: i32
            }

            fn add(pair: Pair): i32
            {
                return pair.x + pair.y
            }

            fn main(): i32
            {
                var pair: Pair
                pair.x = 10
                pair.y = 20
                
                return pair.add()
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(30));
    }

    [Test]
    public void ImplicitCasts()
    {
        const string source =
            """
            fn main(): i32
            {
                var small_int: i32 = 42
                var small_float: f32 = 3.14
            
                var large_int: i64 = small_int
                var int_to_float: f32 = small_int
                var int_to_double: f64 = small_int
                var large_int_to_double: f64 = large_int
                var large_float: f64 = small_float
                
                return 0
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(0));
    }

    [Test]
    public void ExplicitCasts()
    {
        const string source =
            """
            fn main(): i32
            {
                var i32_val: i32 = 10
                var i32_to_i64: i64 = i32_val as i64
                var i64_to_i32: i32 = i32_to_i64 as i32
            
                var i32_to_f32: f32 = i32_val as f32
                var i32_to_f64: f64 = i32_val as f64
                var i64_to_f32: f32 = i32_to_i64 as f32
                var i64_to_f64: f64 = i32_to_i64 as f64
            
                var f32_val: f32 = 10.0
                var f32_to_i32: i32 = f32_val as i32
                var f32_to_i64: i64 = f32_val as i64
                var f64_val: f64 = 10.0 as f64
                var f64_to_i32: i32 = f64_val as i32
                var f64_to_i64: i64 = f64_val as i64
            
                var f32_to_f64: f64 = f32_val as f64
                var f64_to_f32: f32 = f64_val as f32
                
                return 0
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(0));
    }

    [Test]
    public void WhileLoop()
    {
        const string source =
            """
            fn main(): i32
            {
                var x: i32 = 0
                
                while (x < 10)
                {
                    x = x + 1
                }
                
                return x
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(10));
    }

    [Test]
    public void ForLoop()
    {
        const string source =
            """
            fn main(): i32
            {
                var x: i32 = 0
                
                for (var i: i32 = 0; i < 10; i = i + 1)
                {
                    x = x + 1
                }
                
                return x
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(10));
    }

    [Test]
    public void BreakStatement()
    {
        const string source =
            """
            fn main(): i32
            {
                var x: i32 = 0
                
                while (true)
                {
                    x = x + 1
                    
                    if (x == 10)
                    {
                        break
                    }
                }
                
                return x
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(10));
    }

    [Test]
    public void ContinueStatement()
    {
        const string source =
            """
            fn main(): i32
            {
                var x: i32 = 0
                
                while (true)
                {
                    x = x + 1
                    
                    if (x == 10)
                    {
                        break
                    }
                    else
                    {
                        continue
                    }
                    
                    return 0
                }
                
                return x
            }
            """;

        var result = CompileAndExecute(source);

        Assert.That(result.ExitCode, Is.EqualTo(10));
    }

    [Test]
    public void FunctionCallNoArguments()
    {
        const string source =
            """
            fn greet(): i32
            {
                return 100;
            }

            fn main(): i32
            {
                return greet();
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }

    [Test]
    public void NestedFunctionCalls()
    {
        const string source =
            """
            fn add(x: i32, y: i32): i32
            {
                return x + y;
            }

            fn main(): i32
            {
                return add(add(10, 20), add(30, 40));
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }

    [Test]
    public void ConditionalStatement()
    {
        const string source =
            """
            fn check(x: i32): i32
            {
                if (x > 50)
                {
                    return 100;
                }
                else
                {
                    return 0;
                }
            }

            fn main(): i32
            {
                return check(60);
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }

    [Test]
    public void SimpleLoop()
    {
        const string source =
            """
            fn main(): i32
            {
                var sum: i32 = 0
                for (var i: i32 = 1; i <= 5; i = i + 1)
                {
                    sum = sum + i
                }
                return sum
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(15));
    }

    [Test]
    public void FunctionCallWithReturn()
    {
        const string source =
            """
            fn compute(): i32
            {
                return 5 * 5
            }

            fn main(): i32
            {
                var result: i32 = compute()
                return result
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(25));
    }

    [Test]
    public void FunctionRecursion()
    {
        const string source =
            """
            fn factorial(n: i32): i32
            {
                if (n <= 1)
                {
                    return 1
                }
                else
                {
                    return n * factorial(n - 1)
                }
            }

            fn main(): i32
            {
                return factorial(5)
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(120));
    }

    [Test]
    public void ArrayModification()
    {
        const string source =
            """
            fn modify(arr: [3]i32): void
            {
                arr[0] = arr[0] + 10
                arr[1] = arr[1] + 20
                arr[2] = arr[2] + 30
            }

            fn sum(arr: [3]i32): i32
            {
                return arr[0] + arr[1] + arr[2]
            }

            fn main(): i32
            {
                var arr: [3]i32 = [1, 2, 3]
                modify(arr)
                return sum(arr)
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(66));
    }

    [Test]
    public void ReturnPointerToStruct()
    {
        const string source =
            """
            struct Node
            {
                value: i32
            }

            fn make_node(value: i32): ^Node
            {
                var new_node: ^Node = malloc(4) as ^Node
                new_node.value = value
                return new_node
            }

            fn main(): i32
            {
                var node: ^Node = make_node(10)
                return node.value
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(10));
    }

    [Test]
    public void LinkedListNodeCount()
    {
        const string source =
            """
            struct Node
            {
                value: i32
                next: ^Node
            }

            fn insert_head(head: ^Node, value: i32): ^Node
            {
                var new_node: ^Node = malloc(12) as ^Node
                new_node.value = value
                new_node.next = head
                return new_node
            }

            fn count_nodes(head: ^Node): i32
            {
                var count: i32 = 0
                var current: ^Node = head
                                          
                while (current != null)
                {
                  count = count + 1
                  current = current.next
                }
                                          
                return count
            }

            fn sum_nodes(head: ^Node): i32
            {
                var sum: i32 = 0
                var current: ^Node = head
                                          
                while (current != null)
                {
                  sum = sum + current.value
                  current = current.next
                }
                                          
                return sum
            }

            fn main(): i32
            {
                var head: ^Node = null
                head = insert_head(head, 10)
                head = insert_head(head, 20)
                head = insert_head(head, 30)
                return count_nodes(head) + sum_nodes(head)
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(63));
    }

    [Test]
    public void LinkedListSumNodes()
    {
        const string source =
            """
            struct Node
            {
                value: i32
                next: ^Node
            }

            fn insert_head(head: ^Node, value: i32): ^Node
            {
                var new_node: ^Node = malloc(12) as ^Node
                new_node.value = value
                new_node.next = head
                return new_node
            }

            fn sum_nodes(head: ^Node): i32
            {
                var sum: i32 = 0
                var current: ^Node = head
            
                while (current != null)
                {
                    sum = sum + current.value
                    current = current.next
                }
            
                return sum
            }

            fn main(): i32
            {
                var head: ^Node = null
                head = insert_head(head, 1)
                head = insert_head(head, 2)
                head = insert_head(head, 3)
                return sum_nodes(head)
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(6));
    }

    [Test]
    public void LinkedListFindNode()
    {
        const string source =
            """
            struct Node
            {
                value: i32
                next: ^Node
            }

            fn insert_head(head: ^Node, value: i32): ^Node
            {
                var new_node: ^Node = malloc(12) as ^Node
                new_node.value = value
                new_node.next = head
                return new_node
            }

            fn find_node(head: ^Node, target: i32): ^Node
            {
                var current: ^Node = head
            
                while (current != null)
                {
                    if (current.value == target)
                    {
                        return current
                    }
                    current = current.next
                }
            
                return null
            }

            fn main(): i32
            {
                var head: ^Node = null
                head = insert_head(head, 10)
                head = insert_head(head, 20)
                head = insert_head(head, 30)
            
                var found: ^Node = find_node(head, 20)
                if (found != null)
                {
                    return found.value
                }
                return -1
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(20));
    }

    [Test]
    public void LinkedListDeleteHead()
    {
        const string source =
            """
            struct Node
            {
                value: i32
                next: ^Node
            }

            fn insert_head(head: ^Node, value: i32): ^Node
            {
                var new_node: ^Node = malloc(12) as ^Node
                new_node.value = value
                new_node.next = head
                return new_node
            }

            fn delete_head(head: ^Node): ^Node
            {
                if (head == null)
                {
                    return null
                }
                var new_head: ^Node = head.next
                return new_head
            }

            fn main(): i32
            {
                var head: ^Node = null
                head = insert_head(head, 10)
                head = insert_head(head, 20)
                head = delete_head(head)
                return head.value
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(10));
    }

    [Test]
    public void StackPushPop()
    {
        const string source =
            """
            struct Node
            {
                value: i32
                next: ^Node
            }

            struct Stack
            {
                top: ^Node
            }

            fn push(stack: ^Stack, value: i32): void
            {
                var new_node: ^Node = malloc(12) as ^Node
                new_node.value = value
                new_node.next = stack.top
                stack.top = new_node
            }

            fn pop(stack: ^Stack): i32
            {
                if (stack.top == null)
                {
                    return -1
                }
                var top_node: ^Node = stack.top
                var value: i32 = top_node.value
                stack.top = top_node.next
                free(top_node as ^void)
                return value
            }

            fn main(): i32
            {
                var stack: Stack
                stack.top = null
                push(&stack, 10)
                push(&stack, 20)
                var first: i32 = pop(&stack)
                var second: i32 = pop(&stack)
                return first + second
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(30));
    }

    [Test]
    public void TestAddressOfOperator()
    {
        const string source =
            """
            fn modify_value(ptr_to_value: ^i32): void
            {
                ptr_to_value^ = 100
            }

            fn main(): i32
            {
                var value: i32 = 10
                modify_value(&value)
                return value
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }

    [Test]
    public void TestVariableDeclarationAssignmentMemberAccess()
    {
        const string source =
            """
            struct Foo
            {
                x: i32
            }

            fn main(): i32
            {
                var foo: Foo
                foo.x = 100
                var bar: i32 = foo.x
                return bar
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }

    [Test]
    public void TestVariableDeclarationAssignmentIdentifier()
    {
        const string source =
            """
            fn main(): i32
            {
                var foo: i32 = 100
                var bar: i32 = foo
                return bar
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }

    [Test]
    public void TestVariableDeclarationAssignmentIndexAccess()
    {
        const string source =
            """
            fn main(): i32
            {
                var foo: [2]i32 = [100, 200]
                var bar: i32 = foo[0]
                return bar
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }

    [Test]
    public void TestVariableDeclarationAssignmentDereference()
    {
        const string source =
            """
            fn main(): i32
            {
                var foo: ^i32 = malloc(4) as ^i32
                foo^ = 100
                var bar: i32 = foo^
                return bar
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(100));
    }
    
    [Test]
    public void TestThrowsVariableDeclarationTypeMismatchNoImplicitCast()
    {
        const string source =
            """
            fn main(): void
            {
                var foo: ^i32;
                var bar: i32 = foo
            }
            """;

        Assert.Throws(typeof(SemanticException), () => CompileAndExecute(source));
    }
    
    [Test]
    public void TestThrowsAssignmentTypeMismatchNoImplicitCast()
    {
        const string source =
            """
            fn main(): void
            {
                var foo: ^i32;
                var bar: i32
                bar = foo
            }
            """;

        Assert.Throws(typeof(SemanticException), () => CompileAndExecute(source));
    }

    [Test]
    public void TestBrainfuck()
    {
        const string source =
            """
            fn main(): i32
            {
                var tape: [30000]i32 // = [0] // Initialize tape with 30000 cells set to 0
                var ptr: i32 = 0 // Pointer to the current cell on the tape
                var program: [106]i32 = [43,43,43,43,43,43,43,43,43,43,43,43,43,91,62,43,43,43,43,43,43,62,43,43,43,43,43,43,43,43,43,62,43,43,43,62,43,60,60,60,60,45,93,62,43,43,46,62,43,46,43,43,43,43,43,46,46,43,43,43,46,62,43,43,46,60,60,43,43,43,43,43,43,43,43,43,43,43,43,43,46,62,46,43,43,43,46,45,45,45,45,45,46,45,45,45,45,45,45,45,46,62,43,46,62,46]
                var c: i32 = 0 // Current instruction pointer in the program
                var loop: i32 = 0
            
                while (c < 106) // Iterate over the program until the end
                {
                    if (program[c] == 62) // '>'
                    {
                        ptr = ptr + 1
                    }
                    else if (program[c] == 60) // '<'
                    {
                        ptr = ptr - 1
                    }
                    else if (program[c] == 43) // '+'
                    {
                        tape[ptr] = tape[ptr] + 1
                    }
                    else if (program[c] == 45) // '-'
                    {
                        tape[ptr] = tape[ptr] - 1
                    }
                    else if (program[c] == 46) // '.'
                    {
                        putchar(tape[ptr])
                    }
                    else if (program[c] == 44) // ','
                    {
                        tape[ptr] = getchar()
                    }
                    else if (program[c] == 91) // '['
                    {
                        if (tape[ptr] == 0)
                        {
                            loop = 1
                            while (loop != 0)
                            {
                                c = c + 1
                                if (program[c] == 91) // '['
                                {
                                    loop = loop + 1
                                }
                                else if (program[c] == 93) // ']'
                                {
                                    loop = loop - 1
                                }
                            }
                        }
                    }
                    else if (program[c] == 93) // ']'
                    {
                        if (tape[ptr] != 0)
                        {
                            loop = 1
                            while (loop != 0)
                            {
                                c = c - 1
                                if (program[c] == 93) // ']'
                                {
                                    loop = loop + 1
                                }
                                else if (program[c] == 91) // '['
                                {
                                    loop = loop - 1
                                }
                            }
                        }
                    }
                    c = c + 1 // Move to the next instruction
                }
                return 0
            }
            """;
    }

    [Test]
    public void TestFoo()
    {
        const string source =
            """
            fn main(): i32
            {
                putchar(65)
                putchar(66)
                return 0
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.Output, Is.EqualTo("AB"));
    }
    
    [Test]
    public void TestScope1()
    {
        const string source =
            """
            fn main(): i32
            {
                var x: i32 = 10
                
                if (x < 10)
                {
                    var y: i32 = 20
                }
                
                return y
            }
            """;

        var ex = Assert.Throws(typeof(SemanticException), () => CompileAndExecute(source));
        Assert.That(ex.Message, Is.EqualTo("    return y\n-----------^\n(13,12) Identifier 'y' not found."));
    }
    
    [Test]
    public void TestScope2()
    {
        const string source =
            """
            fn main(): i32
            {
                var x: i32 = 10
                
                if (x == 10)
                {
                    var y: i32 = 20
                    
                    if (x == 10)
                    {
                        x = y
                    }
                }
                
                return x
            }
            """;

        var result = CompileAndExecute(source);
        
        Assert.That(result.ExitCode, Is.EqualTo(20));
    }
}