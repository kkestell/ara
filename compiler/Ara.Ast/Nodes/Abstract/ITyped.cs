#region

using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Abstract;

public interface ITyped
{
    public Type Type { get; }
}