namespace MiniTC.Models;

// Data structure for directory contents
public class DirectoryContent
{
    public List<string> Directories { get; set; } = [];
    public List<string> Files { get; set; } = [];
    public string CurrentPath { get; set; } = "";
    public bool IsRoot { get; set; } = false;
}

// Model for file system operations
public class FileSystemModel
{
    public List<string> GetAvailableDrives()
    {
        var drives = new List<string>();
        
        try
        {
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            foreach (DriveInfo drive in driveInfos)
            {
                if (drive.IsReady)
                {
                    drives.Add(drive.Name);
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error loading drives: {ex.Message}", ex);
        }

        return drives;
    }

    public DirectoryContent GetDirectoryContent(string path)
    {
        if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Directory not found: {path}");
        }

        var content = new DirectoryContent
        {
            CurrentPath = path,
            IsRoot = path == Path.GetPathRoot(path)
        };

        try
        {
            // Get directories
            string[] dirPaths = Directory.GetDirectories(path);
            foreach (string dirPath in dirPaths)
            {
                content.Directories.Add(Path.GetFileName(dirPath));
            }

            // Get files
            string[] filePaths = Directory.GetFiles(path);
            foreach (string filePath in filePaths)
            {
                content.Files.Add(Path.GetFileName(filePath));
            }
        }
        catch (UnauthorizedAccessException)
        {
            throw new UnauthorizedAccessException("Access denied to this directory.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error reading directory: {ex.Message}", ex);
        }

        return content;
    }

    public string? GetParentDirectory(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        string? parent = Path.GetDirectoryName(path);
        return string.IsNullOrEmpty(parent) ? null : parent;
    }

    public string CombinePath(string basePath, string relativePath)
    {
        return Path.Combine(basePath, relativePath);
    }

    public bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public void CopyFile(string sourcePath, string destinationPath, bool overwrite = true)
    {
        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException($"Source file not found: {sourcePath}");
        }

        string? destinationDir = Path.GetDirectoryName(destinationPath);
        if (destinationDir != null && !Directory.Exists(destinationDir))
        {
            throw new DirectoryNotFoundException($"Destination directory not found: {destinationDir}");
        }

        try
        {
            File.Copy(sourcePath, destinationPath, overwrite);
        }
        catch (UnauthorizedAccessException)
        {
            throw new UnauthorizedAccessException("Access denied. Cannot copy to destination.");
        }
        catch (IOException ex)
        {
            throw new IOException($"I/O error during copy operation: {ex.Message}", ex);
        }
    }

    public string GetFileName(string filePath)
    {
        return Path.GetFileName(filePath);
    }
}