// Decompiled with JetBrains decompiler
// Type: UILocalize
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/UI/Localize")]
[RequireComponent(typeof (UIWidget))]
[ExecuteInEditMode]
public class UILocalize : MonoBehaviour
{
  public string key;
  public bool mStarted;

  public string value
  {
    set
    {
      if (string.IsNullOrEmpty(value))
        return;
      UIWidget component = this.GetComponent<UIWidget>();
      UILabel uiLabel = component as UILabel;
      UISprite uiSprite = component as UISprite;
      if ((Object) uiLabel != (Object) null)
      {
        UIInput inParents = NGUITools.FindInParents<UIInput>(uiLabel.gameObject);
        if ((Object) inParents != (Object) null && (Object) inParents.label == (Object) uiLabel)
          inParents.defaultText = value;
        else
          uiLabel.text = value;
      }
      else
      {
        if (!((Object) uiSprite != (Object) null))
          return;
        UIButton inParents = NGUITools.FindInParents<UIButton>(uiSprite.gameObject);
        if ((Object) inParents != (Object) null && (Object) inParents.tweenTarget == (Object) uiSprite.gameObject)
          inParents.normalSprite = value;
        uiSprite.spriteName = value;
        uiSprite.MakePixelPerfect();
      }
    }
  }

  public void OnEnable()
  {
    if (!this.mStarted)
      return;
    this.OnLocalize();
  }

  public void Start()
  {
    this.mStarted = true;
    this.OnLocalize();
  }

  public void OnLocalize()
  {
    if (string.IsNullOrEmpty(this.key))
    {
      UILabel component = this.GetComponent<UILabel>();
      if ((Object) component != (Object) null)
        this.key = component.text;
    }
    if (string.IsNullOrEmpty(this.key))
      return;
    this.value = Localization.Get(this.key);
  }
}
