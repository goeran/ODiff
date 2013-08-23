using System;
using System.IO;
using System.Reflection;

namespace BuildTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = args[0];
            var version = args[1];
            if (command.ToLower() == "create-nuspec")
            {
                using (var templateStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("BuildTool.Templates.ODiff.nuspec")))
                {
                    var template = templateStream.ReadToEnd();
                    var output = template.Replace("$(version)", version);
                    File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "ODiff.nuspec"), output);
                }
            }
        }
    }
}
