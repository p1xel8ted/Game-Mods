// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CreditsContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class CreditsContent : MonoBehaviour
{
  [SerializeField]
  public GameObject[] _disableOnNonEnglish;

  public void OnEnable()
  {
    if (!(LocalizationManager.CurrentLanguage != "English"))
      return;
    foreach (GameObject gameObject in this._disableOnNonEnglish)
      gameObject.SetActive(false);
  }
}
