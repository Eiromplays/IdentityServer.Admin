using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Eiromplays.IdentityServer.Application.Common.FileStorage;
using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Infrastructure.Common.Extensions;

namespace Eiromplays.IdentityServer.Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    public async Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
        where T : class
    {
        if (request?.Data is null)
        {
            return string.Empty;
        }

        if (!supportedFileType.GetDescriptionList().Contains(request.Extension.ToLower()))
            throw new InvalidOperationException("File Format Not Supported.");

        if (request.Name is null)
            throw new InvalidOperationException("Name is required.");

        string base64Data = Regex.Match(request.Data, "data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

        var streamData = new MemoryStream(Convert.FromBase64String(base64Data));

        if (streamData.Length <= 0) return string.Empty;

        string folder = typeof(T).Name;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            folder = folder.Replace(@"\", "/");
        }

        string folderName = supportedFileType switch
        {
            FileType.Image => Path.Combine("Files", "Images", folder),
            _ => Path.Combine("Files", "Others", folder),
        };

        string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        Directory.CreateDirectory(pathToSave);

        string fileName = request.Name.Trim('"');
        fileName = RemoveSpecialCharacters(fileName);
        fileName = fileName.ReplaceWhitespace("-");
        fileName += request.Extension.Trim();

        string fullPath = Path.Combine(pathToSave, fileName);
        string dbPath = Path.Combine(folderName, fileName);

        if (File.Exists(dbPath))
        {
            dbPath = NextAvailableFilename(dbPath);
            fullPath = NextAvailableFilename(fullPath);
        }

        await using var stream = new FileStream(fullPath, FileMode.Create);

        await streamData.CopyToAsync(stream, cancellationToken);

        return dbPath.Replace("\\", "/");
    }

    private static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
    }

    public void Remove(string? path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private const string NumberPattern = "-{0}";

    private static string NextAvailableFilename(string path)
    {
        if (!File.Exists(path))
        {
            return path;
        }

        return Path.HasExtension(path)
            ? GetNextFilename(path.Insert(
                path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal),
                NumberPattern))
            : GetNextFilename(path + NumberPattern);
    }

    private static string GetNextFilename(string pattern)
    {
        string tmp = string.Format(pattern, 1);

        if (!File.Exists(tmp))
        {
            return tmp;
        }

        int min = 1, max = 2;

        while (File.Exists(string.Format(pattern, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            int pivot = (max + min) / 2;

            if (File.Exists(string.Format(pattern, pivot)))
            {
                min = pivot;
            }
            else
            {
                max = pivot;
            }
        }

        return string.Format(pattern, max);
    }
}