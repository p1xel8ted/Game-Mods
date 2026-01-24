// Decompiled with JetBrains decompiler
// Type: FollowerRoleBubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

#nullable disable
public class FollowerRoleBubble : MonoBehaviour
{
  [SerializeField]
  public Follower follower;
  [SerializeField]
  public TMP_Text icon;
  public bool IsPlaying;

  public void Start() => this.transform.localScale = Vector3.zero;

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
