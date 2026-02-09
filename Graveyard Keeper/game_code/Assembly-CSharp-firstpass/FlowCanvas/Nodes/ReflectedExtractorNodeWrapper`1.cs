// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReflectedExtractorNodeWrapper`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[DoNotList]
[Description("Chose and expose any number of fields or properties of the type. If you only require a single field / property, it's better to get that field / property directly without an Extractor.")]
[ParadoxNotion.Design.Icon("", false, "GetRuntimeIconType")]
public class ReflectedExtractorNodeWrapper<T> : FlowNode
{
  public static Dictionary<string, MemberInfo> _memberInfos;
  public static List<string> _instanceMemberNames;
  public static List<string> _staticMemberNames;
  [SerializeField]
  public bool _isStatic;
  [SerializeField]
  public string[] _selectedInstanceMembers;
  [SerializeField]
  public string[] _selectedStaticMembers;
  [NonSerialized]
  public BaseReflectedExtractorNode extractorNode;

  public static void FillInfos()
  {
    if (ReflectedExtractorNodeWrapper<T>._memberInfos != null)
      return;
    ReflectedExtractorNodeWrapper<T>._memberInfos = new Dictionary<string, MemberInfo>((IEqualityComparer<string>) StringComparer.Ordinal);
    ReflectedExtractorNodeWrapper<T>._instanceMemberNames = new List<string>();
    ReflectedExtractorNodeWrapper<T>._staticMemberNames = new List<string>();
    System.Type type = typeof (T);
    FieldInfo[] fields = type.RTGetFields();
    PropertyInfo[] properties = type.RTGetProperties();
    foreach (FieldInfo member in fields)
    {
      if (!FieldInfo.op_Equality(member, (FieldInfo) null) && member.IsPublic && !member.IsObsolete())
      {
        ReflectedExtractorNodeWrapper<T>._memberInfos[member.Name] = (MemberInfo) member;
        (member.IsStatic ? ReflectedExtractorNodeWrapper<T>._staticMemberNames : ReflectedExtractorNodeWrapper<T>._instanceMemberNames).Add(member.Name);
      }
    }
    foreach (PropertyInfo propertyInfo in properties)
    {
      if (!PropertyInfo.op_Equality(propertyInfo, (PropertyInfo) null) && !propertyInfo.IsIndexerProperty() && !propertyInfo.IsObsolete())
      {
        MethodInfo getMethod = propertyInfo.RTGetGetMethod();
        if (!MethodInfo.op_Equality(getMethod, (MethodInfo) null) && getMethod.IsPublic)
        {
          ReflectedExtractorNodeWrapper<T>._memberInfos[propertyInfo.Name] = (MemberInfo) getMethod;
          (getMethod.IsStatic ? ReflectedExtractorNodeWrapper<T>._staticMemberNames : ReflectedExtractorNodeWrapper<T>._instanceMemberNames).Add(propertyInfo.Name);
        }
      }
    }
  }

  public System.Type GetRuntimeIconType() => typeof (T);

  public override string name => $"Extract ({typeof (T).FriendlyName()})";

  public override void OnCreate(Graph assignedGraph)
  {
    this._selectedInstanceMembers = new string[ReflectedExtractorNodeWrapper<T>._instanceMemberNames.Count];
    this.GatherPorts();
  }

  public void CheckData()
  {
    ReflectedExtractorNodeWrapper<T>.FillInfos();
    if (this._selectedInstanceMembers != null && this._selectedInstanceMembers.Length == ReflectedExtractorNodeWrapper<T>._instanceMemberNames.Count)
      return;
    this._selectedInstanceMembers = new string[ReflectedExtractorNodeWrapper<T>._instanceMemberNames.Count];
  }

  public override void RegisterPorts()
  {
    this.CheckData();
    string[] strArray = this._isStatic ? this._selectedStaticMembers : this._selectedInstanceMembers;
    List<MemberInfo> memberInfoList = new List<MemberInfo>();
    for (int index = 0; index < strArray.Length; ++index)
    {
      string key = strArray[index];
      if (!string.IsNullOrEmpty(key))
      {
        MemberInfo memberInfo;
        ReflectedExtractorNodeWrapper<T>._memberInfos.TryGetValue(key, out memberInfo);
        if (MemberInfo.op_Inequality(memberInfo, (MemberInfo) null))
          memberInfoList.Add(memberInfo);
      }
    }
    this.extractorNode = BaseReflectedExtractorNode.GetExtractorNode(typeof (T), this._isStatic, memberInfoList.ToArray());
    if (this.extractorNode == null)
      return;
    this.extractorNode.RegisterPorts((FlowNode) this);
  }
}
