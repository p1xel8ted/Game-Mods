// Decompiled with JetBrains decompiler
// Type: src.UI.SpriteByBoolValueSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI;

public class SpriteByBoolValueSelector : SerializedMonoBehaviour
{
  [SerializeField]
  public DataManager.Variables variable;
  [SerializeField]
  public Sprite onImage;
  [SerializeField]
  public Sprite offImage;
  [SerializeField]
  public Image image;

  public void OnEnable()
  {
    this.image.sprite = DataManager.Instance.GetVariable(this.variable) ? this.onImage : this.offImage;
  }
}
