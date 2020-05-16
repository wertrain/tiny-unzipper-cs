using System.IO.Compression;
using Microsoft.Extensions.CommandLineUtils;

namespace TinyUnzipper
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);

            app.Name = nameof(TinyUnzipper);
            app.Description = "unzip the zip file.";
            app.HelpOption("-h|--help");

            var fileNameOption = app.Option(
                template: "-f|--file",
                description: "file name after unzip",
                optionType: CommandOptionType.SingleValue
            );

            var filePathArgument = app.Argument(
                name: "file",
                description: "zip file path",
                multipleValues: false);

            app.OnExecute(() =>
            {
                try
                {
                    var filePath = filePathArgument.Value;
                    if (filePath == null)
                    {
                        app.ShowHelp();
                        return 1;
                    }

                    if (!System.IO.File.Exists(filePath))
                    {
                        return 1;
                    }

                    var unzipFileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    if (fileNameOption.HasValue())
                    {
                        unzipFileName = fileNameOption.Value();
                    }

                    ZipFile.ExtractToDirectory(filePath, unzipFileName);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e);
                    return 2;
                }
                return 0;
            });

            return app.Execute(args);
        }
    }
}
