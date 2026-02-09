// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Port
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas;

[SpoofAOT]
public abstract class Port
{
  [CompilerGenerated]
  public FlowNode \u003Cparent\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CID\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003Cname\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CdisplayName\u003Ek__BackingField;
  [CompilerGenerated]
  public GUIContent \u003CdisplayContent\u003Ek__BackingField;
  [CompilerGenerated]
  public Color \u003CdisplayColor\u003Ek__BackingField;
  [CompilerGenerated]
  public Vector2 \u003Cpos\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CposOffsetY\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003Cconnections\u003Ek__BackingField;
  [NonSerialized]
  public string additional_draw;
  [NonSerialized]
  public Rect additional_draw_rect;
  [NonSerialized]
  public Color additional_draw_color;

  public Port()
  {
  }

  public Port(FlowNode parent, string name, string ID)
  {
    if (string.IsNullOrEmpty(name))
      name = " ";
    if (string.IsNullOrEmpty(ID))
      ID = name;
    this.parent = parent;
    this.name = name;
    this.ID = ID;
  }

  public FlowNode parent
  {
    get => this.\u003Cparent\u003Ek__BackingField;
    set => this.\u003Cparent\u003Ek__BackingField = value;
  }

  public string ID
  {
    get => this.\u003CID\u003Ek__BackingField;
    set => this.\u003CID\u003Ek__BackingField = value;
  }

  public string name
  {
    get => this.\u003Cname\u003Ek__BackingField;
    set => this.\u003Cname\u003Ek__BackingField = value;
  }

  public string displayName
  {
    get => this.\u003CdisplayName\u003Ek__BackingField;
    set => this.\u003CdisplayName\u003Ek__BackingField = value;
  }

  public GUIContent displayContent
  {
    get => this.\u003CdisplayContent\u003Ek__BackingField;
    set => this.\u003CdisplayContent\u003Ek__BackingField = value;
  }

  public Color displayColor
  {
    get => this.\u003CdisplayColor\u003Ek__BackingField;
    set => this.\u003CdisplayColor\u003Ek__BackingField = value;
  }

  public Vector2 pos
  {
    get => this.\u003Cpos\u003Ek__BackingField;
    set => this.\u003Cpos\u003Ek__BackingField = value;
  }

  public float posOffsetY
  {
    get => this.\u003CposOffsetY\u003Ek__BackingField;
    set => this.\u003CposOffsetY\u003Ek__BackingField = value;
  }

  public int connections
  {
    get => this.\u003Cconnections\u003Ek__BackingField;
    set => this.\u003Cconnections\u003Ek__BackingField = value;
  }

  public bool isConnected => this.connections > 0;

  public abstract System.Type type { get; }

  public bool CanAcceptConnections()
  {
    return this is ValueOutput || this is FlowOutput && !this.isConnected || this is FlowInput || this is ValueInput && !this.isConnected;
  }

  public bool IsFlowPort() => this is FlowInput || this is FlowOutput;

  public bool IsValuePort() => this is ValueInput || this is ValueOutput;

  public bool IsInputPort() => this is FlowInput || this is ValueInput;

  public bool IsOutputPort() => this is FlowOutput || this is ValueOutput;

  public bool IsWild() => System.Type.op_Equality(this.type, typeof (Wild));

  public bool IsUnityObject() => typeof (UnityEngine.Object).RTIsAssignableFrom(this.type);

  public bool IsUnitySceneObject()
  {
    return typeof (Component).RTIsAssignableFrom(this.type) || System.Type.op_Equality(this.type, typeof (GameObject));
  }

  public bool IsDelegate() => typeof (Delegate).RTIsAssignableFrom(this.type);

  public bool IsEnumerableCollection() => this.type.IsEnumerableCollection();

  public bool IsVertical()
  {
    if (System.Type.op_Equality(this.type, typeof (string)))
      return true;
    return this.displayName.Length > 0 && this.displayName[this.displayName.Length - 1] == ' ';
  }
}
