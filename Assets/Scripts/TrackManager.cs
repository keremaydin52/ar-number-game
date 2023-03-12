using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackManager : Singleton<TrackManager>
{
    [SerializeField] private TextMeshPro randomNumberText;
    [SerializeField] private TextMeshProUGUI congratulationsText;
    [SerializeField] private List<Option> options;
    [SerializeField] private float congratulationsTime = 0.25f;

    private readonly List<int> _chosenNumbers = new List<int>();
    private int _randomNumber;
    private int _correctNumber;
    private int _correctNumberIndex;
    private int _correctAnswerCount;
    private readonly int _correctAnswerThreshold = 10;

    public void Begin()
    {
        randomNumberText.gameObject.SetActive(true);
        _correctAnswerCount = 0;
        NextNumber();
    }
    
    void NextNumber()
    {
        ChooseRandomNumbers(options.Count);
        AssignCorrectNumberFromList(_chosenNumbers);
        AssignOptions();
    }
    
    void ChooseRandomNumbers(int numbersCount)
    {
        _chosenNumbers.Clear();
        
        while (_chosenNumbers.Count < numbersCount) {
            int randomNumber = Random.Range(1, 11);
            if (!_chosenNumbers.Contains(randomNumber)) 
            {
                _chosenNumbers.Add(randomNumber);
            }
        }
    }

    void AssignCorrectNumberFromList(List<int> chosenNumbers)
    {
        _correctNumberIndex = Random.Range(0, chosenNumbers.Count);
        _correctNumber = chosenNumbers[_correctNumberIndex];
        randomNumberText.text = _correctNumber.ToString();
    }

    void AssignOptions()
    {
        options[_correctNumberIndex].optionText.text = _correctNumber.ToString();
        options[_correctNumberIndex].optionButton.onClick.AddListener(RightChoice);

        for (int i = 0; i < options.Count; i++)
        {
            if (i != _correctNumberIndex)
            {
                options[i].optionText.text = _chosenNumbers[i].ToString();
                options[i].optionButton.onClick.AddListener(WrongChoice);
            }
        }
    }

    void RightChoice()
    {
        RemoveListeners();
        _correctAnswerCount++;
        StartCoroutine(Congratulate());
    }
    
    void WrongChoice()
    {
        //TODO: Encourage child
    }

    IEnumerator Congratulate()
    {
        ActivateCongratulation(true);
        yield return new WaitForSeconds(congratulationsTime);
        ActivateCongratulation(false);
        if (_correctAnswerCount < _correctAnswerThreshold)
        {
            NextNumber();
        }
        else
        {
            GameOver();
        }
    }

    void ActivateCongratulation(bool isActive)
    {
        congratulationsText.gameObject.SetActive(isActive);
        randomNumberText.gameObject.SetActive(!isActive);
        options.ForEach(p => p.gameObject.SetActive(!isActive));
    }

    void RemoveListeners()
    {
        foreach (var option in options)
        {
            option.optionButton.onClick.RemoveAllListeners();
        }
    }

    void GameOver()
    {
        randomNumberText.gameObject.SetActive(false);
        GameManager.Instance.SwitchState("GameOver");
    }
}
