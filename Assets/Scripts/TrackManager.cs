using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackManager : Singleton<TrackManager>
{
    [SerializeField] private TextMeshPro randomNumberText;
    [SerializeField] private List<Option> options;

    private readonly List<int> _chosenNumbers = new List<int>();
    private int _randomNumber;
    private int _correctNumber;
    private int _correctNumberIndex;

    public void Begin()
    {
        randomNumberText.gameObject.SetActive(true);
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
        print("RIGHT CHOICE");
        RemoveListeners();
        NextNumber();
    }

    void RemoveListeners()
    {
        foreach (var option in options)
        {
            option.optionButton.onClick.RemoveAllListeners();
        }
    }

    void WrongChoice()
    {
        print("WRONG CHOICE");
    }
}
