// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SermonCategoryTextIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class SermonCategoryTextIcon : MonoBehaviour
{
  [SerializeField]
  private SermonCategory _sermonCategory;
  [SerializeField]
  private TextMeshProUGUI _label;

  public SermonCategory SermonCategory => this._sermonCategory;

  public void SetLock() => this._label.text = "\uF30D";

  public void SetHidden() => this._label.text = "";
}
