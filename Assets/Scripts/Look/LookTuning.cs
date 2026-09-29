using UnityEngine;

/// All numbers for the VHS look in one asset, same pattern as PlayerTuning.
/// Edit during Play mode and the values stay. Lives at Assets/Settings/LookTuning.asset.
[CreateAssetMenu(fileName = "LookTuning", menuName = "DieAlone/Look Tuning")]
public class LookTuning : ScriptableObject
{
    [Header("Filter")]
    [Tooltip("Master switch for the whole full-screen filter.")]
    public bool filterEnabled = true;

    [Header("Low resolution")]
    [Tooltip("Internal picture height in pixels. Width follows the screen aspect. The picture is scaled back up softly.")]
    [Range(64, 1080)] public int lowResHeight = 480;

    [Header("Tape color")]
    [Tooltip("How far colors smear sideways past their edges. 0 is off, 1 is heavy.")]
    [Range(0f, 1f)] public float colorBleed = 0.5f;
    [Tooltip("How much color drains toward gray. 0 is full color, 1 is nearly gray.")]
    [Range(0f, 1f)] public float washOut = 0.35f;
    [Tooltip("How much shadow detail collapses into flat black. 0 is off.")]
    [Range(0f, 1f)] public float crushBlacks = 0.3f;

    [Header("Grain and tape noise")]
    [Tooltip("Strength of the moving grain over the whole picture. 0 is off.")]
    [Range(0f, 1f)] public float grainStrength = 0.35f;
    [Tooltip("How many times per second the grain pattern changes.")]
    [Range(1f, 60f)] public float grainSpeed = 24f;
    [Tooltip("How strong the noise band is when it appears. 0 turns bands off.")]
    [Range(0f, 1f)] public float noiseBandStrength = 0.7f;
    [Tooltip("How fast the band rolls up the screen, in screen heights per second.")]
    [Range(0.1f, 3f)] public float noiseBandSpeed = 0.6f;
    [Tooltip("Average seconds between bands.")]
    [Range(1f, 60f)] public float noiseBandInterval = 6f;

    [Header("Scan lines, blur, dark corners")]
    [Tooltip("Darkening of every other picture row. 0 is off.")]
    [Range(0f, 1f)] public float scanLines = 0.3f;
    [Tooltip("Sideways softness on top of the low resolution. 0 is off.")]
    [Range(0f, 1f)] public float blur = 0.3f;
    [Tooltip("How dark the corners of the picture go. 0 is off.")]
    [Range(0f, 1f)] public float darkCorners = 0.4f;

    [Header("Fog and darkness")]
    [Tooltip("Master switch for distance fog.")]
    public bool fogEnabled = true;
    [Tooltip("Color things fade into. The sky takes this color too, so the far end blends away.")]
    public Color fogColor = new Color(0.02f, 0.03f, 0.05f);
    [Tooltip("Distance in metres where fog starts.")]
    [Range(0f, 100f)] public float fogStart = 6f;
    [Tooltip("Distance in metres where things are fully hidden.")]
    [Range(1f, 200f)] public float fogEnd = 40f;

    [Header("Fog (sunset scenes)")]
    [Tooltip("Haze color for scenes set to the Sunset fog set. The far ridge fades into this.")]
    public Color sunsetFogColor = new Color(0.62f, 0.36f, 0.22f);
    [Range(0f, 200f)] public float sunsetFogStart = 25f;
    [Range(1f, 600f)] public float sunsetFogEnd = 320f;
    [Tooltip("Flat ambient light for sunset scenes. Lifts the forest floor under the canopy where the low sun never reaches.")]
    public Color sunsetAmbient = new Color(0.5f, 0.34f, 0.28f);

    [Header("Night (the Night fog set; LookSlice.md 4)")]
    [Tooltip("Flat ambient at night. LookSlice #07080A.")]
    public Color nightAmbient = new Color(0.027f, 0.031f, 0.039f);
    [Tooltip("Faint warm fill from the fire in the west at night, silhouettes only. Style #5A2412.")]
    public Color fireFillColor = new Color(0.353f, 0.141f, 0.071f);
    [Range(0f, 2f)] public float fireFillIntensity = 0.25f;

    [Header("Sunset sky")]
    public Color skyTop = new Color(0.22f, 0.11f, 0.10f);
    public Color skyHorizon = new Color(0.85f, 0.40f, 0.18f);
    public Color skyGround = new Color(0.30f, 0.16f, 0.10f);
    public Color sunGlowColor = new Color(1.0f, 0.55f, 0.25f);
    [Tooltip("Higher is a tighter glow around the sun.")]
    [Range(2f, 200f)] public float sunGlowSize = 24f;

    [Header("Sun (day looks; read by the dev look preview, scenes keep their own light)")]
    [Tooltip("Colour of the fixed sun.")]
    public Color sunColor = new Color(1.0f, 0.55f, 0.25f);
    [Tooltip("Sun height above the horizon in degrees.")]
    [Range(0f, 90f)] public float sunElevation = 6f;
    [Tooltip("Compass bearing of the sun in degrees: 0 north, 90 east, 180 south, 270 west.")]
    [Range(0f, 360f)] public float sunBearing = 270f;
    [Tooltip("Sun light intensity.")]
    [Range(0f, 4f)] public float sunIntensity = 1.2f;

