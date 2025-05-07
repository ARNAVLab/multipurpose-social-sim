using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class SpeechBubbleManager : MonoBehaviour
{
    // Singleton for easy access from other classes (optional, but convenient).
    public static SpeechBubbleManager Instance { get; private set; }

    [Header("References to Bubble Objects in the Scene")]
    [SerializeField] private GameObject speechBubble1;     // The "SpeechBubble1" object
    [SerializeField] private TMP_Text speechBubble1Text;       // The Text child under SpeechBubble1 -> Panel -> Text
    
    [SerializeField] private GameObject speechBubble2;     // The "SpeechBubble2" object
    [SerializeField] private TMP_Text speechBubble2Text;       // The Text child under SpeechBubble2 -> Panel -> Text

    [Header("UI Canvas")]
    [SerializeField] private Canvas uiCanvas;              // The Canvas for positioning

    [Header("Offsets")]
    // How far above/left/right to place each bubble relative to the agent's world position.
    [SerializeField] private Vector3 leftBubbleOffset = new Vector3(-0.5f, 2f, 0f);
    [SerializeField] private Vector3 rightBubbleOffset = new Vector3(0.5f, 2f, 0f);

    private void Awake()
    {
        // Set up singleton instance (if you want to access this script easily from anywhere).
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Optionally hide both bubbles at startup.
        speechBubble1.SetActive(false);
        speechBubble2.SetActive(false);
    }

    /// <summary>
    /// Displays the left bubble (SpeechBubble1) above and slightly left of the given world position.
    /// Sets the text to the provided string.
    /// </summary>
    public void ShowLeftBubble(string dialogue, Vector3 agentWorldPosition)
    {
        // Convert the agent's world position (plus offset) to screen coordinates.
        Vector3 screenPos = Camera.main.WorldToScreenPoint(agentWorldPosition + leftBubbleOffset);

        // Place the bubble under the UI canvas and set its position in screen space.
        speechBubble1.transform.SetParent(uiCanvas.transform, false);
        speechBubble1.transform.position = screenPos;

        // Update the text
        if (speechBubble1Text != null)
        {
            speechBubble1Text.text = dialogue;
        }

        // Enable it
        speechBubble1.SetActive(true);
    }

    /// <summary>
    /// Displays the right bubble (SpeechBubble2) above and slightly right of the given world position.
    /// Sets the text to the provided string.
    /// </summary>
    public void ShowRightBubble(string dialogue, Vector3 agentWorldPosition)
    {
        // Convert the agent's world position (plus offset) to screen coordinates.
        Vector3 screenPos = Camera.main.WorldToScreenPoint(agentWorldPosition + rightBubbleOffset);

        // Place the bubble under the UI canvas and set its position in screen space.
        speechBubble2.transform.SetParent(uiCanvas.transform, false);
        speechBubble2.transform.position = screenPos;

        // Update the text
        if (speechBubble2Text != null)
        {
            speechBubble2Text.text = dialogue;
        }

        // Enable it
        speechBubble2.SetActive(true);
    }

    /// <summary>
    /// Hides the left bubble (SpeechBubble1).
    /// </summary>
    public void HideLeftBubble()
    {
        speechBubble1.SetActive(false);
    }

    /// <summary>
    /// Hides the right bubble (SpeechBubble2).
    /// </summary>
    public void HideRightBubble()
    {
        speechBubble2.SetActive(false);
    }

    /// <summary>
    /// Hides both bubbles at once.
    /// </summary>
    public void HideAllBubbles()
    {
        speechBubble1.SetActive(false);
        speechBubble2.SetActive(false);
    }
}
