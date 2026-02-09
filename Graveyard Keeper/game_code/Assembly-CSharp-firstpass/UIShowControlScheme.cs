// Decompiled with JetBrains decompiler
// Type: UIShowControlScheme
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class UIShowControlScheme : MonoBehaviour
{
  public GameObject target;
  public bool mouse;
  public bool touch;
  public bool controller = true;

  public void OnEnable()
  {
    UICamera.onSchemeChange += new UICamera.OnSchemeChange(this.OnScheme);
    this.OnScheme();
  }

  public void OnDisable() => UICamera.onSchemeChange -= new UICamera.OnSchemeChange(this.OnScheme);

  public void OnScheme()
  {
    if (!((Object) this.target != (Object) null))
      return;
    switch (UICamera.currentScheme)
    {
      case UICamera.ControlScheme.Mouse:
        this.target.SetActive(this.mouse);
        break;
      case UICamera.ControlScheme.Touch:
        this.target.SetActive(this.touch);
        break;
      case UICamera.ControlScheme.Controller:
        this.target.SetActive(this.controller);
        break;
    }
  }
}
