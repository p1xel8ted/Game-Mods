namespace KeepersCandles;

internal static class LangDicts
{
    internal enum Messages
    {
        TooFar,
        NoneFound
    }

    private static readonly Dictionary<string, Dictionary<Messages, string>> LanguageDictionaries = new()
    {
        ["en"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "There is a lit candle nearby, but I can't reach it. Will have to move closer."},
            {Messages.NoneFound, "No lit candles found nearby."},
        },
        ["fr"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "Il y a une bougie allumée à proximité, mais je ne peux pas l'atteindre. Je vais devoir m'approcher."},
            {Messages.NoneFound, "Aucune bougie allumée trouvée à proximité."},
        },
        ["de"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "Eine brennende Kerze ist in der Nähe, aber ich kann sie nicht erreichen. Ich muss näher herangehen."},
            {Messages.NoneFound, "Keine brennenden Kerzen in der Nähe gefunden."},
        },
        ["zh-cn"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "附近有点燃的蜡烛，但我无法够到。需要靠近一些。"},
            {Messages.NoneFound, "附近没有发现点燃的蜡烛。"},
        },
        ["zh_cn"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "附近有点燃的蜡烛，但我无法够到。需要靠近一些。"},
            {Messages.NoneFound, "附近没有发现点燃的蜡烛。"},
        },
        ["es"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "Hay una vela encendida cerca, pero no puedo alcanzarla. Tendré que acercarme."},
            {Messages.NoneFound, "No se encontraron velas encendidas cerca."},
        },
        ["pt_br"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "Há uma vela acesa por perto, mas não consigo alcançá-la. Preciso me aproximar."},
            {Messages.NoneFound, "Nenhuma vela acesa encontrada nas proximidades."},
        },
        ["pt-br"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "Há uma vela acesa por perto, mas não consigo alcançá-la. Preciso me aproximar."},
            {Messages.NoneFound, "Nenhuma vela acesa encontrada nas proximidades."},
        },
        ["ko"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "근처에 켜진 양초가 있지만 닿을 수 없습니다. 더 가까이 가야 합니다."},
            {Messages.NoneFound, "근처에 켜진 양초가 없습니다."},
        },
        ["ja"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "近くに点灯したろうそくがありますが、届きません。もっと近づかなければなりません。"},
            {Messages.NoneFound, "近くに点灯したろうそくが見つかりません。"},
        },
        ["ru"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "Поблизости есть зажженная свеча, но я не могу до нее дотянуться. Надо подойти ближе."},
            {Messages.NoneFound, "Поблизости не найдено зажженных свечей."},
        },
        ["it"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "C'è una candela accesa nelle vicinanze, ma non riesco a raggiungerla. Dovrò avvicinarmi."},
            {Messages.NoneFound, "Nessuna candela accesa trovata nelle vicinanze."},
        },
        ["pl"] = new Dictionary<Messages, string>
        {
            {Messages.TooFar, "W pobliżu jest zapalona świeca, ale nie mogę jej dosięgnąć. Muszę się zbliżyć."},
            {Messages.NoneFound, "W pobliżu nie znaleziono zapalonych świec."},
        }
    };

    internal static string GetMessage(Messages msg)
    {
        var lang = GJL.GetCurLng();
        return LanguageDictionaries.TryGetValue(lang, out var dict) ? dict[msg] : LanguageDictionaries["en"][msg];
    }
}