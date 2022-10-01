using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes;

public interface ITyped
{
    public Type Type { get; set; }
}