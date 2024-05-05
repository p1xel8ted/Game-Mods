namespace AddStraightToTable;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;
    
    public class Options
    {
        public bool HideInvalidSelections;
    }

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("HideInvalidSelections", "true"), out var hideInvalidSelections);
        _options.HideInvalidSelections = hideInvalidSelections;

        return _options;
    }
}