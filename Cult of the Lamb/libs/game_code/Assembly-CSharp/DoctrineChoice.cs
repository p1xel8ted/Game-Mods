// Decompiled with JetBrains decompiler
// Type: DoctrineChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DoctrineChoice : MonoBehaviour
{
  public const string kUnlockedLayer = "Unlocked";
  public const string kLockedLayer = "Locked";
  public const string kUnchosenLayer = "Unchosen";
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public Outline _outline;
  [SerializeField]
  public DoctrineCategoryNotification _notification;
  public DoctrineUpgradeSystem.DoctrineType _type;
  public DoctrineChoice.State _state = DoctrineChoice.State.Locked;
  public bool _unlockedWithCrystal;

  public DoctrineUpgradeSystem.DoctrineType Type => this._type;

  public DoctrineChoice.State CurrentState => this._state;

  public bool UnlockedWithCrystal => this._unlockedWithCrystal;

  public void Configure(DoctrineUpgradeSystem.DoctrineType type, bool unlockedWithCrystal)
  {
    this._type = type;
    this._unlockedWithCrystal = unlockedWithCrystal;
    if (type != DoctrineUpgradeSystem.DoctrineType.None)
    {
      this._icon.sprite = DoctrineUpgradeSystem.GetIcon(this._type);
      this._state = DoctrineChoice.State.Unlocked;
    }
    else
      this._state = DoctrineChoice.State.Unchosen;
    if (!this._unlockedWithCrystal)
      return;
    this._outline.effectColor = StaticColors.BlueColor;
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

  public void TryRemoveAlert() => this._notification.TryRemoveAlert();

  public enum State
  {
    Unlocked,
    Locked,
    Unchosen,
  }
}
