using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Edge> OutEdges = new List<Edge>();
    public List<Edge> InEdges = new List<Edge>();
    //public List<Node> ConnectedNodes
    //{
    //    get
    //    {
    //        var connected = new List<Node>();

    //        foreach (var edge in OutEdges)
    //        {
    //            if (edge.NodeA != this)
    //                connected.Add(edge.NodeA);
    //            else
    //                connected.Add(edge.NodeB);
    //        }

    //        return connected;
    //    }
    //}
}
