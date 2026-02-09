// Decompiled with JetBrains decompiler
// Type: I2.Loc.LanguageSourceAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

[CreateAssetMenu(fileName = "I2Languages", menuName = "I2 Localization/LanguageSource", order = 1)]
public class LanguageSourceAsset : ScriptableObject, ILanguageSource
{
  public LanguageSourceData mSource = new LanguageSourceData();

  public LanguageSourceData SourceData
  {
    get => this.mSource;
    set => this.mSource = value;
  }
}
