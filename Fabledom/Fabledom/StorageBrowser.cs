using System;
using Nielsen;

namespace Fabledom;

internal static class StorageBrowser
{
    private sealed class ResourceSummary
    {
        public ResourceData Data { get; set; }
        public int Stored { get; set; }
        public int Available { get; set; }
        public int Reserved { get; set; }
        public int StorageCount { get; set; }
        public int Locked { get; set; }
        public int GeneralStorage { get; set; }
        public int ProductionStorage { get; set; }
        public int HousingStorage { get; set; }
    }

    private static readonly int WindowId = "Fabledom.StorageBrowser".GetHashCode();
    private static Rect _windowRect = new(40f, 40f, 980f, 640f);
    private static Vector2 _resourceScroll;
    private static Vector2 _storageScroll;
    private static string _resourceFilter = string.Empty;
    private static string _storageFilter = string.Empty;
    private static string _selectedResourceKey = string.Empty;
    private static bool _showZeroResources;
    private static bool _showEmptyStorages;
    private static GUIStyle _windowStyle;
    private static GUIStyle _panelStyle;
    private static GUIStyle _cardStyle;
    private static GUIStyle _labelStyle;
    private static GUIStyle _subtleLabelStyle;
    private static GUIStyle _buttonStyle;
    private static GUIStyle _selectedButtonStyle;
    private static GUIStyle _textFieldStyle;
    private static GUIStyle _toggleStyle;

    internal static bool IsVisible { get; private set; }

    internal static void Toggle()
    {
        IsVisible = !IsVisible;
        UpdateEditingState();
    }

    internal static void Hide()
    {
        if (!IsVisible)
        {
            return;
        }

        IsVisible = false;
        UpdateEditingState();
    }

    internal static void Draw()
    {
        if (!IsVisible || !IsGameplayState())
        {
            if (IsVisible && !IsGameplayState())
            {
                Hide();
            }

            return;
        }

        UpdateEditingState();
        EnsureStyles();
        _windowRect = GUILayout.Window(WindowId, _windowRect, DrawWindow, "Storage Browser", _windowStyle);
    }

