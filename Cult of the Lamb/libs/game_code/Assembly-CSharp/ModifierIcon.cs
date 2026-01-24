// Decompiled with JetBrains decompiler
// Type: ModifierIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ModifierIcon : MonoBehaviour
{
  public EnemyModifier modifier;
  public SpriteRenderer spriteRenderer;
  public GameObject TimerObject;
  public SpriteRenderer TimerProgress;
  public bool hasTimer;

  public void Init(EnemyModifier _modifier)
  {
    this.modifier = _modifier;
    this.spriteRenderer.sprite = this.modifier.ModifierIconSprite;
    this.hasTimer = _modifier.HasTimer;
    if (this.hasTimer)
    {
      this.TimerObject.SetActive(true);
      this.TimerProgress.material = new Material(this.TimerProgress.material);
    }
    else
      this.TimerObject.SetActive(false);
  }

  public void Init()
  {
    this.spriteRenderer.enabled = false;
    this.hasTimer = true;
    this.TimerObject.SetActive(true);
    this.TimerProgress.material = new Material(this.TimerProgress.material);
  }

  public void UpdateTimer(float progress)
  {
    this.TimerProgress.material.SetFloat("_Progress", progress);
  }
}
