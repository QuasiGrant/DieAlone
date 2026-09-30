// 8.9h (Play or edit mode): sets the Game view to a fixed test size, or back to Free Aspect, and resets its Scale slider
// to 1x. Edit width and height; width 0 means restore (select Free Aspect and remove the test sizes this recipe added).
// Test sizes are added to the Editor's own Game view size list (user preferences, not the project) named "8.9h WxH".
// Found in 8.9h: Grant's Game view Scale slider sat at 1.13x, so a 3840 x 1976 frame was drawn 4322 x 2224 and about
// 237 px on the left (where the dev panel is) and 84 px top and bottom were outside the window. setScale resets it.
int width = 0, height = 0; bool setScale = true;
const string Tag = "8.9h ";
var F = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static;
var asm = typeof(UnityEditor.EditorWindow).Assembly;
var gvType = asm.GetType("UnityEditor.GameView"); var sizesType = asm.GetType("UnityEditor.GameViewSizes"); var sizeType = asm.GetType("UnityEditor.GameViewSize"); var kindType = asm.GetType("UnityEditor.GameViewSizeType");
var singleton = typeof(UnityEditor.ScriptableSingleton<>).MakeGenericType(sizesType).GetProperty("instance", F).GetValue(null);
var group = sizesType.GetProperty("currentGroup", F).GetValue(singleton); var groupType = group.GetType();
int Total() => (int)groupType.GetMethod("GetTotalCount", F).Invoke(group, null);
string Text(int i) => (string)sizeType.GetProperty("baseText", F).GetValue(groupType.GetMethod("GetGameViewSize", F).Invoke(group, new object[] { i }));
var view = (UnityEditor.EditorWindow)UnityEngine.Resources.FindObjectsOfTypeAll(gvType)[0];
void Select(int i) => gvType.GetMethod("SizeSelectionCallback", F).Invoke(view, new object[] { i, null });
if (width == 0)
{
    Select(0);
    for (int i = Total() - 1; i >= 0; i--) if (Text(i) != null && Text(i).StartsWith(Tag)) groupType.GetMethod("RemoveCustomSize", F).Invoke(group, new object[] { i });   // takes the full-list index
}
else
{
    string name = Tag + width + "x" + height; int index = -1;
    for (int i = 0; i < Total(); i++) if (Text(i) == name) index = i;
    if (index < 0)
    {
        var size = System.Activator.CreateInstance(sizeType, System.Enum.Parse(kindType, "FixedResolution"), width, height, name);
        groupType.GetMethod("AddCustomSize", F).Invoke(group, new[] { size });
        index = Total() - 1;
    }
    Select(index);
}
if (setScale)
{
    var zoom = gvType.GetField("m_ZoomArea", F).GetValue(view);
    var fit = gvType.GetProperty("minScale", F); float s = UnityEngine.Mathf.Min(1f, (float)fit.GetValue(view));
    gvType.GetMethod("SnapZoom", F).Invoke(view, new object[] { s });
}
view.Repaint();
var z = gvType.GetField("m_ZoomArea", F).GetValue(view);
return "game view " + (width == 0 ? "Free Aspect" : width + " x " + height) + " scale " + z.GetType().GetProperty("scale", F).GetValue(z) + " target in view " + gvType.GetProperty("targetInView", F).GetValue(view) + " window " + view.position.size;
