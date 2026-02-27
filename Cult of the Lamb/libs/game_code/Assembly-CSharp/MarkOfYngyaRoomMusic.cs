// Decompiled with JetBrains decompiler
// Type: MarkOfYngyaRoomMusic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class MarkOfYngyaRoomMusic : BaseMonoBehaviour
{
  public float waitBeforeMusicHack = 0.5f;

  public void OnEnable()
  {
    if (PlayerFarming.Location != FollowerLocation.Dungeon1_5 && PlayerFarming.Location != FollowerLocation.Dungeon1_6)
      return;
    this.StartCoroutine(this.WaitThenTriggerMusic());
  }

  public IEnumerator WaitThenTriggerMusic()
  {
    yield return (object) new WaitForSeconds(this.waitBeforeMusicHack);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.YngyaShrine);
  }
}
