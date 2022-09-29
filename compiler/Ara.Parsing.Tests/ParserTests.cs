namespace Ara.Parsing.Tests;

public class ParserTests : TestBase
{
    [Test]
    public void ParsesValidInput()
    {
        using var tree = Parser.Parse(@"
            module main

            fn main() -> int {
              return 0
            }
        ");

        var sexp = tree.Root.Sexp();
        AssertSexp(sexp, @"
            (source_file 
              module_declaration: (module_declaration name: (identifier)) 
              definitions: (definition_list 
                (function_definition
                  name: (identifier) 
                  parameters: (parameter_list) 
                  type: (single_value_type (identifier)) 
                  block: (block 
                    (statement_list 
                      (return_statement (integer)))))))");
    }
}
