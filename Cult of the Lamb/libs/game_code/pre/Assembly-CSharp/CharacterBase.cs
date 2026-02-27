// Decompiled with JetBrains decompiler
// Type: CharacterBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CharacterBase : BaseMonoBehaviour
{
  private StateMachine statemachine;
  private float rotation;
  private float angle;
  public bool LockToGround;
  private SpriteRenderer spriteRender;
  public bool AffectedByQualitySettings = true;

  private void Start()
  {
    this.statemachine = this.GetComponentInParent<StateMachine>();
    if (!this.AffectedByQualitySettings || QualitySettings.shadows == ShadowQuality.Disable)
      return;
    this.spriteRender = this.GetComponent<SpriteRenderer>();
    if (!((Object) this.spriteRender != (Object) null))
      return;
    this.spriteRender.enabled = false;
  }

  private void LateUpdate()
  {
    if (!((Object) this.statemachine != (Object) null))
      return;
    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.statemachine.facingAngle);
    if (!this.LockToGround)
      return;
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
  }
}
