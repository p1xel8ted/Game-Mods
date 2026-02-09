// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CreditsContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
