using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogText;
    public Image portraitImage;
    public GameObject choicesContainer;
    public GameObject choiceButtonPrefab;

    [Header("Dialog Control")]
    public DialogNode startingNode;
    private DialogNode currentNode;

    [Header("AI Control")]
    public bool isAIControlled = true;
    public AIState aiState;

    public float typingSpeed = 0.05f;

    void Start()
    {
        StartDialog(startingNode);
    }

    public void StartDialog(DialogNode node)
    {
        currentNode = node;
        StopAllCoroutines();
        DisplayDialog(currentNode);
    }

    void DisplayDialog(DialogNode node)
    {
        speakerText.text = node.speakerName;
        portraitImage.sprite = node.portrait;
        dialogText.text = "";
        ClearChoices();
        StartCoroutine(TypeSentence(node.dialogText));
    }

    IEnumerator TypeSentence(string sentence)
    {
        foreach (char c in sentence.ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Now show choices
        if (isAIControlled)
        {
            AutoPickChoice();
        }
        else
        {
            ShowChoicesToPlayer();
        }
    }

    void ShowChoicesToPlayer()
    {
        foreach (var choice in currentNode.choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

            var nextNode = choice.nextNode;
            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartDialog(nextNode);
            });
        }
    }

    void AutoPickChoice()
    {
        foreach (var choice in currentNode.choices)
        {
            if (MatchesAI(choice))
            {
                StartDialog(choice.nextNode);
                return;
            }
        }

        // No matching choice — fallback to first available
        Debug.LogWarning("No matching AI choice found. Defaulting to first option.");
        if (currentNode.choices.Count > 0)
        {
            StartDialog(currentNode.choices[0].nextNode);
        }
        else
        {
            Debug.Log("End of dialog.");
        }
    }

    bool MatchesAI(DialogChoice choice)
    {
        bool emotionOk = !choice.requiredEmotion.HasValue || aiState.emotion == choice.requiredEmotion.Value;
        bool moralityOk = !choice.requiredMorality.HasValue || aiState.morality == choice.requiredMorality.Value;
        bool trustOk = aiState.trustLevel >= choice.minTrust && aiState.trustLevel <= choice.maxTrust;

        return emotionOk && moralityOk && trustOk;
    }

    void ClearChoices()
    {
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
