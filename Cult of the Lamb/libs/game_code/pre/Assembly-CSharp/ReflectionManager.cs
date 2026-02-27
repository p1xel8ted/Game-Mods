// Decompiled with JetBrains decompiler
// Type: ReflectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class ReflectionManager : BaseMonoBehaviour
{
  public GameObject ReflectionObject;

  private void Awake()
  {
    SkeletonAnimation.OnCreated += new SkeletonAnimation.Created(this.OnCreated);
  }

  private void OnCreated(SkeletonAnimation skeleton)
  {
    if (!skeleton.ShowReflection || skeleton.HasReflection)
      return;
    Object.Instantiate<GameObject>(this.ReflectionObject, skeleton.transform.position, Quaternion.identity, skeleton.transform).GetComponent<SpineMirror>().Init(skeleton);
    skeleton.HasReflection = true;
  }
}
