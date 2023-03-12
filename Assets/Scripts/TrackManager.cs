using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TrackManager : Singleton<TrackManager>
{
    [Header("Texts")]
    [SerializeField] private TextMeshPro randomNumberText;
    [SerializeField] private TextMeshProUGUI congratulationsText;
    
    [SerializeField] private List<Option> options;
    
    [Header("Variables")]
    [SerializeField] private float congratulationsTime = 1f;

    [Header("Sounds")]
    [SerializeField] private AudioSource loudOutSound;
    [SerializeField] private AudioSource encourageSound;
    [SerializeField] private AudioSource congratulationsSound;

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
    }
    
    void ChooseRandomNumbers(int numbersCount)
    {
        _chosenNumbers.Clear();
        
        while (_chosenNumbers.Count < numbersCount) 
        {
            int randomNumber = Random.Range(1, 11);
            if (!_chosenNumbers.Contains(randomNumber)) 
            {
                _chosenNumbers.Add(randomNumber);
            }
        }
        StartCoroutine(AssignCorrectNumberFromList());
    }

    IEnumerator AssignCorrectNumberFromList()
    {
        _correctNumberIndex = Random.Range(0, _chosenNumbers.Count);
        _correctNumber = _chosenNumbers[_correctNumberIndex];
        
        // Play animation before showing the correct number
        float currentTime = 0f;
        while (currentTime < 0.5f)
        {
            currentTime += Time.deltaTime;
            int randomNumber = Random.Range(1, 11);
            randomNumberText.text = randomNumber.ToString();
            yield return null;
        }
        randomNumberText.text = _correctNumber.ToString();
        
        AssignOptions();
        loudOutSound.Play();
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

        ActivateOptionButtons(true);
    }

    void RightChoice()
    {
        RemoveListeners();
        _correctAnswerCount++;
        StartCoroutine(Congratulate());
    }
    
    void WrongChoice()
    {
        encourageSound.Play();
    }

    IEnumerator Congratulate()
    {
        congratulationsSound.Play();
        float waitTime = (congratulationsTime < congratulationsSound.clip.length) ? congratulationsSound.clip.length : congratulationsTime;
        
        ActivateCongratulation(true);
        yield return new WaitForSeconds(waitTime);
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
        float animationTime = congratulationsSound.clip.length / 3;
        congratulationsText.gameObject.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), animationTime)
            .setEase(LeanTweenType.punch);
        randomNumberText.gameObject.SetActive(!isActive);
        ActivateOptionButtons(false);
    }

    void RemoveListeners()
    {
        foreach (var option in options)
        {
            option.optionButton.onClick.RemoveAllListeners();
        }
    }

    void ActivateOptionButtons(bool isActive)
    {
        options.ForEach(p => p.gameObject.SetActive(isActive));
        if (isActive)
        {
            options.ForEach(p => p.gameObject.LeanRotateAroundLocal(Vector3.forward, 360, 0.25f));
        }
        options.ForEach(p => p.optionButton.interactable = isActive);
    }

    void GameOver()
    {
        randomNumberText.gameObject.SetActive(false);
        GameManager.Instance.SwitchState("GameOver");
    }
}
