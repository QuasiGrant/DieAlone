UnityEngine.Application.runInBackground = true;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
var cc = pc.GetComponent<UnityEngine.CharacterController>();
pc.enabled = false;
var V2 = new System.Func<float, float, UnityEngine.Vector2>((x, z) => new UnityEngine.Vector2(x, z));
var routes = new System.Collections.Generic.Dictionary<string, UnityEngine.Vector2[]> {
  { "cabin_to_trunk", new[] { V2(245f,186f), V2(240f,185f), V2(225f,195f), V2(200f,200f), V2(175f,225f), V2(140f,215f), V2(120f,235f), V2(128f,260f) } },
};
var sb = new System.Text.StringBuilder();
foreach (var kv in routes)
{
    var pts = kv.Value;
    cc.enabled = false; pc.transform.position = new UnityEngine.Vector3(pts[0].x, UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>().SampleHeight(new UnityEngine.Vector3(pts[0].x, 0f, pts[0].y)) + 1.2f, pts[0].y); cc.enabled = true; UnityEngine.Physics.SyncTransforms();
    for (int k = 0; k < 40; k++) cc.Move(UnityEngine.Vector3.down * 0.2f);
    int stalls = 0; float maxY = -999f, minY = 999f; string firstStall = "";
    for (int i = 1; i < pts.Length; i++)
    {
        var target = pts[i]; int steps = 0;
        while (steps < 4000)
        {
            var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z);
            var d = target - flat; if (d.magnitude < 0.5f) break;
            var before = flat;
            var dir = d.normalized * 0.08f;
            cc.Move(new UnityEngine.Vector3(dir.x, -0.2f, dir.y));
            var after = new UnityEngine.Vector2(pc.transform.position.x, pc.transform.position.z);
            if ((after - before).magnitude < 0.01f) { stalls++; if (firstStall == "") firstStall = p.ToString("F1") + " toward " + target; }
            maxY = UnityEngine.Mathf.Max(maxY, pc.transform.position.y); minY = UnityEngine.Mathf.Min(minY, pc.transform.position.y);
            steps++;
        }
        if (steps >= 4000) { sb.Append(kv.Key + ": STUCK at " + pc.transform.position.ToString("F1") + " toward " + target + "\n"); break; }
    }
    var end = pc.transform.position;
    sb.Append(kv.Key + ": end=" + end.ToString("F1") + " stalls=" + stalls + (firstStall != "" ? " first=" + firstStall : "") + " y[" + minY.ToString("F1") + "," + maxY.ToString("F1") + "]\n");
}
pc.enabled = true;
return sb.ToString();
