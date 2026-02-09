// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityStandard_MeshRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityStandard_MeshRenderer : LocalizeTarget<MeshRenderer>
{
  static LocalizeTarget_UnityStandard_MeshRenderer()
  {
    LocalizeTarget_UnityStandard_MeshRenderer.AutoRegister();
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
  {
    LocalizeTargetDesc_Type<MeshRenderer, LocalizeTarget_UnityStandard_MeshRenderer> desc = new LocalizeTargetDesc_Type<MeshRenderer, LocalizeTarget_UnityStandard_MeshRenderer>();
    desc.Name = "MeshRenderer";
    desc.Priority = 800;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override eTermType GetPrimaryTermType(Localize cmp) => eTermType.Mesh;

  public override eTermType GetSecondaryTermType(Localize cmp) => eTermType.Material;

  public override bool CanUseSecondaryTerm() => true;

  public override bool AllowMainTermToBeRTL() => false;

  public override bool AllowSecondTermToBeRTL() => false;

  public override void GetFinalTerms(
    Localize cmp,
    string Main,
    string Secondary,
    out string primaryTerm,
    out string secondaryTerm)
  {
    if ((Object) this.mTarget == (Object) null)
    {
      primaryTerm = secondaryTerm = (string) null;
    }
    else
    {
      MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
      primaryTerm = (Object) component == (Object) null || (Object) component.sharedMesh == (Object) null ? (string) null : component.sharedMesh.name;
    }
    if ((Object) this.mTarget == (Object) null || (Object) this.mTarget.sharedMaterial == (Object) null)
      secondaryTerm = (string) null;
    else
      secondaryTerm = this.mTarget.sharedMaterial.name;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
    if ((Object) secondaryTranslatedObj != (Object) null && (Object) this.mTarget.sharedMaterial != (Object) secondaryTranslatedObj)
      this.mTarget.material = secondaryTranslatedObj;
    Mesh translatedObject = cmp.FindTranslatedObject<Mesh>(mainTranslation);
    MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
    if (!((Object) translatedObject != (Object) null) || !((Object) component.sharedMesh != (Object) translatedObject))
      return;
    component.mesh = translatedObject;
  }
}
