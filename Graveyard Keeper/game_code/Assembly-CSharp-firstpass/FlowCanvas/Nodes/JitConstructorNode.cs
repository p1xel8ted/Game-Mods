// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.JitConstructorNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace FlowCanvas.Nodes;

public class JitConstructorNode : BaseReflectedConstructorNode
{
  public static Dictionary<string, UniversalDelegate> Delegates = new Dictionary<string, UniversalDelegate>((IEqualityComparer<string>) StringComparer.Ordinal);
  public static Type[] DynParamTypes = new Type[1]
  {
    typeof (UniversalDelegateParam[])
  };
  public Type[] tmpTypes = new Type[1];
  public Type[] tmpTypes2 = new Type[2];
  public UniversalDelegate delegat;
  public UniversalDelegateParam[] delegateParams;
  public Action actionCall;

  public void CreateDelegat()
  {
    string generatedKey = ReflectedNodesHelper.GetGeneratedKey((MemberInfo) this.constructorInfo);
    if (JitConstructorNode.Delegates.TryGetValue(generatedKey, out this.delegat) && this.delegat != null)
      return;
    DynamicMethod dynamicMethod = new DynamicMethod("Constructor_Dynamic", (Type) null, JitConstructorNode.DynParamTypes, typeof (JitMethodNode));
    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
    int num = -1;
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index];
      ParamDef paramDef = delegateParam.paramDef;
      ilGenerator.DeclareLocal(delegateParam.GetCurrentType());
      if (paramDef.paramMode == ParamMode.Result)
        num = index;
    }
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index];
      ilGenerator.Emit(OpCodes.Ldarg, 0);
      ilGenerator.Emit(OpCodes.Ldc_I4, index);
      ilGenerator.Emit(OpCodes.Ldelem_Ref);
      ilGenerator.Emit(OpCodes.Ldfld, delegateParam.ValueField);
      ilGenerator.Emit(OpCodes.Stloc, index);
    }
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
    {
      ParamDef paramDef = this.delegateParams[index].paramDef;
      if (paramDef.paramMode != ParamMode.Instance && paramDef.paramMode != ParamMode.Result)
      {
        if (paramDef.paramMode == ParamMode.In)
          ilGenerator.Emit(OpCodes.Ldloc, index);
        else
          ilGenerator.Emit(OpCodes.Ldloca, index);
      }
    }
    ilGenerator.Emit(OpCodes.Newobj, this.constructorInfo);
    if (num >= 0)
      ilGenerator.Emit(OpCodes.Stloc, num);
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index];
      ilGenerator.Emit(OpCodes.Ldarg, 0);
      ilGenerator.Emit(OpCodes.Ldc_I4, index);
      ilGenerator.Emit(OpCodes.Ldelem_Ref);
      ilGenerator.Emit(OpCodes.Ldloc, index);
      ilGenerator.Emit(OpCodes.Stfld, delegateParam.ValueField);
    }
    ilGenerator.Emit(OpCodes.Ret);
    this.delegat = (UniversalDelegate) ((MethodInfo) dynamicMethod).CreateDelegate(typeof (UniversalDelegate));
    JitConstructorNode.Delegates[generatedKey] = this.delegat;
  }

  public void Call()
  {
    if (this.delegat == null)
      return;
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
      this.delegateParams[index].SetFromInput();
    this.delegat(this.delegateParams);
  }

  public override bool InitInternal(ConstructorInfo constructor)
  {
    this.delegat = (UniversalDelegate) null;
    int count = this.paramDefinitions.Count;
    if (this.resultDef.paramMode != ParamMode.Undefined)
      ++count;
    this.delegateParams = new UniversalDelegateParam[count];
    int index1 = 0;
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
      if (paramDef.paramMode == ParamMode.Result)
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
