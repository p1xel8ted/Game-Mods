// Decompiled with JetBrains decompiler
// Type: InspectorScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class InspectorScene : IEquatable<InspectorScene>
{
  [SerializeField]
  public string SceneName;

  public bool Equals(InspectorScene other) => this.SceneName.Equals(other.SceneName);
}
