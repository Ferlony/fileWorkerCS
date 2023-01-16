# fileWorkerCS
About
=====

Package for C# that allows to work with files: create, delete, write, read, get files/directories, create special JSONs and work with them.

Usage
=====

1) Use to create files and directories, write and read files, get files/directories in directories:
```
public static class WorkerFiles
```

Methods
-------

### To create file
```
public static void CreateFile(FileInfo file)
```
Example
```
string path = "AbsolutePathToFile";

FileInfo someFile = new FileInfo(path);

WorkerFiles.CreateFile(someFile);
```
### To delete file
```
public static void DeleteFile(FileInfo file)
```
Example
```
string path = "AbsolutePathToFile";

FileInfo someFile = new FileInfo(path);

WorkerFiles.DeleteFile(someFile);
```
### To create directory
```
public static void CreateDir(string path)
```
Example
```
string path = "AbsolutePathToDirectory";

FileInfo someFile = new FileInfo(path);

WorkerFiles.CreateDir(someFile);
```
### To write text in file
```
public static void WriteFile(FileInfo file, string text, bool writeToEndFlag = true)
```
To write text to the end of a file used:
```
bool writeToEndFlag = true
```
Example
```
string path = "AbsolutePathToFile";

FileInfo someFile = new FileInfo(path);

WorkerFiles.WriteFile(someFile, "SomeText");
```
or
```
WorkerFiles.WriteFile(someFile, "SomeText", false);
```
### To read text from file
```
public static string ReadFile(FileInfo file, bool textflag=true)
```
### To write file name to terminal:
```
bool textflag=true
```
Example
```
string path = "AbsolutePathToFile";

FileInfo someFile = new FileInfo(path);

Console.WriteLine(WorkerFiles.ReadFile(someFile));
```
or
```
Console.WriteLine(WorkerFiles.ReadFile(someFile, false));
```
### To get all files located in directory
```
public static string[] GetAllFilesInDir(string dirPath)
```
Example
```
string dirPath = "AbsolutePathToDirectory";

string[] allfiles = WorkerFiles.GetAllFilesInDir(dirPath);
```
### To get all files located in directory without extensions
```
public static string[] GetAllFilesInDirWithoutExtension(string dirPath)
```
Example
```
string dirPath = "AbsolutePathToDirectory";

string[] allfiles = WorkerFiles.GetAllFilesInDirWithoutExtension(dirPath);
```
### To get all directories located in directory
```
public static string[] GetAllDirsInDir(string dirPath)
```
Example
```
string dirPath = "AbsolutePathToDirectory";

string[] allfiles = WorkerFiles.GetAllDirsInDir(dirPath);
```
### To get file parent folder
```
public static string GetFileDir(string filePath)
```
Example
```
string path = "AbsolutePathToFile";

string parentFolder = GetFileDir(path);
```
### To get file name
```
public static string GetFileName(string filePath)
```
Example
```
string path = "AbsolutePathToFile";

string fileName = GetFileName(path);
```
### To delete some line in file
```
public static void DeleteLineInFile(FileInfo file, int lineIndex)
```
Example
```
string path = "AbsolutePathToFile";

FileInfo someFile = new FileInfo(path);

int lineIndex = 4 // deletes 4th line

WorkerFiles.DeleteLineInFile(someFile, lineIndex);
```
2) Use to work with JSON:
```
public class AWorkerJson
```
Example

To indicate program home directory by default where folder with jsons will be created
```
AWorkerJson jsonWorker = new AWorkerJson();
```
To indicate your path where folder with jsons will be created
```
string dirPath = "AbsolutePathToDirectory";

AWorkerJson jsonWorker = new AWorkerJson(dirPath);
```
Methods
-------

### To add some file information in json
```
public static void AddFileToJson(string path, string someFileName)
```
### To add all files in folders located in some directory into json
```
public static void AddAllDirsToJson(string dirPath, string someFileName)
```
### To create json with some name
```
public static void CreateJson(string someFileName)
```
### To delete json with some name
```
public static void DeleteJson(string someFileName)
```
### To show all jsons in directory
```
public static void ShowJsonFolder(bool flagWithoutExtension=false)
```
### To show all folders in directory
```
public static void ShowAllDirsInDir(string dirPath)
```
### To delete some file by name in json
```
public static void DeleteFileFromJson(string someFileName, string stringToDelete)
```
### To find some file by name in json
```
public static void FindStringInFile(string someFileName, string stringToFind)
```
### To show all files information in json
```
public static void ShowJsonFiles(string someFileName)
```
### To find some file by name in all jsons
```
public static void FindStringInAllFiles(string stringToFind)
```
### To get all files in json
```
public static List<string> GetFilesFromJson(string someFileName)
```
### To get file by key in json
```
public static string GetStringFromJsonLine(string file, string desiredString)
```
### To get all files by key in json
```
public static List<string> GetListFilesFromJson(List<string> list, string desiredString)
```
