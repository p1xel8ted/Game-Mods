// Decompiled with JetBrains decompiler
// Type: UIDoctrineUpgradeTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIDoctrineUpgradeTile : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public SermonCategory Category;
  public DoctrineUpgradeSystem.DoctrineType Type;
  public UIDoctrineUpgradeTile.UpgradeTileState State;
  public UIDoctrineUpgradeTile.UpgradeTileState PreviousState;
  public TextMeshProUGUI NameText;
  public Selectable Selectable;
  public RectTransform Container;
  public Image Icon;
  public Image BG;
  public Image SelectedIcon;
  public bool StartAvailable;
  public bool Revealed;
  public bool Disabled;
  public bool Upgrade;
  public CanvasGroup CanvasGroup;
  public UIDoctrineUpgradeTile PartnerTile;
  public bool _Paused;
  public bool AvailableToSelect = true;

  public bool Paused
  {
    get => this._Paused;
    set
    {
      Debug.Log((object) ("CHANGE VALUE! " + value.ToString()));
      this._Paused = value;
    }
  }

  public virtual void Start()
  {
    this.Init();
    this.ShowAvailability();
  }

  public IEnumerator UnPause(float Delay)
  {
    Debug.Log((object) ("DELAY: " + Delay.ToString()));
    yield return (object) new WaitForSecondsRealtime(Delay);
    Debug.Log((object) "UNPAUSE!");
    this.Paused = false;
    this.ShowAvailability();
  }

  public void ReEnable()
  {
    this.Paused = true;
    this.Selectable.interactable = true;
    this.ShowAvailability();
  }

  public void ShowAvailability()
  {
    this.AvailableToSelect = false;
    if (this.StartAvailable || DoctrineUpgradeSystem.GetUnlocked(this.Type))
    {
      this.Selectable.interactable = true;
      this.Container.gameObject.SetActive(true);
      this.PreviousState = this.State;
      this.State = !DoctrineUpgradeSystem.GetUnlocked(this.Type) ? UIDoctrineUpgradeTile.UpgradeTileState.Unlocked : UIDoctrineUpgradeTile.UpgradeTileState.Bought;
      if (DoctrineUpgradeSystem.GetUnlocked(this.Type))
        this.BG.color = Color.green;
      else
        this.BG.color = Color.white;
      this.Icon.color = Color.white;
      if (!this.Revealed && !this.StartAvailable)
        this.StartCoroutine((IEnumerator) this.Selected(1.5f, 1f));
      this.Revealed = true;
    }
    else if (this.NeighbourUnlocked() && !this.Upgrade)
    {
      this.SetLocked();
      if (this.Revealed || this.StartAvailable)
        return;
      this.StartCoroutine((IEnumerator) this.Selected(1.5f, 1f));
    }
    else
    {
      this.Container.gameObject.SetActive(false);
      this.Selectable.interactable = false;
      this.State = UIDoctrineUpgradeTile.UpgradeTileState.Hidden;
    }
  }

  public void SetLocked()
  {
    this.State = UIDoctrineUpgradeTile.UpgradeTileState.Locked;
    this.Selectable.interactable = true;
    this.Container.gameObject.SetActive(true);
    this.NameText.gameObject.SetActive(false);
    this.BG.color = new Color(1f, 1f, 1f, 0.25f);
    this.Icon.color = new Color(1f, 1f, 1f, 0.25f);
    this.Revealed = true;
  }

  public IEnumerator RevealRoutine()
  {
    yield return (object) new WaitForSecondsRealtime(0.5f);
    float Progress = 0.0f;
    float Duration = 1f;
    this.BG.color = Color.green;
    this.Selectable.interactable = true;
    this.Container.gameObject.SetActive(true);
    this.Container.localScale = Vector3.zero;
    this.CanvasGroup.alpha = 0.0f;
    this.NameText.gameObject.SetActive(true);
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.Lerp(2f, 1f, Progress / Duration);
      this.CanvasGroup.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    this.CanvasGroup.alpha = 1f;
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public IEnumerator LockedRevealRoutine()
  {
    yield return (object) new WaitForSecondsRealtime(0.5f);
    float Progress = 0.0f;
    float Duration = 1f;
    this.SetLocked();
    this.Selectable.interactable = true;
    this.Container.gameObject.SetActive(true);
    this.CanvasGroup.alpha = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.CanvasGroup.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    this.CanvasGroup.alpha = 1f;
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public bool NeighbourUnlocked()
  {
    return (Object) this.PartnerTile != (Object) null && DoctrineUpgradeSystem.GetUnlocked(this.PartnerTile.Type);
  }

  public void Init()
  {
    this.SelectedIcon.enabled = false;
    this.Selectable.interactable = !this.Disabled;
    this.Container.gameObject.SetActive(!this.Disabled);
    this.NameText.text = DoctrineUpgradeSystem.GetLocalizedName(this.Type);
    this.gameObject.name = this.NameText.text;
    this.Icon.preserveAspect = true;
    this.Icon.sprite = DoctrineUpgradeSystem.GetIcon(this.Type);
    if (!((Object) this.Icon.sprite == (Object) null))
      return;
    this.Icon.enabled = false;
  }

  public UIDoctrineUpgradeTile GetLockedNeighbour() => this.PartnerTile;

  public bool CallBack()
  {
    if (!DoctrineUpgradeSystem.UnlockAbility(this.Type))
      return false;
    this.ShowAvailability();
    return true;
  }

  public void BecomeAvailable()
  {
    this.Init();
    this.ShowAvailability();
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.SelectedIcon.enabled = true;
    this.StartCoroutine((IEnumerator) this.Selected(this.Container.localScale.x, 1.3f));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.SelectedIcon.enabled = false;
    this.StartCoroutine((IEnumerator) this.DeSelected());
  }

  public IEnumerator Selected(float Starting, float Target)
  {
    while (this.Paused)
      yield return (object) null;
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.SmoothStep(Starting, Target, Progress / Duration);
      yield return (object) null;
    }
    this.Container.localScale = Vector3.one * Target;
  }

  public IEnumerator DeSelected()
  {
    while (this.Paused)
      yield return (object) null;
    float Progress = 0.0f;
    float Duration = 0.3f;
    float StartingScale = this.Container.localScale.x;
    float TargetScale = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.SmoothStep(StartingScale, TargetScale, Progress / Duration);
      yield return (object) null;
    }
    this.Container.localScale = Vector3.one * TargetScale;
  }

  public enum UpgradeTileState
  {
    Locked,
    Unlocked,
    Bought,
    Hidden,
  }
}
