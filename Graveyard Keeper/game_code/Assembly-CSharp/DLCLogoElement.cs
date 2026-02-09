// Decompiled with JetBrains decompiler
// Type: DLCLogoElement
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DLCLogoElement : MonoBehaviour
{
  [SerializeField]
  public DLCEngine.DLCVersion _dlc_version;
  [SerializeField]
  public GameObject _ampersand_go;

  public DLCEngine.DLCVersion dlc_version => this._dlc_version;

  public void Show(bool is_ampersand_to_show)
  {
    this._ampersand_go.SetActive(is_ampersand_to_show);
    this.gameObject.SetActive(true);
  }

  public void Hide() => this.gameObject.SetActive(false);
}
