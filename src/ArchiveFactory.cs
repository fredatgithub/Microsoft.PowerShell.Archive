// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;

namespace Microsoft.PowerShell.Archive
{
    internal static class ArchiveFactory
    {
        internal static IArchive GetArchive(ArchiveFormat format, string archivePath, ArchiveMode archiveMode, System.IO.Compression.CompressionLevel compressionLevel)
        {
      FileStream archiveFileStream = archiveMode switch
            {
                ArchiveMode.Create => new FileStream(archivePath, mode: FileMode.CreateNew, access: FileAccess.Write, share: FileShare.None),
                ArchiveMode.Update => new FileStream(archivePath, mode: FileMode.Open, access: FileAccess.ReadWrite, share: FileShare.None),
                ArchiveMode.Extract => new FileStream(archivePath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read),
                _ => throw new ArgumentOutOfRangeException(nameof(archiveMode))
            };

            return format switch
            {
                ArchiveFormat.Zip => new ZipArchive(archivePath, archiveMode, archiveFileStream, compressionLevel),
                //ArchiveFormat.tar => new TarArchive(archivePath, archiveMode, archiveFileStream),
                // TODO: Add Tar.gz here
                _ => throw new ArgumentOutOfRangeException(nameof(archiveMode))
            };
        }

        internal static bool TryGetArchiveFormatFromExtension(string path, out ArchiveFormat? archiveFormat)
        {
            archiveFormat = Path.GetExtension(path).ToLowerInvariant() switch
            {
                ".zip" => ArchiveFormat.Zip,
                /* Disable support for tar and tar.gz for preview1 release 
                ".gz" => path.EndsWith(".tar.gz) ? ArchiveFormat.Tgz : null,
                 */
                _ => null
            };
            return archiveFormat is not null;
        }
    }
}
