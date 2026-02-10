// Decompiled with JetBrains decompiler
// Type: ShadowPetGuardian
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShadowPetGuardian : MonoBehaviour
{
  public SpriteRenderer spriteRenderer;
  public Vector3 NewPositon;

  public void OnEnable()
  {
    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    this.spriteRenderer.enabled = false;
  }

  public void Show() => this.spriteRenderer.enabled = true;

  public void Hide() => this.spriteRenderer.enabled = false;

  public void LateUpdate()
  {
    this.transform.localPosition = Vector3.zero;
    this.NewPositon = this.transform.position;
    this.NewPositon.z = 0.0f;
    this.transform.position = this.NewPositon;
  }
}
