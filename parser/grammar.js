module.exports = grammar({
  name: 'ara',

  word: $ => $.identifier,

  rules: {
    source_file: $ => $.function_definition_list,

    function_definition_list: $ => repeat1($.function_definition),

    function_definition: $ => seq(
      'fn',
      $.identifier, // name
      $.parameter_list,
      '->',
      $._type,
      $.block
    ),

    parameter_list: $ => seq(
      '(',
      commaSep($.parameter),
      ')'
    ),

    parameter: $ => seq(
      $.identifier,
      ':',
      $._type
    ),

    block: $ => $.statement_list,

    statement_list: $ => seq(
      '{',
      repeat($._statement),
      '}'
    ),

    _statement: $ => choice(
      $.return_statement,
      $.variable_declaration_statement,
      $.if_statement,
      $.if_else_statement,
      $.assignment_statement,
      $.array_assignment_statement,
      $.for_statement
    ),

    for_statement: $ => seq(
      'for',
      $.identifier,
      'in',
      $._expression,
      '..',
      $._expression,
      $.block
    ),

    return_statement: $ => seq(
      'return',
      $._expression,
    ),

    variable_declaration_statement: $ => seq(
      $.identifier,
      ':',
      choice(
        $._type,
        $._variable_declaration_value,
        seq(
          $._type,
          $._variable_declaration_value
        )
      )
    ),

    _variable_declaration_value: $ => seq(
      '=',
      $._expression
    ),

    if_statement: $ => seq(
      'if',
      $._expression,
      $.block
    ),

    if_else_statement: $ => seq(
      'if',
      $._expression,
      $.block,
      'else',
      $.block
    ),

    assignment_statement: $ => seq(
      $.identifier,
      '=',
      $._expression
    ),

    array_assignment_statement: $ => seq(
      $.identifier,
      '[',
      $._expression,
      ']',
      '=',
      $._expression
    ),

    _expression: $ => choice(
      $._atom,
      $.unary_expression,
      $.binary_expression,
      $.function_call_expression,
      seq('(', $._expression, ')')
    ),

    function_call_expression: $ => seq(
      $.identifier,
      $.argument_list,
    ),

    argument_list: $ => seq(
      '(',
      commaSep($.argument),
      ')'
    ),

    argument: $ => seq(
      $.identifier,
      ':',
      $._expression
    ),

    _atom: $ => choice(
      $.array_index,
      $.variable_reference,
      $.integer,
      $.float,
      $.bool,
      $.string
    ),

    array_index: $ => seq(
      $.variable_reference,
      '[',
      $._expression,
      ']'
    ),

    variable_reference: $ => $.identifier,
  
    unary_expression: $ => {
      const table = [
        [8, '-' ],
        [8, '!' ]
      ]
      return choice(...table.map(([precedence, operator]) => {
        return prec.right(precedence, seq(
          operator,
          $._expression
        ))
      }))
    },

    binary_expression: $ => {
      const table = [
        [7, '*' ],
        [7, '/' ],
        [7, '%' ],
        [6, '+' ],
        [6, '-' ],
        [5, '<' ],
        [5, '<='],
        [5, '>' ],
        [5, '>='],
        [4, '=='],
        [3, '!=']
      ]
      return choice(...table.map(([precedence, operator]) => {
        return prec.left(precedence, seq(
          $._expression,
          field('op', operator),
          $._expression
        ))
      }))
    },

    _type: $ => choice(
      $.single_value_type,
      $.array_type
    ),

    single_value_type: $ => 
      $.identifier,

    array_type: $ => seq(
      $._type,
      '[',
      $.integer,
      ']'
    ),

    string: $ => seq('"', $._string_content, '"'),

    _string_content: $ => repeat1(choice(
      token.immediate(prec(1, /[^\\"\n]+/)),
      $.escape_sequence
    )),

    escape_sequence: $ => token.immediate(seq(
      '\\',
      /(\"|\\|\/|b|f|n|r|t|u)/
    )),

    bool: $ => choice('true', 'false'),

    identifier: $ => /[a-z]+/,
    
    integer: $ => /\d+/,
    
    float: $ => /[0-9]*\.?[0-9]+/
  }
});

function commaSep (rule) {
  return optional(commaSep1(rule))
}

function commaSep1 (rule) {
  return seq(rule, repeat(seq(',', rule)))
}
