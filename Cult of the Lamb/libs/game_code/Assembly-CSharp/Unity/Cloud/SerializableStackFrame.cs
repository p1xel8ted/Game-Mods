// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.SerializableStackFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud;

public class SerializableStackFrame
{
  [CompilerGenerated]
  public string \u003CDeclaringType\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CFileColumn\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CFileLine\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CFileName\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CMethod\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CMethodName\u003Ek__BackingField;

  public SerializableStackFrame()
  {
  }

  public SerializableStackFrame(StackFrame stackFrame)
  {
    MethodBase method = stackFrame.GetMethod();
    Type declaringType = method.DeclaringType;
    this.DeclaringType = declaringType != (Type) null ? declaringType.FullName : (string) null;
    this.Method = method.ToString();
    this.MethodName = method.Name;
    this.FileName = stackFrame.GetFileName();
    this.FileLine = stackFrame.GetFileLineNumber();
    this.FileColumn = stackFrame.GetFileColumnNumber();
  }

  public string DeclaringType
  {
    get => this.\u003CDeclaringType\u003Ek__BackingField;
    set => this.\u003CDeclaringType\u003Ek__BackingField = value;
  }

  public int FileColumn
  {
    get => this.\u003CFileColumn\u003Ek__BackingField;
    set => this.\u003CFileColumn\u003Ek__BackingField = value;
  }

  public int FileLine
  {
    get => this.\u003CFileLine\u003Ek__BackingField;
    set => this.\u003CFileLine\u003Ek__BackingField = value;
  }

  public string FileName
  {
    get => this.\u003CFileName\u003Ek__BackingField;
    set => this.\u003CFileName\u003Ek__BackingField = value;
  }

  public string Method
  {
    get => this.\u003CMethod\u003Ek__BackingField;
    set => this.\u003CMethod\u003Ek__BackingField = value;
  }

  public string MethodName
  {
    get => this.\u003CMethodName\u003Ek__BackingField;
    set => this.\u003CMethodName\u003Ek__BackingField = value;
  }
}
