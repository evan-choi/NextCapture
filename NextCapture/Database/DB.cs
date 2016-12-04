using System;
using ClusterFS;
using ClusterFS.IO;

namespace NextCapture.Database
{
    internal static class DB
    {
        public static string BaseDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\NextCapture";

        public static CursorDB Cursors { get; } = new CursorDB();

        static DB()
        {
            CFSTransaction.Config.BaseDirectory = DB.BaseDirectory + "\\";
        }   

        public static CFSFile Open(string fileName, int size = 128)
        {
            CFSFile db = null;

            if (!CFSFile.TryOpen(fileName, out db))
            {
                DirectoryEx.CreateDirectory(fileName);

                db = CFSFile.Create(fileName, "1.0", size);
            }

            return db;
        }
    }
}