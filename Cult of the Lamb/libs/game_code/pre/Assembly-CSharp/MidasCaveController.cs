// Decompiled with JetBrains decompiler
// Type: MidasCaveController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MidasCaveController : MonoBehaviour
{
  public List<GameObject> Statues = new List<GameObject>();

  public void ShakeStatues()
  {
    AudioManager.Instance.PlayOneShot("event:/dialogue/midas_statues/laugh_midas_statues", PlayerFarming.Instance.gameObject);
    foreach (GameObject statue in this.Statues)
      statue.transform.DOShakeScale(5f, new Vector3(0.0f, 0.2f), 5, 5f);
  }
}
