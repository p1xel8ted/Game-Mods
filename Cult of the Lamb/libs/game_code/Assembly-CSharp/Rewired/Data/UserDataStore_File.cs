// Decompiled with JetBrains decompiler
// Type: Rewired.Data.UserDataStore_File
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Utils.Libraries.TinyJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

#nullable disable
namespace Rewired.Data;

public class UserDataStore_File : UserDataStore_KeyValue
{
  public new static string thisScriptName = typeof (UserDataStore_File).Name;
  public new const string logPrefix = "Rewired: ";
  public const string defaultExtensionText = ".json";
  public const string defaultExtensionBinary = ".bin";
  public const string defaultFileName = "RewiredSaveData.json";
  [Tooltip("The data file name. Changing this will make saved data already stored with the old file name no longer accessible.")]
  [SerializeField]
  public string _fileName = "RewiredSaveData.json";
  [Tooltip("Determines if the file should be stored as binary or text. Changing this will make saved data already stored no longer accessible.")]
  [SerializeField]
  public UserDataStore_File.DataFormat _dataFormat;
  [NonSerialized]
  public string __directory;
  [NonSerialized]
  public UserDataStore_File.DataStore _dataStore;
  [NonSerialized]
  public UserDataStore_File.IDataHandler __dataHandler;
  [NonSerialized]
  public bool _initialized;

  public string directory
  {
    get
    {
      return string.IsNullOrEmpty(this.__directory) ? (this.__directory = Application.persistentDataPath) : this.__directory;
    }
    set
    {
      this.__directory = value;
      if (!this._initialized)
        return;
      this.OnDataSourceChanged();
    }
  }

  public string fileName
  {
    get => this._fileName;
    set
    {
      this._fileName = value;
      if (!this._initialized)
        return;
      this.OnDataSourceChanged();
    }
  }

  public UserDataStore_File.DataFormat dataFormat
  {
    get => this._dataFormat;
    set
    {
      this._dataFormat = value;
      if (!this._initialized)
        return;
      this.OnDataSourceChanged();
    }
  }

  public UserDataStore_File.IDataHandler dataHandler
  {
    get
    {
      return this.__dataHandler == null ? (this.__dataHandler = (UserDataStore_File.IDataHandler) new UserDataStore_File.LocalFileDataHandler((Func<UserDataStore_File.DataFormat>) (() => this._dataFormat), (UserDataStore_File.Codec) new UserDataStore_File.CLZF2())) : this.__dataHandler;
    }
    set
    {
      this.__dataHandler = value;
      if (!this._initialized)
        return;
      this.OnDataSourceChanged();
    }
  }

  public override UserDataStore_KeyValue.IDataStore dataStore
  {
    get => (UserDataStore_KeyValue.IDataStore) this._dataStore;
  }

  public virtual void SetInitialValues()
  {
  }

  public override void OnInitialize()
  {
    this.SetInitialValues();
    this._initialized = true;
    this.OnDataSourceChanged();
    base.OnInitialize();
  }

  public void OnDataSourceChanged()
  {
    this._dataStore = new UserDataStore_File.DataStore(!string.IsNullOrEmpty(this._fileName) ? this._fileName : "RewiredSaveData.json", this.directory, this.dataHandler);
  }

  [CompilerGenerated]
  public UserDataStore_File.DataFormat \u003Cget_dataHandler\u003Eb__17_0() => this._dataFormat;

  public sealed class DataStore : UserDataStore_KeyValue.IDataStore
  {
    public Dictionary<string, object> _data;
    public string _absFilePath;
    public UserDataStore_File.IDataHandler _dataHandler;

    public DataStore(
      string fileName,
      string absDirectory,
      UserDataStore_File.IDataHandler dataHandler)
    {
      this._absFilePath = Path.Combine(absDirectory, fileName);
      this._dataHandler = dataHandler != null ? dataHandler : throw new ArgumentNullException(nameof (dataHandler));
      this._data = new Dictionary<string, object>();
      this.Load();
    }

    public bool TryGetValue(string key, out object value)
    {
      if (!string.IsNullOrEmpty(key))
        return this._data.TryGetValue(key, out value);
      value = (object) null;
      return false;
    }

    public bool SetValue(string key, object value)
    {
      if (string.IsNullOrEmpty(key))
        return false;
      this._data[key] = value;
      return true;
    }

