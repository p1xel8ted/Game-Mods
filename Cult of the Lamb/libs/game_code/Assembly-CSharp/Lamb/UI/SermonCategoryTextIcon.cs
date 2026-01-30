// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SermonCategoryTextIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
