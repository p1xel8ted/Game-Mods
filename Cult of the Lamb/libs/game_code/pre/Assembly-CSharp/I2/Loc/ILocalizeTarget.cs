// Decompiled with JetBrains decompiler
// Type: I2.Loc.ILocalizeTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public abstract class ILocalizeTarget : ScriptableObject
{
  public abstract bool IsValid(Localize cmp);

  public abstract void GetFinalTerms(
    Localize cmp,
    string Main,
    string Secondary,
    out string primaryTerm,
    out string secondaryTerm);

  public abstract void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation);

  public abstract bool CanUseSecondaryTerm();

  public abstract bool AllowMainTermToBeRTL();

  public abstract bool AllowSecondTermToBeRTL();

  public abstract eTermType GetPrimaryTermType(Localize cmp);

  public abstract eTermType GetSecondaryTermType(Localize cmp);
}
