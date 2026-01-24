// Decompiled with JetBrains decompiler
// Type: AnimNameInspector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;

#nullable disable
public class AnimNameInspector : BaseMonoBehaviour
{
  [SpineAnimation("", "", true, false)]
  public string listOfAllAnimations;
  public SkeletonAnimation otherSkeletonData;
  [SpineAnimation("", "otherSkeletonData", true, false)]
  public string otherSkeletonsAnimations;

  public static string listOfAllAnimationsFunction() => "test";

  public string GetListOfAllAnimations() => this.listOfAllAnimations;
}
