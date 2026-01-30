// Decompiled with JetBrains decompiler
// Type: MidasCaveController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MidasCaveController : MonoBehaviour
{
  public List<GameObject> Statues = new List<GameObject>();

  public void Start()
  {
  }

  public void ShakeStatues()
  {
    AudioManager.Instance.PlayOneShot("event:/dialogue/midas_statues/laugh_midas_statues", PlayerFarming.Instance.gameObject);
    foreach (GameObject statue in this.Statues)
    {
      if ((Object) statue != (Object) null)
        statue.transform.DOShakeScale(5f, new Vector3(0.0f, 0.2f), 5, 5f);
    }
  }
}
