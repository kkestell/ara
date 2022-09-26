#include <assert.h>
#include <string.h>
#include <stdio.h>
#include <tree_sitter/api.h>

TSLanguage *tree_sitter_ara();

extern TSParser *create_parser()
{
    return ts_parser_new();
}

extern void delete_parser(TSParser *parser)
{
    ts_parser_delete(parser);
}

extern void delete_tree(TSTree *tree)
{
    ts_tree_delete(tree);
}

extern char *sexp(TSNode node)
{
    return ts_node_string(node);
}

extern TSTree *parse(TSParser *parser, const char *source_code)
{
    ts_parser_set_language(parser, tree_sitter_ara());
    TSTree *tree = ts_parser_parse_string(
        parser,
        NULL,
        source_code,
        strlen(source_code));
    return tree;
}
