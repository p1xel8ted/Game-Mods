// Decompiled with JetBrains decompiler
// Type: CharacterBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CharacterBase : BaseMonoBehaviour
{
  public StateMachine statemachine;
  public float rotation;
  public float angle;
  public bool LockToGround;
  public SpriteRenderer spriteRender;
  public bool AffectedByQualitySettings = true;

  public void Start()
  {
    this.statemachine = this.GetComponentInParent<StateMachine>();
    if (!this.AffectedByQualitySettings || QualitySettings.shadows == ShadowQuality.Disable)
      return;
    this.spriteRender = this.GetComponent<SpriteRenderer>();
    if (!((Object) this.spriteRender != (Object) null))
      return;
    this.spriteRender.enabled = false;
  }

  public void LateUpdate()
  {
    if (!((Object) this.statemachine != (Object) null))
      return;
    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.statemachine.facingAngle);
    if (!this.LockToGround)
      return;
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
  }
}
