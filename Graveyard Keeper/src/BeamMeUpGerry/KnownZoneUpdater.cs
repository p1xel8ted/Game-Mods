namespace BeamMeUpGerry;

public class KnownZoneUpdater : MonoBehaviour
{
    private WorldGameObject _zoneWorldGameObject;
    private string _zoneConstant;

    public void Initialize(string zoneConstant)
    {
        _zoneConstant = zoneConstant;
        _zoneWorldGameObject = GetComponent<WorldGameObject>();

        //Check if the zone is already known
        if (MainGame.me.save.known_world_zones.Contains(_zoneConstant))
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (MainGame.me.save.known_world_zones.Contains(_zoneConstant))
        {
            Destroy(this);
            return;
        }

        var player = MainGame.me.player;
        if (player && Vector3.Distance(player.grid_pos, _zoneWorldGameObject.grid_pos) < 10f)
        {
            Plugin.Log.LogInfo($"Discovered {_zoneConstant}!");
            MainGame.me.save.known_world_zones.TryAdd(_zoneConstant);
        }
    }
}
