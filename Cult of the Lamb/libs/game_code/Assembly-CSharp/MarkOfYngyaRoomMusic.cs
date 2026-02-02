// Decompiled with JetBrains decompiler
// Type: MarkOfYngyaRoomMusic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
    this.StartCoroutine((IEnumerator) this.WaitThenTriggerMusic());
  }

  public IEnumerator WaitThenTriggerMusic()
  {
    yield return (object) new WaitForSeconds(this.waitBeforeMusicHack);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.YngyaShrine);
  }
}
