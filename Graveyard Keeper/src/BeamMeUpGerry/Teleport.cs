namespace BeamMeUpGerry;

public static class Teleport
{
    internal static void TryTeleport(Location chosenLocation)
    {
        Helpers.Log($"[TryTeleport] {chosenLocation.zone}");
        var targetPosition = GetTeleportPosition(chosenLocation);
        if (targetPosition == Vector2.zero) return;

        MainGame.me.player.components.character.TeleportWithFade(targetPosition,
            middle_delegate: () => Helpers.UpdateEnvironmentPreset(chosenLocation),
            finished_delegate: () => PostPortingWork(chosenLocation));

        LogTeleportationDetails(chosenLocation, targetPosition);
    }

    private static void LogTeleportationDetails(Location chosenLocation, Vector2 targetPosition)
    {
        var logMessage = $"Teleporting to {chosenLocation.zone} at {targetPosition}";
        Helpers.Log(logMessage);
    }

    private static Vector2 GetTeleportPosition(Location chosenLocation)
    {
        // Log the initial information about the chosen location
        Helpers.Log($"[GetTeleportPosition] {chosenLocation.zone} {chosenLocation.teleportPoint} {chosenLocation.coords}");

        // If a teleport point is specified and valid, return its position
        if (!chosenLocation.teleportPoint.IsNullOrWhiteSpace())
        {
            var worldGameObject = WorldMap.GetWorldGameObjectByCustomTag(chosenLocation.teleportPoint);
            if (worldGameObject != null)
            {
                Helpers.Log($"[GetTeleportPosition] {worldGameObject.name} - {worldGameObject.grid_pos}");
                return worldGameObject.grid_pos;
            }
        }

        // If the chosen location has valid coordinates, return them
        if (chosenLocation.coords != Vector2.zero)
        {
            Helpers.Log($"[GetTeleportPosition] {chosenLocation.coords}");
            return chosenLocation.coords;
        }

        // As a fallback, use the player's current position
        Helpers.Log("[GetTeleportPosition] No valid grid position found. Using player's current position.");
        return MainGame.me.player.grid_pos;
    }



    private static void PostPortingWork(Location chosenLocation)
    {
        var gerryAppears = Plugin.GerryAppears.Value;
        var gerryCharges = Plugin.GerryCharges.Value;

        // If Gerry appears and charges are applicable, but not for default locations
        if (gerryAppears && gerryCharges && !chosenLocation.defaultLocation)
        {
            Helpers.SpawnGerry("", Vector3.zero, true);
            return;
        }

        // If Gerry appears, but no charges for default locations
        if (gerryAppears && chosenLocation.defaultLocation)
        {
            Helpers.SpawnGerry("", Vector3.zero);
            return;
        }

        // If Gerry doesn't appear, but charges are applicable and not for default locations
        if (!gerryAppears && gerryCharges && !chosenLocation.defaultLocation)
        {
            Helpers.TakeMoney(Helpers.MessagePositioning());
        }
    }
}