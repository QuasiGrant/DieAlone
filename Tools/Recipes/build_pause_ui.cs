var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
if (UnityEngine.GameObject.Find("PauseMenu") != null) return "PauseMenu already exists";
var V2 = new System.Func<float, float, UnityEngine.Vector2>((x, y) => new UnityEngine.Vector2(x, y));

var actions = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.InputSystem.InputActionAsset>("Assets/InputSystem_Actions.inputactions");
var font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
var res = new UnityEngine.UI.DefaultControls.Resources();
res.standard   = UnityEditor.AssetDatabase.GetBuiltinExtraResource<UnityEngine.Sprite>("UI/Skin/UISprite.psd");
res.background = UnityEditor.AssetDatabase.GetBuiltinExtraResource<UnityEngine.Sprite>("UI/Skin/Background.psd");
res.knob       = UnityEditor.AssetDatabase.GetBuiltinExtraResource<UnityEngine.Sprite>("UI/Skin/Knob.psd");
res.checkmark  = UnityEditor.AssetDatabase.GetBuiltinExtraResource<UnityEngine.Sprite>("UI/Skin/Checkmark.psd");
res.inputField = UnityEditor.AssetDatabase.GetBuiltinExtraResource<UnityEngine.Sprite>("UI/Skin/InputFieldBackground.psd");
res.dropdown   = UnityEditor.AssetDatabase.GetBuiltinExtraResource<UnityEngine.Sprite>("UI/Skin/DropdownArrow.psd");
res.mask       = UnityEditor.AssetDatabase.GetBuiltinExtraResource<UnityEngine.Sprite>("UI/Skin/UIMask.psd");

void StyleTexts(UnityEngine.GameObject go, int size)
{
    foreach (var t in go.GetComponentsInChildren<UnityEngine.UI.Text>(true)) { t.font = font; t.fontSize = size; t.color = UnityEngine.Color.white; }
}
UnityEngine.GameObject Label(string name, UnityEngine.Transform parent, string text, int size, float width, UnityEngine.TextAnchor align)
{
    var go = UnityEngine.UI.DefaultControls.CreateText(res); go.name = name; go.transform.SetParent(parent, false);
    var t = go.GetComponent<UnityEngine.UI.Text>(); t.text = text; t.alignment = align; StyleTexts(go, size);
    var le = go.AddComponent<UnityEngine.UI.LayoutElement>(); le.preferredWidth = width; le.preferredHeight = 36;
    return go;
}
UnityEngine.GameObject Row(string name, UnityEngine.Transform parent)
{
    var go = new UnityEngine.GameObject(name, typeof(UnityEngine.RectTransform)); go.transform.SetParent(parent, false);
    var h = go.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>(); h.spacing = 12; h.childAlignment = UnityEngine.TextAnchor.MiddleLeft;
    h.childControlWidth = false; h.childControlHeight = false; h.childForceExpandWidth = false; h.childForceExpandHeight = false;
    var le = go.AddComponent<UnityEngine.UI.LayoutElement>(); le.preferredHeight = 40; le.preferredWidth = 440;
    return go;
}

// ---- EventSystem with the Input System module
var es = new UnityEngine.GameObject("EventSystem");
es.AddComponent<UnityEngine.EventSystems.EventSystem>();
var module = es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
module.actionsAsset = actions;

// ---- Game object owning pause state
var player = UnityEngine.GameObject.Find("Player");
var game = new UnityEngine.GameObject("Game");
var pause = game.AddComponent<GamePause>();
var soP = new UnityEditor.SerializedObject(pause);
var arr = soP.FindProperty("gameplayBehaviours"); arr.arraySize = 2;
arr.GetArrayElementAtIndex(0).objectReferenceValue = player.GetComponent<PlayerController>();
arr.GetArrayElementAtIndex(1).objectReferenceValue = player.GetComponent<PlayerInteractor>();
soP.ApplyModifiedPropertiesWithoutUndo();

// ---- Pause menu canvas
var root = new UnityEngine.GameObject("PauseMenu");
var canvas = root.AddComponent<UnityEngine.Canvas>();
canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 10;
var scaler = root.AddComponent<UnityEngine.UI.CanvasScaler>();
scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = V2(1280f, 720f); scaler.matchWidthOrHeight = 0.5f;
root.AddComponent<UnityEngine.UI.GraphicRaycaster>();

