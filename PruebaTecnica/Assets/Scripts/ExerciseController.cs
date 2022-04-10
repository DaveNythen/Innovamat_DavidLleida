using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseController : MonoBehaviour
{
    public GameObject numberPrefab;
    public int numberOfChoices = 3;
    public Transform numbersContainer;

    public FloatReference animationTime;
    public float waitTime;

    private int correctChoice;
    private Number correctNumber;
    private List<Number> numbersToChoose = new List<Number>();

    private bool isFirstMistake = true;

    private ScoreController scoreController;

    private void Awake()
    {
        scoreController = FindObjectOfType<ScoreController>();
    }

    private void OnEnable()
    {
        Number.OnChooseNumber += CheckSolution;
    }

    private void OnDisable()
    {
        Number.OnChooseNumber -= CheckSolution;
    }

    private void Start()
    {
        for (int i = 0; i < numberOfChoices; i++)
        {
            GameObject instance = Instantiate(numberPrefab, numbersContainer);
            numbersToChoose.Add(instance.GetComponent<Number>());
        }

        BeginExercise();
    }

    private void BeginExercise()
    {
        correctChoice = Random.Range(0, numberOfChoices);
        correctNumber = numbersToChoose[correctChoice];

        StartCoroutine(WaitForResetToStartOver());        
    }

    IEnumerator WaitForResetToStartOver()
    {
        FadeOutAllButtons();

        yield return new WaitForSeconds(animationTime.Value);

        ResetAllButtons();
        SetActiveIncorrectNumbers(false);
        ShowNumber();
    }

    private void FadeOutAllButtons()
    {
        foreach (Number number in numbersToChoose)
        {
            number.FadeOutAnimation();
            number.SetInteraction(false); //avoid clicking while fading away
        }
    }

    private void ResetAllButtons()
    {
        foreach (Number number in numbersToChoose)
        {
            //number.SetInteraction(false);
            number.ResetColor();
        }
    }

    private void SetActiveIncorrectNumbers(bool isActive)
    {
        foreach (Number number in numbersToChoose)
        {
            if (number != correctNumber)
                number.gameObject.SetActive(isActive);
        }
    }

    private void ShowNumber()
    {
        correctNumber.isWords = true;
        correctNumber.Setup(true);

        StartCoroutine(ShowOneNumber(correctNumber));
    }

    IEnumerator ShowOneNumber(Number number)
    {
        number.FadeInAnimation();

        yield return new WaitForSeconds(animationTime.Value + waitTime);

        number.FadeOutAnimation();

        yield return new WaitForSeconds(animationTime.Value);

        ShowAllNumbers();
    }

    private void ShowAllNumbers()
    {
        SetActiveIncorrectNumbers(true);

        foreach (Number number in numbersToChoose)
        {
            number.isWords = false;

            if (number == correctNumber)
                number.Setup(false);
            else
            {
                //avoid repeated numbers
                do
                {
                    number.Setup(true);
                }
                while (number.value == correctNumber.value);
            }

            number.FadeInAnimation();

            StartCoroutine(WaitToTurnInteractionOn(number));
        }
    }

    IEnumerator WaitToTurnInteractionOn(Number number)
    {
        yield return new WaitForSeconds(animationTime.Value - 0.8f);

        number.SetInteraction(true);
    }

    private void CheckSolution(GameObject numberToCheck)
    {
        Number number = numberToCheck.GetComponent<Number>();

        if (number == correctNumber)
        {
            number.ShowOutcome(true);
            scoreController.IncreaseCorrectScore();

            BeginExercise();
            isFirstMistake = true; //Reset the opportunities
        }
        else
        {
            number.ShowOutcome(false);
            
            if (isFirstMistake)
            {
                HideIncorrectAnswer(number);
                isFirstMistake = false;
            }
            else
            {
                correctNumber.ShowOutcome(true);
                scoreController.IncreaseErrorScore();

                isFirstMistake = true; //Reset the opportunities
                BeginExercise();
            }
        }
    }

    private void HideIncorrectAnswer(Number numberToHide)
    {
        numberToHide.FadeOutAnimation();
        numberToHide.SetInteraction(false);
    }
}
