// Decompiled with JetBrains decompiler
// Type: GhostRitualCircle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GhostRitualCircle : BaseMonoBehaviour
{
  [SerializeField]
  public float rotationSpeed = 10f;
  [SerializeField]
  public float radius = 5f;
  [SerializeField]
  public float ZWobbleFrequency = 2f;
  [SerializeField]
  public float ZWobbleAmplitude = 0.5f;
  [SerializeField]
  public bool invertFacing;

  public void Update()
  {
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      Transform child = this.transform.GetChild(index);
      float f = (float) ((double) Time.time * (double) this.rotationSpeed + (double) index * 3.1415927410125732 * 2.0 / (double) this.transform.childCount);
      child.localPosition = (new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f) * this.radius) with
      {
        z = Mathf.Sin(Time.time * this.ZWobbleFrequency + (float) index) * this.ZWobbleAmplitude
      };
      Vector3 localScale = child.localScale with
      {
        x = (double) child.localPosition.x >= 0.0 ? 1f : -1f
      };
      if (this.invertFacing)
        localScale.x *= -1f;
      child.localScale = localScale;
    }
  }
}
