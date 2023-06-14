module.exports = grammar({
  name: 'ara',

  rules: {
    source_file: $ => repeat($._definition),

    _definition: $ => choice(
      $.function_definition,
      $.class_definition,
      $.variable_declaration,
      $.import_statement
    ),

    import_statement: $ => seq(
      'import',
      $.identifier,
      ';'
    ),

    variable_declaration: $ => seq(
      'var',
      $.identifier,
      '=',
      $._expression,
      ';'
    ),

    function_definition: $ => seq(
      'def',
      $.identifier,
      $.parameter_list,
      $.block
    ),

    class_definition: $ => seq(
      'class',
      $.identifier,
      $.class_body
    ),

    class_body: $ => seq(
      '{',
      repeat($.class_statement),
      '}'
    ),

    class_statement: $ => choice(
      $.function_definition,
      $.variable_declaration
    ),

    parameter_list: $ => seq(
      '(',
      optional(seq($.identifier, repeat(seq(',', $.identifier)))),
      ')'
    ),

    argument_list: $ => seq(
      '(',
      optional(seq($._expression, repeat(seq(',', $._expression)))),
      ')'
    ),

    block: $ => seq(
      '{',
      repeat($._statement),
      '}'
    ),

    _statement: $ => choice(
      $.variable_declaration,
      $.index_assignment,
      $.if_statement,
      $.for_statement,
      $.while_statement,
      $.try_statement,
      $.return_statement,
      $.expression_statement,
      $.assignment_statement
    ),

    index_assignment: $ => seq(
      $.index_access,
      '=',
      $._expression,
      ';'
    ),

    function_call: $ => seq(
      $.identifier,
      $.argument_list
    ),

    parameter_list: $ => seq(
      '(',
      optional(seq($._expression, repeat(seq(',', $._expression)))),
      ')'
    ),

    if_statement: $ => seq(
      'if',
      '(',
      $._expression,
      ')',
      $.block,
      repeat($.elif_statement),
      optional($.else_statement)
    ),

    elif_statement: $ => seq(
      'else if',
      '(',
      $._expression,
      ')',
      $.block
    ),

    else_statement: $ => seq(
      'else',
      $.block
    ),

    for_statement: $ => seq(
      'for',
      '(',
      'var',
      $.identifier,
      'in',
      $._expression,
      ')',
      $.block
    ),

    while_statement: $ => seq(
      'while',
      '(',
      $._expression,
      ')',
      $.block
    ),

    try_statement: $ => seq(
      'try',
      $.block,
      'catch',
      '(',
      $.identifier,
      $.identifier,
      ')',
      $.block
    ),

    return_statement: $ => seq(
      'return',
      $._expression,
      ';'
    ),

    expression_statement: $ => seq(
      $._expression,
      ';'
    ),

    assignment_statement: $ => seq(
      $.identifier,
      '=',
      $._expression,
      ';'
    ),

    _expression: $ => choice(
      $.identifier,
      $.number,
      $.string,
      $.boolean,
      $.null,
      $.binary_expression,
      $.comparison_expression,
      $.boolean_expression,
      $.unary_expression,
      $.function_call,
      $.attribute_access,
      $.index_access,
      $.list_expression,
      $.dictionary_expression,
      // $.lambda_expression
    ),   

    attribute_access: $ => seq(
      $._expression,
      '.',
      $.identifier
    ),

    index_access: $ => seq(
      $._expression,
      '[',
      $._expression,
      ']'
    ),

    list_expression: $ => seq(
      '[',
      optional(seq($._expression, repeat(seq(',', $._expression)))),
      ']'
    ),

    dictionary_expression: $ => seq(
      '{',
      optional(seq($.key_value_pair, repeat(seq(',', $.key_value_pair)))),
      '}'
    ),

    key_value_pair: $ => seq(
      $._expression,
      ':',
      $._expression
    ),

    // lambda_expression: $ => seq(
    //   'lambda',
    //   $.parameter_list,
    //   ':',
    //   $._expression
    // ),

    binary_expression: $ => {
      const table = [
        [7, '*', 'left'],
        [7, '/', 'left'],
        [7, '%', 'left'],
        [6, '+', 'left'],
        [6, '-', 'left'],
      ];
      return choice(...table.map(([precedence, operator, associativity]) => {
        return prec[associativity](precedence, seq(
          $._expression,
          field('op', operator),
          $._expression
        ));
      }));
    },
    
    comparison_expression: $ => {
      const table = [
        [5, '<', 'left'],
        [5, '<=', 'left'],
        [5, '>', 'left'],
        [5, '>=', 'left'],
      ];
      return choice(...table.map(([precedence, operator, associativity]) => {
        return prec[associativity](precedence, seq(
          $._expression,
          field('op', operator),
          $._expression
        ));
      }));
    },
    
    boolean_expression: $ => {
      const table = [
        [4, '==', 'left'],
        [3, '!=', 'left']
      ];
      return choice(...table.map(([precedence, operator, associativity]) => {
        return prec[associativity](precedence, seq(
          $._expression,
          field('op', operator),
          $._expression
        ));
      }));
    },

    unary_expression: $ => prec.left(choice(
      seq('-', $._expression),
      seq('!', $._expression)
    )),

    identifier: $ => /_?[a-zA-Z][_a-zA-Z0-9]*/,
    number: $ => /(\d+\.?\d*|\.\d+)/,
    string: $ => /"(\\.|[^"\\])*"/,
    boolean: $ => choice('true', 'false'),
    null: $ => 'null',
  }
});
