namespace BringOutYerDead;

public static class SaveData
{
    private static Data _data;
    private static SaveDataReader _con;

    private const string Morning = "MorningDelivery";
    private const string Day = "DayDelivery";
    private const string Evening = "EveningDelivery";
    private const string Night = "NightDelivery";
    private const string DonkeySpawnedCon = "DonkeySpawned";

    public static void WriteOptions()
    {
        _con.UpdateValue(Morning, _data.MorningDelivery.ToString());
        _con.UpdateValue(Day, _data.DayDelivery.ToString());
        _con.UpdateValue(Evening, _data.EveningDelivery.ToString());
        _con.UpdateValue(Night, _data.NightDelivery.ToString());
        _con.UpdateValue(DonkeySpawnedCon, _data.DonkeySpawned.ToString());
 
    }

    public static Data GetData()
    {
        _data = new Data();
        _con = new SaveDataReader();

        bool.TryParse(_con.Value(Morning, "false"), out var morningDelivery);
        _data.MorningDelivery = morningDelivery;

        bool.TryParse(_con.Value(Day, "false"), out var dayDelivery);
        _data.DayDelivery = dayDelivery;

        bool.TryParse(_con.Value(Evening, "false"), out var eveningDelivery);
        _data.EveningDelivery = eveningDelivery;

        bool.TryParse(_con.Value(Night, "false"), out var nightDelivery);
        _data.NightDelivery = nightDelivery;
        
        bool.TryParse(_con.Value(DonkeySpawnedCon, "false"), out var donkeySpawned);
        _data.DonkeySpawned = donkeySpawned;

        _con.ConfigWrite();

        return _data;
    }

    public class Data
    {
        public bool MorningDelivery;
        public bool DayDelivery;
        public bool EveningDelivery;
        public bool NightDelivery;
        public bool DonkeySpawned;
    }
}