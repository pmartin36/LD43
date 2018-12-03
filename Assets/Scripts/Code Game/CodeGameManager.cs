using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeGameManager : ContextManager
{
	private static Task Task = Task.Code;
	private enum SeriesType {
		Multiply,
		Linear
	}

	[SerializeField]
	private Transform NumberParent;
	private List<int> CorrectSeries;
	private List<Number> Numbers;
	public Number NumberPrefab;
	public ScoreScreen ScoreScreen;

	[SerializeField]
	private Button SubmitButton;
	private TMPro.TMP_Text submitButtonText;

	public override void OnEnable() {
		base.OnEnable();
		Init();	
	}

	public override void OnDisable() {
		base.OnDisable();
		foreach (Number n in Numbers) {
			Destroy(n.gameObject);
		}
	}

	private void SetSubmitButtonActive(bool active) {
		submitButtonText = submitButtonText ?? SubmitButton.GetComponentInChildren<TMPro.TMP_Text>();
		SubmitButton.interactable = active;
		submitButtonText.color = active ? Color.white : new Color(0.25f, 0.25f, 0.25f);
	}

	public override void Init() {
		base.Init();
		SetSubmitButtonActive(true);
		GenerateSeries();
	}

	public void GenerateSeries() {
		bool eating =  ReliefActivity == ReliefActivity.Eating;
		int numSeries = 7;

		CorrectSeries = new List<int>();
		Numbers = new List<Number>();

		float currentLeft = 75;
		float currentRight = 975;

		List<int> missingNumbers = new List<int>();
		if (eating) {
			missingNumbers.Add(0);
		}
		missingNumbers.Add(UnityEngine.Random.Range(2, 4));
		missingNumbers.Add(UnityEngine.Random.Range(5, numSeries));

		do {
			SeriesType seriesType = (SeriesType)UnityEngine.Random.Range(0,2);
			int plus;
			int mult_min;
			int mult_max;
			int currentValue;
			switch (seriesType) {
				default:
				case SeriesType.Multiply:
					mult_min = 2;
					mult_max = 2;
					plus = UnityEngine.Random.Range(-1, 3);
					currentValue = UnityEngine.Random.Range(0, 4);
					break;
				case SeriesType.Linear:
					mult_min = 2;
					mult_max = 10;
					plus = UnityEngine.Random.Range(-2, 4);
					currentValue = plus;
					break;
			}
			int mult = UnityEngine.Random.Range(mult_min, mult_max+1);
		
			CorrectSeries.Clear();
			for(int i = 0; i < numSeries; i++) {
				CorrectSeries.Add(currentValue);
				switch (seriesType) {
					default:
					case SeriesType.Multiply:
						currentValue = currentValue * mult + plus;
						break;
					case SeriesType.Linear:
						currentValue = (i + 1) * mult + plus;
						break;
				}
			}
		} while( CorrectSeries[0] == CorrectSeries[CorrectSeries.Count -1] ); // make sure the series isn't all the same #

		for (int i = 0; i < numSeries; i++) {	
			var number = Instantiate(NumberPrefab, NumberParent);
			number.Init(
				missingNumbers.Contains(i),
				currentLeft,
				currentRight,
				CorrectSeries[i]
			);
			Numbers.Add(number);

			currentLeft += 150f;
			currentRight -= 150f;
		}
	}

	public override void Restart() {
		foreach(Number n in Numbers) {
			Destroy(n.gameObject);
		}
		Init();
	}

	public void Update() {
		
	}

	public void Submit() {
		SetSubmitButtonActive(false);
		var numCorrect = 0;
		for(int i = 0; i < CorrectSeries.Count; i++) {
			if(Numbers[i].Fillable && Numbers[i].Value == CorrectSeries[i] ) {
				numCorrect++;
			}
		}

		float normalScore = numCorrect / 2f;
		GameManager.Instance.TaskTracker.CompleteTask(Task, normalScore);
		ScoreScreen.Show(Task, normalScore);
	}

	public override void HandleInput(InputPackage p) { }
}
