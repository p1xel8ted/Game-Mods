// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityStandard_Prefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityStandard_Prefab : LocalizeTarget<GameObject>
{
  static LocalizeTarget_UnityStandard_Prefab()
  {
    LocalizeTarget_UnityStandard_Prefab.AutoRegister();
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
  {
    LocalizeTargetDesc_Prefab desc = new LocalizeTargetDesc_Prefab();
    desc.Name = "Prefab";
    desc.Priority = 250;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override bool IsValid(Localize cmp) => true;

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
    if (string.IsNullOrEmpty(mainTranslation) || (bool) (Object) this.mTarget && this.mTarget.name == mainTranslation)
      return;
    Transform transform1 = cmp.transform;
    string str = mainTranslation;
    int num = mainTranslation.LastIndexOfAny(LanguageSourceData.CategorySeparators);
    if (num >= 0)
      str = str.Substring(num + 1);
    Transform transform2 = this.InstantiateNewPrefab(cmp, mainTranslation);
    if ((Object) transform2 == (Object) null)
      return;
    transform2.name = str;
    for (int index = transform1.childCount - 1; index >= 0; --index)
    {
      Transform child = transform1.GetChild(index);
      if ((Object) child != (Object) transform2)
        Object.Destroy((Object) child.gameObject);
    }
  }

  public Transform InstantiateNewPrefab(Localize cmp, string mainTranslation)
  {
    GameObject translatedObject = cmp.FindTranslatedObject<GameObject>(mainTranslation);
    if ((Object) translatedObject == (Object) null)
      return (Transform) null;
    GameObject mTarget = this.mTarget;
    this.mTarget = Object.Instantiate<GameObject>(translatedObject);
    if ((Object) this.mTarget == (Object) null)
      return (Transform) null;
    Transform transform1 = cmp.transform;
    Transform transform2 = this.mTarget.transform;
    transform2.SetParent(transform1);
    Transform transform3 = (bool) (Object) mTarget ? mTarget.transform : transform1;
    transform2.rotation = transform3.rotation;
    transform2.position = transform3.position;
    return transform2;
  }
}
