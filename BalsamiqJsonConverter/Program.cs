using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using BalsamiqArchiveModel.Model;

namespace BalsamiqJsonConverter
{
    class Program
    {
        private const int ErrorBadArguments = -0x1;
        private const int ErrorFileDoesntExists = -0x2;
        private const int ErrorUnknown = -0x3;

        static void Main(string[] args)
        {
            string source = null;
            string target = null;

            foreach (string a in args)
            {
                if (a.StartsWith("-"))
                {
                    // Handle flags
                    if (a.Equals("-h"))
                    {
                        PrintHelp();
                        return;
                    }
                }
                else if (source == null)
                {
                    // First argument that is not a flag is considered a source
                    source = a;
                }
                else if (target == null)
                {
                    // Second argument is considered to be the target
                    target = a;
                }
            }

            if (source == null)
            {
                PrintHelp();
                return;
            }

            if (!File.Exists(source))
            {
                PrintError("Input file '" + source + "' does not exist");
                Environment.Exit(ErrorFileDoesntExists);
            }

            try
            {
                MockupProject project = BalsamiqArchiveReader.LoadProject(source);

                // Serialize to JSON
                JsonSerializer serializer = new JsonSerializer();
                Console.Out.WriteLine(JsonConvert.SerializeObject(project, 
                    Formatting.Indented, 
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                Console.Error.WriteLine("\nDebug trace:\n" + e.StackTrace);
#if (!DEBUG)
                Environment.Exit(ErrorUnknown);
#endif
            }
#if (DEBUG)
            Console.In.Read();
#endif
        }

        static void PrintHeader()
        {
            Console.WriteLine("This program dumps the contents of a Balsamic Mockups project file (*.bmpr) into a JSON text file." +
                "\nFor more details, see: https://github.com/waqasilyas/BeyondCompareBalsamiqPlugin/wiki \n");
        }

        static void PrintHelp()
        {
            PrintHeader();
            Console.WriteLine("Usage: " +
                "\n    BeyondCompareBalsamic.CLI [-h] <source> [<target>]");
        }

        static void PrintError(string error)
        {
            Console.Error.WriteLine("error: " + error);
        }
    }
}
