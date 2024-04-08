using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ArchiveUtil
{
	class Program
	{
		/// <summary>
		/// </summary>
		/// <param name="args">
		/// args[0] - command type ('compress' - encode file, 'decompress' - decode file);
		/// args[1] - path to source file;
		/// args[2] - path to result file.
		/// </param>
		static int Main(string[] args)
		{
			//args = new string[] { "compress", "./test.pdf", "./sfd.txt" };
			//args = new string[] { "decompress", "./sfd.gz", "./out.pdf" };

			try
			{
				FileProcessorBase fileProcessor = null;

				Console.CancelKeyPress += (s, e) =>
				{
					if (e.SpecialKey == ConsoleSpecialKey.ControlC)
					{
						e.Cancel = true;
						fileProcessor.Cancel();
					}
				};

				ValidateArgs(args);

				switch (args[0])
				{
					case "compress":
						Console.WriteLine($"Compression started... (press Ctrl+C for cancel)");
						fileProcessor = new FileCompressor(args[1], args[2]);
						fileProcessor.ReadFilter.Progress += OnProgress;
						fileProcessor.Start();
						break;

					case "decompress":
						Console.WriteLine($"Decompression started... (press Ctrl+C for cancel)");
						fileProcessor = new FileDecompressor(args[1], args[2]);
						fileProcessor.ReadFilter.Progress += OnProgress;
						fileProcessor.Start();
						break;

					default:
						Console.WriteLine("Unknown command.");
						break;
				}

				Console.ReadLine();
				return 0;
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine("Invalid argument input format.\n" + ex.Message);
				Console.ReadLine();
				return 1;
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR:\n" + ex.Message);
				Console.ReadLine();
				return 1;
			}
		}

		private static void OnProgress(int curVal, int maxVal)
		{
			var chunkNumber = 50;
			float chunk = (float)chunkNumber / maxVal;

			int position = Console.CursorLeft;

			// Outputting the processed part.
			for( int i = position + 1; i < chunk * curVal + 1; i++)
			{
				Console.BackgroundColor = ConsoleColor.Green;
				Console.CursorLeft = position++;
				Console.Write(" ");
			}
			var completedPosition = position;

			// Outputting the raw part.
			if( position == 0)
			{
				for (int i = 0; i <= chunkNumber - 1; i++)
				{
					Console.BackgroundColor = ConsoleColor.Gray;
					Console.Write(" ");
				}
			}

			// Outputting precents.
			Console.CursorLeft = chunkNumber + 2;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write(curVal.ToString() + " %");
			Console.CursorLeft = completedPosition;
		}

		private static void ValidateArgs(string[] args)
		{
			if (args.Length == 0 || args.Length != 3 || args[0] == null || args[1] == null || args[2] == null)
				throw new ArgumentException("Please, enter 3 arguments: command_type source_filename result_filename.\n" +
					"command_type - type of operation: 'compress' or 'decompress',\n" +
					"source_filename, result_filename - relative path to source and result files respectively.");

			if (args[0] != "compress" && args[0] != "decompress")
				throw new ArgumentException("Wrong command type.\n 1st argument should be 'compress' or 'decompress'.");

			args[1] = Path.GetFullPath(args[1]);
			args[2] = Path.GetFullPath(args[2]);

			if (!args[1].Contains('.') || !args[2].Contains('.'))
				throw new ArgumentException("Cant define file extension.");

			var extension = args[1].Substring(args[1].LastIndexOf('.')); // fastest way to get extension
			if (args[0] == "compress")
			{
				if (args[1].Substring(args[1].LastIndexOf('.')) == ".gz")
					throw new ArgumentException("Source file already compressed.");

				extension = args[2].Substring(args[2].LastIndexOf('.'));
				if (extension != ".gz")
					args[2] = args[2].Replace(extension, ".gz");
			}
			else if (extension != ".gz")
			{
				throw new ArgumentException("Source file must have '.gz' extension.");
				//args[1] = args[1].Replace(extension, ".gz");
			}

			// dont need to check extension of result filename, cause will be read from compressed stream

			if (!File.Exists(args[1]))
				throw new ArgumentException("Source file not found.\nСheck the correctness of the input of the 2nd argument.");

			if (File.Exists(args[2]))
				throw new ArgumentException("Result file already exists.\nСheck the correctness of the input of the 3d argument.");

			if (args[1] == args[2])
				throw new ArgumentException("Source and result files should be different.\nСheck the correctness of the input of the 2nd and 3d arguments.");
		}
	}
}
