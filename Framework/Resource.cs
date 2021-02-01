namespace CG_Projekt.Framework
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class Resource
    {
        public static string LoadString(string name)
        {
            using var stream = LoadStream(name);
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        public static Stream LoadStream(string name)
        {
            var assembly = Assembly.GetEntryAssembly();
            var stream = assembly.GetManifestResourceStream(name);
            if (stream is null)
            {
                var names = string.Join("|", assembly.GetManifestResourceNames());
                throw new ArgumentException($"Could not find resource '{name}' in resources '{names}'");
            }

            return stream;
        }
    }
}
