module.exports = grammar({
  name: 'ara',

  word: $ => $.identifier,

  rules: {
    source_file: $ => seq(
      field('module_declaration', $.module_declaration),
      field('definitions', $.definition_list)
    ),

    module_declaration: $ => seq(
      'module',
      field('name', $.identifier)
    ),

    definition_list: $ => repeat1($._definition),

    _definition: $ => choice(
      $.function_definition,
      // $.record_definition
    ),

    function_definition: $ => seq(
      field('type', $._type),
      field('name', $.identifier),
      field('parameters', $.parameter_list),
      field('block', $.block)
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

    block: $ => seq(
      '{',
      optional($.statement_list),
      '}'
    ),

    statement_list: $ => repeat1($._statement),

    _statement: $ => choice(
      $.return_statement,
      $.variable_declaration_statement,
      $.if_statement,
      $.assignment_statement,
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
      $._type,
      $.identifier,
      optional(
        seq(
          '=',
          $._expression
        )
      )
    ),

    if_statement: $ => seq(
      'if',
      $._expression,
      $.block
    ),

    assignment_statement: $ => seq(
      $.identifier,
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
      $.variable_reference,
      $.integer,
      $.float,
      $.bool,
      $.string
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

    // _type: $ => seq(
    //   $.identifier,
    //   optional(
    //     seq(
    //       '[',
    //       field('size', $.integer),
    //       ']'
    //     )
    //   )
    // ),

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
