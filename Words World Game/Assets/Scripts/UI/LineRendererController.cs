using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField]
    private float _updateLineDeltaTime = 0.01f;

    private LineRenderer _lineRenderer;
    private List<Transform> _points = new();
    private Transform _lastAddedFixedPoint;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void RemoveLastFixedPoint()
    {
        _points.RemoveAt(_points.Count - 1);
        if (_points[_points.Count - 1] == _lastAddedFixedPoint)
        {
            _points.RemoveAt(_points.Count - 1);
        }

        if (_points.Count>0)
        {
            _lastAddedFixedPoint = _points[_points.Count - 1];
        }
        _lineRenderer.positionCount = _points.Count;
    }

    public void AddPointToLine(Transform pointTransform)
    {
        if (_points.Count > 0 && _points[_points.Count - 1] != _lastAddedFixedPoint)
        {
            _points.RemoveAt(_points.Count - 1);
        }

        _points.Add(pointTransform);
        _lastAddedFixedPoint = pointTransform;
        _lineRenderer.positionCount = _points.Count;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pointTransform.position);

        if (_points.Count == 1)
        {
            StartCoroutine("UpdateLineRender");
        }
    }

    private IEnumerator UpdateLineRender()
    {
        while (_points.Count > 0)
        {
            if (_points[_points.Count - 1] != _lastAddedFixedPoint)
            {
                _points.RemoveAt(_points.Count - 1);
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                _points.Add(transform);
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, touch.position);
            }
            yield return new WaitForSeconds(_updateLineDeltaTime);
        }
    }

    public void Clear()
    {
        _points.Clear();
        _lineRenderer.positionCount = 0;
        StopCoroutine("UpdateLineRender");
    }
}
