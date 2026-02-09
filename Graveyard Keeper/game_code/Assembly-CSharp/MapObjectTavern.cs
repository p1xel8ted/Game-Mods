// Decompiled with JetBrains decompiler
// Type: MapObjectTavern
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MapObjectTavern : MonoBehaviour
{
  [SerializeField]
  public GameObject _content;
  public const string _PLAYERS_TAVERN_CUSTOM_TAG = "players_tavern";

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

  public bool CheckIsTavernBuild()
  {
    return (Object) WorldMap.GetWorldGameObjectByCustomTag("players_tavern", true) != (Object) null;
  }
}
