using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    public static event Action<GameObject> OnChooseNumber;

    [Header("Value Configuration")]
    [Range(0, 1000)]
    public int maxValue;
    [HideInInspector]
    public int value;

    public bool isWords;
    [HideInInspector]
    public string words;

    [Header ("Display")]
    [SerializeField] Text textUI;
    [SerializeField] Image buttonColor;
    [SerializeField] Color correctColor, incorrectColor;
    public FloatReference animationTime;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
        SetInteraction(false);
    }

    //Called from the button
    public void ChooseNumber()
    {
        OnChooseNumber?.Invoke(gameObject);
    }

    public void Setup(bool needNewValue)
    {
        if(needNewValue)
            SetValue();

        if (isWords) 
        {
            SetWords();
            textUI.text = words;
        }
        else
            textUI.text = value.ToString();
    }

    private void SetValue()
    {
        value = UnityEngine.Random.Range(0, maxValue);
    }

    private void SetWords()
    {
        words = NumberToWords.ConvertNumberToWords(value);
    }

    public void FadeInAnimation()
    {
        StartCoroutine(Fade(false));
    }

    public void FadeOutAnimation()
    {
        StartCoroutine(Fade(true));
    }

    IEnumerator Fade(bool fadeOut)
    {
        if (fadeOut)
        {
            if (canvasGroup.alpha != 0)
            {
                //from opaque to transparent
                for (float i = animationTime.Value; i >= 0; i -= Time.deltaTime)
                {
                    canvasGroup.alpha = i;
                    yield return null;
                }
                canvasGroup.alpha = 0;
            }
        }
        else
        {
            //from transparent to opaque
            for (float i = 0; i <= animationTime.Value; i += Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }
            canvasGroup.alpha = 1;
        }
    }

    public void SetInteraction(bool isInteractable)
    {
        canvasGroup.interactable = isInteractable;
    }

    public void ShowOutcome(bool isCorrect)
    {
        if (isCorrect)
            buttonColor.color = correctColor;
        else
            buttonColor.color = incorrectColor;
    }

    public void ResetColor()
    {
        buttonColor.color = Color.white;
    }
}
