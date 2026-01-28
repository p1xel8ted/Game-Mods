// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.TutorialConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Tutorial Configuration", menuName = "Massive Monster/Tutorial Configuration", order = 1)]
public class TutorialConfiguration : ScriptableObject
{
  [SerializeField]
  public TutorialCategory[] _categories;

  public TutorialCategory[] Categories => this._categories;

  public TutorialCategory GetCategory(TutorialTopic topic)
  {
    foreach (TutorialCategory category in this._categories)
    {
      if (category.Topic == topic)
        return category;
    }
    return (TutorialCategory) null;
  }
}
