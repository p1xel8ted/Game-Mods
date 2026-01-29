// Decompiled with JetBrains decompiler
// Type: PlayerCanvas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class PlayerCanvas : MonoBehaviour
{
  public CanvasGroup canvasGroup;

  public void Awake() => this.canvasGroup = this.GetComponent<CanvasGroup>();

  public void OnEnable() => this.canvasGroup.alpha = 0.0f;

  public void Update()
  {
    if (LetterBox.IsPlaying && GameManager.IsDungeon(PlayerFarming.Location))
    {
      if ((double) this.canvasGroup.alpha != 1.0)
        return;
      this.canvasGroup.DOKill();
      this.canvasGroup.DOFade(0.0f, 0.25f);
    }
    else
    {
      if ((double) this.canvasGroup.alpha != 0.0)
        return;
      this.canvasGroup.DOKill();
      this.canvasGroup.DOFade(1f, 0.25f);
    }
  }
}
