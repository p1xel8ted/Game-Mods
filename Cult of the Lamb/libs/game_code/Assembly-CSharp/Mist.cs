// Decompiled with JetBrains decompiler
// Type: Mist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Mist : BaseMonoBehaviour
{
  public int dir = 1;

  public void Start() => this.dir = (double) Random.Range(0.0f, 1f) <= 0.5 ? -1 : 1;

  public void Update()
  {
    this.transform.Translate(new Vector3(0.2f * (float) this.dir * Time.deltaTime, 0.0f, 0.0f));
  }
}
