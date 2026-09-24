namespace DotnetAPIBasics.Models;

public class AttachmentOptions
{
    public string AllowedExtensions { get; set; } = string.Empty;
    public string MaxFileSize { get; set; } = string.Empty;
    public bool EnableCompression { get; set; } = false;
}
