// Decompiled with JetBrains decompiler
// Type: AnimNameInspector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
