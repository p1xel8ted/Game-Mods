// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIControllerElementGlyphBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

public abstract class UnityUIControllerElementGlyphBase : ControllerElementGlyphBase
{
  public static GameObject s_defaultGlyphOrTextPrefab;
  public static Func<GameObject> s_defaultGlyphOrTextPrefabProvider;

  public override GameObject GetDefaultGlyphOrTextPrefab()
  {
    return UnityUIControllerElementGlyphBase.defaultGlyphOrTextPrefab;
  }

  public static GameObject defaultGlyphOrTextPrefab
  {
    get
    {
      return !((UnityEngine.Object) UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefab != (UnityEngine.Object) null) ? (UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefab = UnityUIControllerElementGlyphBase.CreateDefaultGlyphOrTextPrefab()) : UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefab;
    }
    set => UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefab = value;
  }

  public static Func<GameObject> defaultGlyphOrTextPrefabProvider
  {
    get => UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefabProvider;
    set => UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefabProvider = value;
  }

  public static GameObject CreateDefaultGlyphOrTextPrefab()
  {
    if (UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefabProvider != null)
      return UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefabProvider();
    GameObject target = new GameObject("Glyph or text prefab");
    target.hideFlags = HideFlags.HideAndDontSave;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
    UnityUIGlyphOrText unityUiGlyphOrText = target.AddComponent<UnityUIGlyphOrText>();
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
    unityUiGlyphOrText.glyphComponent = image;
    GameObject gameObject2 = new GameObject("Text");
    gameObject2.hideFlags = HideFlags.HideAndDontSave;
    gameObject2.transform.SetParent(target.transform, false);
    Text text = gameObject2.AddComponent<Text>();
    text.alignment = TextAnchor.MiddleCenter;
    text.fontSize = 32 /*0x20*/;
    text.resizeTextForBestFit = true;
    text.resizeTextMinSize = 10;
    text.resizeTextMaxSize = 32 /*0x20*/;
    text.font = UnityEngine.Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    text.raycastTarget = false;
    unityUiGlyphOrText.textComponent = text;
    return target;
  }
}
