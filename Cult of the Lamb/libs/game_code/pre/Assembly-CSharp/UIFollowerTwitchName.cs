// Decompiled with JetBrains decompiler
// Type: UIFollowerTwitchName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

#nullable disable
public class UIFollowerTwitchName : MonoBehaviour
{
  [SerializeField]
  private TMP_Text nameText;
  private Follower follower;
  private bool _shown = true;

  private void Awake()
  {
    this.follower = this.GetComponentInParent<Follower>();
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      this.follower.OnFollowerBrainAssigned += new System.Action(this.OnBrainAssigned);
      if (this.follower.Brain != null)
        this.OnBrainAssigned();
    }
    this.Hide(false);
  }

  private void OnDestroy()
  {
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this.follower.OnFollowerBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  private void OnBrainAssigned()
  {
    this.follower.OnFollowerBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.nameText.text = !string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID) ? "<sprite name=\"icon_TwitchIcon\"> " + this.follower.Brain.Info.Name : "";
    this.Show();
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && this.follower.Brain != null && !string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID))
    {
      this.nameText.text = !string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID) ? "<sprite name=\"icon_TwitchIcon\"> " + this.follower.Brain.Info.Name : "";
      this.nameText.transform.localScale = Vector3.one;
    }
    else
      this.nameText.text = "";
  }

  public void Show()
  {
    if (this._shown || string.IsNullOrEmpty(this.follower.Brain.Info.ViewerID))
      return;
    this._shown = true;
    this.transform.DOKill();
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.follower.Interaction_FollowerInteraction.UpdateLayoutContent();
  }

  public void Hide(bool animate = true)
  {
    if (!this._shown)
      return;
    this._shown = false;
    this.follower.Interaction_FollowerInteraction.UpdateLayoutContent();
    this.transform.DOKill();
    this.transform.localScale = Vector3.one;
    if (animate)
      this.transform.DOScale(Vector3.zero, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    else
      this.transform.localScale = Vector3.zero;
  }
}
