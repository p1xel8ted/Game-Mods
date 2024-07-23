using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BeamMeUpGerry;

[Serializable]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal class Location
{
    private const string Locations = "Locations";
    [NonSerialized] public bool enabled;

    /// <summary>
    /// Is this a default location normally in the port menu?
    /// </summary>
    /// <value>
    ///  <c>true</c> if default location; otherwise, <c>false</c>.
    /// </value>
    [NonSerialized] public bool defaultLocation;

    /// <summary>
    /// Gets the zone name. Use 'zone_name' from the game for proper translations.
    /// </summary>
    /// <value>
    /// The name of the zone.
    /// </value>
    public string zone;

    /// <summary>
    /// Gets the environment preset. This controls aspects like lighting and weather.
    /// </summary>
    /// <value>
    /// The environment preset.
    /// </value>
    public string preset;

    /// <summary>
    /// Gets the teleport point. Prefer this over <see cref="coords"/> if available.
    /// </summary>
    /// <value>
    /// The built-in teleport point.
    /// </value>
    [NonSerialized] public string teleportPoint;

    /// <summary>
    /// Gets the coordinates. Used if there's no predefined teleport point.
    /// </summary>
    /// <value>
    /// The coordinates for the location.
    /// </value>
    public Vector2 coords;

    /// <summary>
    /// The state of the environment, serialized as a string.
    /// </summary>
    public EnvironmentEngine.State state;

    public bool customZone;

    public Location()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> class.
    /// </summary>
    /// <param name="zone">The zone name.</param>
    /// <param name="preset">The environment preset.</param>
    /// <param name="teleportPoint">The teleport point.</param>
    /// <param name="coords">The coordinates.</param>
    /// <param name="defaultLocation">Is this a default location normally in the port menu?</param>
    /// <param name="state">The environment state, with a default of RealTime.</param>
    public Location(string zone, string preset, string teleportPoint, Vector2 coords, bool defaultLocation = false, EnvironmentEngine.State state = EnvironmentEngine.State.RealTime)
    {
        this.zone = zone;
        this.preset = preset;
        this.teleportPoint = teleportPoint;
        this.coords = coords;
        this.state = state;
        this.defaultLocation = defaultLocation;
        customZone = false;
    }

    internal static string GetSavePath()
    {
        // Get the name of the current assembly this code is running in
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        // Strip special characters/spaces/etc. from assembly name as it's going to be used in a system path
        assemblyName = Regex.Replace(assemblyName, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        // Get the folder assemblyName is in
        var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var returnPath = assemblyFolder != null ? Path.Combine(assemblyFolder, Locations) : Path.Combine(Paths.PluginPath, assemblyName, Locations);
        return returnPath;
    }

    public void SaveJson()
    {
        var json = JsonUtility.ToJson(this, true);
        var path = GetSavePath();

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        var fileName = $"{zone}_{DateTime.Now:HH_mm_ss_dd_MM_yyyy}.json";
        if (zone.IsNullOrWhiteSpace())
        {
            fileName = $"no_zone_{DateTime.Now:HH_mm_ss_dd_MM_yyyy}.json";
        }

        var saveLocation = Path.Combine(path, fileName);
        try
        {
            File.WriteAllText(saveLocation, json);
            if (!Plugin.OpenNewLocationFileOnSave.Value) return;
            var startInfo = new ProcessStartInfo
            {
                FileName = saveLocation,
                UseShellExecute = true,
                CreateNoWindow = false
            };
            Process.Start(startInfo);
        }
        catch (Exception e)
        {
            Plugin.Log.LogError(e);
        }
    }

    public static Location LoadFromJson(string path)
    {
        if (!File.Exists(path))
        {
            Plugin.Log.LogError($"Location.LoadFromJson - File {path} does not exist!");
            return new Location();
        }

        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<Location>(json);
    }
}