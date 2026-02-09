// Decompiled with JetBrains decompiler
// Type: MA_PlayerSpawnerControl
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using DarkTonic.MasterAudio;
using UnityEngine;

#nullable disable
public class MA_PlayerSpawnerControl : MonoBehaviour
{
  public GameObject Player;
  public float nextSpawnTime;

  public void Awake()
  {
    this.useGUILayout = false;
    this.nextSpawnTime = -1f;
  }

  public bool PlayerActive => this.Player.activeInHierarchy;

  public void Update()
  {
    if (this.PlayerActive)
      return;
    if ((double) this.nextSpawnTime < 0.0)
      this.nextSpawnTime = AudioUtil.Time + 1f;
    if ((double) Time.time < (double) this.nextSpawnTime)
      return;
    this.Player.SetActive(true);
    this.nextSpawnTime = -1f;
  }
}
