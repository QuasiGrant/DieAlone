// Main3 8.6 check (Play mode): from inside the front zone, push the player east into the gate opening and the fence at
// several points, walking and with a jump's worth of lift each step, and report how far east each got (fence at x 396).
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var sb = new System.Text.StringBuilder(); bool pass = true;
foreach (var z in new[] { 168f, 170f, 172f, 20f, 100f, 150f, 200f, 239f, 290f })
    foreach (var jump in new[] { false, true })
    {
        cc.enabled = false; pc.transform.position = new UnityEngine.Vector3(392.5f, 3.3f, z); cc.enabled = true; UnityEngine.Physics.SyncTransforms();
        for (int k = 0; k < 10; k++) cc.Move(UnityEngine.Vector3.down * 0.1f);
        float maxX = 0f;
        for (int i = 0; i < 600; i++)
        {
            float up = jump && i % 40 < 8 ? 0.08f : -0.15f;   // hop about 0.6 m every 40 steps while pushing
            cc.Move(new UnityEngine.Vector3(0.05f, up, 0f)); maxX = UnityEngine.Mathf.Max(maxX, pc.transform.position.x);
        }
        if (maxX >= 396f) pass = false;
        sb.Append("z " + z + (jump ? " hop" : " walk") + ": max x " + maxX.ToString("F2") + "\n");
    }
pc.enabled = true;
return (pass ? "PASS: never reached x 396" : "FAIL: crossed x 396") + "\n" + sb;
