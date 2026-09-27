UnityEngine.Application.runInBackground = true;
var p = UnityEngine.GameObject.Find("Player");
var cc = p.GetComponent<UnityEngine.CharacterController>();
var sb = new System.Text.StringBuilder();

void Teleport(UnityEngine.Vector3 pos) { cc.enabled = false; p.transform.position = pos; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); }
float Drive(UnityEngine.Vector3 stepMove, int iterations, bool wiggle, out float maxY)
{
    maxY = p.transform.position.y;
    for (int i = 0; i < iterations; i++)
    {
        var m = stepMove;
        if (wiggle) m.x += (i % 40 < 20 ? 0.012f : -0.012f);   // sidestep back and forth while pushing
        cc.Move(m + UnityEngine.Vector3.down * 0.006f);
        if (p.transform.position.y > maxY) maxY = p.transform.position.y;
    }
    return p.transform.position.y;
}
float my;

// A. Tower flight 1: ground at x -1.9 on the north lane, push east 3.6 m.
Teleport(new UnityEngine.Vector3(-1.9f, 0f, 8.4f));
float yA = Drive(new UnityEngine.Vector3(0.012f, 0f, 0f), 300, false, out my);
sb.Append("flight1: end=" + p.transform.position.ToString("F2") + " (landing1 top=2.5) | ");

// B. Continue up flight 2 westward from landing 1.
Teleport(new UnityEngine.Vector3(2.0f, 2.5f, 7.0f));
float yB = Drive(new UnityEngine.Vector3(-0.012f, 0f, 0f), 330, false, out my);
sb.Append("flight2: end=" + p.transform.position.ToString("F2") + " (landing2 top=5.0) | ");

// C. Course stairs: from the ground at z 8.3 push north toward the platform.
Teleport(new UnityEngine.Vector3(-17f, 0f, 8.3f));
float yC = Drive(new UnityEngine.Vector3(0f, 0f, -0.012f), 250, false, out my);
sb.Append("courseStairs: end=" + p.transform.position.ToString("F2") + " (platform top=1.5) | ");

// D. Low step (0.4 m) approached from the south while sidestepping: must NOT climb.
Teleport(new UnityEngine.Vector3(-17f, 0f, -15.5f));
float yD = Drive(new UnityEngine.Vector3(0f, 0f, 0.012f), 400, true, out my);
sb.Append("stepWiggle: end=" + p.transform.position.ToString("F2") + " maxY=" + my.ToString("F3") + " (step top=0.4) | ");

// E. Log (0.4 m) same approach.
Teleport(new UnityEngine.Vector3(-17f, 0f, -11.5f));
float yE = Drive(new UnityEngine.Vector3(0f, 0f, 0.012f), 400, true, out my);
sb.Append("logWiggle: end=" + p.transform.position.ToString("F2") + " maxY=" + my.ToString("F3") + " | ");

// F. Course ramp still walkable from its foot.
Teleport(new UnityEngine.Vector3(-17f, 0f, -3.8f));
float yF = Drive(new UnityEngine.Vector3(0f, 0f, 0.012f), 600, false, out my);
sb.Append("courseRamp: end=" + p.transform.position.ToString("F2") + " (platform top=1.5)");

Teleport(new UnityEngine.Vector3(-10f, 0f, -10f));
return sb.ToString();
