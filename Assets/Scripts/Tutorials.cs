using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tutorials : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialMessage;
    [SerializeField] TextMeshProUGUI tutorialNumber;

    private int tutorialIndex = 0;
    public List<string> tutorialMessages = new List<string>();
    public GameObject tutButton;
    [SerializeField] GameObject tutorialWindow;
    [SerializeField] GameObject previousButton;

    private void Start()
    {
        tutorialNumber.text = tutorialIndex + 1 + "/" + tutorialMessages.Count;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        tutorialWindow.SetActive(false);
    }

    public void ShowNextTutorialMessage()
    {
        tutorialIndex++;

        if (tutorialIndex>= tutorialMessages.Count)
        {
            ResumeGame();
            return;
        }

        if (!previousButton.activeInHierarchy && tutorialIndex > 0)
        {
            previousButton.SetActive(true);
        }

        tutorialNumber.text = tutorialIndex + 1 + "/" + tutorialMessages.Count;
        tutorialMessage.text = tutorialMessages[tutorialIndex];

    }

    public void ShowPreviousTutorialMessage()
    {
        tutorialIndex--;

        if (tutorialIndex <= 1)
        {
            previousButton.SetActive(false);
        }
        if (!previousButton.activeInHierarchy && tutorialIndex > 0)
        {
            previousButton.SetActive(true);
        }

        tutorialNumber.text = tutorialIndex + 1 + "/" + tutorialMessages.Count;
        tutorialMessage.text = tutorialMessages[tutorialIndex];

    }

    public void OpenTutorial()
    {
        tutorialIndex = 0;
        tutorialNumber.text = tutorialIndex + 1 + "/" + tutorialMessages.Count;
        tutorialMessage.text = tutorialMessages[tutorialIndex];
    }
}
