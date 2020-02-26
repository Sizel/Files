using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Files_SyncFolders
{
	class DirectoryInfoEqualityComparer : IEqualityComparer<DirectoryInfo>
	{
		public bool Equals([AllowNull] DirectoryInfo x, [AllowNull] DirectoryInfo y)
		{
			return x.Name.Equals(y.Name);
		}

		public int GetHashCode([DisallowNull] DirectoryInfo obj)
		{
			return obj.Name.GetHashCode();
		}
	}
}
