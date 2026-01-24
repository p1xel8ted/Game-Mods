// Decompiled with JetBrains decompiler
// Type: MarkOfYngyaRoomMusic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
