using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;


namespace Dotnet
{
    public static class WorkerFiles
    {

        public static void CreateFile(FileInfo file)
        {
            if (file.Exists)
            {
                Console.WriteLine($"File {file.Name} already exists");
            }
            else
            {
                Console.WriteLine(file.FullName);
                file.Create().Dispose();
                Console.WriteLine($"File {file.Name} created successfully");
            }
        }

        public static void DeleteFile(FileInfo file)
        {
            if (file.Exists)
            {
                Console.WriteLine(file.FullName);
                file.Delete();
                Console.WriteLine($"File {file.Name} deleted successfully");
            }
            else
            {
                Console.WriteLine($"File {file.Name} doesn't exists");
            }
        }

        public static void CreateDir(string path)
        {
            if (Directory.Exists(path))
            {
                Console.WriteLine($"Folder {path} already exists");
            }
            else
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Folder {path} created successfully");
            }
        }

        public async static void WriteFile(FileInfo file, string text, bool writeToEndFlag = true)
        {
            string path = file.FullName;
            StreamWriter writer = new StreamWriter(path, writeToEndFlag, Encoding.UTF8);
            writer.WriteLine(text);
            writer.Close();
        }

        public static void WriteFileJson(FileInfo file, string text)
        {
            string path = file.FullName;

            string[] lines = File.ReadAllLines(path.Replace(",", ""));
            string newtext = "";
            if (lines.Length != 2)
            {
                for (int i = 1; i < lines.Length - 1; i++)
                {
                    if (i == lines.Length - 2)
                    {
                        newtext += lines[i] + "," + "\n";
                    }
                    else
                    {
                        newtext += lines[i] + "\n";
                    }
                }
            }

            StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

            string parentFolder = GetFileDir(text);
            string fileName = GetFileName(text);

            string jsonReq = ("{" + 
            "\"fileName\":" + "\"" + fileName + "\"" + ", " + 
            "\"parentFolder\":" + "\"" + parentFolder + "\"" + ", " +
            "\"filePath\":" + "\"" + text + "\"" +
            "}");

            writer.WriteLine("{\"file\":[");
            
            writer.WriteLine(newtext);
            writer.WriteLine(jsonReq);
            writer.WriteLine("]}");
            
            writer.Close();
            Console.WriteLine($"Test written in {file}");
        }

        public static string ReadFile(FileInfo file, bool textflag=true)
        {
            string path = file.FullName;
            StreamReader reader = new StreamReader(path, Encoding.UTF8);
            string text = reader.ReadToEnd();
            if (textflag)
            {
                Console.WriteLine($"\nFile {file.Name}");
            }
            reader.Close();
            return text;
        }

        public static string[] GetAllFilesInDir(string dirPath)
        {
            string[] allfiles = Directory.GetFiles(dirPath).Select(x => System.IO.Path.GetFileName(x)).ToArray();
            return allfiles;
        }

        public static string[] GetAllFilesInDirWithoutExtension(string dirPath)
        {
            string[] allfiles = Directory.GetFiles(dirPath).Select(x => System.IO.Path.GetFileNameWithoutExtension(x)).ToArray();
            return allfiles;
        }
        
        public static string[] GetAllDirsInDir(string dirPath)
        {
            string[] alldirs = Directory.GetDirectories(dirPath).Select(x => System.IO.Path.GetFileNameWithoutExtension(x)).ToArray();
            return alldirs;
        }

