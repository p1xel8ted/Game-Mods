// Decompiled with JetBrains decompiler
// Type: I2.Loc.ILocalizeTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
