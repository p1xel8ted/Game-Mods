// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.JitFieldNode
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

public class JitFieldNode : BaseReflectedFieldNode
{
  public static Dictionary<string, UniversalDelegate> GetDelegates = new Dictionary<string, UniversalDelegate>((IEqualityComparer<string>) StringComparer.Ordinal);
  public static Dictionary<string, UniversalDelegate> SetDelegates = new Dictionary<string, UniversalDelegate>((IEqualityComparer<string>) StringComparer.Ordinal);
  public static Type[] DynParamTypes = new Type[1]
  {
    typeof (UniversalDelegateParam[])
  };
  public Type[] tmpTypes = new Type[1];
  public UniversalDelegate getDelegat;
  public UniversalDelegate setDelegat;
  public UniversalDelegateParam[] delegateParams;
  public Action getValue;
  public bool isConstant;

  public void CreateDelegates()
  {
    string generatedKey = ReflectedNodesHelper.GetGeneratedKey((MemberInfo) this.fieldInfo);
    if (!JitFieldNode.GetDelegates.TryGetValue(generatedKey, out this.getDelegat) || this.getDelegat == null)
    {
      DynamicMethod dynamicMethod = new DynamicMethod(this.fieldInfo.Name + "_DynamicGet", (Type) null, JitFieldNode.DynParamTypes, typeof (JitFieldNode));
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      int index1 = -1;
      int index2 = -1;
      int num1 = 0;
      int num2 = -1;
      int num3 = -1;
      for (int index3 = 0; index3 <= this.delegateParams.Length - 1; ++index3)
      {
        ParamDef paramDef = this.delegateParams[index3].paramDef;
        if (paramDef.paramMode == ParamMode.Instance)
        {
          index1 = index3;
          ilGenerator.DeclareLocal(paramDef.paramType);
          num2 = num1;
          ++num1;
        }
        if (paramDef.paramMode == ParamMode.Result)
        {
          index2 = index3;
          ilGenerator.DeclareLocal(paramDef.paramType);
          num3 = num1;
          ++num1;
        }
      }
      if (index1 >= 0)
      {
        ilGenerator.Emit(OpCodes.Ldarg, 0);
        ilGenerator.Emit(OpCodes.Ldc_I4, index1);
        ilGenerator.Emit(OpCodes.Ldelem_Ref);
        ilGenerator.Emit(OpCodes.Ldfld, this.delegateParams[index1].ValueField);
        ilGenerator.Emit(OpCodes.Stloc, num2);
        ilGenerator.Emit(OpCodes.Ldloc, num2);
        ilGenerator.Emit(OpCodes.Ldfld, this.fieldInfo);
      }
      else
        ilGenerator.Emit(OpCodes.Ldsfld, this.fieldInfo);
      if (index2 >= 0)
      {
        ilGenerator.Emit(OpCodes.Stloc, num3);
        ilGenerator.Emit(OpCodes.Ldarg, 0);
        ilGenerator.Emit(OpCodes.Ldc_I4, index2);
        ilGenerator.Emit(OpCodes.Ldelem_Ref);
        ilGenerator.Emit(OpCodes.Ldloc, num3);
        ilGenerator.Emit(OpCodes.Stfld, this.delegateParams[index2].ValueField);
      }
      ilGenerator.Emit(OpCodes.Ret);
      this.getDelegat = (UniversalDelegate) ((MethodInfo) dynamicMethod).CreateDelegate(typeof (UniversalDelegate));
      JitFieldNode.GetDelegates[generatedKey] = this.getDelegat;
    }
    if (JitFieldNode.SetDelegates.TryGetValue(generatedKey, out this.setDelegat) && this.setDelegat != null || this.fieldInfo.IsReadOnly())
      return;
    DynamicMethod dynamicMethod1 = new DynamicMethod(this.fieldInfo.Name + "_DynamicSet", (Type) null, JitFieldNode.DynParamTypes, typeof (JitFieldNode));
    ILGenerator ilGenerator1 = dynamicMethod1.GetILGenerator();
    int index4 = -1;
    int index5 = -1;
    int num4 = 0;
    int num5 = -1;
    int num6 = -1;
    for (int index6 = 0; index6 <= this.delegateParams.Length - 1; ++index6)
    {
      ParamDef paramDef = this.delegateParams[index6].paramDef;
      if (paramDef.paramMode == ParamMode.Instance)
      {
        index4 = index6;
        ilGenerator1.DeclareLocal(paramDef.paramType);
        num5 = num4;
        ++num4;
      }
      if (paramDef.paramMode == ParamMode.Result)
      {
        index5 = index6;
        ilGenerator1.DeclareLocal(paramDef.paramType);
        num6 = num4;
        ++num4;
      }
    }
    if (index4 >= 0)
    {
      ilGenerator1.Emit(OpCodes.Ldarg, 0);
      ilGenerator1.Emit(OpCodes.Ldc_I4, index4);
      ilGenerator1.Emit(OpCodes.Ldelem_Ref);
      ilGenerator1.Emit(OpCodes.Ldfld, this.delegateParams[index4].ValueField);
      ilGenerator1.Emit(OpCodes.Stloc, num5);
    }
    if (index5 >= 0)
    {
      ilGenerator1.Emit(OpCodes.Ldarg, 0);
      ilGenerator1.Emit(OpCodes.Ldc_I4, index5);
      ilGenerator1.Emit(OpCodes.Ldelem_Ref);
      ilGenerator1.Emit(OpCodes.Ldfld, this.delegateParams[index5].ValueField);
      ilGenerator1.Emit(OpCodes.Stloc, num6);
      if (index4 >= 0)
      {
        ilGenerator1.Emit(this.delegateParams[index4].GetCurrentType().RTIsValueType() ? OpCodes.Ldloca : OpCodes.Ldloc, num5);
        ilGenerator1.Emit(OpCodes.Ldloc, num6);
        ilGenerator1.Emit(OpCodes.Stfld, this.fieldInfo);
      }
      else
      {
        ilGenerator1.Emit(OpCodes.Ldloc, num6);
        ilGenerator1.Emit(OpCodes.Stsfld, this.fieldInfo);
      }
    }
    if (index4 >= 0)
    {
      ilGenerator1.Emit(OpCodes.Ldarg, 0);
      ilGenerator1.Emit(OpCodes.Ldc_I4, index4);
      ilGenerator1.Emit(OpCodes.Ldelem_Ref);
      ilGenerator1.Emit(OpCodes.Ldloc, num5);
      ilGenerator1.Emit(OpCodes.Stfld, this.delegateParams[index4].ValueField);
    }
    ilGenerator1.Emit(OpCodes.Ret);
    this.setDelegat = (UniversalDelegate) ((MethodInfo) dynamicMethod1).CreateDelegate(typeof (UniversalDelegate));
    JitFieldNode.SetDelegates[generatedKey] = this.setDelegat;
  }