    [Header("Horizon fire")]
    public Color fireGlowColor = new Color(1.0f, 0.42f, 0.10f);
    [Tooltip("Brightness of the glow strips along the burning ridge. 0 hides them.")]
    [Range(0f, 6f)] public float fireGlowIntensity = 2.0f;
    [Tooltip("Smoke columns over the ridge. 0 is none, 1 is the built amount.")]
    [Range(0f, 2f)] public float fireSmoke = 1.0f;
    [Tooltip("Embers and ash drifting over the ridge. 0 is none, 1 is the built amount.")]
    [Range(0f, 2f)] public float fireEmbers = 1.0f;
    [Tooltip("How much each glow patch breathes around its set brightness. 0 is a steady glow.")]
    [Range(0f, 1f)] public float fireFlicker = 0.3f;
    [Tooltip("Speed of that breathing. Low is a slow smoulder.")]
    [Range(0.1f, 5f)] public float fireFlickerSpeed = 0.8f;

    [Header("Smoke (DieAlone/Smoke shader)")]
    [Tooltip("Colour of the smoke body where the sun reaches it. Style.md #4A3A32.")]
    public Color smokeBodyColor = new Color(0.290f, 0.227f, 0.196f);
    [Tooltip("Colour the fire lights the smoke underside with. Style.md #6B2A12.")]
    public Color smokeFireColor = new Color(0.420f, 0.165f, 0.071f);
    [Tooltip("How bright the side of the smoke facing away from the sun stays. 0 is black, 1 is no shading.")]
    [Range(0f, 1f)] public float smokeShadowFloor = 0.45f;
    [Tooltip("Strength of the fire light on the smoke underside. 0 turns it off.")]
    [Range(0f, 4f)] public float smokeFireStrength = 1.0f;

    [Header("Practical lights (lamps, lanterns, stove, fire pit; LookSlice.md 4)")]
    [Tooltip("Colour of every practical light, the warmest light on the map. LookSlice #FFA860.")]
    public Color practicalColor = new Color(1.0f, 0.659f, 0.376f);
    [Tooltip("Fire pit light at night. The brightest practical; nothing else beats it.")]
    [Range(0f, 8f)] public float firePitIntensity = 3.0f;
    [Tooltip("Wood stove light inside the cabin.")]
    [Range(0f, 8f)] public float stoveIntensity = 1.2f;
    [Tooltip("Oil lamps: the desk lamp and the cab lamp.")]
    [Range(0f, 8f)] public float lampIntensity = 1.0f;
    [Tooltip("Lanterns: the porch lantern and the tower foot lantern (night only).")]
    [Range(0f, 8f)] public float lanternIntensity = 0.8f;
    [Tooltip("Share of the night brightness a practical keeps in a daylight look (low, modest).")]
    [Range(0f, 1f)] public float practicalDayScale = 0.4f;
    [Tooltip("Brightness of the cab lamp bulb, drawn without fog so it marks the tower at night.")]
    [Range(0f, 10f)] public float cabLampBulbIntensity = 3.0f;
    [Tooltip("Brightness of the cab's lit windows at night, drawn without fog so the tower reads from the Ward pass.")]
    [Range(0f, 4f)] public float cabWindowGlowIntensity = 0.6f;

    [Header("Far markers (drawn without fog so they mark places at night; Style.md 2.4)")]
    [Tooltip("Office lot lights, sodium. Style #F08A2A.")]
    public Color lotLightColor = new Color(0.941f, 0.541f, 0.165f);
    [Tooltip("Red lamp on the office mast. Style #B0201C.")]
    public Color mastLampColor = new Color(0.690f, 0.125f, 0.110f);
    [Tooltip("Brightness of the lot lights and the mast lamp.")]
    [Range(0f, 10f)] public float farMarkerIntensity = 2.5f;

    [Header("Distance layers (off-map backdrop, DieAlone/Backdrop; Main3.md 2.11)")]
    [Tooltip("Forested hills and the rolling forest east of the fence. Style #4F4A2C.")]
    public Color backdropForestColor = new Color(0.310f, 0.290f, 0.173f);
    [Tooltip("The far ranges. Style granite #6E6660.")]
    public Color backdropRangeColor = new Color(0.431f, 0.400f, 0.376f);
    [Tooltip("Off-map ground under the edge forest and the skirt below the map edge.")]
    public Color backdropGroundColor = new Color(0.290f, 0.271f, 0.180f);
    [Tooltip("How far the near layers (ridges 120 to 300 m out, the east forest) blend toward the fog colour.")]
    [Range(0f, 1f)] public float backdropNearHaze = 0.55f;
    [Tooltip("How far the far ranges (1.2 to 2.5 km out) blend toward the fog colour.")]
    [Range(0f, 1f)] public float backdropFarHaze = 0.8f;
    [Tooltip("How far the off-map ground and the map-edge skirt blend toward the fog colour.")]
    [Range(0f, 1f)] public float backdropGroundHaze = 0.2f;
}
