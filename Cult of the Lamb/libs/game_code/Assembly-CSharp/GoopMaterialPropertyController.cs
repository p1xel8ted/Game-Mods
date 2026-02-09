// Decompiled with JetBrains decompiler
// Type: GoopMaterialPropertyController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GoopMaterialPropertyController : MaterialImagePropertyController
{
  public const string KPositionOffset = "_PositionOffset";
  [SerializeField]
  public Vector2 positionOffsetProperty = new Vector2(0.0f, 0.0f);
  public static int PositionOffset = Shader.PropertyToID("_PositionOffset");

  public override void UpdateMaterialProperties()
  {
    this._material.SetVector(GoopMaterialPropertyController.PositionOffset, new Vector4(this.positionOffsetProperty.x, this.positionOffsetProperty.y, 0.0f, 0.0f));
  }
}
