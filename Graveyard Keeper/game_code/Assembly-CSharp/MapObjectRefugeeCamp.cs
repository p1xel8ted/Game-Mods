// Decompiled with JetBrains decompiler
// Type: MapObjectRefugeeCamp
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MapObjectRefugeeCamp : MonoBehaviour
{
  [SerializeField]
  public GameObject _content;

  public void Awake()
  {
    this.gameObject.SetActive(true);
    this._content.gameObject.SetActive(false);
  }

  public void OnEnable()
  {
    if (!this.CheckIsTavernBuild())
      return;
    this._content.SetActive(true);
  }

  public void OnDisable() => this._content.SetActive(false);

  public bool CheckIsTavernBuild() => (double) MainGame.me.player.GetParam("camp_is_live") != 0.0;
}
