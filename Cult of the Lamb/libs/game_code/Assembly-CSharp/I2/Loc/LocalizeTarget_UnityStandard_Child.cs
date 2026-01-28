// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityStandard_Child
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityStandard_Child : LocalizeTarget<GameObject>
{
  static LocalizeTarget_UnityStandard_Child() => LocalizeTarget_UnityStandard_Child.AutoRegister();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
  {
    LocalizeTargetDesc_Child desc = new LocalizeTargetDesc_Child();
    desc.Name = "Child";
    desc.Priority = 200;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override bool IsValid(Localize cmp) => cmp.transform.childCount > 1;

  public override eTermType GetPrimaryTermType(Localize cmp) => eTermType.GameObject;

  public override eTermType GetSecondaryTermType(Localize cmp) => eTermType.Text;

  public override bool CanUseSecondaryTerm() => false;

  public override bool AllowMainTermToBeRTL() => false;

  public override bool AllowSecondTermToBeRTL() => false;

  public override void GetFinalTerms(
    Localize cmp,
    string Main,
    string Secondary,
    out string primaryTerm,
    out string secondaryTerm)
  {
    primaryTerm = cmp.name;
    secondaryTerm = (string) null;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    if (string.IsNullOrEmpty(mainTranslation))
      return;
    Transform transform = cmp.transform;
    string str = mainTranslation;
    int num = mainTranslation.LastIndexOfAny(LanguageSourceData.CategorySeparators);
    if (num >= 0)
      str = str.Substring(num + 1);
    for (int index = 0; index < transform.childCount; ++index)
    {
      Transform child = transform.GetChild(index);
      child.gameObject.SetActive(child.name == str);
    }
  }
}
