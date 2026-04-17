using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Shared;

internal class UpdateLabelPositioner : MonoBehaviour
{
    private const float MarginX = 20f;
    private const float MarginY = 20f;

    private UIRoot _root;

    private void LateUpdate()
    {
        if (!_root)
        {
            _root = NGUITools.FindInParents<UIRoot>(gameObject);
            if (!_root) return;
        }

        var h = _root.activeHeight;
        var aspect = Screen.height > 0 ? (float)Screen.width / Screen.height : 16f / 9f;
        var w = h * aspect;
        transform.localPosition = new Vector3(w / 2f - MarginX, h / 2f - MarginY, 0f);
    }
}

[Harmony]
internal static class UpdateCheckerUI
{
    private const string LabelName = "GYK_UpdateLabel";
    private const int MaxVisibleEntries = 10;

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

        UILabel label;
        if (existing)
        {
            label = existing.GetComponent<UILabel>();
            existing.gameObject.SetActive(true);
        }
        else
        {
            label = CreateLabel(mainMenu, parent);
            if (!label) return;
            UIEventListener.Get(label.gameObject).onClick = OnLabelClick;
        }

        label.text = BuildLabelText(outdated);
        label.MakePixelPerfect();
        NGUITools.AddWidgetCollider(label.gameObject);
    }

    private static UILabel CreateLabel(MainMenuGUI mainMenu, Transform parent)
    {
        var reference = mainMenu.version_txt;
        if (!reference) return null;

        var go = new GameObject(LabelName);
        go.layer = reference.gameObject.layer;
        go.transform.SetParent(parent, false);
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        var label = go.AddComponent<UILabel>();
        if (reference.bitmapFont)
        {
            label.bitmapFont = reference.bitmapFont;
        }
        else if (reference.trueTypeFont)
        {
            label.trueTypeFont = reference.trueTypeFont;
        }
        label.fontSize = reference.fontSize;
        label.fontStyle = reference.fontStyle;
        label.pivot = UIWidget.Pivot.TopRight;
        label.alignment = NGUIText.Alignment.Right;
        label.overflowMethod = UILabel.Overflow.ResizeFreely;
        label.multiLine = true;
        label.supportEncoding = true;
        label.color = Color.white;
        label.depth = reference.depth + 1;

        go.AddComponent<UpdateLabelPositioner>();
        return label;
    }

    private static string BuildLabelText(List<UpdateChecker.OutdatedEntry> outdated)
    {
        var sb = new StringBuilder();
        var count = outdated.Count;
        sb.Append("[FF8040]");
        sb.Append(count);
        sb.Append(count == 1 ? " mod update available[-]" : " mod updates available[-]");

        var visible = count <= MaxVisibleEntries ? count : MaxVisibleEntries - 1;
        for (var i = 0; i < visible; i++)
        {
            var e = outdated[i];
            sb.Append('\n');
            sb.Append("[F7B000]");
            sb.Append(e.ModName);
            sb.Append("[-]  ");
            sb.Append(e.InstalledVersion);
            sb.Append(" \u2192 ");
            if (!string.IsNullOrEmpty(e.NexusUrl))
            {
                sb.Append("[url=");
                sb.Append(e.NexusUrl);
                sb.Append(']');
                sb.Append(e.LatestVersion);
                sb.Append("[/url]");
            }
            else
            {
                sb.Append(e.LatestVersion);
            }
        }

        if (count > MaxVisibleEntries)
        {
            sb.Append("\n[707070]+ ");
            sb.Append(count - visible);
            sb.Append(" more \u2014 see BepInEx log[-]");
        }

        return sb.ToString();
    }

    private static void OnLabelClick(GameObject go)
    {
        var label = go ? go.GetComponent<UILabel>() : null;
        if (!label) return;
        var url = label.GetUrlAtPosition(UICamera.lastWorldPosition);
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
    }
}
