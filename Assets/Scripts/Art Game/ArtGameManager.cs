using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LinePoint {
	public Vector2 Point { get; set; }
	public Vector2 Normal { get; set; }

	public LinePoint(Vector2 point, Vector2 normal) {
		Point = point;
		Normal = normal;
	}
}

public class ArtGameManager : ContextManager
{
	private static Task Task = Task.Art;

	private bool newLinesEnabled = false;
	public ScoreScreen ScoreScreen;

	private Stack<LineSegment> PlayerTraceLineSegments;
	private List<LinePoint> ActualTraceCheckedLinePoints;
	private List<LinePoint> PlayerTraceCheckedLinePoints;

	private LineSegment ActiveLineSegment;
	[SerializeField]
	private Tutorial Tutorial;
	[SerializeField]
	private TrailRenderer Tracer;
	[SerializeField]
	private LineSegment LineSegmentPrefab;
	[SerializeField]
	private Transform LineSegmentContainer;

	[SerializeField]
	private ArtCanvas ArtCanvas;

	private Vector2 MaxDrawCoordinates, MinDrawCoordinates;

	public override void OnEnable() {
		base.OnEnable();

		Camera c = Camera.main;
		MaxDrawCoordinates = new Vector2(
			4,
			c.orthographicSize * 0.85f + 1
		);
		MinDrawCoordinates = new Vector2(
			-c.orthographicSize * c.aspect * 0.85f,
			-2
		);

		ArtCanvas.gameObject.SetActive(true);
		if (!PlayedBefore) {
			Tutorial.Show();
		}
		Init();
	}

	public void Cleanup() {
		Tracer.Clear();
		if (ActiveLineSegment != null) {
			Destroy(ActiveLineSegment.gameObject);
			ActiveLineSegment = null;
		}
		foreach (Transform child in LineSegmentContainer) {
			DestroyImmediate(child.gameObject);
		}
		PlayerTraceLineSegments.Clear();
	}

	public void OnDisable() {
		if(ArtCanvas != null) {
			ArtCanvas.gameObject.SetActive(false);
		}
		Cleanup();
	}

	public void Submit() {
		newLinesEnabled = false;
		Vector2[] playerPoints = GetExtrudedPoints(PlayerTraceCheckedLinePoints, 0.075f);
		Vector2[] tracePoints = GetExtrudedPoints(ActualTraceCheckedLinePoints, 0.075f);

		float totalPoints = playerPoints.Length + tracePoints.Length;
		float numCorrect = 0;

		int traceMask = 1 << LayerMask.NameToLayer("ActualTrace");
		int playerMask = 1 << LayerMask.NameToLayer("PlayerTrace");

		foreach(var point in playerPoints) {
			if( Physics2D.OverlapPoint(point, traceMask) != null ) {
				numCorrect++;
			}
		}

		foreach(var point in tracePoints) {
			if (Physics2D.OverlapPoint(point, playerMask) != null) {
				numCorrect++;
			}
		}

		float normalScore = numCorrect / totalPoints;
		GameManager.Instance.TaskTracker.CompleteTask(Task, normalScore);
		ScoreScreen.Show(Task, normalScore);
	}

	public void Init() {
		newLinesEnabled = true;
		PlayerTraceLineSegments = new Stack<LineSegment>();
		PlayerTraceCheckedLinePoints = new List<LinePoint>();
		ActualTraceCheckedLinePoints = new List<LinePoint>();
		StartCoroutine(DrawTrace());
	}

	public override void Restart() {
		Cleanup();
		Init();
	}

	public void CreateCollider(GameObject parent, List<LinePoint> linepoints){
		float width = 0.1f;

		var polygonCollider = parent.AddComponent<PolygonCollider2D>();
		Vector2[] points = GetExtrudedPoints(linepoints, width);

		polygonCollider.points = points;
		polygonCollider.offset = -parent.transform.position;
	}

	public Vector2[] GetExtrudedPoints(List<LinePoint> linepoints, float extrusion) {
		int count = linepoints.Count * 2;
		Vector2[] points = new Vector2[count];

		for (int i = 0; i < linepoints.Count; i++) {
			LinePoint l = linepoints[i];
			points[i] = l.Point + extrusion * l.Normal;
			points[count - 1 - i] = l.Point - extrusion * l.Normal;
		}

		return points;
	}

