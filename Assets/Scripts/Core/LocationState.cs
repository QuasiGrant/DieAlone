using System;
using UnityEngine;

/// One checked location for the current day: SAFE, or an open CHECK at some stage.
/// Stage 1 is the day the CHECK appears; it rises by one each night it stays open.
[Serializable]
public class LocationState
{
    [SerializeField] private LocationStatus status;
    [SerializeField] private int stage;

    public LocationStatus Status => status;
    public int Stage => stage;
    public bool IsOpenCheck => status == LocationStatus.Anomaly;

    internal void Open()
    {
        status = LocationStatus.Anomaly;
        stage = 1;
    }

    internal void Advance() => stage++;

    internal void Clear()
    {
        status = LocationStatus.Safe;
        stage = 0;
    }
}
