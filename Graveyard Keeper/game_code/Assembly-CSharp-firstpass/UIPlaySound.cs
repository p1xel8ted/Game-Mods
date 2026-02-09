// Decompiled with JetBrains decompiler
// Type: UIPlaySound
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
  public AudioClip audioClip;
  public UIPlaySound.Trigger trigger;
  [Range(0.0f, 1f)]
  public float volume = 1f;
  [Range(0.0f, 2f)]
  public float pitch = 1f;
  public bool mIsOver;

  public bool canPlay
  {
    get
    {
      if (!this.enabled)
        return false;
      UIButton component = this.GetComponent<UIButton>();
      return (Object) component == (Object) null || component.isEnabled;
    }
  }

  public void OnEnable()
  {
    if (this.trigger != UIPlaySound.Trigger.OnEnable)
      return;
    NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
  }

  public void OnDisable()
  {
    if (this.trigger != UIPlaySound.Trigger.OnDisable)
      return;
    NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
  }

  public void OnHover(bool isOver)
  {
    if (this.trigger == UIPlaySound.Trigger.OnMouseOver)
    {
      if (this.mIsOver == isOver)
        return;
      this.mIsOver = isOver;
    }
    if (!this.canPlay || (!isOver || this.trigger != UIPlaySound.Trigger.OnMouseOver) && (isOver || this.trigger != UIPlaySound.Trigger.OnMouseOut))
      return;
    NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
  }

  public void OnPress(bool isPressed)
  {
    if (this.trigger == UIPlaySound.Trigger.OnPress)
    {
      if (this.mIsOver == isPressed)
        return;
      this.mIsOver = isPressed;
    }
    if (!this.canPlay || (!isPressed || this.trigger != UIPlaySound.Trigger.OnPress) && (isPressed || this.trigger != UIPlaySound.Trigger.OnRelease))
      return;
    NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
  }

  public void OnClick()
  {
    if (!this.canPlay || this.trigger != UIPlaySound.Trigger.OnClick)
      return;
    NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
  }

  public void OnSelect(bool isSelected)
  {
    if (!this.canPlay || isSelected && UICamera.currentScheme != UICamera.ControlScheme.Controller)
      return;
    this.OnHover(isSelected);
  }

  public void Play() => NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);

  public enum Trigger
  {
    OnClick,
    OnMouseOver,
    OnMouseOut,
    OnPress,
    OnRelease,
    Custom,
    OnEnable,
    OnDisable,
  }
}
