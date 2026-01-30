// Decompiled with JetBrains decompiler
// Type: ShowIfCoop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShowIfCoop : MonoBehaviour
{
  [SerializeField]
  public bool orIfGoat;

  public void OnEnable()
  {
    if (CoopManager.CoopActive || this.orIfGoat && PlayerFarming.players[0].IsGoat)
      return;
    this.gameObject.SetActive(false);
  }
}