    private static void DrawWindow(int _)
    {
        var summaries = BuildResourceSummaries();
        var filteredSummaries = FilterResources(summaries);
        EnsureSelectedResource(filteredSummaries);

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Resource Filter", _labelStyle, GUILayout.Width(100f));
        _resourceFilter = GUILayout.TextField(_resourceFilter, _textFieldStyle, GUILayout.Width(220f));
        GUILayout.Space(16f);
        GUILayout.Label("Storage Filter", _labelStyle, GUILayout.Width(95f));
        _storageFilter = GUILayout.TextField(_storageFilter, _textFieldStyle, GUILayout.Width(220f));
        GUILayout.Space(16f);
        _showZeroResources = GUILayout.Toggle(_showZeroResources, "Show Zero Resources", _toggleStyle, GUILayout.Width(150f));
        _showEmptyStorages = GUILayout.Toggle(_showEmptyStorages, "Show Empty Storages", _toggleStyle, GUILayout.Width(150f));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Close", _buttonStyle, GUILayout.Width(80f)))
        {
            Hide();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(8f);

        GUILayout.BeginHorizontal();
        DrawResourcePane(filteredSummaries);
        GUILayout.Space(10f);
        DrawStoragePane();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUI.DragWindow(new Rect(0f, 0f, 10000f, 22f));
    }

    private static void DrawResourcePane(List<ResourceSummary> summaries)
    {
        GUILayout.BeginVertical(_panelStyle, GUILayout.Width(330f), GUILayout.ExpandHeight(true));
        GUILayout.Label("Resources", _labelStyle);
        GUILayout.Label("Free = not reserved | Total = all stored | Claimed = already reserved", _subtleLabelStyle);
        GUILayout.Space(4f);

        _resourceScroll = GUILayout.BeginScrollView(_resourceScroll);
        foreach (var summary in summaries)
        {
            var title = FormatLabel(summary.Data.title.GetLocalizedString());
            var isSelected = summary.Data.key == _selectedResourceKey;
            var label = $"{title} [{summary.Data.key}]  Free:{summary.Available}  Total:{summary.Stored}  Claimed:{summary.Reserved}  ({summary.StorageCount})";
            if (GUILayout.Toggle(isSelected, label, isSelected ? _selectedButtonStyle : _buttonStyle))
            {
                _selectedResourceKey = summary.Data.key;
            }
        }

        if (summaries.Count == 0)
        {
            GUILayout.Label("No resources matched the current filter.", _subtleLabelStyle);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private static void DrawStoragePane()
    {
        GUILayout.BeginVertical(_panelStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        if (string.IsNullOrEmpty(_selectedResourceKey))
        {
            GUILayout.Label("Select a resource to inspect its storages.", _labelStyle);
            GUILayout.EndVertical();
            return;
        }

        var resourceData = DataManager.Instance.GetResourceDataFromKey(_selectedResourceKey);
        var storages = BuildStorageRows(_selectedResourceKey);
        var title = resourceData ? FormatLabel(resourceData.title.GetLocalizedString()) : _selectedResourceKey;
        GUILayout.Label($"Storages for {title}", _labelStyle);
        GUILayout.Label(BuildSelectedResourceSummary(_selectedResourceKey, storages), _subtleLabelStyle);
        GUILayout.Label("Free = usable now | Total = all stored here | Claimed = already reserved | Space = free capacity", _subtleLabelStyle);
        GUILayout.Space(4f);

        _storageScroll = GUILayout.BeginScrollView(_storageScroll);
        foreach (var row in storages)
        {
            GUILayout.BeginVertical(_cardStyle);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Go", _buttonStyle, GUILayout.Width(40f)))
            {
                FocusStorage(row.Storage);
            }

            GUILayout.Label($"{row.Title} [{row.Type}]", _labelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label(row.Flags, _subtleLabelStyle, GUILayout.Width(200f));
            GUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(row.Context))
            {
                GUILayout.Label(row.Context, _subtleLabelStyle);
            }
            GUILayout.Label($"Free to use now {row.Available} | Total here {row.Stored} | Already claimed {row.Reserved} | Free space {row.Remaining}", _labelStyle);
            GUILayout.EndVertical();
        }

        if (storages.Count == 0)
        {
            GUILayout.Label("No storages matched the current filters.", _subtleLabelStyle);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private static List<ResourceSummary> BuildResourceSummaries()
    {
        var summaries = new Dictionary<string, ResourceSummary>();
        foreach (var storage in Storage.AllInstances)
        {
            if (!storage || !storage.worldObject || !storage.IsAvailable())
            {
                continue;
            }

            foreach (var stored in storage.resourcesStored)
            {
                if (stored == null)
                {
                    continue;
                }

                var data = DataManager.Instance.GetResourceDataFromKey(stored.key);
                if (!data || data.hideFromGame)
                {
                    continue;
                }

                if (!summaries.TryGetValue(stored.key, out var summary))
                {
                    summary = new ResourceSummary { Data = data };
                    summaries.Add(stored.key, summary);
                }

                summary.Stored += stored.amount;
                summary.Available += stored.GetAvailable();
                summary.Reserved += stored.reserved;
                if (stored.isLocked)
                {
                    summary.Locked += stored.amount;
                }
                if (storage.isGeneralStorage)
                {
                    summary.GeneralStorage += stored.amount;
                }
                if (storage.GetComponent<ProductionPlace>())
                {
                    summary.ProductionStorage += stored.amount;
                }
                if (storage.GetComponent<Housing>())
                {
                    summary.HousingStorage += stored.amount;
                }
                if (stored.amount > 0 || stored.reserved > 0 || storage.GetRemainingCapacity(stored.key) > 0)
                {
                    summary.StorageCount++;
                }
            }
        }

        return summaries.Values
            .OrderByDescending(x => x.Available)
            .ThenBy(x => x.Data.title.GetLocalizedString())
            .ToList();
    }

    private static List<ResourceSummary> FilterResources(List<ResourceSummary> summaries)
    {
        var filter = _resourceFilter.Trim();
        return summaries
            .Where(x => _showZeroResources || x.Stored > 0 || x.Reserved > 0)
            .Where(x => string.IsNullOrWhiteSpace(filter)
                        || x.Data.key.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0
                        || x.Data.title.GetLocalizedString().IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();
    }

    private static void EnsureSelectedResource(List<ResourceSummary> summaries)
    {
        if (summaries.Count == 0)
        {
            _selectedResourceKey = string.Empty;
            return;
        }

        if (summaries.All(x => x.Data.key != _selectedResourceKey))
        {
            _selectedResourceKey = summaries[0].Data.key;
        }
    }

    private static List<StorageRow> BuildStorageRows(string resourceKey)
    {
        var filter = _storageFilter.Trim();
        return Storage.AllInstances
            .Where(x => x && x.worldObject && x.IsAvailable())
            .Select(x => BuildStorageRow(x, resourceKey))
            .Where(x => x.SupportsResource)
            .Where(x => _showEmptyStorages || x.Stored > 0 || x.Reserved > 0)
            .Where(x => string.IsNullOrWhiteSpace(filter)
                        || x.Title.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0
                        || x.Storage.gameObject.name.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0)
            .OrderByDescending(x => x.Available)
            .ThenByDescending(x => x.Stored)
            .ThenBy(x => x.Title)
            .ToList();
    }

    private static StorageRow BuildStorageRow(Storage storage, string resourceKey)
    {
        var title = FormatLabel(storage.worldObject.data.title.GetLocalizedString());
        var capacity = storage.GetResourceCapacity(resourceKey);
        var supports = capacity != null || storage.GetResourceStored(resourceKey) > 0 || storage.GetResourceReserved(resourceKey) > 0;
        var type = DescribeStorageType(storage);
        var context = DescribeStorageContext(storage, resourceKey);
        var flags = new List<string>();
        if (storage.isGeneralStorage) flags.Add("General");
        if (storage.isBeingEmptied) flags.Add("Emptying");
        if (supports && storage.IsResourceLocked(resourceKey)) flags.Add("Locked");
        if (!storage.IsAvailable()) flags.Add("Incomplete");

        return new StorageRow
        {
            Storage = storage,
            Title = title,
            Type = type,
            Context = context,
            Available = storage.GetResourceAvailable(resourceKey),
            Stored = storage.GetResourceStored(resourceKey),
            Reserved = storage.GetResourceReserved(resourceKey),
            Remaining = supports ? storage.GetRemainingCapacity(resourceKey) : 0,
            SupportsResource = supports,
            Flags = flags.Count > 0 ? string.Join(" | ", flags) : "Standard"
        };
    }

    private static string BuildSelectedResourceSummary(string resourceKey, List<StorageRow> storages)
    {
        var summary = BuildResourceSummaries().FirstOrDefault(x => x.Data.key == resourceKey);
        if (summary == null)
        {
            return "No town-wide summary available.";
        }

        var parts = new List<string>
        {
            $"Town total {summary.Stored}",
            $"usable now {summary.Available}",
            $"claimed {summary.Reserved}",
            $"locked {summary.Locked}",
            $"general storage {summary.GeneralStorage}",
            $"production {summary.ProductionStorage}",
            $"housing {summary.HousingStorage}"
        };

        var biggestStock = storages.OrderByDescending(x => x.Stored).FirstOrDefault();
        if (biggestStock != null && biggestStock.Stored > 0)
        {
            parts.Add($"biggest stock: {biggestStock.Title} ({biggestStock.Stored})");
        }

        var mostFree = storages.OrderByDescending(x => x.Available).FirstOrDefault();
        if (mostFree != null && mostFree.Available > 0)
        {
            parts.Add($"most usable now: {mostFree.Title} ({mostFree.Available})");
        }

        return string.Join(" | ", parts);
    }

    private static string DescribeStorageType(Storage storage)
    {
        if (storage.GetComponent<Granary>()) return "Granary";
        if (storage.GetComponent<StockpileLot>()) return "Stockpile";
        if (storage.GetComponent<Market>()) return "Market";
        if (storage.GetComponent<Shop>()) return "Shop";
        if (storage.GetComponent<Tavern>()) return "Tavern";
        if (storage.GetComponent<Housing>()) return "Housing";
        if (storage.GetComponent<ProductionPlace>()) return "Production";
        if (storage.GetComponent<Depot>()) return "Depot";
        return storage.isGeneralStorage ? "General" : "Building";
    }

    private static string DescribeStorageContext(Storage storage, string resourceKey)
    {
        var production = storage.GetComponent<ProductionPlace>();
        if (production)
        {
            var path = production.GetResourcePathData();
            if (path.output.resourceData.key == resourceKey)
            {
                return $"Output buffer for {FormatLabel(path.output.resourceData.title.GetLocalizedString())}";
            }

            if (path.inputs.Any(x => x.resourceData.key == resourceKey))
            {
                return $"Input buffer for {FormatLabel(path.output.resourceData.title.GetLocalizedString())}";
            }
        }

        var housing = storage.GetComponent<Housing>();
        if (housing)
        {
            return $"Household storage for {housing.residents.Count}/{housing.GetTotalCapacity()} residents";
        }

        if (storage.GetComponent<Granary>())
        {
            return "General food storage";
        }

        if (storage.GetComponent<StockpileLot>())
        {
            return "General industry storage";
        }

        if (storage.GetComponent<Market>())
        {
            return "Local market stock";
        }

        if (storage.GetComponent<Shop>())
        {
            return "Shop storage";
        }

        if (storage.GetComponent<Tavern>())
        {
            return "Tavern stock";
        }

        return storage.isGeneralStorage ? "General-purpose storage" : "Building-local storage";
    }

    private static void FocusStorage(Storage storage)
    {
        if (!storage)
        {
            return;
        }

        GameplayCameraManager.Instance?.controller?.ForcePositionTransition(storage.transform.position, 1f);
        if (storage.TryGetComponent<Nielsen.Selectable>(out var selectable))
        {
            selectable.Select();
        }
    }

    private static bool IsGameplayState()
    {
        return GameManager.Instance && GameManager.Instance.GetMainState() is MainState.PLAYING or MainState.PAUSE;
    }

    private static void UpdateEditingState()
    {
        if (UIManager.Instance)
        {
            UIManager.Instance.isEditingText = IsVisible;
        }
    }

    private static void EnsureStyles()
    {
        if (_windowStyle != null)
        {
            return;
        }

        _windowStyle = new GUIStyle(GUI.skin.window)
        {
            padding = new RectOffset(12, 12, 28, 12)
        };
        _windowStyle.normal.background = MakeTex(new Color(0.08f, 0.08f, 0.09f, 0.97f));
        _windowStyle.normal.textColor = new Color(0.96f, 0.93f, 0.88f);

        _panelStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(10, 10, 10, 10)
        };
        _panelStyle.normal.background = MakeTex(new Color(0.12f, 0.12f, 0.14f, 0.96f));
        _panelStyle.normal.textColor = new Color(0.92f, 0.9f, 0.85f);

        _cardStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(8, 8, 8, 8)
        };
        _cardStyle.normal.background = MakeTex(new Color(0.17f, 0.17f, 0.2f, 0.96f));
        _cardStyle.normal.textColor = new Color(0.94f, 0.92f, 0.88f);

        _labelStyle = new GUIStyle(GUI.skin.label)
        {
            wordWrap = true,
            richText = false
        };
        _labelStyle.normal.textColor = new Color(0.96f, 0.94f, 0.9f);

        _subtleLabelStyle = new GUIStyle(_labelStyle);
        _subtleLabelStyle.normal.textColor = new Color(0.77f, 0.8f, 0.83f);

        _buttonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleLeft,
            wordWrap = false
        };
        _buttonStyle.normal.background = MakeTex(new Color(0.24f, 0.24f, 0.28f, 1f));
        _buttonStyle.hover.background = MakeTex(new Color(0.31f, 0.31f, 0.36f, 1f));
        _buttonStyle.active.background = MakeTex(new Color(0.18f, 0.4f, 0.49f, 1f));
        _buttonStyle.normal.textColor = new Color(0.95f, 0.94f, 0.91f);
        _buttonStyle.hover.textColor = Color.white;
        _buttonStyle.active.textColor = Color.white;

        _selectedButtonStyle = new GUIStyle(_buttonStyle);
        _selectedButtonStyle.normal.background = MakeTex(new Color(0.2f, 0.47f, 0.58f, 1f));
        _selectedButtonStyle.hover.background = MakeTex(new Color(0.23f, 0.54f, 0.66f, 1f));

        _textFieldStyle = new GUIStyle(GUI.skin.textField);
        _textFieldStyle.normal.background = MakeTex(new Color(0.09f, 0.09f, 0.11f, 1f));
        _textFieldStyle.focused.background = MakeTex(new Color(0.12f, 0.12f, 0.15f, 1f));
        _textFieldStyle.normal.textColor = new Color(0.97f, 0.95f, 0.91f);
        _textFieldStyle.focused.textColor = Color.white;

        _toggleStyle = new GUIStyle(GUI.skin.toggle);
        _toggleStyle.normal.textColor = new Color(0.93f, 0.92f, 0.88f);
        _toggleStyle.onNormal.textColor = Color.white;
    }

    private static Texture2D MakeTex(Color color)
    {
        var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
        tex.SetPixel(0, 0, color);
        tex.Apply();
        return tex;
    }

    private static string FormatLabel(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var cleaned = value;
        while (true)
        {
            var start = cleaned.IndexOf('<');
            if (start < 0)
            {
                break;
            }

            var end = cleaned.IndexOf('>', start);
            if (end < 0)
            {
                break;
            }

            cleaned = cleaned.Remove(start, end - start + 1);
        }

        return cleaned.Trim();
    }

    private sealed class StorageRow
    {
        public Storage Storage { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Context { get; set; }
        public int Available { get; set; }
        public int Stored { get; set; }
        public int Reserved { get; set; }
        public int Remaining { get; set; }
        public bool SupportsResource { get; set; }
        public string Flags { get; set; }
    }
}
