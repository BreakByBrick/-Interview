using System;
using System.IO;
using System.Text;
using System.Threading;

using ArchiveUtil.Enums;

namespace ArchiveUtil
{
	public delegate void ProgressCallback(int curVal, int maxVal);

	/// <summary>
	/// A base type of file processor that performs operations on a file.
	/// </summary>
	public abstract class FileProcessorBase
	{
		protected Thread _writeThread;
		protected static int _processThreadNumber = Environment.ProcessorCount * 2 - 2;
		protected Thread[] _processThreads = new Thread[_processThreadNumber];
		protected Thread _readThread;

		protected FileProcessResult _result = FileProcessResult.Success;
		protected Exception _exception;

		public FileProcessorMode Mode { get; private set; }

		public string SourceFilename { get; set; }

		public string ResultFilename { get; set; }

		public FileSliceQueue ProcessQueue { get; private set; }

		public FileSliceQueue WriteQueue { get; private set; }

		public FileWriteFilter WriteFilter { get; private set; }

		public FileReadFilter ReadFilter { get; private set; }

		public FileProcessFilter ProcessFilter { get; private set; }

		public int SliceSize { get; set; } = 1024 * 1024;

		public FileProcessorBase(FileProcessorMode mode, string sourceFilename, string resultFilename)
		{
			Mode = mode;
			SourceFilename = sourceFilename;
			ResultFilename = resultFilename;

			ProcessQueue = new FileSliceQueue(FileQueueMode.Process, ref _processThreadNumber);
			WriteQueue = new FileSliceQueue(FileQueueMode.Write, ref _processThreadNumber);

			ReadFilter = new FileReadFilter(this);
			WriteFilter = new FileWriteFilter(this);
			ProcessFilter = new FileProcessFilter(this);
		}

		/// <summary>
		/// Start file processing.
		/// </summary>
		public void Start()
		{
			var start = DateTime.Now;

			try
			{

				// Write thread 
				_writeThread = new Thread(() => SafeThreadStart(WriteFilter.Execute));
				_writeThread.Name = $"{_writeThread.ManagedThreadId} - WRITING :: ";
				_writeThread.Start();

				// Process threads
				for (int i = 0; i < _processThreadNumber; i++)
				{
					var processThread = new Thread(() => SafeThreadStart(ProcessFilter.Execute));
					processThread.Name = $"{processThread.ManagedThreadId} - PROCESSING :: ";
					_processThreads[i] = processThread;
					processThread.Start();
				}

				// Read thread
				_readThread = new Thread(() => SafeThreadStart(ReadFilter.Execute));
				_readThread.Name = $"{_readThread.ManagedThreadId} - READING :: ";
				_readThread.Start();
			}
			catch
			{
				throw;
			}
			finally
			{
				// Waiting for write thread completion
				_writeThread.Join();
			}

			switch (_result)
			{
				case FileProcessResult.Success:
					Console.WriteLine($"\nCompleted. Time of work: {DateTime.Now - start}.");
					break;

				case FileProcessResult.Canceled:
					File.Delete(ResultFilename);
					Console.WriteLine($"\nCanceled. Time of work: {DateTime.Now - start}.");
					break;

				case FileProcessResult.Exception:
					File.Delete(ResultFilename);
					Console.WriteLine($"\nException caught. Time of work: {DateTime.Now - start}.");
					Console.WriteLine($"Exception message: {_exception.Message}");
					break;
			}
		}

		/// <summary>
		/// Cancel file processing.
		/// </summary>
		public void Cancel()
		{
			_result = FileProcessResult.Canceled;

			Stop();
		}

		private void SafeThreadStart(Action threadStart)
		{
			try
			{
				threadStart();
			}
			catch (Exception ex)
			{
				_result = FileProcessResult.Exception;
				_exception = ex;

				Stop();
			}
		}

		private void Stop()
		{
			ProcessQueue.Stop();
			WriteQueue.Stop();
		}
	}

	/// <summary>
	/// File processor that performs compression.
	/// </summary>
	public class FileCompressor : FileProcessorBase
	{
		public FileCompressor(string sourceFilename, string resultFilename)
			: base(FileProcessorMode.Compress, sourceFilename, resultFilename)
		{
		}
	}

	/// <summary>
	/// File processor that performs decompression.
	/// </summary>
	public class FileDecompressor : FileProcessorBase
	{
		public FileDecompressor(string sourceFilename, string resultFilename)
			: base(FileProcessorMode.Decompress, sourceFilename, resultFilename)
		{
		}
	}
}
