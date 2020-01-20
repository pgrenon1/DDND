using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Node NodeA { get; set; }
    public Node NodeB { get; set; }
    public bool IsHidden { get; set; }

    private LineRenderer _lineRenderer;
    public LineRenderer LineRenderer
    {
        get
        {
            if (!_lineRenderer)
                _lineRenderer = GetComponent<LineRenderer>();
            return _lineRenderer;
        }
    }

    public void RefreshLineRenderer()
    {
        LineRenderer.SetPosition(0, new Vector3(NodeA.transform.localPosition.x, NodeA.transform.localPosition.y));
        LineRenderer.SetPosition(1, new Vector3(NodeB.transform.localPosition.x, NodeB.transform.localPosition.y));

        if (IsHidden)
        {
            LineRenderer.startColor = Color.grey;
            LineRenderer.endColor = Color.grey;

        }
    }
}
