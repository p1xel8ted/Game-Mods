// Decompiled with JetBrains decompiler
// Type: src.UI.SpriteByValueSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI;

public class SpriteByValueSelector : SerializedMonoBehaviour
{
  [SerializeField]
  public DataManager.Variables variable;
  [SerializeField]
  public Dictionary<int, Sprite> stages = new Dictionary<int, Sprite>();
  [SerializeField]
  public Image image;
  public bool forceLastSprite;

  public void OnEnable()
  {
    if (this.forceLastSprite)
    {
      this.ForceLastSprite();
    }
    else
    {
      this.image.sprite = CollectionExtensions.GetValueOrDefault<int, Sprite>((IReadOnlyDictionary<int, Sprite>) this.stages, DataManager.Instance.GetVariableInt(this.variable));
      this.image.enabled = (Object) this.image.sprite != (Object) null;
    }
  }

  public void SetForceLastSprite() => this.forceLastSprite = true;

  public void ForceLastSprite()
  {
    if (this.stages.Count == 0)
      return;
    int num = int.MinValue;
    foreach (int key in this.stages.Keys)
    {
      if (key > num)
        num = key;
    }
    this.image.sprite = CollectionExtensions.GetValueOrDefault<int, Sprite>((IReadOnlyDictionary<int, Sprite>) this.stages, num);
    this.image.enabled = (Object) this.image.sprite != (Object) null;
  }
}
