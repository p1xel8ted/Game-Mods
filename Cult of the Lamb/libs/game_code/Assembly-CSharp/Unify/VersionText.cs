// Decompiled with JetBrains decompiler
// Type: Unify.VersionText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Unify;

public class VersionText : MonoBehaviour
{
  public static string versionString;
  public static string librarySemanticVersionString;

  public void Awake() => this.InitVersionStrings();

  public void Start()
  {
    this.InitVersionStrings();
    Text component1 = this.GetComponent<Text>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.text = component1.text.Replace("{version}", VersionText.versionString);
      component1.text = component1.text.Replace("{librarySemanticVersion}", VersionText.librarySemanticVersionString);
    }
    TextMeshProUGUI component2 = this.GetComponent<TextMeshProUGUI>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    component2.text = component2.text.Replace("{version}", VersionText.versionString);
    component2.text = component2.text.Replace("{librarySemanticVersion}", VersionText.librarySemanticVersionString);
  }

  public void InitVersionStrings()
  {
    if (VersionText.versionString == null)
    {
      TextAsset textAsset = (TextAsset) UnityEngine.Resources.Load("version");
      if ((UnityEngine.Object) textAsset != (UnityEngine.Object) null)
      {
        string[] strArray = textAsset.text.Split('\n', StringSplitOptions.None);
        string str1 = "(null)";
        string str2 = "";
        if (strArray.Length != 0)
          str1 = strArray[0].Trim();
        if (strArray.Length > 1)
          str2 = " / " + strArray[1].Trim();
        VersionText.versionString = str1 + str2;
        Debug.Log((object) ("Game Version: " + VersionText.versionString));
      }
    }
    if (VersionText.librarySemanticVersionString != null)
      return;
    if (UnifyManager.Instance != null)
    {
      VersionText.librarySemanticVersionString = UnifyManager.Instance.GetLibrarySemanticVersion();
      Debug.Log((object) ("Unify Library Semantic Version: " + VersionText.librarySemanticVersionString));
    }
    else
      Debug.LogWarning((object) "Unify VersionText.Awake(): UnifyManager not ready.");
  }
}
