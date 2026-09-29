using System;
using UnityEngine;

/// One checked location for the current day: SAFE, or an open CHECK. A CHECK does not
/// age; an event left unresolved costs what that event says (DailyLoop.md 3.5).
[Serializable]
public class LocationState
{
    [SerializeField] private LocationStatus status;

    public LocationStatus Status => status;
    public bool IsOpenCheck => status == LocationStatus.Anomaly;

    internal void Open() => status = LocationStatus.Anomaly;

    internal void Clear() => status = LocationStatus.Safe;
}
