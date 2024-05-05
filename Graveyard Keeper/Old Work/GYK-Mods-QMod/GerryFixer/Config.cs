namespace GerryFixer;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);
        
        bool.TryParse(_con.Value("AttemptToFixCutsceneGerrys", "false"), out var attemptToFixOtherGerrys);
        _options.AttemptToFixCutsceneGerrys = attemptToFixOtherGerrys;

        bool.TryParse(_con.Value("SpawnTavernCellarGerry", "false"), out var spawnTavernCellarGerry);
        _options.SpawnTavernCellarGerry = spawnTavernCellarGerry;

        bool.TryParse(_con.Value("SpawnTavernMorgueGerry", "false"), out var spawnTavernMorgueGerry);
        _options.SpawnTavernMorgueGerry = spawnTavernMorgueGerry;
        
        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.Debug = debug;
        
        _con.ConfigWrite();

        return _options;
    }



    public class Options
    {
        public bool SpawnTavernCellarGerry;
        public bool SpawnTavernMorgueGerry;
        public bool AttemptToFixCutsceneGerrys;
        public bool Debug;
    }
}