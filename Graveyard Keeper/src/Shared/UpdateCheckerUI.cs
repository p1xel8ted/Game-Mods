using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Shared;

internal class UpdateLabelPositioner : MonoBehaviour
{
    // GYK's NGUI UIRoot is in Flexible mode with activeHeight = Screen.height,
    // so widget-space localPosition maps 1:1 with screen pixels offset from the
    // panel center. Instead of positioning at the top-right corner minus a small
    // margin (which lands tight against pc2 banner territory), we place the
    // label midway between center and the top-right corner — specifically at
    // 49.5% of the half-extent. This was user-calibrated against (850, 355) at
    // Screen 3440x1440 (activeHeight 1440) and scales proportionally to any
    // resolution or aspect ratio.
    private const float PositionRatio = 0.495f;

    private UIRoot _root;

    private void LateUpdate()
    {
        if (!_root)
        {
            _root = GetComponentInParent<UIRoot>();
            if (!_root) return;
        }

        var h = _root.activeHeight;
        var aspect = Screen.height > 0 ? (float)Screen.width / Screen.height : 16f / 9f;
        var w = h * aspect;
        transform.localPosition = new Vector3(w / 2f * PositionRatio, h / 2f * PositionRatio, 0f);
    }
}

[Harmony]
internal static class UpdateCheckerUI
{
    private const string LabelName = "GYK_UpdateLabel";
    private const int MaxVisibleEntries = 10;

    private static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(UpdateChecker.LogSourceName);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open), typeof(bool))]
    public static void MainMenuGUI_Open_Postfix(MainMenuGUI __instance)
    {
        if (!__instance) return;
        Render(__instance);
    }

    // Called by UpdateCheckerCoordinator when a fetch completes after the menu is
    // already showing (avoids the user missing the label if they reach the menu
    // before the HTTP fetch returns).
    internal static void RefreshIfMenuOpen()
    {
        if (!GUIElements.me) return;
        var menu = GUIElements.me.main_menu;
        if (!menu || !menu.gameObject.activeInHierarchy) return;
        Render(menu);
    }

    private static void Render(MainMenuGUI mainMenu)
    {
        var outdated = UpdateChecker.GetOutdated();
        var parent = mainMenu.version_txt ? mainMenu.version_txt.transform.parent : mainMenu.transform;
        var existing = parent.Find(LabelName);

        if (outdated.Count == 0)
        {
            if (existing) existing.gameObject.SetActive(false);
            return;
        }

        // Rebuild from scratch each Render. Tiny cost (≤10 labels) and
        // eliminates stale-child / count-mismatch edge cases.
        if (existing)
        {
            UnityEngine.Object.DestroyImmediate(existing.gameObject);
        }

        BuildContainer(mainMenu, parent, outdated);
    }

    private static void BuildContainer(MainMenuGUI mainMenu, Transform parent, List<UpdateChecker.OutdatedEntry> outdated)
    {
        var reference = mainMenu.version_txt;
        if (!reference)
        {
            Log.LogWarning("version_txt missing — cannot build update label");
            return;
        }

        var container = new GameObject(LabelName) { layer = reference.gameObject.layer };
        container.transform.SetParent(parent, false);
        container.transform.localRotation = Quaternion.identity;
        container.transform.localScale = Vector3.one;
        container.AddComponent<UpdateLabelPositioner>();

        // Header (non-clickable).
        var header = CreateChildLabel(container, reference, BuildHeaderText(outdated.Count), reference.depth + 1);
        header.transform.localPosition = Vector3.zero;
        header.MakePixelPerfect();

        var lineHeight = (reference.fontSize > 0 ? reference.fontSize : 20) + 2;
        var y = -lineHeight;

        var visible = outdated.Count <= MaxVisibleEntries ? outdated.Count : MaxVisibleEntries - 1;
        for (var i = 0; i < visible; i++)
        {
            var e = outdated[i];
            var text = "[F7B000]" + e.ModName + "[-]  " + e.InstalledVersion + " \u2192 " + e.LatestVersion;
            var entry = CreateChildLabel(container, reference, text, reference.depth + 2);
            entry.transform.localPosition = new Vector3(0f, y, 0f);
            entry.MakePixelPerfect();

            if (!string.IsNullOrEmpty(e.NexusUrl))
            {
                // Whole-widget collider + closure-captured URL: the entire entry
                // label is the click target, not just glyphs inside a [url=] span.
                NGUITools.AddWidgetCollider(entry.gameObject);
                var url = e.NexusUrl;
                UIEventListener.Get(entry.gameObject).onClick = _ => Application.OpenURL(url);
            }

            y -= lineHeight;
        }

        if (outdated.Count > MaxVisibleEntries)
        {
            var more = CreateChildLabel(container, reference, "[707070]+ " + (outdated.Count - visible) + " more \u2014 see BepInEx log[-]", reference.depth + 2);
            more.transform.localPosition = new Vector3(0f, y, 0f);
            more.MakePixelPerfect();
        }
    }

    private static UILabel CreateChildLabel(GameObject parent, UILabel reference, string text, int depth)
    {
        var go = new GameObject("UpdateEntry") { layer = reference.gameObject.layer };
        go.transform.SetParent(parent.transform, false);
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        var label = go.AddComponent<UILabel>();
        if (reference.bitmapFont) label.bitmapFont = reference.bitmapFont;
        else if (reference.trueTypeFont) label.trueTypeFont = reference.trueTypeFont;
        label.fontSize = reference.fontSize;
        label.fontStyle = reference.fontStyle;
        label.pivot = UIWidget.Pivot.TopRight;
        label.alignment = NGUIText.Alignment.Right;
        label.overflowMethod = UILabel.Overflow.ResizeFreely;
        label.multiLine = false;
        label.supportEncoding = true;
        label.color = Color.white;
        label.depth = depth;
        label.text = text;
        return label;
    }

    private static string BuildHeaderText(int count)
    {
        return "[FF8040]" + count + (count == 1 ? " mod update available[-]" : " mod updates available[-]");
    }
}
