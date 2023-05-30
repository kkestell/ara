#region

using Ara.Parsing.Abstract;

#endregion

namespace Ara.Parsing;

public class SyntheticParseNode : IParseNode
{
    public Location Location => throw new NotSupportedException();
}
