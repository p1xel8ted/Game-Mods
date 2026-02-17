// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SermonCategoryTextIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class SermonCategoryTextIcon : MonoBehaviour
{
  [SerializeField]
  public SermonCategory _sermonCategory;
  [SerializeField]
  public TextMeshProUGUI _label;
  [SerializeField]
  public TMP_FontAsset fontAwesomeFontAsset;

  public SermonCategory SermonCategory => this._sermonCategory;

  public void SetLock()
  {
    this._label.font = this.fontAwesomeFontAsset;
    this._label.text = "\uF30D";
  }

  public void SetHidden() => this._label.text = "";
}
