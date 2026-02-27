// Decompiled with JetBrains decompiler
// Type: DoctrineChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DoctrineChoice : MonoBehaviour
{
  private const string kUnlockedLayer = "Unlocked";
  private const string kLockedLayer = "Locked";
  private const string kUnchosenLayer = "Unchosen";
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private Animator _animator;
  [SerializeField]
  private DoctrineCategoryNotification _notification;
  private DoctrineUpgradeSystem.DoctrineType _type;
  private DoctrineChoice.State _state = DoctrineChoice.State.Locked;

  public DoctrineUpgradeSystem.DoctrineType Type => this._type;

  public DoctrineChoice.State CurrentState => this._state;

  public void Configure(DoctrineUpgradeSystem.DoctrineType type)
  {
    this._type = type;
    if (type != DoctrineUpgradeSystem.DoctrineType.None)
    {
      this._icon.sprite = DoctrineUpgradeSystem.GetIcon(this._type);
      this._state = DoctrineChoice.State.Unlocked;
    }
    else
      this._state = DoctrineChoice.State.Unchosen;
  }

  public void OnEnable()
  {
    this._notification.Configure(this._type);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), this._state == DoctrineChoice.State.Unlocked ? 1f : 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), this._state == DoctrineChoice.State.Locked ? 1f : 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unchosen"), this._state == DoctrineChoice.State.Unchosen ? 1f : 0.0f);
    this._button.OnSelected += new System.Action(this.TryRemoveAlert);
  }

  public void OnDisable() => this._button.OnSelected -= new System.Action(this.TryRemoveAlert);

  private void TryRemoveAlert() => this._notification.TryRemoveAlert();

  public enum State
  {
    Unlocked,
    Locked,
    Unchosen,
  }
}
