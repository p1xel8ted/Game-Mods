namespace Fabledom;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.fabledom.tweaks";
    private const string PluginName = "Fabledom Tweaks";
    private const string PluginVer = "0.1.0";

    /// <summary>The native 16:9 aspect ratio.</summary>
    internal const float NativeAspect = 16f / 9f;

    /// <summary>Current display aspect ratio, updated on scene load.</summary>
    internal static float CurrentAspect { get; private set; }

    internal static ManualLogSource Log { get; private set; }

    // Performance settings
    internal static ConfigEntry<int> TargetFrameRate { get; private set; }
    internal static ConfigEntry<float> VfxRenderDistance { get; private set; }
    internal static ConfigEntry<float> AnimRenderDistance { get; private set; }
    internal static ConfigEntry<float> WorldTextRenderDistance { get; private set; }
    internal static ConfigEntry<float> WalkSpeedMultiplier { get; private set; }
    internal static ConfigEntry<WorkerOptimizeTrigger> WorkerOptimizeMode { get; private set; }
    internal static ConfigEntry<float> SeasonLengthMultiplier { get; private set; }
    internal static ConfigEntry<float> DayLengthMultiplier { get; private set; }
    // internal static ConfigEntry<KeyCode> StorageBrowserHotkey { get; private set; }
    internal static ConfigEntry<bool> AutoAssignImmigrants { get; private set; }
    internal static ConfigEntry<bool> BottleneckAlertEnabled { get; private set; }
    internal static ConfigEntry<bool> OptimizeNotifications { get; private set; }
    internal static float BaseDeltaTimeInDay { get; set; }
    internal static ConfigEntry<bool> ImmigrationEnabled { get; private set; }
    private static int StuckWorkerIndex;
    internal static string PendingRelocatePath;
    internal static GameObject PendingRelocateOriginal;
    private static GameObject RelocateGhost;

    // URP Pipeline settings
    private static ConfigEntry<float> ShadowDistance { get; set; }
    private static ConfigEntry<int> ShadowCascades { get; set; }
    private static ConfigEntry<int> Msaa { get; set; }
    private static ConfigEntry<bool> SoftShadows { get; set; }
    private static ConfigEntry<int> ShadowResolution { get; set; }

    // QualitySettings
    private static ConfigEntry<float> LodBias { get; set; }
    private static ConfigEntry<int> VSyncCount { get; set; }
    private static ConfigEntry<int> TextureQuality { get; set; }
    private static ConfigEntry<AnisotropicFiltering> AnisotropicFilter { get; set; }

    private static UniversalRenderPipelineAsset UrpAsset => QualitySettings.renderPipeline as UniversalRenderPipelineAsset;

    private void Awake()
    {
        Log = Logger;

        Debug.unityLogger.logEnabled = false;

        CurrentAspect = (float) Display.main.systemWidth / Display.main.systemHeight;

        // 02. Performance
        TargetFrameRate = Config.Bind("02. Performance", "Frame Rate Limit", 0,
            new ConfigDescription("Limit the frame rate. 0 = unlimited. Common values: 30, 60, 120, 144, 165, 240.",
                new AcceptableValueRange<int>(0, 1000),
                new ConfigurationManagerAttributes { Order = 98 }));
        TargetFrameRate.SettingChanged += (_, _) => ApplyFrameRate();

        VfxRenderDistance = Config.Bind("02. Performance", "VFX Render Distance", 150f,
            new ConfigDescription("Maximum distance for visual effects to render. Lower = better performance.",
                new AcceptableValueRange<float>(50f, 1000f),
                new ConfigurationManagerAttributes { Order = 97 }));
        VfxRenderDistance.SettingChanged += (_, _) => ApplyRenderDistances();

        AnimRenderDistance = Config.Bind("02. Performance", "Animation Render Distance", 150f,
            new ConfigDescription("Maximum distance for character animations to render. Lower = better performance.",
                new AcceptableValueRange<float>(50f, 1000f),
                new ConfigurationManagerAttributes { Order = 96 }));
        AnimRenderDistance.SettingChanged += (_, _) => ApplyRenderDistances();

        WorldTextRenderDistance = Config.Bind("02. Performance", "World Text Render Distance", 100f,
            new ConfigDescription("Maximum distance for world-space text to render. Lower = better performance.",
                new AcceptableValueRange<float>(25f, 600f),
                new ConfigurationManagerAttributes { Order = 95 }));
        WorldTextRenderDistance.SettingChanged += (_, _) => ApplyRenderDistances();

        // 03. Quality — URP Pipeline
        var urp = UrpAsset;

        ShadowDistance = Config.Bind("03. Quality", "Shadow Distance", urp ? urp.shadowDistance : 150f,
            new ConfigDescription("Maximum distance shadows are rendered. Lower = better performance.",
                new AcceptableValueRange<float>(25f, 1000f),
                new ConfigurationManagerAttributes { Order = 88 }));
        ShadowDistance.SettingChanged += (_, _) => ApplyUrpSettings();

        ShadowCascades = Config.Bind("03. Quality", "Shadow Cascades", urp ? urp.shadowCascadeCount : 4,
            new ConfigDescription("Number of shadow cascade splits. More = better shadow quality near camera, lower performance.",
                new AcceptableValueRange<int>(1, 4),
                new ConfigurationManagerAttributes { Order = 87 }));
        ShadowCascades.SettingChanged += (_, _) => ApplyUrpSettings();

        Msaa = Config.Bind("03. Quality", "Anti-Aliasing (MSAA)", urp ? urp.msaaSampleCount : 1,
            new ConfigDescription("Multi-sample anti-aliasing. Higher = smoother edges, lower performance. 1 = off.",
                new AcceptableValueList<int>(1, 2, 4, 8),
                new ConfigurationManagerAttributes { Order = 86 }));
        Msaa.SettingChanged += (_, _) => ApplyUrpSettings();

        SoftShadows = Config.Bind("03. Quality", "Soft Shadows", urp && urp.supportsSoftShadows,
            new ConfigDescription("Enables softer, smoother shadow edges. Slight performance cost.",
                null,
                new ConfigurationManagerAttributes { Order = 85 }));
        SoftShadows.SettingChanged += (_, _) => ApplyUrpSettings();

        ShadowResolution = Config.Bind("03. Quality", "Shadow Resolution", urp ? urp.mainLightShadowmapResolution : 2048,
            new ConfigDescription("Shadow map resolution. Higher = sharper shadows, more GPU memory.",
                new AcceptableValueList<int>(256, 512, 1024, 2048, 4096),
                new ConfigurationManagerAttributes { Order = 84 }));
        ShadowResolution.SettingChanged += (_, _) => ApplyUrpSettings();

        // 04. Quality — QualitySettings
        LodBias = Config.Bind("04. Quality Settings", "LOD Bias", QualitySettings.lodBias,
            new ConfigDescription("Controls when objects switch to lower detail models. Higher = more detail at distance.",
                new AcceptableValueRange<float>(0.25f, 8f),
                new ConfigurationManagerAttributes { Order = 79 }));
        LodBias.SettingChanged += (_, _) =>
        {
            RoundToStep(LodBias);
            ApplyQualitySettings();
        };

        VSyncCount = Config.Bind("04. Quality Settings", "VSync", QualitySettings.vSyncCount,
            new ConfigDescription("Vertical sync. 0 = off (uncapped), 1 = every frame, 2 = every other frame. Reduces screen tearing.",
                new AcceptableValueRange<int>(0, 4),
                new ConfigurationManagerAttributes { Order = 78 }));
        VSyncCount.SettingChanged += (_, _) => ApplyQualitySettings();

        TextureQuality = Config.Bind("04. Quality Settings", "Texture Quality", QualitySettings.masterTextureLimit,
            new ConfigDescription("Texture resolution. 0 = full, 1 = half, 2 = quarter, 3 = eighth. Lower = less memory.",
                new AcceptableValueRange<int>(0, 3),
                new ConfigurationManagerAttributes { Order = 77 }));
        TextureQuality.SettingChanged += (_, _) => ApplyQualitySettings();

        AnisotropicFilter = Config.Bind("04. Quality Settings", "Anisotropic Filtering", QualitySettings.anisotropicFiltering,
            new ConfigDescription("Improves texture clarity at steep viewing angles. ForceEnable applies to all textures.",
                null,
                new ConfigurationManagerAttributes { Order = 76 }));
        AnisotropicFilter.SettingChanged += (_, _) => ApplyQualitySettings();

        // 05. Gameplay
        WalkSpeedMultiplier = Config.Bind("05. Gameplay", "Walk Speed Multiplier", 1f,
            new ConfigDescription("Multiplier for villager movement speed. Higher = faster villagers. Does not affect game speed.",
                new AcceptableValueRange<float>(0.5f, 5f),
                new ConfigurationManagerAttributes { Order = 69 }));
        WalkSpeedMultiplier.SettingChanged += (_, _) => RoundToStep(WalkSpeedMultiplier);

        WorkerOptimizeMode = Config.Bind("05. Gameplay", "Auto-Optimize Workers", WorkerOptimizeTrigger.Disabled,
            new ConfigDescription("When to auto-optimize worker assignments to minimize commute distance. F6 always works as manual trigger.",
                null,
                new ConfigurationManagerAttributes { Order = 66 }));

        SeasonLengthMultiplier = Config.Bind("05. Gameplay", "Season Length Multiplier", 1f,
            new ConfigDescription("Multiplier for days per season. 2 = double the days, 0.5 = half the days.",
                new AcceptableValueRange<float>(0.5f, 5f),
                new ConfigurationManagerAttributes { Order = 68 }));
        SeasonLengthMultiplier.SettingChanged += (_, _) => RoundToStep(SeasonLengthMultiplier);

        DayLengthMultiplier = Config.Bind("05. Gameplay", "Day Length Multiplier", 1f,
            new ConfigDescription("Multiplier for how long each day lasts in real time. 2 = days last twice as long.",
                new AcceptableValueRange<float>(0.5f, 5f),
                new ConfigurationManagerAttributes { Order = 67 }));
        DayLengthMultiplier.SettingChanged += (_, _) =>
        {
            RoundToStep(DayLengthMultiplier);
            var dtm = DateTimeManager.Instance;
            if (dtm) dtm.deltaTimeInDay = BaseDeltaTimeInDay * DayLengthMultiplier.Value;
        };

        // StorageBrowserHotkey = Config.Bind("05. Gameplay", "Storage Browser Hotkey", KeyCode.F8,
        //     new ConfigDescription("Hotkey to open the storage browser overlay.",
        //         null,
        //         new ConfigurationManagerAttributes { Order = 66 }));

        AutoAssignImmigrants = Config.Bind("05. Gameplay", "Auto-Assign Immigrants", true,
            new ConfigDescription("Automatically assign new immigrants to the nearest vacant job matching their class.",
                null,
                new ConfigurationManagerAttributes { Order = 65 }));

        BottleneckAlertEnabled = Config.Bind("05. Gameplay", "Production Bottleneck Alerts", false,
            new ConfigDescription("Log alerts once per day for production buildings that are idle, understaffed, or missing resources.",
                null,
                new ConfigurationManagerAttributes { Order = 64 }));

        OptimizeNotifications = Config.Bind("05. Gameplay", "Worker Optimize Notifications", true,
            new ConfigDescription("Show in-game notification when worker optimization completes.",
                null,
                new ConfigurationManagerAttributes { Order = 63 }));

        ImmigrationEnabled = Config.Bind("05. Gameplay", "Immigration Enabled", true,
            new ConfigDescription("Allow new settlers to arrive. Toggle with F11.",
                null,
                new ConfigurationManagerAttributes { Order = 62 }));

        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded! Resolution: {Display.main.systemWidth}x{Display.main.systemHeight}, Aspect: {CurrentAspect:F4}, IsWider: {CurrentAspect > NativeAspect}");
    }

    private void Update()
    {
        var state = GameManager.Instance.GetMainState();

        // Relocate deposit: ghost follows cursor, click to place
        if (PendingRelocatePath != null && state == MainState.PLAYING)
        {
            // Spawn ghost on first frame
            if (!RelocateGhost)
            {
                RelocateGhost = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, PendingRelocatePath);
                RelocateGhost.SetActive(true);
                // Disable all colliders and scripts so ghost doesn't interact with game
                foreach (var col in RelocateGhost.GetComponentsInChildren<Collider>())
                {
                    col.enabled = false;
                }
                foreach (var mb in RelocateGhost.GetComponentsInChildren<MonoBehaviour>())
                {
                    mb.enabled = false;
                }
                // Make ghost semi-transparent where possible
                foreach (var renderer in RelocateGhost.GetComponentsInChildren<Renderer>())
                {
                    foreach (var mat in renderer.materials)
                    {
                        if (mat.HasProperty("_Color"))
                        {
                            var color = mat.color;
                            color.a = 0.5f;
                            mat.color = color;
                        }
                    }
                }
            }

            // Move ghost to cursor position
            var canPlace = false;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var ghostHit, float.PositiveInfinity, PlacementManager.Instance.placeableLayers))
            {
                var gx = Mathf.RoundToInt(ghostHit.point.x / 10f) * 10f;
                var gz = Mathf.RoundToInt(ghostHit.point.z / 10f) * 10f;
                RelocateGhost.transform.position = new Vector3(gx, ghostHit.point.y, gz);

                var origGridObj = PendingRelocateOriginal.GetComponent<Nielsen.GridObject>();
                var occupied = GridManager.ObjectGrid.GetAllOccupiedCellsCoveredByObject(RelocateGhost.transform.position, origGridObj);
                canPlace = occupied.Count == 0;
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                // Cancel — restore original, destroy ghost
                foreach (var renderer in PendingRelocateOriginal.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = true;
                }
                Object.Destroy(RelocateGhost);
                RelocateGhost = null;
                Log.LogInfo("Relocate: cancelled, original restored");
                PendingRelocatePath = null;
                PendingRelocateOriginal = null;
            }
            else if (Input.GetMouseButtonDown(0) && canPlace && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                var placementPos = RelocateGhost.transform.position;

                // Destroy ghost
                Object.Destroy(RelocateGhost);
                RelocateGhost = null;

                // Destroy original (re-enable renderers first so destroy effects work)
                foreach (var renderer in PendingRelocateOriginal.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = true;
                }
                var destroyable = PendingRelocateOriginal.GetComponent<Nielsen.Destroyable>();
                if (destroyable)
                {
                    destroyable.DestroyWoImmidiateWithEffects();
                }

                // Spawn new at target position
                var spawned = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, PendingRelocatePath);
                spawned.transform.position = placementPos;
                spawned.SetActive(true);

                var gridObj = spawned.GetComponent<Nielsen.GridObject>();
                if (gridObj)
                {
                    gridObj.AddToGrid();
                }

                // Deduct coins
                if (!GameManager.Instance.IsSandboxMode(false))
                {
                    var cost = KingdomManager.Instance.kingdomConfig.resourceDestroyCost * 2;
                    Utils_Resources.RemoveCoins(cost);
                }

                Log.LogInfo($"Relocate: placed at {placementPos}");
                PendingRelocatePath = null;
                PendingRelocateOriginal = null;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.F5) && state is MainState.PLAYING or MainState.PAUSE)
        {
            SaveMaster.WriteActiveSaveToDisk();
            Log.LogInfo("Quick save triggered (F5)");
        }

        if (Input.GetKeyDown(KeyCode.F6) && state == MainState.PLAYING)
        {
            StartCoroutine(WorkerOptimizer.OptimizeCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.F7) && state == MainState.PLAYING)
        {
            Patches.RunBottleneckCheck();
        }

        if (Input.GetKeyDown(KeyCode.F10) && state == MainState.PLAYING)
        {
            var stuck = Nielsen.Worker.Instances.Where(w => w && w.unit && w.unit.ai && w.unit.ai.isStuck).ToList();
            if (stuck.Count == 0)
            {
                Log.LogInfo("No stuck workers found");
            }
            else
            {
                StuckWorkerIndex = StuckWorkerIndex % stuck.Count;
                var worker = stuck[StuckWorkerIndex];
                GameplayCameraManager.Instance.controller.ForcePositionTransition(worker.transform.position, 0.5f);
                Log.LogInfo($"Stuck worker {StuckWorkerIndex + 1}/{stuck.Count}: {worker.unit.GetLocalizedBirthName()}");
                StuckWorkerIndex = (StuckWorkerIndex + 1) % stuck.Count;
            }
        }

        if (Input.GetKeyDown(KeyCode.F11) && state == MainState.PLAYING)
        {
            ImmigrationEnabled.Value = !ImmigrationEnabled.Value;
            WorkerOptimizer.ShowModNotification(ImmigrationEnabled.Value
                ? "Immigration enabled"
                : "Immigration paused");
        }
    }

    private void OnGUI()
    {
    }

    private static void SceneManagerOnSceneLoaded(Scene a, LoadSceneMode l)
    {
        CurrentAspect = (float) Display.main.systemWidth / Display.main.systemHeight;
        Patches.UpdateScalers(CurrentAspect);
        ApplyFrameRate();
        ApplyRenderDistances();
        ApplyUrpSettings();
        ApplyQualitySettings();
    }

    private static void ApplyFrameRate()
    {
        Application.targetFrameRate = TargetFrameRate.Value <= 0 ? -1 : TargetFrameRate.Value;
    }

    private static void ApplyRenderDistances()
    {
        if (GameManager.Instance.GetMainState() != MainState.PLAYING) return;

        var cam = GameplayCameraManager.Instance;
        if (!cam)
        {
            return;
        }
        cam.vfxDistance = VfxRenderDistance.Value;
        cam.animDistance = AnimRenderDistance.Value;
        cam.worldTextDistance = WorldTextRenderDistance.Value;
    }

    private static void ApplyUrpSettings()
    {
        var urp = UrpAsset;
        if (!urp) return;
        
        urp.shadowDistance = ShadowDistance.Value;
        QualitySettings.shadowDistance = ShadowDistance.Value;
        urp.shadowCascadeCount = ShadowCascades.Value;
        QualitySettings.shadowCascades = ShadowCascades.Value;
        urp.msaaSampleCount = Msaa.Value;
        QualitySettings.antiAliasing = Msaa.Value;
        urp.supportsSoftShadows = SoftShadows.Value;
        urp.mainLightShadowmapResolution = ShadowResolution.Value;
    }

    private static void RoundToStep(ConfigEntry<float> entry, float step = 0.1f)
    {
        var rounded = Mathf.Round(entry.Value / step) * step;
        if (!Mathf.Approximately(entry.Value, rounded))
        {
            entry.Value = rounded;
        }
    }

    private static void ApplyQualitySettings()
    {
        QualitySettings.lodBias = LodBias.Value;
        QualitySettings.vSyncCount = VSyncCount.Value;
        QualitySettings.masterTextureLimit = TextureQuality.Value;
        QualitySettings.anisotropicFiltering = AnisotropicFilter.Value;
    }
}
