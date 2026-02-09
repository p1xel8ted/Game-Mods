// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.JitMethodNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class JitMethodNode : BaseReflectedMethodNode
{
  public static Dictionary<string, UniversalDelegate> Delegates = new Dictionary<string, UniversalDelegate>((IEqualityComparer<string>) StringComparer.Ordinal);
  public static System.Type[] DynParamTypes = new System.Type[1]
  {
    typeof (UniversalDelegateParam[])
  };
  public System.Type[] tmpTypes = new System.Type[1];
  public System.Type[] tmpTypes2 = new System.Type[2];
  public UniversalDelegate delegat;
  public UniversalDelegateParam[] delegateParams;
  public Action actionCall;

  public void CreateDelegat()
  {
    string generatedKey = ReflectedNodesHelper.GetGeneratedKey((MemberInfo) this.methodInfo);
    if (JitMethodNode.Delegates.TryGetValue(generatedKey, out this.delegat) && this.delegat != null)
      return;
    DynamicMethod dynamicMethod = new DynamicMethod(this.methodInfo.Name + "_Dynamic", (System.Type) null, JitMethodNode.DynParamTypes, typeof (JitMethodNode));
    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
    int index1 = -1;
    int num = -1;
    for (int index2 = 0; index2 <= this.delegateParams.Length - 1; ++index2)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index2];
      ParamDef paramDef = delegateParam.paramDef;
      ilGenerator.DeclareLocal(delegateParam.GetCurrentType());
      if (paramDef.paramMode == ParamMode.Instance)
        index1 = index2;
      if (paramDef.paramMode == ParamMode.Result)
        num = index2;
    }
    for (int index3 = 0; index3 <= this.delegateParams.Length - 1; ++index3)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index3];
      ilGenerator.Emit(OpCodes.Ldarg, 0);
      ilGenerator.Emit(OpCodes.Ldc_I4, index3);
      ilGenerator.Emit(OpCodes.Ldelem_Ref);
      ilGenerator.Emit(OpCodes.Ldfld, delegateParam.ValueField);
      ilGenerator.Emit(OpCodes.Stloc, index3);
    }
    if (index1 >= 0)
      ilGenerator.Emit(this.delegateParams[index1].GetCurrentType().RTIsValueType() ? OpCodes.Ldloca : OpCodes.Ldloc, index1);
    for (int index4 = 0; index4 <= this.delegateParams.Length - 1; ++index4)
    {
      ParamDef paramDef = this.delegateParams[index4].paramDef;
      if (paramDef.paramMode != ParamMode.Instance && paramDef.paramMode != ParamMode.Result)
        ilGenerator.Emit(paramDef.paramMode == ParamMode.In ? OpCodes.Ldloc : OpCodes.Ldloca, index4);
    }
    if (index1 < 0 || this.delegateParams[index1].GetCurrentType().RTIsValueType())
      ilGenerator.Emit(OpCodes.Call, this.methodInfo);
    else
      ilGenerator.Emit(OpCodes.Callvirt, this.methodInfo);
    if (num >= 0)
      ilGenerator.Emit(OpCodes.Stloc, num);
    for (int index5 = 0; index5 <= this.delegateParams.Length - 1; ++index5)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index5];
      ilGenerator.Emit(OpCodes.Ldarg, 0);
      ilGenerator.Emit(OpCodes.Ldc_I4, index5);
      ilGenerator.Emit(OpCodes.Ldelem_Ref);
      ilGenerator.Emit(OpCodes.Ldloc, index5);
      ilGenerator.Emit(OpCodes.Stfld, delegateParam.ValueField);
    }
    ilGenerator.Emit(OpCodes.Ret);
    this.delegat = (UniversalDelegate) ((MethodInfo) dynamicMethod).CreateDelegate(typeof (UniversalDelegate));
    JitMethodNode.Delegates[generatedKey] = this.delegat;
  }

  public void Call()
  {
    if (this.delegat == null)
      return;
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
      this.delegateParams[index].SetFromInput();
    try
    {
      this.delegat(this.delegateParams);
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
  }

  public override bool InitInternal(MethodInfo method)
  {
    this.delegat = (UniversalDelegate) null;
    int count = this.paramDefinitions.Count;
    if (this.instanceDef.paramMode != ParamMode.Undefined)
      ++count;
    if (this.resultDef.paramMode != ParamMode.Undefined)
      ++count;
    this.delegateParams = new UniversalDelegateParam[count];
    int index1 = 0;
    if (this.instanceDef.paramMode != ParamMode.Undefined)
    {
      this.tmpTypes[0] = this.instanceDef.paramType;
      this.delegateParams[index1] = (UniversalDelegateParam) typeof (UniversalDelegateParam<>).RTMakeGenericType(this.tmpTypes).CreateObject();
      this.delegateParams[index1].paramDef = this.instanceDef;
      ++index1;
    }
    if (this.resultDef.paramMode != ParamMode.Undefined)
    {
      this.tmpTypes[0] = this.resultDef.paramType;
      this.delegateParams[index1] = (UniversalDelegateParam) typeof (UniversalDelegateParam<>).RTMakeGenericType(this.tmpTypes).CreateObject();
      this.delegateParams[index1].paramDef = this.resultDef;
      ++index1;
    }
    for (int index2 = 0; index2 <= this.paramDefinitions.Count - 1; ++index2)
    {
      if (this.options.exposeParams && this.paramDefinitions[index2].isParamsArray)
      {
        this.tmpTypes2[0] = this.paramDefinitions[index2].paramType;
        this.tmpTypes2[1] = this.paramDefinitions[index2].arrayType;
        this.delegateParams[index1] = (UniversalDelegateParam) typeof (UniversalDelegateParam<,>).RTMakeGenericType(this.tmpTypes2).CreateObject();
        this.delegateParams[index1].paramsArrayNeeded = this.options.exposeParams;
        this.delegateParams[index1].paramsArrayCount = this.options.exposedParamsCount;
      }
      else
      {
        this.tmpTypes[0] = this.paramDefinitions[index2].paramType;
        this.delegateParams[index1] = (UniversalDelegateParam) typeof (UniversalDelegateParam<>).RTMakeGenericType(this.tmpTypes).CreateObject();
      }
      this.delegateParams[index1].paramDef = this.paramDefinitions[index2];
      ++index1;
    }
    try
    {
      this.CreateDelegat();
    }
    catch
    {
      return false;
    }
    return true;
  }

  public override void RegisterPorts(FlowNode node, ReflectedMethodRegistrationOptions options)
  {
    if (this.actionCall == null)
      this.actionCall = new Action(this.Call);
    if (options.callable)
    {
      FlowOutput output = node.AddFlowOutput(" ");
      node.AddFlowInput(" ", (FlowHandler) (flow =>
      {
        this.Call();
        output.Call(flow);
      }));
    }
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index];
      ParamDef paramDef = delegateParam.paramDef;
      if (paramDef.paramMode == ParamMode.Instance)
      {
        delegateParam.RegisterAsInput(node);
        if (options.callable)
          delegateParam.RegisterAsOutput(node);
      }
      else if (paramDef.paramMode == ParamMode.Result)
        delegateParam.RegisterAsOutput(node, options.callable ? (Action) null : this.actionCall);
      else if (paramDef.paramMode == ParamMode.Ref)
      {
        delegateParam.RegisterAsInput(node);
        delegateParam.RegisterAsOutput(node, options.callable ? (Action) null : this.actionCall);
      }
      else if (paramDef.paramMode == ParamMode.In)
        delegateParam.RegisterAsInput(node);
      else if (paramDef.paramMode == ParamMode.Out)
        delegateParam.RegisterAsOutput(node, options.callable ? (Action) null : this.actionCall);
    }
  }
}
