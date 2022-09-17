using Ara.Ast.Nodes;
/*
using DotNetGraph;
using DotNetGraph.Edge;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
*/

namespace Ara.Ast;

/*
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
        var dotNode = AddNode(node, parent);

        foreach (var p in properties)
        {
            if (p.Name.StartsWith('_'))
                continue;
            
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

    DotNode AddNode(AstNode node, DotNode? parent = null)
    {
        var name = node.GetType().ToString().Split('.').Last();
        
        var graphNode = new DotNode(Guid.NewGuid().ToString())
        {
            Shape = DotNodeShape.Rectangle,
            Label = name
        };

        if (node is BinaryExpression b)
        {
            graphNode.Label.Text = $"{graphNode.Label.Text} {b.Node.ChildByFieldName("op")!.Span.ToString()}";
        }

        if (node is Identifier i)
        {
            graphNode.Label.Text = $"{graphNode.Label.Text} ({i.Value})";
        }
        else if (node is Expression en)
        {
            graphNode.Label.Text = $"{graphNode.Label.Text} ({en.InferredType?.Value})";
        }

        graph.Elements.Add(graphNode);

        if (parent is null) 
            return graphNode;
        
        var myEdge = new DotEdge(parent, graphNode)
        {
            ArrowHead = DotEdgeArrowType.Normal
        };

        graph.Elements.Add(myEdge);

        return graphNode;
    }
}
*/
