using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;

namespace Files_SyncFolders
{
	class FileInfoEqualityComparer : IEqualityComparer<FileInfo>
	{
		public bool Equals([AllowNull] FileInfo x, [AllowNull] FileInfo y)
		{
			return GetHashCode(x) == GetHashCode(y) && x.Name.Equals(y.Name);
		}

		public int GetHashCode([DisallowNull] FileInfo file)
		{
			SHA256 sha256 = SHA256.Create();

			FileStream fileStream = file.OpenRead();

			byte[] hash = sha256.ComputeHash(fileStream);

			fileStream.Close(); // why is the stream not closed automaticly after return?

			return BitConverter.ToInt32(hash, 0);
		}
	}
}
