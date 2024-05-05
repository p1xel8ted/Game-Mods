using UnityEngine;

namespace BeamMeUpGerry;

internal class Location
{
    public string Zone { get; }
    public string Preset { get; }
    public Vector3 Coords { get; }
    public EnvironmentEngine.State State { get; }

    public Location(string zone, string preset, Vector3 coords, EnvironmentEngine.State state = EnvironmentEngine.State.RealTime)
    {
        Zone = zone;
        Preset = preset;
        Coords = coords;
        State = state;
    }
}