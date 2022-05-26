using CodeMagic.ImagesCompiler;

const string imagesFolderName = "Images";
const string outputFileName = "images-compiled.json";

Console.WriteLine("Images compiler started.");

var projectPath = args[0];

Console.WriteLine($"Compiling images for project {projectPath}");

var imagesFolder = Path.Combine(projectPath, imagesFolderName);
var webRootPath = Path.Combine(projectPath, "wwwroot");
var outputFilePath = Path.Combine(webRootPath, outputFileName);

await new ImagesProcessor().Process(imagesFolder, outputFilePath);

Console.WriteLine("Images compilation completed.");