
using ImagesListBuilder;

var imagesFolderPath = args[0];
var outputFilePath = args[1];

Console.WriteLine($"Creating images list from \"{imagesFolderPath}\". Output file path: {outputFilePath}");

await new ImagesProcessor().Process(imagesFolderPath, outputFilePath);

Console.WriteLine("Completed");
