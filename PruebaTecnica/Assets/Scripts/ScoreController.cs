using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] Text correctValue, errorValue;

    private int correctScore, errorScore;

    public void IncreaseCorrectScore()
    {
        correctScore++;
        correctValue.text = correctScore.ToString();
    }

    public void IncreaseErrorScore()
    {
        errorScore++;
        errorValue.text = errorScore.ToString();
    }
}
