using System;
using UnityEngine.Serialization;

namespace GerrysJunkTrunk;

public static class InternalConfig
{
    private static Options _options;
    private static InternalConfigReader _con;

    public static void WriteOptions()
    {
        _con.UpdateValue("ShippingBoxBuilt", _options.shippingBoxBuilt.ToString());
        _con.UpdateValue("ShowIntroMessage", _options.showIntroMessage.ToString());
    }

    
    
    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new InternalConfigReader(external);

        bool.TryParse(_con.Value("ShippingBoxBuilt", "false"), out var shippingBoxBuilt);
        _options.shippingBoxBuilt = shippingBoxBuilt;

        bool.TryParse(_con.Value("ShowIntroMessage", "true"), out var showIntroMessage);
        _options.showIntroMessage = showIntroMessage;

        _con.ConfigWrite();

        return _options;
    }

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("ShippingBoxBuilt")] public bool shippingBoxBuilt;
        [FormerlySerializedAs("ShowIntroMessage")] public bool showIntroMessage;
    }
}