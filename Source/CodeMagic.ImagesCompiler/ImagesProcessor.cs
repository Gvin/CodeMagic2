using CodeMagic.Game.Drawing;
using Newtonsoft.Json;

namespace CodeMagic.ImagesCompiler;

public class ImagesProcessor
{
    private const string ImagesExtensionFilter = "*.simg";

    public async Task Process(string imagesFolder, string outputFilePath)
    {
        var images = await GetImages(imagesFolder);
        var outputFileContent = JsonConvert.SerializeObject(images);
        await File.WriteAllTextAsync(outputFilePath, outputFileContent);
    }

    private static async Task<Dictionary<string, SymbolsImage>> GetImages(string imagesFolder)
    {
        var result = new Dictionary<string, SymbolsImage>();

        var imageFiles = Directory.GetFiles(imagesFolder, ImagesExtensionFilter, SearchOption.AllDirectories);
        foreach (var imageFilePath in imageFiles)
        {
            var image = await LoadImageFromFile(imageFilePath);
            var name = GetImageName(imageFilePath);

            result.Add(name, image);
        }

        return result;
    }

    private static async Task<SymbolsImage> LoadImageFromFile(string path)
    {
        var fileContent = await File.ReadAllTextAsync(path);

        var image = JsonConvert.DeserializeObject<SymbolsImage>(fileContent);

        if (image == null)
        {
            throw new Exception($"Unable to load image from file: {path}");
        }

        return image;
    }

    private static string GetImageName(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }
}
