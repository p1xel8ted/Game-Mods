// Decompiled with JetBrains decompiler
// Type: ObjectShadow
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
[ExecuteInEditMode]
public class ObjectShadow : MonoBehaviour
{
  public Material mat;
  public SpriteRenderer _spr;

  public void Update()
  {
    this._spr = this.gameObject.GetComponent<SpriteRenderer>();
    if ((Object) this.mat == (Object) null)
      this.mat = this._spr.sharedMaterial;
    if ((Object) this.mat == (Object) null || (Object) TimeOfDay.me == (Object) null)
      return;
    this.mat.SetFloat("_HSkew", TimeOfDay.me.time_of_day * 1.25f);
    this.mat.SetFloat("_Rotation", TimeOfDay.me.time_of_day / 3f);
    this._spr.color = this._spr.color with
    {
      a = TimeOfDay.me.shadow_alpha.Evaluate((float) (((double) TimeOfDay.me.time_of_day + 1.0) / 2.0))
    };
  }
}
