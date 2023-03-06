using System;
using System.Collections;
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

	public void AddPointToLine(RectTransform point)
	{
		_lineRenderer.positionCount = points.Count + 1;
		this.points.Add(point);
	}

	public void Clear()
	{
		points.Clear();
	}

	private void Update()
	{
		if (points.Count <= 0)
			return;

		for (int i = 0; i < points.Count; ++i)
		{
			if (i == points.Count - 1)
			{
				Touch touch = Input.GetTouch(0);
				_lineRenderer.SetPosition(i,touch.position);
			}

			_lineRenderer.SetPosition(i,points[i].position);
		}
	}
}
