using Newtonsoft.Json;

namespace ImagesListBuilder;

public class ImagesProcessor
{
    private const string ImagesExtensionFilter = "*.simg";

    public async Task Process(string imagesFolder, string outputFilePath)
    {
        var images = await GetImages(imagesFolder);
        var outputFileContent = JsonConvert.SerializeObject(images);
        await File.WriteAllTextAsync(outputFilePath, outputFileContent);
    }

    private static async Task<ImageFileRecord[]> GetImages(string imagesFolder)
    {
        var result = new List<ImageFileRecord>();

        var imageFiles = Directory.GetFiles(imagesFolder, ImagesExtensionFilter, SearchOption.AllDirectories);
        foreach (var imageFile in imageFiles)
        {
            var hash = await CalculateImageHash(imageFile);

            var filePath = ConvertFilePath(imageFile, imagesFolder);
            result.Add(new ImageFileRecord { FilePath = filePath, Hash = hash });
        }

        return result.ToArray();
    }

    private static string ConvertFilePath(string filePath, string imagesFolder)
    {
        return filePath.Replace(imagesFolder, "").Replace("\\", "/");
    }

    private static async Task<int> CalculateImageHash(string filePath)
    {
        var fileContent = await File.ReadAllTextAsync(filePath);
        return CalculateStableHash(fileContent);
    }

    private static int CalculateStableHash(string content)
    {
        return content.Sum(ch => ch * 10 + ch);
    }
}