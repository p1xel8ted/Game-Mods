using Il2CppBattleBrewCore.Localization;

namespace CuisineerTweaks;

public static class Lang
{
    internal static string GetReloadedConfigMessage()
    {
        return LanguageSettings.Language switch
        {
            LANGUAGE.EN => "Cuisineer Tweaks Configuration Reloaded!",
            LANGUAGE.ZHTW => "Cuisineer Tweaks 配置已重新載入！",
            LANGUAGE.ZHCN => "Cuisineer Tweaks 配置已重新加载！",
            LANGUAGE.JA => "Cuisineer Tweaks の設定が再読み込みされました！",
            LANGUAGE.KO => "Cuisineer Tweaks 구성이 다시 로드되었습니다!",
            LANGUAGE.DE => "Cuisineer Tweaks Konfiguration wurde neu geladen!",
            LANGUAGE.FR => "Cuisineer Tweaks Configuration rechargée!",
            LANGUAGE.ES => "¡Cuisineer Tweaks Configuración ha sido recargada!",
            LANGUAGE.BRPT => "Cuisineer Tweaks Configuração recarregada!",
            _ => "Cuisineer Tweaks Configuration Reloaded!"
        };
    }

    internal static string GetZoomAdjustedByMessage()
    {
        return LanguageSettings.Language switch
        {
            LANGUAGE.EN => "Zoom adjusted by",
            LANGUAGE.ZHTW => "縮放調整由",
            LANGUAGE.ZHCN => "缩放调整由",
            LANGUAGE.JA => "ズーム調整者",
            LANGUAGE.KO => "줌 조정자",
            LANGUAGE.DE => "Zoom angepasst von",
            LANGUAGE.FR => "Zoom ajusté par",
            LANGUAGE.ES => "Zoom ajustado por",
            LANGUAGE.BRPT => "Zoom ajustado por",
            _ => "Zoom adjusted by"
        };
    }
    
    internal static string GetZoomSetToMessage()
    {
        return LanguageSettings.Language switch
        {
            LANGUAGE.EN => "Zoom adjusted to",
            LANGUAGE.ZHTW => "縮放調整至",
            LANGUAGE.ZHCN => "缩放调整至",
            LANGUAGE.JA => "ズーム調整済み",
            LANGUAGE.KO => "줌 조정됨",
            LANGUAGE.DE => "Zoom angepasst auf",
            LANGUAGE.FR => "Zoom ajusté à",
            LANGUAGE.ES => "Zoom ajustado a",
            LANGUAGE.BRPT => "Zoom ajustado para",
            _ => "Zoom adjusted to"
        };
    }

}