// Decompiled with JetBrains decompiler
// Type: destroyMe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class destroyMe : BaseMonoBehaviour
{
  private float timer;
  public float deathtimer = 10f;

  private void OnEnable() => this.timer = 0.0f;

  private void OnDisable() => this.timer = 0.0f;

  private void Update()
  {
    this.timer += Time.deltaTime;
    if ((double) this.timer < (double) this.deathtimer)
      return;
    this.gameObject.Recycle();
  }
}
