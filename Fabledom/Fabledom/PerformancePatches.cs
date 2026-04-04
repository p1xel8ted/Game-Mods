using Pathfinding;

namespace Fabledom;

/// <summary>
/// Performance optimizations via Harmony patches.
/// Targets hot-path inefficiencies found in decompiled game code.
/// </summary>
[Harmony]
public static class PerformancePatches
{
    private sealed class AstarAgentSpeedCache
    {
        public Vector2Int Tile;
        public float MaxSpeed;
        public bool HasValue;
    }

    private sealed class AudioManagerCache
    {
        public float LastCameraHeight;
        public bool HasCameraHeight;
        public Vector3 LastEmitterPosition;
        public bool HasEmitterPosition;
    }

    private sealed class MissionPanelTextCache
    {
        public bool IsActiveMission;
        public int DaysLeft;
        public int ExpirationDays;
        public string DisplayText;
        public bool HasValue;
    }

    // ═══════════════════════════════════════════════════════════════
    // 1. CrowdAnimationController — Replace LINQ.First() with Dictionary
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Cache for crowd animation lookups. Replaces LINQ.First() in SetTrigger().
    /// Key: (animationInfoKey, triggerKey) → CrowdAnimationInfo.
    /// </summary>
    private static readonly Dictionary<(string, string), CrowdAnimationInfo> CrowdAnimationCache = new();
    private static readonly Dictionary<int, AstarAgentSpeedCache> AstarAgentSpeedCaches = new();
    private static readonly Dictionary<int, AudioManagerCache> AudioManagerCaches = new();
    private static readonly Dictionary<int, MissionPanelTextCache> MissionPanelCaches = new();

