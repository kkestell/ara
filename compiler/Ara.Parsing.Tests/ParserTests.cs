namespace Ara.Parsing.Tests;

public class ParserTests : TestBase
{
    [Test]
    public void ParsesValidInput()
    {
        using var tree = Parser.Parse(@"
            fn main() -> int {
              return 0
            }
        ");

        var sexp = tree.Root.Sexp();
        AssertSexp(sexp, @"
            (source_file 
              (function_definition_list 
                (function_definition 
                  (identifier) 
                  (parameter_list) 
                  (single_value_type (identifier))
                  (block 
                    (statement_list
                       (return_statement (integer)))))))");
    }
}