	public override void HandleInput(InputPackage p) {
		if(!newLinesEnabled) {
			return;
		}

		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f,-0.1f);
		if (p.LeftMouse && ActiveLineSegment == null) {
			// started new line
			LineSegment l = Instantiate(LineSegmentPrefab, LineSegmentContainer); 
			l.Init(mousePosition);
			this.ActiveLineSegment = l;
		}
		else if(p.LeftMouse && ActiveLineSegment != null) {
			// continuing line
			this.ActiveLineSegment.UpdatePosition(mousePosition);
		}
		else if(!p.LeftMouse && ActiveLineSegment != null) {
			// ended line
			this.ActiveLineSegment.EndSegment(mousePosition);
			PlayerTraceLineSegments.Push(this.ActiveLineSegment);
			PlayerTraceCheckedLinePoints.AddRange(this.ActiveLineSegment.CheckedPoints);
			CreateCollider(this.ActiveLineSegment.gameObject, this.ActiveLineSegment.CheckedPoints);
			this.ActiveLineSegment = null;
		} 
		else if (p.Z) {
			// delete last segment
			if (PlayerTraceLineSegments.Count > 0) {
				if(GameManager.Instance.Status.ReliefActivity != ReliefActivity.Eating ) {
					var popped = PlayerTraceLineSegments.Pop();
					PlayerTraceCheckedLinePoints.RemoveRange(PlayerTraceCheckedLinePoints.Count - popped.CheckedPoints.Count, popped.CheckedPoints.Count );
					GameManager.DestroyImmediate(popped.gameObject);
				}
				else {
					// can't undo while eating

				}
			}
		}
	}

	private bool GenerateNewPoint(Vector2 lastPosition, ref Vector2 nextPosition, float offset = 0) {
		var posSeed = (Tracer.transform.position.x + Tracer.transform.position.y + offset);
		var seed = (posSeed + Time.time) / 4f;
		float noise = Perlin.Noise(seed) * 6f;

		Vector3 lastDiff = (Vector2)Tracer.transform.position - lastPosition;
		float lastAngle = Vector2.SignedAngle(Vector2.right, lastDiff);

		float nextAngle = lastAngle + noise;
		Vector2 direction = Utils.AngleToVector(nextAngle);
		nextPosition = new Vector2(
			Mathf.Clamp(Tracer.transform.position.x + direction.x * 0.1f, MinDrawCoordinates.x, MaxDrawCoordinates.x),
			Mathf.Clamp(Tracer.transform.position.y + direction.y * 0.1f, MinDrawCoordinates.y, MaxDrawCoordinates.y)
		);

		return Vector2.Distance(Tracer.transform.position, nextPosition) > 0.03f;
	}

	private IEnumerator DrawTrace() {
		Vector2 lastPosition = new Vector2(
			UnityEngine.Random.Range( -6f, 1f ),
			UnityEngine.Random.Range(  0f, 3f )
		);

		ArtCanvas.SetSubmitButtonActive(false);
		var newTracer = Instantiate(Tracer, lastPosition, Quaternion.identity, Tracer.transform.parent);
		Destroy(Tracer.gameObject);
		Tracer = newTracer;

		Vector2 nextPosition = lastPosition;

		ActualTraceCheckedLinePoints = new List<LinePoint>();

		float timeSinceLastReachedPoint = 0f;
		bool generating = true;
		while(ActualTraceCheckedLinePoints.Count < 200f && generating) {
			if( Vector2.Distance(Tracer.transform.position, nextPosition) < 0.001f ) {
				bool success = GenerateNewPoint(lastPosition, ref nextPosition);	
				if(!success) {
					// try one more time before quitting
					success = GenerateNewPoint(lastPosition, ref nextPosition, 0.5f);
				}
				lastPosition = Tracer.transform.position;
				generating = success;

				// add this position to the array
				Vector2 normal = (nextPosition - (Vector2)Tracer.transform.position).Rotate(90f).normalized;
				ActualTraceCheckedLinePoints.Add(new LinePoint(Tracer.transform.position, normal));
				timeSinceLastReachedPoint = 0;
			}
			else {
				// move towards position
				Tracer.transform.position = Vector3.Lerp(lastPosition, nextPosition, timeSinceLastReachedPoint / 0.025f);
				timeSinceLastReachedPoint += Time.deltaTime;
			}

			yield return new WaitForEndOfFrame();
		}
		
		CreateCollider(Tracer.gameObject, ActualTraceCheckedLinePoints);
		ArtCanvas.SetSubmitButtonActive(true);
	}
}
