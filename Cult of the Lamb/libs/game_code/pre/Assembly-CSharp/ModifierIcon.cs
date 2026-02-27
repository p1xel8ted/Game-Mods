// Decompiled with JetBrains decompiler
// Type: ModifierIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ModifierIcon : MonoBehaviour
{
  private EnemyModifier modifier;
  public SpriteRenderer spriteRenderer;
  public GameObject TimerObject;
  public SpriteRenderer TimerProgress;
  private bool hasTimer;

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

  public void UpdateTimer(float progress)
  {
    this.TimerProgress.material.SetFloat("_Progress", progress);
  }
}
