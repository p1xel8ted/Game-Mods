// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.TutorialConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Tutorial Configuration", menuName = "Massive Monster/Tutorial Configuration", order = 1)]
public class TutorialConfiguration : ScriptableObject
{
  [SerializeField]
  private TutorialCategory[] _categories;

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
