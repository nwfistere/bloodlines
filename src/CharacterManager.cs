using Bloodlines.src;
using Il2CppVampireSurvivors.Data;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Bloodlines
{
    public class CharacterManager
    {
        public List<CharacterDataModelWrapper> characters { get; protected set; } = new();
        public Dictionary<CharacterType, CharacterDataModelWrapper> characterDict { get; set; } = new();
        readonly string ZipPath;
        readonly string ExtractPath;
        readonly string SavePackPath;
        public readonly bool success;
        public CharacterManager(string ZipPath, string DataPath, string ExtractPath)
        {
            this.ZipPath = ZipPath;
            this.ExtractPath = ExtractPath;
            this.SavePackPath = Path.Combine(DataPath, "Installed Packs");

            try
            {
                ParseExistingCharacterFiles();
                ParseZipFiles();
                success = true;
            }
            catch (Exception e)
            {
                Melon<BloodlinesMod>.Logger.Error($"Error: {e}");
                Melon<BloodlinesMod>.Logger.Error($"Submit an issue for this exception including all of the stacktrace and extra data.");
                Melon<BloodlinesMod>.Logger.Error($"To: https://github.com/nwfistere/bloodlines/issues");
                if (e.Data.Count > 0)
                {
                    Melon<BloodlinesMod>.Logger.Error("Extra Data:");
                    foreach (DictionaryEntry de in e.Data)
                        Melon<BloodlinesMod>.Logger.Error("    Key: {0,-20}      Value: {1}", "'" + de.Key.ToString() + "'", de.Value);
                }
                success = false;
            }
        }

        public void ParseExistingCharacterFiles()
        {
            CreateDirectory(ExtractPath);

            foreach (string dir in Directory.GetDirectories(ExtractPath))
            {
                string jsonFile = Path.Combine(dir, "character.json");

                if (File.Exists(jsonFile))
                {
                    Melon<BloodlinesMod>.Logger.Msg($"Loading up character json from {Path.GetDirectoryName(dir)}");
                    string fileContent = File.ReadAllText(jsonFile);
                    handleJsonFileString(jsonFile, fileContent);
                }
            }
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
                        Melon<BloodlinesMod>.Logger.Msg($"Parsing {Path.GetFileName(zip)}");
                        handleZipFile(zip);
                    }
                    catch (Exception)
                    {
                        Melon<BloodlinesMod>.Logger.Error($"Failed to extract zip file: '{zip}'");
                    }
                }
            }
        }

        void CleanupFiles(List<string> files)
        {
            foreach (string file in files.Where(file => File.Exists(file)))
            {
                try
                {
                    SavePack(file);
                }
                catch (Exception e)
                {
                    Melon<BloodlinesMod>.Logger.Error($"Caught the following error when cleaning up '{file}': {e}");
                    Melon<BloodlinesMod>.Logger.Error("***** Make sure to clean it up manually *****");
                }
            }
        }

        public void SavePack(string filePath)
        {
            if (File.Exists(filePath))
            {
                CreateDirectory(SavePackPath);

                string fileName = Path.GetFileName(filePath);
                string newFilePath = GetUnusedName(Path.Combine(SavePackPath, fileName), 0);

                File.Move(filePath, newFilePath);
            }
        }

        public string GetUnusedName(string filePath, int level)
        {
            if (level == 0 && !File.Exists(filePath))
            {
                return filePath;
            }

            string newFilePath = filePath.Insert(filePath.Length - 4, "-" + level);
            if (!File.Exists(newFilePath))
            {
                return newFilePath;
            }

            return GetUnusedName(filePath, ++level);
        }


        public static bool DirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            } catch (Exception e)
            {
                e.Data.Add("CharacterManager.CreateDirectory()", path);
                throw;
            }
        }

        void handleZipFile(string filePath)
        {
            List<string> filesToClean = new();

            string outputDirectory = Path.Combine(ExtractPath, Path.GetFileNameWithoutExtension(filePath));

            if (Directory.Exists(outputDirectory))
            {
                Melon<BloodlinesMod>.Logger.Warning($"Output directory '{outputDirectory}' already exists");

                if (!DirectoryEmpty(outputDirectory))
                {
                    if (File.Exists(Path.Combine(outputDirectory, "character.json")))
                    {
                        Melon<BloodlinesMod>.Logger.Error($"A character with this name already exists...");
                    }
                    else
                    {
                        Melon<BloodlinesMod>.Logger.Error($"Output directory '{outputDirectory}' isn't empty.");
                    }
                }
            }
            else
            {
                CreateDirectory(outputDirectory);
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
                                jsonString = reader.ReadToEnd();
                            

                            handleJsonFileString(outputJsonFile, jsonString);
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
                                Melon<BloodlinesMod>.Logger
                                    .Error($"Error: {imageFilePath} already exists.\nAre you trying to reimport the same character? Rename the zip file to something unique to fix this issue.");
                                throw;
                            }
                            catch (Exception e)
                            {
                                Melon<BloodlinesMod>.Logger
                                    .Error($"Error: Unexpected error while extracting image {entry.Name} from zip file. {e}");
                                throw;
                            }
                        }
                        else
                        {
                            Melon<BloodlinesMod>.Logger.Warning($"Found invalid file: '{entry.FullName}', ignoring.");
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
                Melon<BloodlinesMod>.Logger
                    .Error($"Error: File did not exist. Do you have permission to access the directory?");
                Melon<BloodlinesMod>.Logger.Error($"Skipping file: {filePath} - {exception}");
                CleanupFiles(filesToClean);
                return;
            }
            catch (Exception e)
            {
                Melon<BloodlinesMod>.Logger.Error($"Copy and paste the following exception to new issue on github.");
                Melon<BloodlinesMod>.Logger.Error($"Caught unexpected exception: {e}");
                CleanupFiles(filesToClean);
                throw;
            }

            Melon<BloodlinesMod>.Logger
                .Msg($"Extraction of {Path.GetFileNameWithoutExtension(filePath)} successful. Deleting zip file.");

            CleanupFiles(new List<string>() { filePath });
        }

        void handleJsonFileString(string filePath, string json)
        {
            BaseCharacterFileModel characterDto = GetFileDto(json, filePath, out Type actualType);

            characterDto.GetCharacterList()
                .ForEach((data) =>
            {
                data.BaseDirectory = Path.GetDirectoryName(filePath);
                characters.Add(data);
            });
        }

        BaseCharacterFileModel GetFileDto(string json, string filePath, out Type type)
        {
            JObject jObject;

            try
            {
                jObject = JObject.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                Melon<BloodlinesMod>.Logger.Error($"** NOTE: This is unlikely an issue with Bloodlines **");
                Melon<BloodlinesMod>.Logger.Error($"Verify that the json file has valid a json body in it!");
                Melon<BloodlinesMod>.Logger.Error($"Use a tool like https://jsonlint.com to verify your json file.");
                Melon<BloodlinesMod>.Logger.Error($"Failed to parse json to JObject. Invalid Json. {ex}");
                throw new InvalidDataException("Failed to parse json to JObject", ex);
            }

            JToken? jTokenVersion = jObject[propertyName: "version"] ?? throw new InvalidDataException("Invalid json provided, no version string.");
            Version? version = jTokenVersion.ToObject<Version>();

            switch (version?.ToString())
            {
                case CharacterFileModelV0_1._version:
                    {
                        type = typeof(CharacterFileModelV0_1);
                        CharacterFileModelV0_1? c = JsonConvert.DeserializeObject<CharacterFileModelV0_1>(json);

                        if (c == null)
                        {
                            break;
                        }

                        return c;
                    }
                case CharacterFileModelV0_2._version:
                    {
                        type = typeof(CharacterFileModelV0_2);
                        CharacterFileModelV0_2? c = JsonConvert.DeserializeObject<CharacterFileModelV0_2>(json);

                        if (c == null)
                        {
                            break;
                        }

                        return c;
                    }
                default:
                    throw new InvalidDataException($"Invalid version number found in json string {(version == null ? "null" : version.ToString())} in <{filePath}>.");
            }

            throw new InvalidDataException($"Invalid Json object in file <{filePath}>");
        }
    }
}