// Decompiled with JetBrains decompiler
// Type: MaterialImagePropertyController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public abstract class MaterialImagePropertyController : MaterialPropertyController<Image>
{
  public override void SetupSource()
  {
    if (!Application.isPlaying)
    {
      this._material = this._sourceComponent.materialForRendering;
    }
    else
    {
      this._material = new Material(this._sourceComponent.material);
      this._sourceComponent.material = this._material;
    }
    this.UpdateMaterialProperties();
  }
}
