
const WHITE_SPACE = choice(" ", "\t", "\v", "\f");
const NEWLINE = /\r?\n/;

module.exports = grammar({
  name: 'ara',

  word: $ => $.identifier,

  conflicts: $ => [
    [$.if_statement, $.if_else_statement],
    [$.array_assignment_statement, $.single_value_type]
  ],

  rules: {
    source_file: $ => seq(
      optional($.external_function_declaration_list),
      $.definition_list
    ),

    external_function_declaration_list: $ => repeat1($.external_function_declaration),

    external_function_declaration: $ => seq(
      'extern',
      $._type,
      $.identifier,
      $.parameter_list,
      ';'
    ),

    definition_list: $ => repeat1($._definition),

    _definition: $ => choice(
      $.function_definition,
      $.struct_definition
    ),

    function_definition: $ => seq(
      $._type,
      $.identifier,
      $.parameter_list,
      $.block
    ),

    parameter_list: $ => seq(
      '(',
      commaSep($.parameter),
      ')'
    ),

    parameter: $ => seq(
      $._type,
      $.identifier
    ),

    block: $ => $.statement_list,

    struct_definition: $ => seq(
      'struct',
      $.identifier,
      $.struct_field_list
    ),

    struct_field_list: $ => seq(
      '{',
      repeat1($.struct_field),
      '}'
    ),

    struct_field: $ => seq(
      $._type,
      $.identifier
    ),

    statement_list: $ => seq(
      '{',
      repeat($._statement),
      '}'
    ),

    // =========================================================================
    // Function Calls
    // =========================================================================

    _function_call: $ => seq(
      $.identifier,
      $.argument_list,
    ),

    argument_list: $ => seq(
      '(',
      commaSep($.argument),
      ')'
    ),

    argument: $ => seq(
      $._expression
    ),

    // =========================================================================
    // Statements
    // =========================================================================

    _statement: $ => choice(     
      $.block,
      $.return_statement,
      $.variable_declaration_statement,
      $.if_statement,
      $.if_else_statement,
      $.assignment_statement,
      $.array_assignment_statement,
      $.for_statement,
      $.function_call_statement
    ),

    return_statement: $ => seq(
      'return',
      $._expression,
      ';'
    ),

    variable_declaration_statement: $ => seq(
      $._type,
      $.identifier,
      optional(
        seq(
          '=',
          $._expression
        )
      ),
      ';'
    ),
  
    if_statement: $ => seq(
      'if',
      '(',
      $._expression,
      ')',
      $.block
    ),

    if_else_statement: $ => seq(
      'if',
      '(',
      $._expression,
      ')',
      $.block,
      'else',
      $.block
    ),

    assignment_statement: $ => seq(
      $.identifier,
      '=',
      $._expression,
      ';'
    ),

    array_assignment_statement: $ => seq(
      $.identifier,
      '[',
      $._expression,
      ']',
      '=',
      $._expression,
      ';'
    ),

    for_statement: $ => seq(
      'for',
      '(',
      $.identifier,
      'in',
      $._expression,
      '..',
      $._expression,
      ')',
      $.block
    ),

    function_call_statement: $ => seq(
      $._function_call,
      ';'
    ),

    // =========================================================================
    // Expressions
    // =========================================================================

    _expression: $ => choice(
      $._atom,
      $.unary_expression,
      $.binary_expression,
      $.function_call_expression,
      seq('(', $._expression, ')')
    ),

    function_call_expression: $ => $._function_call,
    
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

    identifier: $ => /[a-z]+[a-z0-9_]*/,
    
    integer: $ => /\d+/,
    
    float: $ => /[0-9]*\.?[0-9]+/,

    __comment: $ => token(seq('#', /.*/)),

    __newline: $ => NEWLINE,
    __whitespace: $ => token(WHITE_SPACE),
  },

  extras: $ => [
    $.__comment,
    $.__newline,
    $.__whitespace,
  ]
});

function commaSep (rule) {
  return optional(commaSep1(rule))
}

function commaSep1 (rule) {
  return seq(rule, repeat(seq(',', rule)))
}