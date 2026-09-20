using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// The pause screen. Listens for the Pause action, shows the panel while GamePause
/// is paused, and binds each settings control to PlayerSettings.
/// To add a setting: add a control to the panel in the scene, a field here, and a
/// Bind* call in Awake that reads and writes PlayerSettings.Current.
public class PauseMenu : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Screen")]
    [SerializeField] private GameObject panel;
    [Tooltip("Selected when the menu opens, so a gamepad can navigate.")]
    [SerializeField] private Selectable firstSelected;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    [Header("Settings")]
    [SerializeField] private Slider lookSensitivity;
    [SerializeField] private Text lookSensitivityValue;
    [SerializeField] private Toggle invertLook;

    private InputAction pauseAction;

    private void Awake()
    {
        var map = inputActions.FindActionMap("Player", throwIfNotFound: true);
        pauseAction = map.FindAction("Pause", throwIfNotFound: true);

        resumeButton.onClick.AddListener(OnResume);
        quitButton.onClick.AddListener(OnQuit);
        BindLookSensitivity();
        BindInvertLook();

        panel.SetActive(false);
    }

    private void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.performed += OnPausePerformed;
        if (GamePause.Instance != null)
        {
            GamePause.Instance.Paused += Show;
            GamePause.Instance.Resumed += Hide;
        }
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPausePerformed;
        pauseAction.Disable();
        if (GamePause.Instance != null)
        {
            GamePause.Instance.Paused -= Show;
            GamePause.Instance.Resumed -= Hide;
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        if (GamePause.Instance != null) GamePause.Instance.Toggle();
    }

    private void Show()
    {
        RefreshFromSettings();
        panel.SetActive(true);
        if (EventSystem.current != null && firstSelected != null)
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }

    private void Hide()
    {
        panel.SetActive(false);
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnResume()
    {
        if (GamePause.Instance != null) GamePause.Instance.Resume();
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ---- Settings bindings. One method per setting keeps additions mechanical.

    private void BindLookSensitivity()
    {
        lookSensitivity.minValue = PlayerSettings.MinLookSensitivity;
        lookSensitivity.maxValue = PlayerSettings.MaxLookSensitivity;
        lookSensitivity.onValueChanged.AddListener(v =>
        {
            PlayerSettings.Current.lookSensitivity = v;
            lookSensitivityValue.text = v.ToString("0.00");
            PlayerSettings.Save();
        });
    }

    private void BindInvertLook()
    {
        invertLook.onValueChanged.AddListener(on =>
        {
            PlayerSettings.Current.invertLook = on;
            PlayerSettings.Save();
        });
    }

    private void RefreshFromSettings()
    {
        var s = PlayerSettings.Current;
        lookSensitivity.SetValueWithoutNotify(s.lookSensitivity);
        lookSensitivityValue.text = s.lookSensitivity.ToString("0.00");
        invertLook.SetIsOnWithoutNotify(s.invertLook);
    }
}
