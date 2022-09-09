using Ara.Ast.Nodes;
using DotNetGraph;
using DotNetGraph.Edge;
using DotNetGraph.Extensions;
using DotNetGraph.Node;

namespace Ara.Ast;

// dot -Tpdf ara.dot -o ara.pdf && open ara.pdf
public class GraphGenerator
{
    readonly DotGraph graph = new("Ara", true);

    public void Generate(AstNode node, string filename)
    {
        Visit(node);
        var dot = graph.Compile(true);
        File.WriteAllText(filename, dot);
    }

    void Visit(AstNode node, DotNode? parent = null)
    {
        var properties = node.GetType().GetProperties();
        var dotNode = AddNode(node.GetType().ToString().Split('.').Last(), parent);

        foreach (var p in properties)
        {
            if (typeof(AstNode).IsAssignableFrom(p.PropertyType))
            {
                Visit((AstNode)p.GetValue(node)!, dotNode);
            }
            else if (typeof(IEnumerable<AstNode>).IsAssignableFrom(p.PropertyType))
            {
                foreach (var c in (IEnumerable<AstNode>)p.GetValue(node)!) 
                    Visit(c, dotNode);
            }
        }
    }

    DotNode AddNode(string name, DotNode? parent = null)
    {
        var node = new DotNode(Guid.NewGuid().ToString())
        {
            Shape = DotNodeShape.Rectangle,
            Label = name
        };

        graph.Elements.Add(node);

        if (parent is null) 
            return node;
        
        var myEdge = new DotEdge(parent, node)
        {
            ArrowHead = DotEdgeArrowType.Normal
        };

        graph.Elements.Add(myEdge);

        return node;
    }
}