    /// <summary>
    /// Intercepts SetTrigger to use cached dictionary lookup instead of LINQ.First().
    /// The original does: animationInfos[key].crowdAnimationInfos.First(x => x.key == key)
    /// which allocates an enumerator on every call.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CrowdAnimationController), nameof(CrowdAnimationController.SetTrigger))]
    public static bool CrowdAnimationController_SetTrigger(CrowdAnimationController __instance, string key, string rightItem, string leftItem, float speed, bool skipStop)
    {
        if (!__instance.isImplemented || __instance.inspectorDebug) return false;

        if (!skipStop)
        {
            __instance.Stop();
        }

        var cacheKey = (__instance.animationInfoKey, key);
        if (!CrowdAnimationCache.TryGetValue(cacheKey, out var info))
        {
            var animInfos = CrowdAnimationClipManager.Instance.animationInfos[__instance.animationInfoKey].crowdAnimationInfos;
            foreach (var animInfo in animInfos)
            {
                if (animInfo.key == key)
                {
                    info = animInfo;
                    break;
                }
            }

            if (info == null)
            {
                Debug.LogError("Could not find animationInfo: " + key);
                return false;
            }

            CrowdAnimationCache[cacheKey] = info;
        }

        __instance.activeAnimation = info;
        try
        {
            __instance.crowdPrefab.StartAnimation(info.GetClip(), speed: speed, transitionTime: 0.25f);
        }
        catch
        {
            Debug.LogError("Could not start clip for animationInfo: " + key);
        }

        __instance.ActivateItems(rightItem, leftItem);
        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // 2. GrassManager — Camera movement threshold
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Replaces GrassManager.Update() with a version that uses sqrMagnitude threshold
    /// instead of exact position comparison. The original triggers chunk recalculation
    /// on every sub-pixel camera movement.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GrassManager), nameof(GrassManager.Update))]
    public static bool GrassManager_Update(GrassManager __instance)
    {
        if (__instance.grassQuality == GrassQuality.VERYLOW) return false;

        var camPos = __instance.currentCamera.transform.position;
        var delta = camPos - __instance.prevFrameCameraPos;

        // Only recalculate when camera has moved more than 0.5 units (avoids per-pixel updates)
        if (delta.sqrMagnitude > 0.25f)
        {
            __instance.prevFrameCameraPos = camPos;
            __instance.cameraPos = __instance.GetCameraToChunckPosition(camPos);
            if (__instance.cameraPos != __instance.prevCameraPos)
            {
                __instance.prevCameraPos = __instance.cameraPos;
                __instance.DeactivateOldChuncks();
                __instance.SetActiveNeighbours(__instance.cameraPos);
            }
        }

        if (__instance.changingGround)
        {
            __instance.GetGroundTexture();
        }

        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // 3. IndirectInstanceManager — Null guard on Update
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Adds null guards to avoid NullReferenceException when mesh, material, or
    /// argsBuffer haven't been initialized yet. The original has no guards.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(IndirectInstanceManager), nameof(IndirectInstanceManager.Update))]
    public static bool IndirectInstanceManager_Update(IndirectInstanceManager __instance)
    {
        if (__instance.mesh == null || __instance.material == null || __instance.argsBuffer == null) return false;

        Graphics.DrawMeshInstancedIndirect(
            __instance.mesh, 0, __instance.material, __instance.bounds,
            __instance.argsBuffer, castShadows: __instance.castingMode);

        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // 4. ToggleVfxCameraDistance — sqrMagnitude + state tracking
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Replaces the VFX distance culling Update with optimized version:
    /// - Uses sqrMagnitude instead of Distance (avoids sqrt)
    /// - Computes distance once instead of twice (original checks in both if/else branches)
    /// - Uses the existing isOutOfRange field for state tracking
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ToggleVfxCameraDistance), nameof(ToggleVfxCameraDistance.Update))]
    public static bool ToggleVfxCameraDistance_Update(ToggleVfxCameraDistance __instance)
    {
        __instance.currentFrame++;
        if (__instance.currentFrame < 30) return false;
        __instance.currentFrame = 0;

        var cameraPos = GameplayCameraManager.Instance.controller.cameraTransform.position;
        var sqrDist = (__instance.transform.position - cameraPos).sqrMagnitude;
        var maxDist = GameplayCameraManager.Instance.vfxDistance;
        var outOfRange = sqrDist > maxDist * maxDist;

        if (outOfRange == __instance.isOutOfRange) return false;

        __instance.isOutOfRange = outOfRange;
        var active = !outOfRange;
        for (var i = 0; i < __instance.vfxToggles.Count; i++)
        {
            __instance.vfxToggles[i].SetActive(active);
        }

        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // 5. NavigationManager — Cache NNConstraint and bitmask
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Cached NNConstraint with pre-built tag bitmask.
    /// The original creates a new NNConstraint and rebuilds the 32-bit tag mask
    /// on every call to GetNearestWalkablePosition/Node.
    /// </summary>
    private static NNConstraint _cachedWalkableConstraint;

    private static NNConstraint GetCachedWalkableConstraint()
    {
        if (_cachedWalkableConstraint != null) return _cachedWalkableConstraint;

        _cachedWalkableConstraint = NNConstraint.Default;
        _cachedWalkableConstraint.constrainTags = true;

        var mask = 0;
        for (var i = 0; i < 32; i++)
        {
            if (i != 11 && i != 12)
            {
                mask |= 1 << i;
            }
        }

        _cachedWalkableConstraint.tags = mask;
        return _cachedWalkableConstraint;
    }

    /// <summary>
    /// Replaces GetNearestWalkablePosition with cached constraint version.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NavigationManager), nameof(NavigationManager.GetNearestWalkablePosition))]
    public static bool NavigationManager_GetNearestWalkablePosition(ref Vector3 __result, Vector3 origin)
    {
        __result = AstarPath.active.GetNearest(origin, GetCachedWalkableConstraint()).position;
        return false;
    }

    /// <summary>
    /// Replaces GetNearestWalkableNode with cached constraint version.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NavigationManager), nameof(NavigationManager.GetNearestWalkableNode))]
    public static bool NavigationManager_GetNearestWalkableNode(ref GraphNode __result, Vector3 origin)
    {
        __result = AstarPath.active.GetNearest(origin, GetCachedWalkableConstraint()).node;
        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // 6. AstarAgent — Cache speed multiplier by tile
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Replaces UpdateSpeedMultiplier() with a version that reuses the previous
    /// road-speed result while the agent remains on the same tile.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AstarAgent), nameof(AstarAgent.UpdateSpeedMultiplier))]
    public static bool AstarAgent_UpdateSpeedMultiplier(AstarAgent __instance)
    {
        __instance.agent.maxSpeed = __instance.baseSpeed;

        var pos = __instance.transform.position;
        var tile = new Vector2Int(Mathf.RoundToInt(pos.x / 10f), Mathf.RoundToInt(pos.z / 10f));
        var instanceId = __instance.GetInstanceID();

        // Road speed is tile-based, so repeating the grid lookup while the agent
        // remains on the same tile just burns CPU for the same result.
        if (AstarAgentSpeedCaches.TryGetValue(instanceId, out var cache) && cache.HasValue && cache.Tile == tile)
        {
            __instance.agent.maxSpeed = cache.MaxSpeed;
            return false;
        }

        cache ??= new AstarAgentSpeedCache();
        cache.Tile = tile;
        cache.HasValue = true;
        cache.MaxSpeed = __instance.baseSpeed;

        try
        {
            var cellAtWorldPos = GridManager.ObjectGrid.GetCellAtWorldPos(pos, GridLayer.ROAD);
            if (cellAtWorldPos != null && cellAtWorldPos.IsOccupied())
            {
                if (cellAtWorldPos.id == 16)
                {
                    cache.MaxSpeed = __instance.baseSpeed * __instance.movementConfig.gravelRoadMultiplier;
                }
                else if (cellAtWorldPos.id == 90 || cellAtWorldPos.id == 236)
                {
                    cache.MaxSpeed = __instance.baseSpeed * __instance.movementConfig.cobbleRoadMultiplier;
                }
                else if (cellAtWorldPos.id == 62)
                {
                    cache.MaxSpeed = __instance.baseSpeed * __instance.movementConfig.woodenBridgeMultiplier;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Something went wrong setting speed multiplier (pos: {__instance.transform.position} go: {__instance.gameObject.name}): {ex}");
        }

        __instance.agent.maxSpeed = cache.MaxSpeed;
        AstarAgentSpeedCaches[instanceId] = cache;
        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // 7. AudioManager — Skip redundant global updates
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Avoids pushing the same camera-height RTPC value every frame when the
    /// camera distance has not materially changed.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AudioManager), nameof(AudioManager.UpdateCameraHeight))]
    public static bool AudioManager_UpdateCameraHeight(AudioManager __instance)
    {
        if (!GameplayCameraManager.Instance)
        {
            return false;
        }

        var cache = GetAudioManagerCache(__instance);
        var height = GameplayCameraManager.Instance.controller.distanceDown;
        // Wwise RTPC writes are global engine calls, so skipping identical values
        // avoids needless per-frame work when the camera height is effectively unchanged.
        if (cache.HasCameraHeight && Mathf.Abs(cache.LastCameraHeight - height) < 0.01f)
        {
            return false;
        }

        cache.LastCameraHeight = height;
        cache.HasCameraHeight = true;
        __instance.cameraHeight_rtpc.SetGlobalValue(height);
        return false;
    }

    /// <summary>
    /// Avoids iterating shoreline emitters when the listener position has not changed.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AudioManager), nameof(AudioManager.UpdateEnvironmentalEmiitersPositions))]
    public static bool AudioManager_UpdateEnvironmentalEmiitersPositions(AudioManager __instance, Vector3 virtualListenerPosition)
    {
        var cache = GetAudioManagerCache(__instance);
        // The original loops all shoreline emitters every frame even if the listener
        // has not moved, so reusing the previous position avoids redundant emitter updates.
        if (cache.HasEmitterPosition && cache.LastEmitterPosition == virtualListenerPosition)
        {
            return false;
        }

        cache.LastEmitterPosition = virtualListenerPosition;
        cache.HasEmitterPosition = true;
        for (var i = 0; i < __instance.waterBodies.Count; i++)
        {
            __instance.waterBodies[i].UpdateEmitterPosition(virtualListenerPosition);
        }

        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // 8. WorldMapMissionPanel — Cache timer text
    // ═══════════════════════════════════════════════════════════════

    /// <summary>
    /// Avoids rebuilding mission timer text every frame when the visible value
    /// has not changed.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldMapMissionPanel), nameof(WorldMapMissionPanel.Update))]
    public static bool WorldMapMissionPanel_Update(WorldMapMissionPanel __instance)
    {
        if (__instance.saveableMission == null || __instance.expireTimer == null)
        {
            return false;
        }

        var cache = GetMissionPanelCache(__instance);
        var isActiveMission = __instance.isActiveMission;
        var daysLeft = __instance.saveableMission.daysLeft;
        var expirationDays = __instance.saveableMission.expirationDays;

        // This panel rebuilds localized strings every frame, but the visible timer
        // only changes when the underlying day counters change.
        if (!cache.HasValue || cache.IsActiveMission != isActiveMission || cache.DaysLeft != daysLeft || cache.ExpirationDays != expirationDays)
        {
            cache.IsActiveMission = isActiveMission;
            cache.DaysLeft = daysLeft;
            cache.ExpirationDays = expirationDays;
            cache.DisplayText = BuildMissionTimerText(__instance);
            cache.HasValue = true;
            __instance.expireTimer.text = cache.DisplayText;
        }

        return false;
    }

    private static AudioManagerCache GetAudioManagerCache(AudioManager instance)
    {
        var instanceId = instance.GetInstanceID();
        if (AudioManagerCaches.TryGetValue(instanceId, out var cache))
        {
            return cache;
        }

        cache = new AudioManagerCache();
        AudioManagerCaches[instanceId] = cache;
        return cache;
    }

    private static MissionPanelTextCache GetMissionPanelCache(WorldMapMissionPanel instance)
    {
        var instanceId = instance.GetInstanceID();
        if (MissionPanelCaches.TryGetValue(instanceId, out var cache))
        {
            return cache;
        }

        cache = new MissionPanelTextCache();
        MissionPanelCaches[instanceId] = cache;
        return cache;
    }

    private static string BuildMissionTimerText(WorldMapMissionPanel instance)
    {
        if (instance.isActiveMission)
        {
            return $"{instance.saveableMission.daysLeft} {Utils_Text.GetLocalizedString("HARD_DAYS_REMAINING").ToLower()}";
        }

        if (instance.saveableMission.expirationDays == -99)
        {
            return Utils_Text.GetLocalizedString("NO_EXPIRATION");
        }

        return string.Format(Utils_Text.GetLocalizedString("EXPIRES_IN_DAYS"), instance.saveableMission.expirationDays);
    }

}
