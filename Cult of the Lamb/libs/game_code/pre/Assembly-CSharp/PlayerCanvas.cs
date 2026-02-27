// Decompiled with JetBrains decompiler
// Type: PlayerCanvas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class PlayerCanvas : MonoBehaviour
{
  public static PlayerCanvas Instance;
  private CanvasGroup canvasGroup;

  private void Awake()
  {
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    PlayerCanvas.Instance = this;
  }

  private void OnEnable() => this.canvasGroup.alpha = 0.0f;

  private void Update()
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
