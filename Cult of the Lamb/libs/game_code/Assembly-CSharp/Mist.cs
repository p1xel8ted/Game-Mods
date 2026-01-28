// Decompiled with JetBrains decompiler
// Type: Mist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