    public bool Save()
    {
      try
      {
        return this._dataHandler.Save(this._absFilePath, JsonWriter.ToJson((object) this._data));
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        return false;
      }
    }

    public bool Load()
    {
      try
      {
        string data;
        int num = this._dataHandler.Load(this._absFilePath, out data) ? 1 : 0;
        if (num != 0)
          this._data = JsonParser.FromJson<Dictionary<string, object>>(data) ?? new Dictionary<string, object>();
        return num != 0;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        return false;
      }
    }

    public bool Clear()
    {
      bool flag;
      try
      {
        flag = this._dataHandler.Clear(this._absFilePath);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        flag = false;
      }
      this._data.Clear();
      return flag;
    }
  }

  public sealed class LocalFileDataHandler : UserDataStore_File.IDataHandler
  {
    public Func<UserDataStore_File.DataFormat> _dataFormatDelegate;
    public UserDataStore_File.Codec _codec;

    public LocalFileDataHandler(
      Func<UserDataStore_File.DataFormat> dataFormatDelegate,
      UserDataStore_File.Codec codec)
    {
      this._dataFormatDelegate = dataFormatDelegate != null ? dataFormatDelegate : throw new ArgumentNullException(nameof (dataFormatDelegate));
      if (codec == null)
        codec = (UserDataStore_File.Codec) new UserDataStore_File.UTF8Text();
      this._codec = codec;
    }

    public bool Load(string absoluteFilePath, out string data)
    {
      data = (string) null;
      if (string.IsNullOrEmpty(absoluteFilePath))
        return false;
      if (!File.Exists(absoluteFilePath))
        return false;
      try
      {
        switch (this._dataFormatDelegate())
        {
          case UserDataStore_File.DataFormat.Text:
            data = File.ReadAllText(absoluteFilePath);
            return !string.IsNullOrEmpty(data);
          case UserDataStore_File.DataFormat.Binary:
            byte[] data1 = File.ReadAllBytes(absoluteFilePath);
            data = this._codec.Decode(data1);
            return data1 != null && data1.Length != 0;
          default:
            throw new NotImplementedException();
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        return false;
      }
    }

    public bool Save(string absoluteFilePath, string data)
    {
      if (string.IsNullOrEmpty(absoluteFilePath))
        return false;
      try
      {
        if (!Directory.Exists(Path.GetDirectoryName(absoluteFilePath)))
          Directory.CreateDirectory(Path.GetDirectoryName(absoluteFilePath));
        switch (this._dataFormatDelegate())
        {
          case UserDataStore_File.DataFormat.Text:
            File.WriteAllText(absoluteFilePath, data);
            break;
          case UserDataStore_File.DataFormat.Binary:
            File.WriteAllBytes(absoluteFilePath, this._codec.Encode(data));
            break;
          default:
            throw new NotImplementedException();
        }
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        return false;
      }
    }

    public bool Clear(string absoluteFilePath)
    {
      if (string.IsNullOrEmpty(absoluteFilePath))
        return false;
      try
      {
        if (File.Exists(absoluteFilePath))
        {
          File.Delete(absoluteFilePath);
          return true;
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
      }
      return false;
    }
  }

  public abstract class Codec
  {
    public abstract byte[] Encode(string @string);

    public abstract string Decode(byte[] data);
  }

  public sealed class UTF8Text : UserDataStore_File.Codec
  {
    public override byte[] Encode(string @string) => Encoding.UTF8.GetBytes(@string);

    public override string Decode(byte[] data) => Encoding.UTF8.GetString(data);
  }

  public sealed class CLZF2 : UserDataStore_File.Codec
  {
    public Rewired.Utils.Libraries.CLZF2.CLZF2 _cLZF2;

    public CLZF2() => this._cLZF2 = new Rewired.Utils.Libraries.CLZF2.CLZF2();

    public override byte[] Encode(string @string)
    {
      return this._cLZF2.Compress(Encoding.UTF8.GetBytes(@string));
    }

    public override string Decode(byte[] data)
    {
      return Encoding.UTF8.GetString(this._cLZF2.Decompress(data));
    }
  }

  public interface IDataHandler
  {
    bool Load(string absoluteFilePath, out string data);

    bool Save(string absoluteFilePath, string data);

    bool Clear(string absoluteFilePath);
  }

  public enum DataFormat
  {
    Text,
    Binary,
  }
}
