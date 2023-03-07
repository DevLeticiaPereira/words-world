using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
	private LineRenderer _lineRenderer;
	private List<Transform> points = new();

	private void Awake()
	{
		_lineRenderer = GetComponent<LineRenderer>();
	}

	public void AddPointToLine()
	{
		Touch touch = Input.GetTouch(0);
		points.Add(transform);
		_lineRenderer.positionCount = points.Count;
		_lineRenderer.SetPosition(_lineRenderer.positionCount - 1, touch.position);
	}

	public void Clear()
	{
		points.Clear();
		_lineRenderer.positionCount = 0;
	}
}
