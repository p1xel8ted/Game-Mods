// Decompiled with JetBrains decompiler
// Type: ReflectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class ReflectionManager : BaseMonoBehaviour
{
  public GameObject ReflectionObject;

  public void Awake()
  {
    SkeletonAnimation.OnCreated += new SkeletonAnimation.Created(this.OnCreated);
  }

  public void OnCreated(SkeletonAnimation skeleton)
  {
    if (!skeleton.ShowReflection || skeleton.HasReflection)
      return;
    Object.Instantiate<GameObject>(this.ReflectionObject, skeleton.transform.position, Quaternion.identity, skeleton.transform).GetComponent<SpineMirror>().Init(skeleton);
    skeleton.HasReflection = true;
  }
}
