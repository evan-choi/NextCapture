using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NextCapture
{
    public static class DirectoryEx
    {
        public static void CreateDirectory(string directory)
        {
            directory = Path.GetDirectoryName(directory);

            var paths = new Queue<string>(directory.Split('\\'));
            var sb = new StringBuilder();

            while (paths.Count > 0)
            {
                sb.Append(paths.Dequeue());

                if (!Directory.Exists(sb.ToString()))
                    Directory.CreateDirectory(sb.ToString());

                sb.Append("\\");
            }
        }
    }
}
