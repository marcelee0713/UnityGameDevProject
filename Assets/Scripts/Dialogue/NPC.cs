using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueManager dialogueManager;

    public GameObject DialogueBox;
    public Button triggerButton;
    public bool inRanged = false;
    public GameObject QuestionMark;

    public static UnityEvent currentEvent;

    private void Awake()
    {
        triggerButton.interactable = inRanged;
        QuestionMark.SetActive(inRanged);

        triggerButton.onClick.AddListener(() =>
        {
            if (triggerButton.interactable)
            {
                DialogueBox.SetActive(true);
                dialogueTrigger.StartDialogue();

                currentEvent.Invoke();

                // Also disable the button and the questionmark
                inRanged = false;
                triggerButton.interactable = inRanged;
                QuestionMark.SetActive(inRanged);
            }
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Message[] newMessages = dialogueTrigger.messages;
            Actor[] newActors = dialogueTrigger.actors;
            UnityEvent unityEvent = dialogueTrigger.StartCallback;

            currentEvent = unityEvent;

            DialogueManager.currentMessages =  newMessages;
            DialogueManager.currentActors = newActors;

            inRanged = true;
            triggerButton.interactable = inRanged;
            QuestionMark.SetActive(inRanged);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRanged = false;
            triggerButton.interactable = inRanged;
            QuestionMark.SetActive(inRanged);
        }
    }

}
