// Decompiled with JetBrains decompiler
// Type: Unify.VersionText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Unify;

public class VersionText : MonoBehaviour
{
  public static string versionString;
  private static string librarySemanticVersionString;

  public void Awake() => this.InitVersionStrings();

  public void Start()
  {
    this.InitVersionStrings();
    Text component1 = this.GetComponent<Text>();
    if ((Object) component1 != (Object) null)
    {
      component1.text = component1.text.Replace("{version}", VersionText.versionString);
      component1.text = component1.text.Replace("{librarySemanticVersion}", VersionText.librarySemanticVersionString);
    }
    TextMeshProUGUI component2 = this.GetComponent<TextMeshProUGUI>();
    if (!((Object) component2 != (Object) null))
      return;
    component2.text = component2.text.Replace("{version}", VersionText.versionString);
    component2.text = component2.text.Replace("{librarySemanticVersion}", VersionText.librarySemanticVersionString);
  }

  private void InitVersionStrings()
  {
    if (VersionText.versionString == null)
    {
      TextAsset textAsset = (TextAsset) UnityEngine.Resources.Load("version");
      if ((Object) textAsset != (Object) null)
      {
        VersionText.versionString = textAsset.text;
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
