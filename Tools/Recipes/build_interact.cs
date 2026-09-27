var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
if (UnityEngine.GameObject.Find("HUD") != null) return "HUD already exists";

var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var actions = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.InputSystem.InputActionAsset>("Assets/InputSystem_Actions.inputactions");
var font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
if (actions == null || font == null) return "missing actions=" + (actions == null) + " font=" + (font == null);

// ---- HUD canvas: screen-space overlay, scaled to a 1280x720 reference.
var hud = new UnityEngine.GameObject("HUD");
var canvas = hud.AddComponent<UnityEngine.Canvas>();
canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceOverlay;
var scaler = hud.AddComponent<UnityEngine.UI.CanvasScaler>();
scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
scaler.referenceResolution = new UnityEngine.Vector2(1280f, 720f);
scaler.matchWidthOrHeight = 0.5f;

// Center dot
var dotGo = new UnityEngine.GameObject("Dot");
dotGo.transform.SetParent(hud.transform, false);
var dot = dotGo.AddComponent<UnityEngine.UI.Image>();
dot.color = new UnityEngine.Color(1f, 1f, 1f, 0.8f);
dot.raycastTarget = false;
var dotRt = dot.rectTransform;
dotRt.anchorMin = dotRt.anchorMax = new UnityEngine.Vector2(0.5f, 0.5f);
dotRt.sizeDelta = new UnityEngine.Vector2(4f, 4f);
dotRt.anchoredPosition = UnityEngine.Vector2.zero;

// Prompt label under the dot
var labelGo = new UnityEngine.GameObject("Prompt");
labelGo.transform.SetParent(hud.transform, false);
var label = labelGo.AddComponent<UnityEngine.UI.Text>();
label.font = font;
label.fontSize = 22;
label.alignment = UnityEngine.TextAnchor.UpperCenter;
label.color = UnityEngine.Color.white;
label.raycastTarget = false;
label.text = "";
label.enabled = false;
var labelRt = label.rectTransform;
labelRt.anchorMin = labelRt.anchorMax = new UnityEngine.Vector2(0.5f, 0.5f);
labelRt.sizeDelta = new UnityEngine.Vector2(400f, 40f);
labelRt.anchoredPosition = new UnityEngine.Vector2(0f, -40f);

var promptUi = hud.AddComponent<InteractPromptUI>();
var soUi = new UnityEditor.SerializedObject(promptUi);
soUi.FindProperty("label").objectReferenceValue = label;
soUi.ApplyModifiedPropertiesWithoutUndo();

// ---- Interactor on the Player
var player = UnityEngine.GameObject.Find("Player");
var cam = player.transform.Find("Main Camera");
var interactor = player.AddComponent<PlayerInteractor>();
var soI = new UnityEditor.SerializedObject(interactor);
soI.FindProperty("inputActions").objectReferenceValue = actions;
soI.FindProperty("eye").objectReferenceValue = cam;
soI.FindProperty("promptUI").objectReferenceValue = promptUi;
soI.ApplyModifiedPropertiesWithoutUndo();

// ---- Test box near spawn, 0.6 m cube on the ground
var box = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
box.name = "TestInteractBox";
box.transform.position = V(-8.5f, 0.3f, -8f);
box.transform.localScale = V(0.6f, 0.6f, 0.6f);
var toggle = box.AddComponent<ToggleColorInteractable>();
var soT = new UnityEditor.SerializedObject(toggle);
soT.FindProperty("prompt").stringValue = "Use";
soT.FindProperty("target").objectReferenceValue = box.GetComponent<UnityEngine.Renderer>();
soT.ApplyModifiedPropertiesWithoutUndo();

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " roots=" + scene.rootCount + " eye=" + (soI.FindProperty("eye").objectReferenceValue != null) + " ui=" + (soI.FindProperty("promptUI").objectReferenceValue != null);