        public static string GetFileDir(string filePath)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(filePath));
            string dirName = dir.Name;
            return dirName;
        }

        public static string GetFileName(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            return fileName;
        }

        public static int FindStringLineInFile(FileInfo file, string stringToFind)
        {
            
            string path = file.FullName;
            int lineCounter = 0;
            string line;
            bool flag = false;
            string lineChecker = "\"fileName\"" + ":" + "\"" + stringToFind + "\"";
            
            StreamReader reader = new StreamReader(path, Encoding.UTF8);
            while ((line = reader.ReadLine()) != null)
            {
                lineCounter++;
                if (line.Contains(lineChecker))
                {
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                return lineCounter;
            }
            else
            {
                return -1;
            }
        }

        public static List<string> GetFilesInJson(FileInfo file)
        {
            string path = file.FullName;
            string[] lines = File.ReadAllLines(path);
            List<string> newtext = new List<string>();
            
            if (lines.Length != 2)
            {
                for (int i = 1; i < lines.Length - 1; i++)
                {
                    if ((i % 2 == 0))
                    {
                        newtext.Add(lines[i]);
                    }
                }
            }
            return newtext;
        }

        public static void DeleteLineInFile(FileInfo file, int lineIndex)
        {
            string path = file.FullName;
            var lines = File.ReadAllLines(path).ToList();
            lines.RemoveAt(lineIndex);
            File.WriteAllLines(path, lines, Encoding.UTF8);
        }

    }

    public class AWorkerJson
    {
        public AWorkerJson()
        {
            this.dirPath = @$".{Path.DirectorySeparatorChar}JsonFilesFolder";
        }
        
        public AWorkerJson(string dirPath)
        {
            this.dirPath = dirPath;
        }

        private string dirPath;
        static string signJson = ".json";

        public static void AddFileToJson(string path, string someFileName)
        {
            FileInfo someFile = new FileInfo(dirPath + Path.DirectorySeparatorChar + someFileName + signJson);
            
            CreateJson(someFileName, someFile);
            WorkerFiles.WriteFileJson(someFile, path);
            Console.WriteLine(WorkerFiles.ReadFile(someFile));
        }

        internal static void AddOneDirToJson(string dirPath, string someFileName)
        {
            string[] allfiles = WorkerFiles.GetAllFilesInDir(dirPath);
            foreach (string file in allfiles)
            {
                AddFileToJson(dirPath + file, someFileName);
            }
        }

        public static void AddAllDirsToJson(string dirPath, string someFileName)
        {
            string[] alldirs = WorkerFiles.GetAllDirsInDir(dirPath);
            string newDirPath;

            if (alldirs.Length > 0)
            {
                foreach (string dir in alldirs)
                {
                    newDirPath = dirPath + dir + Path.DirectorySeparatorChar;
                    AddOneDirToJson(newDirPath, someFileName);
                }
            }
            else
            {
                AddOneDirToJson(dirPath, someFileName);
            }
        }
        
        public static void CreateJson(string someFileName)
        {
            FileInfo someFile = new FileInfo(dirPath + Path.DirectorySeparatorChar + someFileName + signJson);
            string jsonReq = "{\"file\":[";
            WorkerFiles.CreateDir(dirPath);
            WorkerFiles.CreateFile(someFile);
            WorkerFiles.WriteFile(someFile, jsonReq);
            WorkerFiles.WriteFile(someFile, "]}");
        }
        internal static void CreateJson(string someFileName, FileInfo someFile)
        {
            WorkerFiles.CreateDir(dirPath);
            WorkerFiles.CreateFile(someFile);
        }


        public static void DeleteJson(string someFileName)
        {
            FileInfo someFile = new FileInfo(dirPath + Path.DirectorySeparatorChar + someFileName + signJson);
            WorkerFiles.DeleteFile(someFile);
        }

        public static void ShowJsonFolder(bool flagWithoutExtension=false)
        {
            string[] allfiles;
            if (flagWithoutExtension)
            {
                allfiles = WorkerFiles.GetAllFilesInDirWithoutExtension(dirPath);

            }
            else
            {
                allfiles = WorkerFiles.GetAllFilesInDir(dirPath);
            }
            foreach (string filename in allfiles)
            {
                Console.WriteLine(filename);
            }
        }

        public static void ShowAllDirsInDir(string dirPath)
        {
            string[] alldirs = WorkerFiles.GetAllDirsInDir(dirPath);
            foreach (string dir in alldirs)
            {
                Console.WriteLine(dir);
            }
        }

        
        public static void DeleteFileFromJson(string someFileName, string stringToDelete)
        {
            FileInfo file = new FileInfo(dirPath + Path.DirectorySeparatorChar + someFileName + signJson);
            int lineIndex = WorkerFiles.FindStringLineInFile(file, stringToDelete);
            if ((lineIndex > 2) && (lineIndex % 2 > 0))
            {
                WorkerFiles.DeleteLineInFile(file, lineIndex - 1);
                WorkerFiles.DeleteLineInFile(file, lineIndex - 2);
                Console.WriteLine($"Text {stringToDelete} \nsuccessfully deleted in json {someFileName}");
            }
            else
            {
                Console.WriteLine(
                    $"Text {stringToDelete} \nnot found in json {someFileName} or can't be deleted");
            }
        }

        public static void FindStringInFile(string someFileName, string stringToFind)
        {
            FileInfo file = new FileInfo(dirPath + Path.DirectorySeparatorChar + someFileName + signJson);
            int lineIndex = WorkerFiles.FindStringLineInFile(file, stringToFind);
            if (lineIndex > 0)
            {
                Console.WriteLine($"Text {stringToFind} \nfound in {lineIndex.ToString()} line of json {someFileName}");
            }
            else
            {
                Console.WriteLine($"Text {stringToFind} \nnot found in json {someFileName}");
            }
        }

        public static void ShowJsonFiles(string someFileName)
        {
            FileInfo file = new FileInfo(dirPath + Path.DirectorySeparatorChar + someFileName + signJson);
            string filesInJson = WorkerFiles.ReadFile(file, false);
            Console.WriteLine(filesInJson);
        }

        public static void FindStringInAllFiles(string stringToFind)
        {
            string[] allfiles = WorkerFiles.GetAllFilesInDirWithoutExtension(dirPath);
            	
            List<string> foundSongsInJsons = new List<string>();
            List<int> foundSongsInJsonsIndexLine = new List<int>();
            foreach (string someFileName in allfiles)
            {
                FileInfo file = new FileInfo(dirPath + Path.DirectorySeparatorChar + someFileName + signJson);
                int lineIndex = WorkerFiles.FindStringLineInFile(file, stringToFind);
                if (lineIndex > 0)
                {
                    foundSongsInJsons.Add(someFileName);
                    foundSongsInJsonsIndexLine.Add(lineIndex);

                }

            }

            if (foundSongsInJsons.Count > 0)
            {
                for (int i = 0; i < foundSongsInJsons.Count; i++)
                {
                    Console.WriteLine($"Text {stringToFind} \nfound in {foundSongsInJsonsIndexLine[i].ToString()} line of json {foundSongsInJsons[i]}");
                }
            }
            else
            {
                Console.WriteLine($"Text {stringToFind} \nnot found");
            }

            Console.WriteLine($"Total matches: {foundSongsInJsons.Count}");

        }

        public static List<string> GetFilesFromJson(string someFileName)
        {
            FileInfo someFile = new FileInfo(dirPath + Path.DirectorySeparatorChar + someFileName + signJson);
            List<string> allSongsInJson = WorkerFiles.GetFilesInJson(someFile);

            return allSongsInJson;

        }

        public static string GetStringFromJsonLine(string file, string desiredString)
        {            
            string checktext = "";
            int counter = 0;
            bool flag = false;
            for (int i = 0; i < file.Length; i++)
            {
                if (!flag)
                {
                    if (file[i].ToString() == "\"")
                    {
                        counter++;
                    }
                    if (counter > 0)
                    {
                        if (counter % 2 > 0)
                        {
                            checktext += file[i];
                        }
                        else
                        {
                            if (checktext == "\"" + desiredString)
                            {
                                flag = true;
                                counter = 0;
                                checktext = "";
                            }
                            else
                            {
                                checktext = "";
                            }
                        }
                    }
                }
                else
                {
                    if (counter < 1)
                    {
                        i += 2;
                        counter++;
                    }
                    checktext += file[i];
                    if (file[i].ToString() == "\"")
                    {
                        return checktext.Replace("\"", "");
                    }
                }
            }
            return "";
        }
        
        public static List<string> GetListFilesFromJson(List<string> list, string desiredString)
        {
            List<string> newlist = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                newlist.Add(GetStringFromJsonLine(list[i], desiredString));   
            }
            return newlist;
        }
    }
}
