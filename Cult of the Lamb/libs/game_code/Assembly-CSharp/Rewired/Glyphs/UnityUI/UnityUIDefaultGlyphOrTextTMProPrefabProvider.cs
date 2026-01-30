// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIDefaultGlyphOrTextTMProPrefabProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

public class UnityUIDefaultGlyphOrTextTMProPrefabProvider
{
  [RuntimeInitializeOnLoadMethod]
  public static void Initialize()
  {
    UnityUIControllerElementGlyphBase.defaultGlyphOrTextPrefabProvider = new Func<GameObject>(UnityUIDefaultGlyphOrTextTMProPrefabProvider.CreateDefaultGlyphOrTextPrefab);
  }

  public static GameObject CreateDefaultGlyphOrTextPrefab()
  {
    GameObject target = new GameObject("Glyph or text prefab");
    target.hideFlags = HideFlags.HideAndDontSave;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
    UnityUIGlyphOrTextTMPro glyphOrTextTmPro = target.AddComponent<UnityUIGlyphOrTextTMPro>();
    VerticalLayoutGroup verticalLayoutGroup = target.AddComponent<VerticalLayoutGroup>();
    verticalLayoutGroup.childControlHeight = true;
    verticalLayoutGroup.childControlWidth = true;
    verticalLayoutGroup.childForceExpandHeight = true;
    verticalLayoutGroup.childForceExpandWidth = true;
    GameObject gameObject1 = new GameObject("Glyph");
    gameObject1.hideFlags = HideFlags.HideAndDontSave;
    gameObject1.transform.SetParent(target.transform, false);
    Image image = gameObject1.AddComponent<Image>();
    image.preserveAspect = true;
    glyphOrTextTmPro.glyphComponent = image;
    GameObject gameObject2 = new GameObject("Text");
    gameObject2.hideFlags = HideFlags.HideAndDontSave;
    gameObject2.transform.SetParent(target.transform, false);
    TextMeshProUGUI textMeshProUgui = gameObject2.AddComponent<TextMeshProUGUI>();
    textMeshProUgui.alignment = TextAlignmentOptions.Center;
    textMeshProUgui.fontSize = 32f;
    textMeshProUgui.enableAutoSizing = true;
    textMeshProUgui.fontSizeMin = 10f;
    textMeshProUgui.fontSizeMax = 32f;
    textMeshProUgui.raycastTarget = false;
    glyphOrTextTmPro.textComponent = (TMP_Text) textMeshProUgui;
    return target;
  }
}
