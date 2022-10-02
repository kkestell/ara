using Ara.Parsing.Abstract;

namespace Ara.Parsing;

public class SyntheticParseNode : IParseNode
{
    public Location Location => throw new NotSupportedException();
}
