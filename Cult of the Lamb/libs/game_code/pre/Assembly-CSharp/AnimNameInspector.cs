// Decompiled with JetBrains decompiler
// Type: AnimNameInspector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private string GetListOfAllAnimations() => this.listOfAllAnimations;
}
