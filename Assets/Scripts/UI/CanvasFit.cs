using UnityEngine;
using UnityEngine.UI;

/// Scales a screen-space canvas so it stays readable and on screen at any window size and shape (DECISIONS 2026-09-29).
/// The scale follows the screen height from a reference height, never drops below a floor (so short or small windows
/// keep readable text), and never grows past what lets the fit target (the menu's content box plus a margin) fit on
/// screen. Replaces the CanvasScaler's own mode: the scaler is set to Constant Pixel Size and its factor driven here.
[RequireComponent(typeof(CanvasScaler))]
public class CanvasFit : MonoBehaviour
{
    [Tooltip("Screen height, in pixels, at which one reference unit is one pixel.")]
    [SerializeField] private float referenceHeight = 1080f;   // player canvases: Style.md 7.2 sizes typed as written
    [Tooltip("Smallest scale from the height rule, so small windows keep readable text.")]
    [SerializeField] private float minScale = 0.667f;         // 720 rows
    [Tooltip("Optional: the box that must always fit on screen (a menu's content). Empty means no fit limit.")]
    [SerializeField] private RectTransform fitTarget;
    [Tooltip("Space kept around the fit target, in reference units on each side.")]
    [SerializeField] private float fitMargin = 24f;

    private CanvasScaler scaler;
    private Vector2Int fitted;

    private void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    private void OnEnable() => fitted = Vector2Int.zero;

    private void Update()
    {
        var screen = new Vector2Int(Screen.width, Screen.height);
        if (screen == fitted || screen.x <= 0 || screen.y <= 0) return;
        fitted = screen;
        scaler.scaleFactor = Scale(screen);
    }

    private float Scale(Vector2Int screen)
    {
        float scale = Mathf.Max(screen.y / referenceHeight, minScale);
        if (fitTarget == null) return scale;
        var size = fitTarget.rect.size + 2f * fitMargin * Vector2.one;
        if (size.x <= 0f || size.y <= 0f) return scale;
        return Mathf.Min(scale, screen.x / size.x, screen.y / size.y);
    }
}
