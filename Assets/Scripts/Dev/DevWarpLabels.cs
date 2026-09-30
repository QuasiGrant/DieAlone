#if UNITY_EDITOR || DEVELOPMENT_BUILD
/// Plain names and route order for the dev panel's warp rows (Pim, Check2_UI.md sections 1 and 3).
/// Keys are DevWarps child names. The FirstWarp row goes above every group; the other known warps follow
/// group by group in route order; warps not listed here keep their own order and a name made readable by DevMenu.
/// Dev-only: the whole file compiles out of release builds.
public static class DevWarpLabels
{
    public const string FirstWarp = "Ward";
    public const string FirstLabel = "Ward: stones on the ledge";
    public const string OtherGroup = "OTHER";

    public struct Row
    {
        public string Group, Name, Label;
        public Row(string group, string name, string label) { Group = group; Name = name; Label = label; }
    }

    /// Route order, top to bottom.
    public static readonly Row[] Rows =
    {
        new Row("TOWER AND CAMP", "Keepers_Camp", "Keeper's camp (tower foot)"),
        new Row("TOWER AND CAMP", "Cabin", "Cabin (wake spot)"),
        new Row("TOWER AND CAMP", "Tower_Deck", "Tower deck"),
        new Row("WARD CLIMB", "Junction_J", "Ward trail start (cairn)"),
        new Row("WARD CLIMB", "Ward_P3", "Ward climb, third bend"),
        new Row("WARD CLIMB", "Ward_P4", "Ward climb, top of the legs"),
        new Row("LAKE", "Lake_Pump", "Lake pump"),
        new Row("LAKE", "Lake_Boathouse", "Boathouse"),
        new Row("LAKE", "Junction_W1", "Lake west fork (Camp 3, cave)"),
        new Row("CAMPS", "Camp_1", "Camp 1"),
        new Row("CAMPS", "Camp_2", "Camp 2"),
        new Row("CAMPS", "Camp_2_Top", "Camp 2, top of the stack"),
        new Row("CAMPS", "Camp_3", "Camp 3"),
        new Row("CAMPS", "Camp_3_Rim", "Above Camp 3"),
        new Row("FRONT", "Office", "Office"),
        new Row("FRONT", "Store", "Store"),
        new Row("FRONT", "Gate_Booth", "Gate booth"),
        new Row("FRONT", "Trailhead_T", "Parking lot trailhead"),
        new Row("FRONT", "Closed_Campground", "Closed campground"),
        new Row("FRONT", "Old_Burn", "Old burn"),
        new Row("FRONT", "Junction_Jg", "Burn fork (to Camp 1 and the lot)"),
        new Row("CAVE", "Cave_Mouth", "Cave mouth"),
        new Row("CAVE", "Cave_Chamber", "Cave chamber"),
    };

    public static bool IsListed(string warpName)
    {
        if (warpName == FirstWarp) return true;
        foreach (var r in Rows) if (r.Name == warpName) return true;
        return false;
    }
}
#endif
