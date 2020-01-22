using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public float margin = 40f;

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
        var direction = (NodeA.transform.localPosition - NodeB.transform.localPosition).normalized;

        var aPos = new Vector3(NodeA.transform.localPosition.x, NodeA.transform.localPosition.y);
        var aPosWithMargin = aPos - direction * margin;

        var bPos = new Vector3(NodeB.transform.localPosition.x, NodeB.transform.localPosition.y);
        var bPosWithMargin = bPos + direction * margin;

        LineRenderer.SetPosition(0, aPosWithMargin);
        LineRenderer.SetPosition(1, bPosWithMargin);

        if (IsHidden)
        {
            LineRenderer.startColor = Color.grey;
            LineRenderer.endColor = Color.grey;
        }
    }

    private void Update()
    {
        if (IsHidden)
        {
            LineRenderer.enabled = Input.GetKey(KeyCode.S);
        }
    }
}
