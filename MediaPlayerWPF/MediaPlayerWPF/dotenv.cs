using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesServer
{
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists("C:\\project_gal\\MediaPlayerWPF\\MediaPlayerWPF\\.env"))
                throw new FileNotFoundException("The .env file was not found.");

            foreach (var line in File.ReadAllLines("C:\\project_gal\\MediaPlayerWPF\\MediaPlayerWPF\\.env"))
            {
                var parts = line.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }

    }

}
