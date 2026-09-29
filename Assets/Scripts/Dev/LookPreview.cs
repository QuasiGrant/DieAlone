using UnityEngine;

/// F2 cycles the whole look between the scene's own LookTuning and the preview looks listed
/// here (day one, day two), in Play mode, without changing any asset or scene.
/// A daylight look forces the Sunset fog set and sky, turns the scene's directional lights off
/// and adds a sun placed and coloured from that LookTuning; going back restores everything.
/// Exists only in the Editor and development builds, like DevMenu: in a release build the
/// class compiles to an empty component.
public class LookPreview : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [System.Serializable]
    public struct Look
    {
        public string label;
        [Tooltip("Empty means the scene's own look.")]
        public LookTuning tuning;
        [Tooltip("Daytime look: Sunset fog set and sky, and a sun from the LookTuning sun fields.")]
        public bool daylight;
    }

    [SerializeField] private Look[] looks;
    [Tooltip("Material using DieAlone/SkyGradient. Copied at start so the preview never writes to the asset.")]
    [SerializeField] private Material skyMaterial;

    private int current;
    private Material skyCopy;
    private Light sun;
    private Light[] sceneLights;
    private Light previousSun;
    private UnityEngine.Rendering.AmbientMode previousAmbientMode;
    private Color previousAmbient;
    private Material previousSkybox;

    public int Count => looks != null ? looks.Length : 0;
    public int Current => current;
    public string CurrentLabel => Count > 0 ? looks[current].label : "";

    private void Awake()
    {
        if (skyMaterial != null) skyCopy = new Material(skyMaterial);
    }

    private void Update()
    {
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null && keyboard.f2Key.wasPressedThisFrame && Count > 0) Select((current + 1) % Count);
        if (sun != null) PlaceSun(looks[current].tuning);
    }

    /// Switches to look i. Index 0 is normally the scene's own look.
    public void Select(int i)
    {
        if (i < 0 || i >= Count) return;
        RestoreScene();
        current = i;
        var look = looks[i];
        if (look.tuning == null)
        {
            LookOverride.Clear();
            return;
        }
        LookOverride.Set(look.tuning, look.daylight, skyCopy);
        if (look.daylight) AddSun(look.tuning);
    }

    private void AddSun(LookTuning tuning)
    {
        previousSun = RenderSettings.sun;
        previousAmbientMode = RenderSettings.ambientMode;
        previousAmbient = RenderSettings.ambientLight;
        previousSkybox = RenderSettings.skybox;

        var lights = FindObjectsByType<Light>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        var off = new System.Collections.Generic.List<Light>();
        foreach (var l in lights)
        {
            if (l.type != LightType.Directional || !l.enabled) continue;
            l.enabled = false;
            off.Add(l);
        }
        sceneLights = off.ToArray();

        var go = new GameObject("LookPreviewSun");
        sun = go.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.shadows = LightShadows.Soft;
        RenderSettings.sun = sun;
        PlaceSun(tuning);
    }

    private void PlaceSun(LookTuning tuning)
    {
        sun.color = tuning.sunColor;
        sun.intensity = tuning.sunIntensity;
        // Light travels away from the sun's bearing, tilted down by its elevation.
        sun.transform.rotation = Quaternion.Euler(tuning.sunElevation, tuning.sunBearing + 180f, 0f);
    }

    private void RestoreScene()
    {
        if (sun == null) return;
        Destroy(sun.gameObject);
        sun = null;
        if (sceneLights != null) foreach (var l in sceneLights) if (l != null) l.enabled = true;
        sceneLights = null;
        RenderSettings.sun = previousSun;
        RenderSettings.ambientMode = previousAmbientMode;
        RenderSettings.ambientLight = previousAmbient;
        RenderSettings.skybox = previousSkybox;
    }

    private void OnDisable()
    {
        RestoreScene();
        LookOverride.Clear();
        current = 0;
    }

    private void OnDestroy()
    {
        if (skyCopy != null) Destroy(skyCopy);
    }

    private void OnGUI()
    {
        if (current == 0 || Count == 0) return;
        GUI.Label(new Rect(Screen.width - 260f, 10f, 250f, 24f), "Look preview: " + CurrentLabel + "  (F2)");
    }
#endif
}
