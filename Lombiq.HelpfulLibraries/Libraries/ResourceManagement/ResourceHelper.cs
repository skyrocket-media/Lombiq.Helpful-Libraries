﻿using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Linq;

namespace Lombiq.HelpfulLibraries.Libraries.ResourceManagement
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Returns the contents of an embedded text file in a given assembly.
        /// Returns the text content of the file located in the <paramref name="path"/> of the
        /// <paramref name="provider"/> or <see langword="null"/> if it doesn't exist.
        /// </summary>
        public static string GetFile(IFileProvider provider, string path)
        {
            var fileInfo = provider.GetDirectoryContents(string.Empty).SingleOrDefault(x => x.Name == path);
            if (fileInfo?.Exists != true) return null;

            using var stream = fileInfo.CreateReadStream();
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Returns the text content of the file located in the "Resources/{typeName}.{extension}" path of the
        /// <paramref name="provider"/> or <see langword="null" /> if it doesn't exist.
        /// </summary>
        /// <param name="extension">The extension of the target file.</param>
        public static string GetFile<T>(IFileProvider provider, string extension) =>
            GetFile(provider, GetTypeFilePath<T>(extension));

        /// <summary>
        /// Returns the contents of an embedded text file inside the same assembly as the given type. The embedded path
        /// is "Resources/{typeName}.{extension}".
        /// </summary>
        /// <typeparam name="T">A type defined in the same assembly where the embedded resource is.</typeparam>
        /// <param name="extension">The extension of the target file.</param>
        public static string GetEmbeddedFile<T>(string extension) =>
            GetFile(new EmbeddedFileProvider(typeof(T).Assembly), GetTypeFilePath<T>(extension));

        /// <summary>
        /// Returns a path usable for looking up resources.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this if you want to expand on the above but maintain the "Resources/{typeName}.{extension}" path format.
        /// </para>
        /// </remarks>
        public static string GetTypeFilePath<T>(string extension, char separator = '>') =>
            $"Resources{separator}{typeof(T).Name}.{extension}";
    }
}