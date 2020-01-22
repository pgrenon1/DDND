using System.Collections.Generic;
using UnityEngine;

public class Map : BaseBehaviour
{
    public int width = 5;
    public int height = 10;
    public Node nodePrefab;
    public Edge edgePrefab;
    public int maxConnections = 2;
    public int maxPathRange = 2;
    public int minNodesPerRow = 0;
    public int maxNodesPerRow = 3;
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

            if (y == 0)
            {
                CreateNode(width / 2, y, 0, yPos);
                continue;
            }

            var numberOfNodeOnThisRow = Random.Range(minNodesPerRow, maxNodesPerRow);

            var positions = new List<int>();
            for (int i = 0; i < width; i++)
            {
                positions.Add(i);
            }

            for (int i = 0; i <= numberOfNodeOnThisRow; i++)
            {
                var xIndex = positions.RandomElement(true);
                var xPos = xIndex * xIncrement - halfWidth + xIncrement / 2f;

                CreateNode(xIndex, y, xPos, yPos);
            }
        }
    }

    private void CreateEdges()
    {
        for (int i = 0; i < nodeRows.Count; i++)
        {
            var row = nodeRows[i];

            var potentialConnections = GetNodesFromAdjacentRows(i, maxPathRange);

            for (int j = 0; j < row.Count; j++)
            {
                var node = row[j];

                if (i < nodeRows.Count - 1)
                {
                    potentialConnections.Sort(delegate (Node a, Node b)
                    {
                        return Vector2.Distance(node.transform.position, a.transform.position)
                        .CompareTo(Vector2.Distance(node.transform.position, b.transform.position));
                    });

                    //if (potentialConnections.Count < 1)
                    //    Debug.Log("No potential connections from " + node.name);

                    var numberOfPaths = 1;

                    if (Random.Range(0f, 1f) > 75)
                        numberOfPaths = 2;

                    var bestConnections = potentialConnections.GetRange(0, numberOfPaths);

                    foreach (var newConnection in bestConnections)
                    {
                        CreateEdge(node, newConnection);
                    }
                }

                // Hidden Edges
                if (i > 0 && node.InEdges.Count < 1)
                {
                    var nodesFromPrecedingRows = GetNodesFromAdjacentRows(i, -maxPathRange);
                    var randomNode = nodesFromPrecedingRows.RandomElement();

                    CreateEdge(randomNode, node, true);
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

    private void CreateEdge(Node nodeA, Node nodeB, bool isHidden = false)
    {
        var newEdge = Instantiate(edgePrefab, transform);
        newEdge.IsHidden = isHidden;
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
            i != currentIndex + rowsToGet
            && i <= nodeRows.Count - 1;
            i += indexIncrement)
        {
            var nodesOfThatRow = nodeRows[i];
            if (nodesOfThatRow.Count < 1)
            {
                Debug.Log("Row " + i + " was empty");
            }
            else
            {
                nodesFromAdjacentRows.AddRange(nodesOfThatRow);
            }
        }

        return nodesFromAdjacentRows;
    }
}
