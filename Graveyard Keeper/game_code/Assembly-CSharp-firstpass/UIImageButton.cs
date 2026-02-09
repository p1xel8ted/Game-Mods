// Decompiled with JetBrains decompiler
// Type: UIImageButton
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
  public UISprite target;
  public string normalSprite;
  public string hoverSprite;
  public string pressedSprite;
  public string disabledSprite;
  public bool pixelSnap = true;

  public bool isEnabled
  {
    get
    {
      Collider component = this.gameObject.GetComponent<Collider>();
      return (bool) (Object) component && component.enabled;
    }
    set
    {
      Collider component = this.gameObject.GetComponent<Collider>();
      if (!(bool) (Object) component || component.enabled == value)
        return;
      component.enabled = value;
      this.UpdateImage();
    }
  }

  public void OnEnable()
  {
    if ((Object) this.target == (Object) null)
      this.target = this.GetComponentInChildren<UISprite>();
    this.UpdateImage();
  }

  public void OnValidate()
  {
    if (!((Object) this.target != (Object) null))
      return;
    if (string.IsNullOrEmpty(this.normalSprite))
      this.normalSprite = this.target.spriteName;
    if (string.IsNullOrEmpty(this.hoverSprite))
      this.hoverSprite = this.target.spriteName;
    if (string.IsNullOrEmpty(this.pressedSprite))
      this.pressedSprite = this.target.spriteName;
    if (!string.IsNullOrEmpty(this.disabledSprite))
      return;
    this.disabledSprite = this.target.spriteName;
  }

  public void UpdateImage()
  {
    if (!((Object) this.target != (Object) null))
      return;
    if (this.isEnabled)
      this.SetSprite(UICamera.IsHighlighted(this.gameObject) ? this.hoverSprite : this.normalSprite);
    else
      this.SetSprite(this.disabledSprite);
  }

  public void OnHover(bool isOver)
  {
    if (!this.isEnabled || !((Object) this.target != (Object) null))
      return;
    this.SetSprite(isOver ? this.hoverSprite : this.normalSprite);
  }

  public void OnPress(bool pressed)
  {
    if (pressed)
      this.SetSprite(this.pressedSprite);
    else
      this.UpdateImage();
  }

  public void SetSprite(string sprite)
  {
    if ((Object) this.target.atlas == (Object) null || this.target.atlas.GetSprite(sprite) == null)
      return;
    this.target.spriteName = sprite;
    if (!this.pixelSnap)
      return;
    this.target.MakePixelPerfect();
  }
}
