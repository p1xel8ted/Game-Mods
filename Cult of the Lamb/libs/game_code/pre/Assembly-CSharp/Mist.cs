// Decompiled with JetBrains decompiler
// Type: Mist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Mist : BaseMonoBehaviour
{
  private int dir = 1;

  private void Start() => this.dir = (double) Random.Range(0.0f, 1f) <= 0.5 ? -1 : 1;

  private void Update()
  {
    this.transform.Translate(new Vector3(0.2f * (float) this.dir * Time.deltaTime, 0.0f, 0.0f));
  }
}
