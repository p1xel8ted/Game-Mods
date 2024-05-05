namespace NoTimeForFishing;


public static class TranspilerMethods
{
    internal static bool MuseumChance(float chance)
    {
        if (!Plugin.IncreaseMuseumFishChance.Value) return Wish.Utilities.Chance(chance);
        
        var newChance = Wish.Utilities.Chance(Plugin.IncreaseMuseumFishChanceValue.Value);
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo($"Museum chance modified from {chance} to {Plugin.IncreaseMuseumFishChanceValue.Value}");
        }
        return newChance;
    }
    
    public static float GetPathMoveSpeed(float defaultSpeed, Collider2D collider, Bobber bobber)
    {
        const float baseMoveSpeed = 1.25f;
        var newSpeed = baseMoveSpeed;

        var message = $"\nOriginal base path move speed: {baseMoveSpeed}";

        message += $"\nPassed in default path move speed: {defaultSpeed}";

        if (Plugin.DoubleBaseFishSwimSpeed.Value)
        {
            newSpeed = baseMoveSpeed * 2f;
            message += $"\nNew base path move speed: {newSpeed}";
        }

        if (SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Professions[ProfessionType.Fishing]
            .GetNode("Fishing1b"))
        {
            newSpeed *= 1.3f;
            message += $"\nNew base path move speed (talented): {newSpeed}";
        }

        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogWarning(message);
        }

        return newSpeed;
    }

    internal static bool ModifyNibbleChance(float chance)
    {
        return Plugin.NibblingBehaviour.Value && Wish.Utilities.Chance(chance);
    }  
}