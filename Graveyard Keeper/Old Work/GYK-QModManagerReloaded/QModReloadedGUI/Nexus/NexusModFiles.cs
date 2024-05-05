using System;
using System.Text.Json.Serialization;

namespace QModReloadedGUI;

public class NexusModFiles
{
    [JsonPropertyName("files")]
    public Files[] Files { get; set; }
    [JsonPropertyName("file_updates")]
    public FileUpdates[] FileUpdates { get; set; }
}

public class Files
{
    [JsonPropertyName("id")]
    public int[] Id { get; set; }
    [JsonPropertyName("uid")]
    public long Uid { get; set; }
    [JsonPropertyName("file_id")]
    public int FileId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("version")]
    public string Version { get; set; }
    [JsonPropertyName("category_id")]
    public int CategoryId { get; set; }
    [JsonPropertyName("category_name")]
    public string CategoryName { get; set; }
    [JsonPropertyName("is_primary")]
    public bool IsPrimary { get; set; }
    [JsonPropertyName("size")]
    public int Size { get; set; }
    [JsonPropertyName("file_name")]
    public string FileName { get; set; }
    [JsonPropertyName("uploaded_timestamp")]
    public int UploadedTimestamp { get; set; }
    [JsonPropertyName("uploaded_time")]
    public DateTime UploadedTime { get; set; }
    [JsonPropertyName("mod_version")]
    public string ModVersion { get; set; }
    [JsonPropertyName("external_virus_scan_url")]
    public string ExternalVirusScanUrl { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("size_kb")]
    public int SizeKb { get; set; }
    [JsonPropertyName("size_in_bytes")]
    public int SizeInBytes { get; set; }
    [JsonPropertyName("changelog_html")]
    public string ChangelogHtml { get; set; }
    [JsonPropertyName("content_preview_link")]
    public string ContentPreviewLink { get; set; }
}

public class FileUpdates
{
    [JsonPropertyName("old_file_id")]
    public int OldFileId { get; set; }
    [JsonPropertyName("new_file_id")]
    public int NewFileId { get; set; }
    [JsonPropertyName("old_file_name")]
    public string OldFileName { get; set; }
    [JsonPropertyName("new_file_name")]
    public string NewFileName { get; set; }
    [JsonPropertyName("uploaded_timestamp")]
    public int UploadedTimestamp { get; set; }
    [JsonPropertyName("uploaded_time")]
    public DateTime UploadedTime { get; set; }
}