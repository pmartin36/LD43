using UnityEngine;
using System.Collections.Generic;

public class LineSegment : MonoBehaviour {

	private LineRenderer lineRenderer;
	public List<LinePoint> CheckedPoints;

	Vector3 lrPosition;

	public void Init(Vector3 position) {
		lineRenderer = GetComponent<LineRenderer>();
		CheckedPoints = new List<LinePoint>();
		lrPosition = position;
		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, position);
	}

	public void UpdatePosition(Vector3 position) {
		if( Vector3.Distance(position, lrPosition) > 0.1f ) {
			AddPoint(position);
		}	
	}

	private void AddPoint(Vector3 position) {
		Vector3 lastPosition = lrPosition;
		lrPosition = position;

		var normal = (lrPosition - lastPosition).Rotate(90).normalized;
		CheckedPoints.Add(new LinePoint(lastPosition, normal));

		var count = ++lineRenderer.positionCount;
		lineRenderer.SetPosition(count - 1, position);
	}

	public void EndSegment(Vector3 position) {
		AddPoint(position);
	}

	// Update is called once per frame
	void Update() {

	}
}
