using UnityEngine;
using UnityEngine.UI;

/// Owns the center dot and the prompt label. Empty text hides the label.
public class InteractPromptUI : MonoBehaviour
{
    [SerializeField] private Text label;

    public void SetPrompt(string text)
    {
        bool show = !string.IsNullOrEmpty(text);
        if (label.enabled != show) label.enabled = show;
        if (show && label.text != text) label.text = text;
    }
}
