// using System.Diagnostics;
// using System.Text;
// using Ara.Parsing.Nodes;
// using Ara.Parsing.Utils;
// using Ara.Testing.Common;
//
// namespace Ara.Parsing.Tests;
//
// [TestFixture]
// public class ParserTests
// {
//     [Test]
//     public void TestParseFunctionDeclaration()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): int
//             {
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         Assert.That(unit, Is.InstanceOf<CompilationUnit>());
//
//         var declarations = unit.Definitions;
//         Assert.That(declarations, Has.Count.EqualTo(1));
//
//         var declaration = declarations[0];
//         Assert.That(declaration, Is.InstanceOf<FunctionDefinition>());
//
//         var functionDeclaration = (FunctionDefinition)declaration;
//         Assert.Multiple(() =>
//         {
//             Assert.That(functionDeclaration.Name, Is.EqualTo("test"));
//             Assert.That(functionDeclaration.Parameters, Is.Empty);
//             Assert.That(functionDeclaration.TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)functionDeclaration.TypeRef).Type, Is.EqualTo("int"));
//             Assert.That(functionDeclaration.Body, Is.InstanceOf<Block>());
//             Assert.That(((Block)functionDeclaration.Body).Statements, Is.Empty);
//         });
//     }
//
//     [Test]
//     public void TestParseFunctionDeclarationWithParameter()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn foo(x: int): int
//             {
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         Assert.That(unit, Is.InstanceOf<CompilationUnit>());
//
//         var declarations = unit.Definitions;
//         Assert.That(declarations, Has.Count.EqualTo(1));
//
//         var declaration = declarations[0];
//         Assert.That(declaration, Is.InstanceOf<FunctionDefinition>());
//
//         var functionDeclaration = (FunctionDefinition)declaration;
//         Assert.Multiple(() =>
//         {
//             Assert.That(functionDeclaration.Name, Is.EqualTo("foo"));
//             Assert.That(functionDeclaration.Parameters, Has.Count.EqualTo(1));
//             Assert.That(functionDeclaration.Parameters[0].Name, Is.EqualTo("x"));
//             Assert.That(functionDeclaration.Parameters[0].TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)functionDeclaration.Parameters[0].TypeRef).Type, Is.EqualTo("int"));
//             Assert.That(functionDeclaration.TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)functionDeclaration.TypeRef).Type, Is.EqualTo("int"));
//             Assert.That(functionDeclaration.Body, Is.InstanceOf<Block>());
//             Assert.That(((Block)functionDeclaration.Body).Statements, Is.Empty);
//         });
//     }
//
//     [Test]
//     public void TestStructDeclaration()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             struct Point {
//                 x: int
//                 y: int
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         Assert.That(unit, Is.InstanceOf<CompilationUnit>());
//
//         var declarations = unit.Definitions;
//         Assert.That(declarations, Has.Count.EqualTo(1));
//
//         var declaration = declarations[0];
//         Assert.That(declaration, Is.InstanceOf<StructDefinition>());
//
//         var structDeclaration = (StructDefinition)declaration;
//         Assert.Multiple(() =>
//         {
//             Assert.That(structDeclaration.Name, Is.EqualTo("Point"));
//             Assert.That(structDeclaration.Fields, Has.Count.EqualTo(2));
//             Assert.That(structDeclaration.Fields[0].Name, Is.EqualTo("x"));
//             Assert.That(structDeclaration.Fields[0].TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)structDeclaration.Fields[0].TypeRef).Type, Is.EqualTo("int"));
//             Assert.That(structDeclaration.Fields[1].Name, Is.EqualTo("y"));
//             Assert.That(structDeclaration.Fields[1].TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)structDeclaration.Fields[1].TypeRef).Type, Is.EqualTo("int"));
//         });
//     }
//     
//     [Test]
//     public void TestStructDeclarationNoFields()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             struct Point {}
//         ");
//
//         var unit = parser.ParseUnit();
//         Assert.That(unit, Is.InstanceOf<CompilationUnit>());
//
//         var declarations = unit.Definitions;
//         Assert.That(declarations, Has.Count.EqualTo(1));
//
//         var declaration = declarations[0];
//         Assert.That(declaration, Is.InstanceOf<StructDefinition>());
//
//         var structDeclaration = (StructDefinition)declaration;
//         Assert.Multiple(() =>
//         {
//             Assert.That(structDeclaration.Name, Is.EqualTo("Point"));
//             Assert.That(structDeclaration.Fields, Has.Count.EqualTo(0));
//         });
//     }
//     
//     [Test]
//     public void TestParseVariableDeclaration()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 var x: int = 123
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<VariableDeclaration>());
//         
//         var variableDeclaration = (VariableDeclaration)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(variableDeclaration.Name, Is.EqualTo("x"));
//             Assert.That(variableDeclaration.Expression, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(variableDeclaration.TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)variableDeclaration.TypeRef).Type, Is.EqualTo("int"));
//             Assert.That(((IntegerLiteral)variableDeclaration.Expression).Value, Is.EqualTo(123));
//         });
//     }
//     
//     [Test]
//     public void TestParseFor()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 for(var x: int = 0; x < 10; x = x + 1)
//                 {
//                     break
//                     continue
//                 }
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<For>());
//         
//         var forStatement = (For)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(forStatement.Initializer, Is.InstanceOf<VariableDeclaration>());
//             Assert.That(forStatement.Condition, Is.InstanceOf<BinaryExpression>());
//             Assert.That(forStatement.Increment, Is.InstanceOf<Assignment>());
//             Assert.That(forStatement.Body, Is.InstanceOf<Block>());
//         });
//
//         var block = (Block)forStatement.Body;
//         Assert.That(block.Statements[0], Is.InstanceOf<Break>());
//         Assert.That(block.Statements[1], Is.InstanceOf<Continue>());
//     }
//
//     [Test]
//     public void TestParseAssignment()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 x = 123
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<Assignment>());
//         
//         var assignment = (Assignment)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(assignment.Left, Is.InstanceOf<Identifier>());
//             Assert.That(((Identifier)assignment.Left).Value, Is.EqualTo("x"));
//             Assert.That(assignment.Right, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)assignment.Right).Value, Is.EqualTo(123));
//         });
//     }
//     
//     [Test]
//     public void TestParseNegation()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 x = -1
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<Assignment>());
//         
//         var assignment = (Assignment)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(assignment.Left, Is.InstanceOf<Identifier>());
//             Assert.That(((Identifier)assignment.Left).Value, Is.EqualTo("x"));
//             Assert.That(assignment.Right, Is.InstanceOf<UnaryExpression>());
//         });
//         
//         var unaryExpression = (UnaryExpression)assignment.Right;
//         Assert.That(unaryExpression.Operator, Is.EqualTo(TokenType.Minus));
//         
//         var numberLiteral = (IntegerLiteral)unaryExpression.Operand;
//         Assert.That(numberLiteral.Value, Is.EqualTo(1));
//     }
//     
//     [Test]
//     public void TestParseReturn()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 return 123
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<Return>());
//         
//         var assignment = (Return)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(assignment.Expression, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)assignment.Expression).Value, Is.EqualTo(123));
//         });
//     }
//
//     [Test]
//     public void TestParseIf()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 if (true)
//                 {
//                     return 123
//                 }
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<If>());
//         
//         var ifStatement = (If)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(ifStatement.Condition, Is.InstanceOf<BooleanLiteral>());
//             Assert.That(((BooleanLiteral)ifStatement.Condition).Value, Is.True);
//             Assert.That(ifStatement.Then, Is.InstanceOf<Block>());
//             Assert.That(((Block)ifStatement.Then).Statements, Has.Count.EqualTo(1));
//             Assert.That(((Block)ifStatement.Then).Statements[0], Is.InstanceOf<Return>());
//             Assert.That(((Return)((Block)ifStatement.Then).Statements[0]).Expression, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)((Return)((Block)ifStatement.Then).Statements[0]).Expression).Value, Is.EqualTo(123));
//         });
//     }
//     
//     [Test]
//     public void TestParseElif()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 if (true)
//                 {
//                     return 123
//                 } 
//                 elif (false)
//                 {
//                     return 456
//                 }
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<If>());
//         
//         var ifStatement = (If)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(ifStatement.Condition, Is.InstanceOf<BooleanLiteral>());
//             Assert.That(((BooleanLiteral)ifStatement.Condition).Value, Is.True);
//             Assert.That(ifStatement.Then, Is.InstanceOf<Block>());
//             Assert.That(((Block)ifStatement.Then).Statements, Has.Count.EqualTo(1));
//             Assert.That(((Block)ifStatement.Then).Statements[0], Is.InstanceOf<Return>());
//             Assert.That(((Return)((Block)ifStatement.Then).Statements[0]).Expression, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)((Return)((Block)ifStatement.Then).Statements[0]).Expression).Value, Is.EqualTo(123));
//         });
//         
//         var elifStatement = (Elif)statements[1];
//         Assert.Multiple(() =>
//         {
//             Assert.That(elifStatement.Condition, Is.InstanceOf<BooleanLiteral>());
//             Assert.That(((BooleanLiteral)elifStatement.Condition).Value, Is.False);
//             Assert.That(elifStatement.Then, Is.InstanceOf<Block>());
//             Assert.That(((Block)elifStatement.Then).Statements, Has.Count.EqualTo(1));
//             Assert.That(((Block)elifStatement.Then).Statements[0], Is.InstanceOf<Return>());
//             Assert.That(((Return)((Block)elifStatement.Then).Statements[0]).Expression, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)((Return)((Block)elifStatement.Then).Statements[0]).Expression).Value, Is.EqualTo(456));
//         });
//     }
//     
//     [Test]
//     public void TestParseElse()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 if (true)
//                 {
//                     return 123
//                 } 
//                 elif (false)
//                 {
//                     return 456
//                 }
//                 else
//                 {
//                     return 789
//                 }
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<If>());
//         
//         var ifStatement = (If)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(ifStatement.Condition, Is.InstanceOf<BooleanLiteral>());
//             Assert.That(((BooleanLiteral)ifStatement.Condition).Value, Is.True);
//             Assert.That(ifStatement.Then, Is.InstanceOf<Block>());
//             Assert.That(((Block)ifStatement.Then).Statements, Has.Count.EqualTo(1));
//             Assert.That(((Block)ifStatement.Then).Statements[0], Is.InstanceOf<Return>());
//             Assert.That(((Return)((Block)ifStatement.Then).Statements[0]).Expression, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)((Return)((Block)ifStatement.Then).Statements[0]).Expression).Value, Is.EqualTo(123));
//         });
//         
//         var elifStatement = (Elif)statements[1];
//         Assert.Multiple(() =>
//         {
//             Assert.That(elifStatement.Condition, Is.InstanceOf<BooleanLiteral>());
//             Assert.That(((BooleanLiteral)elifStatement.Condition).Value, Is.False);
//             Assert.That(elifStatement.Then, Is.InstanceOf<Block>());
//             Assert.That(((Block)elifStatement.Then).Statements, Has.Count.EqualTo(1));
//             Assert.That(((Block)elifStatement.Then).Statements[0], Is.InstanceOf<Return>());
//             Assert.That(((Return)((Block)elifStatement.Then).Statements[0]).Expression, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)((Return)((Block)elifStatement.Then).Statements[0]).Expression).Value, Is.EqualTo(456));
//         });
//         
//         var elseStatement = (Else)statements[2];
//         Assert.Multiple(() =>
//         {
//             Assert.That(elseStatement.Then, Is.InstanceOf<Block>());
//             Assert.That(((Block)elseStatement.Then).Statements, Has.Count.EqualTo(1));
//             Assert.That(((Block)elseStatement.Then).Statements[0], Is.InstanceOf<Return>());
//             Assert.That(((Return)((Block)elseStatement.Then).Statements[0]).Expression, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)((Return)((Block)elseStatement.Then).Statements[0]).Expression).Value, Is.EqualTo(789));
//         });
//     }
//
//     [Test]
//     public void TestParseWhile()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 while (true)
//                 {
//                 }
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<While>());
//         
//         var whileStatement = (While)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(whileStatement.Condition, Is.InstanceOf<BooleanLiteral>());
//             Assert.That(((BooleanLiteral)whileStatement.Condition).Value, Is.True);
//             Assert.That(whileStatement.Then, Is.InstanceOf<Block>());
//         });
//     }
//     
//     [Test]
//     public void TestParseAddition()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 var x: int = 1 + 2
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<VariableDeclaration>());
//         
//         var variableDeclaration = (VariableDeclaration)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(variableDeclaration.Name, Is.EqualTo("x"));
//             Assert.That(variableDeclaration.Expression, Is.InstanceOf<BinaryExpression>());
//             Assert.That(variableDeclaration.TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)variableDeclaration.TypeRef).Type, Is.EqualTo("int"));
//         });
//         
//         var binaryExpression = (BinaryExpression)variableDeclaration.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(binaryExpression.Operator, Is.EqualTo(TokenType.Plus));
//             Assert.That(binaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)binaryExpression.Left).Value, Is.EqualTo(1));
//             Assert.That(binaryExpression.Right, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)binaryExpression.Right).Value, Is.EqualTo(2));
//         });
//     }
//     
//     [Test]
//     public void TestParseModulo()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 var x: int = 1 % 2
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<VariableDeclaration>());
//         
//         var variableDeclaration = (VariableDeclaration)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(variableDeclaration.Name, Is.EqualTo("x"));
//             Assert.That(variableDeclaration.Expression, Is.InstanceOf<BinaryExpression>());
//             Assert.That(variableDeclaration.TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)variableDeclaration.TypeRef).Type, Is.EqualTo("int")); 
//         });
//         
//         var binaryExpression = (BinaryExpression)variableDeclaration.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(binaryExpression.Operator, Is.EqualTo(TokenType.Modulo));
//             Assert.That(binaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)binaryExpression.Left).Value, Is.EqualTo(1));
//             Assert.That(binaryExpression.Right, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)binaryExpression.Right).Value, Is.EqualTo(2));
//         });
//     }
//
//     [Test]
//     public void TestNestedParentheses()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 var x: int = (1 + 2) * (3 - (4 / 2))
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<VariableDeclaration>());
//         
//         var variableDeclaration = (VariableDeclaration)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(variableDeclaration.Name, Is.EqualTo("x"));
//             Assert.That(variableDeclaration.Expression, Is.InstanceOf<BinaryExpression>());
//             Assert.That(variableDeclaration.TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)variableDeclaration.TypeRef).Type, Is.EqualTo("int"));
//         });
//         
//         var binaryExpression = (BinaryExpression)variableDeclaration.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(binaryExpression.Operator, Is.EqualTo(TokenType.Star));
//             Assert.That(binaryExpression.Left, Is.InstanceOf<BinaryExpression>());
//             Assert.That(binaryExpression.Right, Is.InstanceOf<BinaryExpression>());
//         });
//         
//         var leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
//         var rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
//         Assert.Multiple(() =>
//         {
//             Assert.That(leftBinaryExpression.Operator, Is.EqualTo(TokenType.Plus));
//             Assert.That(leftBinaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)leftBinaryExpression.Left).Value, Is.EqualTo(1));
//             Assert.That(leftBinaryExpression.Right, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)leftBinaryExpression.Right).Value, Is.EqualTo(2));
//             Assert.That(rightBinaryExpression.Operator, Is.EqualTo(TokenType.Minus));
//             Assert.That(rightBinaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)rightBinaryExpression.Left).Value, Is.EqualTo(3));
//             Assert.That(rightBinaryExpression.Right, Is.InstanceOf<BinaryExpression>());
//         });
//         
//         var rightRightBinaryExpression = (BinaryExpression)rightBinaryExpression.Right;
//         Assert.Multiple(() =>
//         {
//             Assert.That(rightRightBinaryExpression.Operator, Is.EqualTo(TokenType.Slash));
//             Assert.That(rightRightBinaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)rightRightBinaryExpression.Left).Value, Is.EqualTo(4));
//             Assert.That(rightRightBinaryExpression.Right, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)rightRightBinaryExpression.Right).Value, Is.EqualTo(2));
//         });
//     }
//
//     [Test]
//     public void TestParseOperatorPrecedence()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 var x: int = 1 + 2 * 3 - (4 / 2)
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<VariableDeclaration>());
//         
//         var variableDeclaration = (VariableDeclaration)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(variableDeclaration.Name, Is.EqualTo("x"));
//             Assert.That(variableDeclaration.Expression, Is.InstanceOf<BinaryExpression>());
//             Assert.That(variableDeclaration.TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)variableDeclaration.TypeRef).Type, Is.EqualTo("int"));
//         });
//         
//         var binaryExpression = (BinaryExpression)variableDeclaration.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(binaryExpression.Operator, Is.EqualTo(TokenType.Minus));
//             Assert.That(binaryExpression.Left, Is.InstanceOf<BinaryExpression>());
//             Assert.That(binaryExpression.Right, Is.InstanceOf<BinaryExpression>());
//         });
//         
//         var leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
//         var rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
//         Assert.Multiple(() =>
//         {
//             Assert.That(leftBinaryExpression.Operator, Is.EqualTo(TokenType.Plus));
//             Assert.That(leftBinaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)leftBinaryExpression.Left).Value, Is.EqualTo(1));
//             Assert.That(leftBinaryExpression.Right, Is.InstanceOf<BinaryExpression>());
//         });
//         
//         var leftRightBinaryExpression = (BinaryExpression)leftBinaryExpression.Right;
//         Assert.Multiple(() =>
//         {
//             Assert.That(leftRightBinaryExpression.Operator, Is.EqualTo(TokenType.Star));
//             Assert.That(leftRightBinaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)leftRightBinaryExpression.Left).Value, Is.EqualTo(2));
//             Assert.That(leftRightBinaryExpression.Right, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)leftRightBinaryExpression.Right).Value, Is.EqualTo(3));
//             Assert.That(rightBinaryExpression.Operator, Is.EqualTo(TokenType.Slash));
//             Assert.That(rightBinaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)rightBinaryExpression.Left).Value, Is.EqualTo(4));
//             Assert.That(rightBinaryExpression.Right, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(((IntegerLiteral)rightBinaryExpression.Right).Value, Is.EqualTo(2));
//         });
//     }
//
//     [Test]
//     public void TestStringLiteral()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 var str: string = ""hello \""world\""""
//             }
//         ");
//
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<VariableDeclaration>());
//         
//         var variableDeclaration = (VariableDeclaration)statements[0];
//         Assert.Multiple(() =>
//         {
//             Assert.That(variableDeclaration.Name, Is.EqualTo("str"));
//             Assert.That(variableDeclaration.Expression, Is.InstanceOf<Nodes.StringLiteral>());
//             Assert.That(variableDeclaration.TypeRef, Is.InstanceOf<BasicTypeRef>());
//             Assert.That(((BasicTypeRef)variableDeclaration.TypeRef).Type, Is.EqualTo("string"));
//         });
//         
//         var stringLiteral = (StringLiteral)variableDeclaration.Expression;
//         Assert.That(stringLiteral.Value, Is.EqualTo("hello \\\"world\\\""));
//     }
//
//     [Test]
//     public void TestExpressionStatement()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 1 + 2
//             }
//         ");
//         
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.Multiple(() =>
//         {
//             Assert.That(statements[0], Is.InstanceOf<ExpressionStatement>());
//             Assert.That(((ExpressionStatement)statements[0]).Expression, Is.InstanceOf<BinaryExpression>());
//         });
//         
//         var expressionStatement = (ExpressionStatement)statements[0];
//         var binaryExpression = (BinaryExpression)expressionStatement.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(binaryExpression.Operator, Is.EqualTo(TokenType.Plus));
//             Assert.That(binaryExpression.Left, Is.InstanceOf<IntegerLiteral>());
//             Assert.That(binaryExpression.Right, Is.InstanceOf<IntegerLiteral>());
//         });
//
//         var leftNumber = (IntegerLiteral)binaryExpression.Left;
//         var rightNumber = (IntegerLiteral)binaryExpression.Right;
//         Assert.Multiple(() =>
//         {
//             Assert.That(leftNumber.Value, Is.EqualTo(1));
//             Assert.That(rightNumber.Value, Is.EqualTo(2));
//         });
//     }
//
//     [Test]
//     public void TestCallExpression()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 foo()
//             }
//         ");
//         
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<ExpressionStatement>());
//         
//         var expresionStatement = (ExpressionStatement)statements[0];
//         Assert.That(expresionStatement.Expression, Is.InstanceOf<CallExpression>());
//
//         var callExpression = (CallExpression)expresionStatement.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(callExpression.Callee, Is.InstanceOf<Identifier>());
//             Assert.That(callExpression.Arguments, Is.Empty);
//         });
//     }
//     
//     [Test]
//     public void TestMemberAccessExpression()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 return foo.bar
//             }
//         ");
//         
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<Return>());
//         
//         var returnStatement = (Return)statements[0];
//         Assert.That(returnStatement.Expression, Is.InstanceOf<MemberAccessExpression>());
//         
//         var memberAccessExpression = (MemberAccessExpression)returnStatement.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(memberAccessExpression.Left, Is.InstanceOf<Identifier>());
//             Assert.That(memberAccessExpression.Right, Is.InstanceOf<Identifier>());
//         });
//     }
//     
//     [Test]
//     public void TestChainedMemberAccessExpression()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 return foo.bar.baz
//             }
//         ");
//         
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<Return>());
//         
//         var returnStatement = (Return)statements[0];
//         Assert.That(returnStatement.Expression, Is.InstanceOf<MemberAccessExpression>());
//         
//         var outer = (MemberAccessExpression)returnStatement.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(outer.Left, Is.InstanceOf<MemberAccessExpression>());
//             Assert.That(outer.Right, Is.InstanceOf<Identifier>());
//             Assert.That(((Identifier)outer.Right).Value, Is.EqualTo("baz"));
//         });
//         
//         var inner = (MemberAccessExpression)outer.Left;
//         Assert.Multiple(() =>
//         {
//             Assert.That(inner.Left, Is.InstanceOf<Identifier>());
//             Assert.That(inner.Right, Is.InstanceOf<Identifier>());
//             Assert.That(((Identifier)inner.Right).Value, Is.EqualTo("bar"));
//             Assert.That(((Identifier)inner.Left).Value, Is.EqualTo("foo"));
//         });
//     }
//
//     [Test]
//     public void TestChainedMemberAccessCallExpression()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 return foo.bar.baz()
//             }
//         ");
//         
//         var unit = parser.ParseUnit();
//         var declarations = unit.Definitions;
//         var functionDeclaration = declarations[0];
//         var functionBody = ((FunctionDefinition)functionDeclaration).Body;
//         
//         var statements = ((Block)functionBody).Statements;
//         Assert.That(statements[0], Is.InstanceOf<Return>());
//         
//         var returnStatement = (Return)statements[0];
//         Assert.That(returnStatement.Expression, Is.InstanceOf<CallExpression>());
//         
//         var callExpression = (CallExpression)returnStatement.Expression;
//         Assert.Multiple(() =>
//         {
//             Assert.That(callExpression.Callee, Is.InstanceOf<MemberAccessExpression>());
//             Assert.That(callExpression.Arguments, Is.Empty);
//         });
//         
//         var outer = (MemberAccessExpression)callExpression.Callee;
//         Assert.Multiple(() =>
//         {
//             Assert.That(outer.Left, Is.InstanceOf<MemberAccessExpression>());
//             Assert.That(outer.Right, Is.InstanceOf<Identifier>());
//             Assert.That(((Identifier)outer.Right).Value, Is.EqualTo("baz"));
//         });
//         
//         var inner = (MemberAccessExpression)outer.Left;
//         Assert.Multiple(() =>
//         {
//             Assert.That(inner.Left, Is.InstanceOf<Identifier>());
//             Assert.That(inner.Right, Is.InstanceOf<Identifier>());
//             Assert.That(((Identifier)inner.Right).Value, Is.EqualTo("bar"));
//             Assert.That(((Identifier)inner.Left).Value, Is.EqualTo("foo"));
//         });
//     }
//
//     [Test]
//     public void TestParserException()
//     {
//         var parser = TestHelpers.CreateParser(@"
//             fn test(): void
//             {
//                 fn
//             }
//         ");
//
//         var ex = Assert.Throws<ParsingException>(() => parser.ParseUnit());
//
//         if (ex is null)
//             throw new AssertionException("Expected ParserException");
//         
//         Assert.Multiple(() =>
//         {
//             Assert.That(ex.Message, Is.EqualTo("    fn\n----^\n(3,5) Unexpected Fn"));
//             Assert.That(ex.Location.Row, Is.EqualTo(3));
//             Assert.That(ex.Location.Column, Is.EqualTo(5));
//             Assert.That(ex.Location.LineContent.ToString(), Is.EqualTo("    fn"));
//             Assert.That(ex.Location.FileName, Is.Null);
//         });
//     }
//     
//     [Test]
//     public void TestBigProgram()
//     {
//         var source = @"
//             // the quick brown fox jumps over the lazy dog
//
//             struct Dog
//             {
//                 name: string
//                 age: int
//                 isGoodBoy: bool
//             }
//
//             fn test(): void
//             {
//                 // another comments
//
//                 var x: int = 0
//                 var y: float = 0.0
//                 var z: bool = true
//                 var w: string = ""hello""
//
//                 return 1 + 2 - 3 * 4 / 5 + (2 * 10) % 3 / -2.5
//             }
//
//             fn foo_bar_baz(a: int, b: float, c: bool): int
//             {
//                 if (a != false)
//                 {
//                     return 1 // blah!
//                 }
//                 else if (a < 0)
//                 {
//                     return -1
//                 }
//                 else
//                 {
//                     return 0
//                 }
//             }
//
//             fn bar(): void
//             {
//                 for (var i: int = 0; i < 10; i = i + 1)
//                 {
//                     if (x >= 1)
//                         break
//
//                     if (x == -2)
//                         continue
//
//                     print(i.x.y().z)
//                 }
//             }
//         ";
//
//         var sb = new StringBuilder();
//
//         for (var i = 0; i < 10000; i++)
//             sb.Append(source);
//
//         var sbSizeInMb = sb.Length / 1024 / 1024;
//         
//         var parser = TestHelpers.CreateParser(sb.ToString());
//
//         Assert.DoesNotThrow(() =>
//         {
//             var sw = new Stopwatch();
//             sw.Start();
//             var unit = parser.ParseUnit();
//             var elapsedMs = sw.Elapsed.TotalMilliseconds;
//             Debug.WriteLine($"Parsed {sbSizeInMb} MB in {elapsedMs} ms");
//         });
//     }
//     
// //     [Test]
// //     public void TestPrettyPrint()
// //     {
// //         var parser = TestHelpers.CreateParser(@"
// //             fn test(): void
// //             {
// //                 var x: int = 1 + 2 * 3 - (4 / 2)
// //                 x = x + 1
// //             }
// //         ");
// //
// //         var unit = parser.ParseUnit();
// //         var pretty = new AstFormatter().Format(unit.Print(), new List<string> { "Identifier", "IntegerLiteral" });
// //         Assert.That(pretty, Is.EqualTo(@"
// // (CompilationUnit
// //   (FunctionDeclaration
// //     (Identifier test)
// //     (BasicType
// //       (Identifier void))
// //     (Block
// //       (VariableDeclaration
// //         (Identifier x)
// //         (BasicType
// //           (Identifier int))
// //         (BinaryExpression
// //           Minus
// //           (BinaryExpression
// //             Plus
// //             (IntegerLiteral 1)
// //             (BinaryExpression
// //               Star
// //               (IntegerLiteral 2)
// //               (IntegerLiteral 3)))
// //           (BinaryExpression
// //             Slash
// //             (IntegerLiteral 4)
// //             (IntegerLiteral 2))))
// //       (Assignment
// //         (Identifier x)
// //         (BinaryExpression
// //           Plus
// //           (Identifier x)
// //           (IntegerLiteral 1))))))".Trim()));
// //     }
// }
