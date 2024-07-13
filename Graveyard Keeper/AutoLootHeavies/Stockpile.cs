namespace AutoLootHeavies;

public class Stockpile(Vector3 location, Stockpile.StockpileType type, float distanceFromPlayer, WorldGameObject wgo)
{
    public Vector3 Location { get; } = location;
    public StockpileType Type { get; } = type;
    public float DistanceFromPlayer { get; set; } = distanceFromPlayer;
    public WorldGameObject Wgo { get; } = wgo;

    public enum StockpileType
    {
        Timber,
        Ore,
        Stone,
        Unknown
    }
}