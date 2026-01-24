// Decompiled with JetBrains decompiler
// Type: NonUILayoutElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class NonUILayoutElement : MonoBehaviour
{
  public NonUILayout ParentLayout;
  [CompilerGenerated]
  public bool \u003CIgnoreLayout\u003Ek__BackingField;

  public bool IgnoreLayout
  {
    get => this.\u003CIgnoreLayout\u003Ek__BackingField;
    set => this.\u003CIgnoreLayout\u003Ek__BackingField = value;
  }

  public void Start()
  {
    if (!((Object) this.ParentLayout == (Object) null))
      return;
    this.ParentLayout = this.GetComponentInParent<NonUILayout>();
  }

  public void OnDisable() => this.ParentLayout.RefreshElements();

  public void OnEnable() => this.ParentLayout.RefreshElements();

  public void OnDestroy() => this.ParentLayout.RefreshElements();
}
