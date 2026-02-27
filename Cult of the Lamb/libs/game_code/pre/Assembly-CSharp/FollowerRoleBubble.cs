// Decompiled with JetBrains decompiler
// Type: FollowerRoleBubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

#nullable disable
public class FollowerRoleBubble : MonoBehaviour
{
  [SerializeField]
  private Follower follower;
  [SerializeField]
  private TMP_Text icon;
  private bool IsPlaying;

  private void Start() => this.transform.localScale = Vector3.zero;

  public void Show()
  {
    if (this.IsPlaying)
      return;
    this.icon.text = FontImageNames.IconForRole(this.follower.Brain.Info.FollowerRole);
    if (this.icon.text == "" || this.follower.Brain.Info.CursedState != Thought.None)
    {
      this.transform.localScale = Vector3.zero;
    }
    else
    {
      this.transform.DOKill();
      this.transform.localScale = Vector3.zero;
      this.transform.DOScale(Vector3.one * 0.7f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
  }

  public void Hide()
  {
    if (this.IsPlaying)
      return;
    this.transform.DOKill();
    this.transform.DOScale(Vector3.zero, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }
}
