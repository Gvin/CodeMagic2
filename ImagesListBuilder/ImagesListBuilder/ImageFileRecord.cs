namespace ImagesListBuilder;

[Serializable]
public class ImageFileRecord
{
    public string? FilePath { get; set; }

    public int Hash { get; set; }
}