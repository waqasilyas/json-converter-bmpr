using System;
using System.IO;
using BalsamiqArchiveModel.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonConverterBalsamiq
{
    class Program
    {
        private const int ErrorBadArguments = -0x1;
        private const int ErrorFileDoesntExists = -0x2;
        private const int ErrorUnknown = -0x3;

        static void Main(string[] args)
        {
            String source = null;
            String target = null;
            bool forceOverwrite = false;

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
                        else if (a.Equals("-f"))
                        {
                            forceOverwrite = true;
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

                // Load project file
                MockupProject project = BalsamiqArchiveReader.LoadProject(source);

                // Create a writer
                TextWriter writer;
                if (target != null)
                {
                    if (File.Exists(target) && !forceOverwrite)
                    {
                        throw new Exception(String.Format("File already exists '{0}'. Use '-f' to overwrite.", target));
                    }
                    else
                    {
                        writer = new StreamWriter(target);
                    }
                }
                else
                {
                    writer = Console.Out;
                }

                // Serialize to JSON
                JsonSerializer serializer = new JsonSerializer();
                serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, project);
                writer.Close();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
#if (DEBUG)
                Console.Error.WriteLine("\nDebug trace:\n" + e.StackTrace);
#endif
#if (!DEBUG)
                Environment.Exit(ErrorUnknown);
#endif
            }
            finally
            {
#if (DEBUG)
                Console.WriteLine("\nPress any key to exit...");
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
    JsonConverterBalsamiq.exe [-hf] SOURCE [TARGET]

    SOURCE    A *.bmpr file to convert.
    TARGET    The destination file to save the JSON output. If not give, the JSON is emitted on standard output.
    -f        Force overwrite if TARGET exists
    -h        Prints this description.");
        }

        static void PrintError(string error)
        {
            Console.Error.WriteLine("error: " + error);
        }
    }
}
