// Decompiled with JetBrains decompiler
// Type: OnlyReflectUpright
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OnlyReflectUpright : MonoBehaviour
{
  public Quaternion NORMAL;
  public static Quaternion REFLECT = Quaternion.Euler(-130f, 0.0f, 0.0f);

  public void Awake() => this.NORMAL = this.transform.localRotation;

  public void OnEnable()
  {
    ReflectionCamera.OnPreReflectionRender += new System.Action(this.PreRender);
    ReflectionCamera.OnPostReflectionRender += new System.Action(this.PostRender);
  }

  public void OnDisable()
  {
    ReflectionCamera.OnPreReflectionRender -= new System.Action(this.PreRender);
    ReflectionCamera.OnPostReflectionRender -= new System.Action(this.PostRender);
  }

  public void PreRender() => this.transform.localRotation = OnlyReflectUpright.REFLECT;

  public void PostRender() => this.transform.localRotation = this.NORMAL;
}