  public override bool InitInternal(FieldInfo field)
  {
    this.isConstant = field.IsStatic && field.IsReadOnly();
    if (this.isConstant)
    {
      this.delegateParams = new UniversalDelegateParam[1];
      this.tmpTypes[0] = field.FieldType;
      this.delegateParams[0] = (UniversalDelegateParam) typeof (UniversalDelegateParam<>).RTMakeGenericType(this.tmpTypes).CreateObject();
      this.delegateParams[0].paramDef = this.resultDef;
      this.delegateParams[0].SetFromValue(field.GetValue((object) null));
      return true;
    }
    this.getDelegat = (UniversalDelegate) null;
    this.setDelegat = (UniversalDelegate) null;
    int length = 0;
    if (this.instanceDef.paramMode != ParamMode.Undefined)
      ++length;
    if (this.resultDef.paramMode != ParamMode.Undefined)
      ++length;
    this.delegateParams = new UniversalDelegateParam[length];
    int index = 0;
    if (this.instanceDef.paramMode != ParamMode.Undefined)
    {
      this.tmpTypes[0] = this.instanceDef.paramType;
      this.delegateParams[index] = (UniversalDelegateParam) typeof (UniversalDelegateParam<>).RTMakeGenericType(this.tmpTypes).CreateObject();
      this.delegateParams[index].paramDef = this.instanceDef;
      ++index;
    }
    if (this.resultDef.paramMode != ParamMode.Undefined)
    {
      this.tmpTypes[0] = this.resultDef.paramType;
      this.delegateParams[index] = (UniversalDelegateParam) typeof (UniversalDelegateParam<>).RTMakeGenericType(this.tmpTypes).CreateObject();
      this.delegateParams[index].paramDef = this.resultDef;
    }
    try
    {
      this.CreateDelegates();
    }
    catch
    {
      return false;
    }
    return true;
  }

  public void SetValue()
  {
    if (this.setDelegat == null || this.fieldInfo.IsReadOnly())
      return;
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
      this.delegateParams[index].SetFromInput();
    this.setDelegat(this.delegateParams);
  }

  public void GetValue()
  {
    if (this.getDelegat == null)
      return;
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
      this.delegateParams[index].SetFromInput();
    this.getDelegat(this.delegateParams);
  }

  public override void RegisterPorts(FlowNode node, ReflectedFieldNodeWrapper.AccessMode accessMode)
  {
    if (this.isConstant)
      this.delegateParams[0].RegisterAsOutput(node);
    if (this.getValue == null)
      this.getValue = new Action(this.GetValue);
    if (accessMode == ReflectedFieldNodeWrapper.AccessMode.SetField && !this.fieldInfo.IsReadOnly())
    {
      FlowOutput output = node.AddFlowOutput(" ");
      node.AddFlowInput(" ", (FlowHandler) (flow =>
      {
        this.SetValue();
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
        if (accessMode == ReflectedFieldNodeWrapper.AccessMode.SetField && !this.fieldInfo.IsReadOnly())
          delegateParam.RegisterAsOutput(node);
      }
      if (paramDef.paramMode == ParamMode.Result)
      {
        if (accessMode == ReflectedFieldNodeWrapper.AccessMode.SetField && !this.fieldInfo.IsReadOnly())
          delegateParam.RegisterAsInput(node);
        else
          delegateParam.RegisterAsOutput(node, this.getValue);
      }
    }
  }
}
