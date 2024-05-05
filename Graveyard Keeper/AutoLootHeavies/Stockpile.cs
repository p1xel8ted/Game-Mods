using UnityEngine;

namespace AutoLootHeavies;

public class Stockpile
{
    public Vector3 Location { get; }
    public StockpileType Type { get; }
    public float DistanceFromPlayer { get; set; }
    public WorldGameObject Wgo { get; }

    public Stockpile(Vector3 location, StockpileType type, float distanceFromPlayer, WorldGameObject wgo)
    {
        Location = location;
        Type = type;
        DistanceFromPlayer = distanceFromPlayer;
        Wgo = wgo;
    }

    public enum StockpileType
    {
        Timber,
        Ore,
        Stone,
        Unknown
    }
}