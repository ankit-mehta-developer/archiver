﻿using System;
using Serilog;
using Utility.CommandLine;

namespace Archiver
{
    class Program
    {
        [Argument('s', "source", "Source directory")]
        private static string Source { get; set; }

        [Argument('d', "destination", "Destination directory")]
        private static string Destination { get; set; }

        [Argument('e', "extensions", "Extensions to archive, example: \".mp4|.avi|.mpeg\", default is \".*\"")]
        private static string Extensions { get; set; }

        static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Archiver.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information($"Starting {Environment.CommandLine} from {Environment.CurrentDirectory}");
            Arguments.Populate();
            if (Source == null || Destination == null)
            {
                Log.Warning("Invalid arguments");
                return 1;
            }
            var archivingMechanism = new ArchivingMechanism();
            int retCode = 0;
            try
            {
                retCode = archivingMechanism.Archive(Source, Destination);
            }
            catch(Exception ex)
            {
                Log.Error($"Exception during archiving - {ex}");
                retCode = 10;
            }
            Log.Information($"Exiting with code {retCode}");
            return retCode;
        }
    }
}
