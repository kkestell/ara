/*
using DotNetGraph;
using DotNetGraph.Edge;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
*/

using Ara.Ast.Nodes;
using DotNetGraph;
using DotNetGraph.Edge;
using DotNetGraph.Extensions;
using DotNetGraph.Node;

namespace Ara.Ast;

public class GraphGenerator
{
    readonly DotGraph graph = new("Ara", true);

    public void Generate(AstNode node, string filename)
    {
        Visit(node);
        var dot = graph.Compile(true);
        File.WriteAllText(filename, dot);
    }

    void Visit(AstNode? node, DotNode? parent = null)
    {
        if (node is null)
            return;
        
        var properties = node.GetType().GetProperties();
        var dotNode = AddNode(node, parent);

        foreach (var p in properties)
        {
            if (p.Name.StartsWith('_'))
                continue;
            
            if (typeof(AstNode).IsAssignableFrom(p.PropertyType))
            {
                var value = (AstNode?)p.GetValue(node);

                if (value is null)
                    AddEmptyNode(p.Name, dotNode);
                else
                    Visit(value, dotNode);
            }
            else if (typeof(IEnumerable<AstNode>).IsAssignableFrom(p.PropertyType))
            {
                foreach (var c in (IEnumerable<AstNode>)p.GetValue(node)!) 
                    Visit(c, dotNode);
            }
        }
    }

    void AddEmptyNode(string name, DotNode? parent = null)
    {
        var graphNode = new DotNode(Guid.NewGuid().ToString())
        {
            Shape = DotNodeShape.Rectangle,
            Label = name,
            Style = DotNodeStyle.Dotted
        };
        
        graph.Elements.Add(graphNode);

        if (parent is null) 
            return;
        
        var myEdge = new DotEdge(parent, graphNode)
        {
            ArrowHead = DotEdgeArrowType.Normal
        };

        graph.Elements.Add(myEdge);
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
            graphNode.Label.Text = $"{graphNode.Label.Text} ({en.Type})";
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
