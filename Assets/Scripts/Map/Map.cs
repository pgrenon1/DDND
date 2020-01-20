using System.Collections.Generic;
using UnityEngine;

public class Map : BaseBehaviour
{
    public int width = 5;
    public int height = 10;
    public Node nodePrefab;
    public Edge edgePrefab;
    public int maxConnections = 2;
    public int maxRowsSkipped = 1;
    public float noisePower = 25;

    public List<List<Node>> nodeRows = new List<List<Node>>();

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        CreateNodes();

        CreateEdges();
    }

    private void CreateNodes()
    {
        var yIncrement = RectTransform.rect.height / height;
        var xIncrement = RectTransform.rect.width / width;
        var halfHeight = RectTransform.rect.height / 2f;
        var halfWidth = RectTransform.rect.width / 2f;

        for (int y = 0; y < height; y++)
        {
            nodeRows.Add(new List<Node>());
            var yPos = y * yIncrement - halfHeight + yIncrement / 2;

            for (int x = 0; x < width; x++)
            {
                var xPos = x * xIncrement - halfWidth + xIncrement / 2f;

                if (y == 0)
                {
                    if (x == width / 2)
                    {
                        CreateNode(x, y, xPos, yPos);
                    }

                    continue;
                }

                if (Random.Range(0f, 1f) >= 0.5f)
                {
                    CreateNode(x, y, xPos, yPos);
                }
            }
        }
    }

    private void CreateEdges()
    {
        for (int i = 0; i < nodeRows.Count; i++)
        {
            var nodeRow = nodeRows[i];

            if (i >= nodeRows.Count - 1)
                break;

            for (int j = 0; j < nodeRow.Count; j++)
            {
                var node = nodeRow[j];

                var numberOfConnections = Random.Range(1, 3);
                var potentialConnections = GetNodesFromAdjacentRows(i, maxRowsSkipped + 1);

                potentialConnections.Sort(delegate (Node a, Node b)
                {
                    return Vector2.Distance(node.transform.position, a.transform.position)
                    .CompareTo(Vector2.Distance(node.transform.position, b.transform.position));
                });

                var bestConnections = potentialConnections.GetRange(0, Mathf.Min(numberOfConnections, potentialConnections.Count));

                foreach (var newConnection in bestConnections)
                {
                    CreateEdge(node, newConnection);
                }

                if (i > 0 && node.InEdges.Count < 1)
                {
                    var nodesFromLastRows = GetNodesFromAdjacentRows
                }
            }
        }
    }

    private void CreateNode(int graphX, int graphY, float xPos, float yPos)
    {
        var node = Instantiate(nodePrefab, transform);
        node.transform.localPosition = new Vector2(xPos + Random.Range(-1, 1) * noisePower, yPos + Random.Range(-1, 1) * noisePower);
        node.name = "Node " + graphX + ", " + graphY;
        nodeRows[graphY].Add(node);
    }

    private void CreateEdge(Node nodeA, Node nodeB)
    {
        var newEdge = Instantiate(edgePrefab, transform);
        newEdge.name = nodeA.name + " -> " + nodeB.name;

        newEdge.NodeA = nodeA;
        nodeA.OutEdges.Add(newEdge);

        newEdge.NodeB = nodeB;
        nodeB.InEdges.Add(newEdge);

        newEdge.RefreshLineRenderer();
    }

    //private Node GetClosestFreeNode(List<Node> potentialConnections, Node currentNode)
    //{
    //    var closestNode = potentialConnections[0];
    //    var closestDistance = float.MaxValue;
    //    foreach (var potentialConnection in potentialConnections)
    //    {
    //        var distance = Vector2.Distance(potentialConnection.transform.position, currentNode.transform.position);
    //        if (distance < closestDistance && !currentNode.ConnectedNodes.Contains(potentialConnection))
    //        {
    //            closestNode = potentialConnection;
    //            closestDistance = distance;
    //        }
    //    }
    //    return closestNode;
    //}

    private List<Node> GetNodesFromAdjacentRows(int currentIndex, int rowsToGet)
    {
        var indexIncrement = rowsToGet / Mathf.Abs(rowsToGet);
        var nodesFromAdjacentRows = new List<Node>();

        var startingIndex = currentIndex + indexIncrement;
        for (int i = startingIndex;
            i != currentIndex + rowsToGet;
            i += indexIncrement)
        {
            nodesFromAdjacentRows.AddRange(nodeRows[i]);
        }

        return nodesFromAdjacentRows;
    }

    private List<Node> GetNodesFromNextRows(int currentRow)
    {
        var nodesOfNextRows = new List<Node>();

        for (int i = 1; i <= maxRowsSkipped + 1; i++)
        {
            if (currentRow + i > nodeRows.Count - 1)
                break;

            nodesOfNextRows.AddRange(nodeRows[currentRow + i]);
        }

        return nodesOfNextRows;
    }
}
