using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using BalsamiqArchiveModel.Model;

namespace JsonConverterBalsamiq
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

            try
            {
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
                        else
                        {
                            PrintError(String.Format("Unknown flag '{0}'\n", a));
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
            finally
            {
#if (DEBUG)
                Console.In.Read();
#endif
            }
        }

        static void PrintHeader()
        {
            Console.WriteLine("This program dumps the contents of a Balsamic Mockups project file (*.bmpr) into a JSON text file." +
                "\nFor more details, see: https://github.com/waqasilyas/JsonConverterBalsamiq/wiki \n");
        }

        static void PrintHelp()
        {
            PrintHeader();
            Console.WriteLine(@"Usage: 
    JsonConverterBalsamiq.exe [-h] <source> [<target>]

     -h          Prints this description.
     <source>    A *.bmpr file to convert.
     <target>    The destination file to save the JSON output. If not give, the JSON is emitted on standard output.");
        }

        static void PrintError(string error)
        {
            Console.Error.WriteLine("error: " + error);
        }
    }
}
