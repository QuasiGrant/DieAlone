using UnityEngine;

/// Far markers that only show outside the daylight looks, such as the tower cab's lit windows, which mark the
/// tower from the Ward pass at night (Main3.md 3.6.2). Their material ignores fog, so they read past the night fog end.
public class NightMarker : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    private void OnEnable() => Apply();
    private void Update() => Apply();

    private void Apply()
    {
        bool night = !LookOverride.ForceSunset;
        foreach (var r in renderers) if (r != null) r.enabled = night;
    }
}
