using Il2CppVampireSurvivors.Objects.Characters.Enemies;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO.Compression;

namespace EasyAddCharacter
{
    internal class CharacterManager
    {
        public List<Character> characters { get; protected set; } = new();
        private readonly string ZipPath;
        private readonly string ExtractPath;
        private Dictionary<Type, List<BaseCharacterFile>> ParsedCharacterFiles = new();
        public CharacterManager(string ZipPath, string ExtractPath)
        {
            this.ZipPath = ZipPath;
            this.ExtractPath = ExtractPath;
        }

        public void ParseZipFiles()
        {
            if (Directory.Exists(ZipPath) && !DirectoryEmpty(ZipPath))
            {
                List<string> zipFiles = Directory.GetFiles(ZipPath, "*.zip").ToList();
                foreach (string zip in zipFiles)
                {
                    try
                    {
                        handleZipFile(zip);
                    }
                    catch (Exception)
                    {
                        Melon<Mod>.Logger.Error($"Failed to extract zip file: '{zip}'");
                    }
                }
            }
        }

        private void cleanupFiles(List<string> files)
        {
            foreach (string file in files.Where(file => File.Exists(file)))
            {
                try
                {
                    File.Delete(file);
                } catch (Exception e)
                {
                    Melon<Mod>.Logger.Error($"Caught the following error when cleaning up '{file}': {e}");
                    Melon<Mod>.Logger.Error("***** Make sure to clean it up manually *****");
                }
            }
        }
        
        public static bool DirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void handleZipFile(string filePath)
        {
            List<string> filesToClean = new();

            string outputDirectory = Path.Combine(ExtractPath, Path.GetFileNameWithoutExtension(filePath));

            if (Directory.Exists(outputDirectory))
            {
                Melon<Mod>.Logger.Warning($"Output directory '{outputDirectory}' already exists");
                if (!DirectoryEmpty(outputDirectory))
                {
                    if (File.Exists(Path.Combine(outputDirectory, "character.json")))
                        Melon<Mod>.Logger.Error($"A character witht this name already exists...");
                    else
                        Melon<Mod>.Logger.Error($"Output directory '{outputDirectory}' isn't empty.");
                }
            } else
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception e)
                {
                    Melon<Mod>.Logger.Error($"Failed to create the directory '{outputDirectory}' - {e}");
                    throw;
                }
            }

            try
            {
                string jsonString = "";

                using (FileStream fs = new(filePath, FileMode.Open))
                using (ZipArchive archive = new(fs, ZipArchiveMode.Read))
                {

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name.EndsWith(".json"))
                        {
                            string outputJsonFile = Path.Combine(outputDirectory, "character.json");
                            Stream stream = entry.Open();
                            using (StreamReader reader = new(stream))
                            {
                                jsonString = reader.ReadToEnd();
                            }

                            handleJsonFileString(jsonString);
                            entry.ExtractToFile(outputJsonFile);
                            filesToClean.Add(outputJsonFile);
                        }
                        else if (entry.Name.EndsWith(".png"))
                        {
                            string imageFilePath = Path.Combine(outputDirectory, entry.Name);

                            try
                            {
                                // Don't overwrite output for now.
                                entry.ExtractToFile(imageFilePath, false);
                                filesToClean.Add(imageFilePath);
                            }
                            catch (IOException ioe)
                            {
                                Melon<Mod>.Logger.Error($"Error: {imageFilePath} already exists.\nAre you trying to reimport the same character? Rename the zip file to something unique to fix this issue.");
                                throw;
                            }
                            catch (Exception e)
                            {
                                Melon<Mod>.Logger.Error($"Error: Unexpected error while extracting image {entry.Name} from zip file. {e}");
                                throw;
                            }
                        }
                        else
                        {
                            Melon<Mod>.Logger.Warning($"Found invalid file: '{entry.FullName}', ignoring.");
                        }
                    }
                    if (!jsonString.Any())
                    {
                        throw new InvalidDataException("Didn't find any json file.");
                    }
                }
            }
            catch (FileNotFoundException exception)
            {
                Melon<Mod>.Logger.Error($"Error: File did not exist. Do you have permission to access the directory?");
                Melon<Mod>.Logger.Error($"Skipping file: {filePath} - {exception}");
                cleanupFiles(filesToClean);
                return;
            }
            catch (Exception e)
            {
                Melon<Mod>.Logger.Error($"Copy and paste the following exception to new issue on github.");
                Melon<Mod>.Logger.Error($"Caught unexpected exception: {e}");
                cleanupFiles(filesToClean);
                throw;
            }

            Melon<Mod>.Logger.Msg($"Extraction of {Path.GetFileNameWithoutExtension(filePath)} successful. Deleting zip file.");

            cleanupFiles(new List<string>() { filePath });
        }

        private void handleJsonFileString(string json)
        {
            BaseCharacterFile characterDto = GetFileDto(json, out Type actualType);
            ParsedCharacterFiles[actualType].Add(characterDto);
        }

        private BaseCharacterFile GetFileDto(string json, out Type type)
        {
            JObject jObject;
            try
            {
                jObject = JObject.Parse(json);
            } catch (JsonReaderException ex)
            {
                Melon<Mod>.Logger.Error($"Failed to parse json to JObject. Invalid Json. {ex}");
                throw new InvalidDataException("Failed to parse json to JObject", ex);
            }

            JToken? jTokenVersion = jObject[propertyName: "version"] ?? throw new InvalidDataException("Invalid json provided, no version string.");
            Version? version = jTokenVersion.ToObject<Version>();

            switch(version?.ToString())
            {
                case CharacterFileV0_1._version:
                    {
                        type = typeof(CharacterFileV0_1);
                        CharacterFileV0_1? c = JsonConvert.DeserializeObject<CharacterFileV0_1>(json);
                        if (c == null)
                            break;
                        return c;
                    }
            }

            throw new InvalidDataException($"Invalid version found in json string { (version == null ? "null" : version.ToString()) }.");
        }
    }
}