var panel = UnityEngine.UI.DefaultControls.CreatePanel(res); panel.name = "Panel"; panel.transform.SetParent(root.transform, false);
panel.GetComponent<UnityEngine.UI.Image>().color = new UnityEngine.Color(0f, 0f, 0f, 0.65f);

var content = new UnityEngine.GameObject("Content", typeof(UnityEngine.RectTransform)); content.transform.SetParent(panel.transform, false);
var crt = content.GetComponent<UnityEngine.RectTransform>(); crt.anchorMin = crt.anchorMax = V2(0.5f, 0.5f); crt.sizeDelta = V2(460f, 360f);
var vl = content.AddComponent<UnityEngine.UI.VerticalLayoutGroup>(); vl.spacing = 14; vl.childAlignment = UnityEngine.TextAnchor.UpperCenter;
vl.childControlWidth = false; vl.childControlHeight = false; vl.childForceExpandWidth = false; vl.childForceExpandHeight = false;

Label("Title", content.transform, "Paused", 40, 440, UnityEngine.TextAnchor.MiddleCenter).GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 60;

// Look sensitivity row: label, slider, value
var rowS = Row("LookSensitivityRow", content.transform);
Label("Label", rowS.transform, "Look sensitivity", 20, 200, UnityEngine.TextAnchor.MiddleLeft);
var slider = UnityEngine.UI.DefaultControls.CreateSlider(res); slider.name = "Slider"; slider.transform.SetParent(rowS.transform, false);
var sle = slider.AddComponent<UnityEngine.UI.LayoutElement>(); sle.preferredWidth = 160; sle.preferredHeight = 24;
var value = Label("Value", rowS.transform, "1.00", 20, 60, UnityEngine.TextAnchor.MiddleRight);

// Invert look row: label, toggle
var rowT = Row("InvertLookRow", content.transform);
Label("Label", rowT.transform, "Invert look", 20, 200, UnityEngine.TextAnchor.MiddleLeft);
var toggle = UnityEngine.UI.DefaultControls.CreateToggle(res); toggle.name = "Toggle"; toggle.transform.SetParent(rowT.transform, false);
toggle.GetComponentInChildren<UnityEngine.UI.Text>().text = ""; StyleTexts(toggle, 20);
var tle = toggle.AddComponent<UnityEngine.UI.LayoutElement>(); tle.preferredWidth = 40; tle.preferredHeight = 24;

// Buttons
UnityEngine.GameObject MakeButton(string name, string text)
{
    var b = UnityEngine.UI.DefaultControls.CreateButton(res); b.name = name; b.transform.SetParent(content.transform, false);
    b.GetComponentInChildren<UnityEngine.UI.Text>().text = text; StyleTexts(b, 22);
    b.GetComponent<UnityEngine.UI.Image>().color = new UnityEngine.Color(0.25f, 0.25f, 0.25f, 1f);
    var le = b.AddComponent<UnityEngine.UI.LayoutElement>(); le.preferredWidth = 240; le.preferredHeight = 44;
    return b;
}
var resume = MakeButton("ResumeButton", "Resume");
var quit = MakeButton("QuitButton", "Quit");

var menu = root.AddComponent<PauseMenu>();
var so = new UnityEditor.SerializedObject(menu);
so.FindProperty("inputActions").objectReferenceValue = actions;
so.FindProperty("panel").objectReferenceValue = panel;
so.FindProperty("firstSelected").objectReferenceValue = resume.GetComponent<UnityEngine.UI.Button>();
so.FindProperty("resumeButton").objectReferenceValue = resume.GetComponent<UnityEngine.UI.Button>();
so.FindProperty("quitButton").objectReferenceValue = quit.GetComponent<UnityEngine.UI.Button>();
so.FindProperty("lookSensitivity").objectReferenceValue = slider.GetComponent<UnityEngine.UI.Slider>();
so.FindProperty("lookSensitivityValue").objectReferenceValue = value.GetComponent<UnityEngine.UI.Text>();
so.FindProperty("invertLook").objectReferenceValue = toggle.GetComponent<UnityEngine.UI.Toggle>();
so.ApplyModifiedPropertiesWithoutUndo();
panel.SetActive(false);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " roots=" + scene.rootCount + " sprites=" + (res.standard != null && res.knob != null && res.checkmark != null);
