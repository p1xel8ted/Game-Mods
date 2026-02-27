// Decompiled with JetBrains decompiler
// Type: GoopMaterialPropertyController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GoopMaterialPropertyController : MaterialImagePropertyController
{
  private const string KPositionOffset = "_PositionOffset";
  [SerializeField]
  private Vector2 positionOffsetProperty = new Vector2(0.0f, 0.0f);
  private static readonly int PositionOffset = Shader.PropertyToID("_PositionOffset");

  protected override void UpdateMaterialProperties()
  {
    this._material.SetVector(GoopMaterialPropertyController.PositionOffset, new Vector4(this.positionOffsetProperty.x, this.positionOffsetProperty.y, 0.0f, 0.0f));
  }
}
