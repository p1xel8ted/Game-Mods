using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Shared;

// Holds cross-mod debug-warning registrations on a single named GameObject so every mod's
// linked copy of this file sees the same registry. Without this the statics would be
// per-assembly and the "one dialog for all mods" behaviour wouldn't work.
internal class DebugWarningRegistry : MonoBehaviour
{
    private const string HostObjectName = "~DebugWarningRegistry";
    private static DebugWarningRegistry _instance;

    internal readonly Dictionary<string, Func<bool>> Registrations = new();
    internal bool DialogShown;

    internal static DebugWarningRegistry Instance
    {
        get
        {
            if (_instance != null) return _instance;
            var go = GameObject.Find(HostObjectName);
            if (go == null)
            {
                go = new GameObject(HostObjectName);
                DontDestroyOnLoad(go);
            }
            _instance = go.GetComponent<DebugWarningRegistry>() ?? go.AddComponent<DebugWarningRegistry>();
            return _instance;
        }
    }
}

// Call DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled) in Awake.
// The first mod's copy of this file to be loaded creates the shared registry GameObject;
// every mod shares it at runtime via GameObject.Find.
public static class DebugWarningDialog
{
    public static void Register(string pluginName, Func<bool> isDebugEnabled)
    {
        if (string.IsNullOrEmpty(pluginName) || isDebugEnabled == null) return;
        DebugWarningRegistry.Instance.Registrations[pluginName] = isDebugEnabled;
    }
}

[Harmony]
internal static class DebugWarningDialogPatch
{
    // Inline translations rather than JSON files so the shared source drop-in keeps zero
    // filesystem coupling. Falls back to English on any unrecognised language code.
    // {0} is substituted with the bulleted list of mod names.
    private static readonly Dictionary<string, (string Title, string Template)> Translations = new()
    {
        ["en"] = ("Debug Logging",
            "Debug Logging is ON for:\n\n{0}\n\nVerbose diagnostics are being written to the BepInEx console. Turn it off in each mod's settings once you're done troubleshooting."),
        ["de"] = ("Debug-Protokollierung",
            "Debug-Protokollierung ist AN für:\n\n{0}\n\nAusführliche Diagnosen werden in die BepInEx-Konsole geschrieben. Schalte sie in den Einstellungen jedes Mods aus, wenn du mit der Fehlersuche fertig bist."),
        ["es"] = ("Registro de depuración",
            "El Registro de depuración está ACTIVO para:\n\n{0}\n\nSe están escribiendo diagnósticos detallados en la consola de BepInEx. Desactívalo en los ajustes de cada mod cuando termines de solucionar problemas."),
        ["fr"] = ("Journalisation de débogage",
            "La journalisation de débogage est ACTIVE pour :\n\n{0}\n\nDes diagnostics détaillés sont écrits dans la console BepInEx. Désactive-la dans les paramètres de chaque mod une fois le dépannage terminé."),
        ["it"] = ("Log di debug",
            "Il Log di debug è ATTIVO per:\n\n{0}\n\nDiagnostiche dettagliate vengono scritte nella console di BepInEx. Disattivalo nelle impostazioni di ciascun mod una volta finito di cercare problemi."),
        ["ja"] = ("デバッグログ",
            "デバッグログが有効になっているMOD:\n\n{0}\n\n詳細な診断情報がBepInExコンソールに出力されています。トラブルシューティングが終わったら、各MODの設定で無効にしてください。"),
        ["ko"] = ("디버그 로깅",
            "디버그 로깅이 켜져 있는 모드:\n\n{0}\n\nBepInEx 콘솔에 자세한 진단 정보가 기록되고 있습니다. 문제 해결이 끝나면 각 모드의 설정에서 꺼 주세요."),
        ["pl"] = ("Logowanie debugowania",
            "Logowanie debugowania jest WŁĄCZONE dla:\n\n{0}\n\nSzczegółowa diagnostyka jest zapisywana do konsoli BepInEx. Wyłącz je w ustawieniach każdego moda, gdy skończysz rozwiązywać problemy."),
        ["pt-br"] = ("Registro de depuração",
            "O Registro de depuração está LIGADO para:\n\n{0}\n\nDiagnósticos detalhados estão sendo gravados no console do BepInEx. Desligue nas configurações de cada mod quando terminar de resolver problemas."),
        ["ru"] = ("Отладочное логирование",
            "Отладочное логирование ВКЛЮЧЕНО для:\n\n{0}\n\nПодробная диагностика записывается в консоль BepInEx. Отключите его в настройках каждого мода, когда закончите устранение неполадок."),
        ["zh-cn"] = ("调试日志",
            "以下模组的调试日志已开启：\n\n{0}\n\n详细的诊断信息正在写入 BepInEx 控制台。排查问题完成后，请在各个模组的设置中将其关闭。"),
    };

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open), typeof(bool))]
    public static void MainMenuGUI_Open_ShowDebugWarning()
    {
        var registry = DebugWarningRegistry.Instance;
        if (registry.DialogShown) return;
        if (GUIElements.me == null || GUIElements.me.dialog == null) return;

        var enabled = registry.Registrations
            .Where(kv =>
            {
                try { return kv.Value(); }
                catch { return false; }
            })
            .Select(kv => kv.Key)
            .OrderBy(name => name)
            .ToList();

        if (enabled.Count == 0) return;

        registry.DialogShown = true;

        var list = string.Join("\n", enabled.Select(n => $"\u2022 {n}").ToArray());
        var (title, template) = GetTranslation();
        var message = string.Format(template, list);

        GUIElements.me.dialog.OpenOK(title, null, message, true, string.Empty);
    }

    private static (string Title, string Template) GetTranslation()
    {
        var lang = GameSettings._cur_lng ?? string.Empty;
        lang = lang.ToLowerInvariant().Replace('_', '-');
        return Translations.TryGetValue(lang, out var t) ? t : Translations["en"];
    }
}
