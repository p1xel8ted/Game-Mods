// Decompiled with JetBrains decompiler
// Type: ReflectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
