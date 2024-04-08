using System.IO;
using System.Text;
using System;
using System.IO.Compression;

using ArchiveUtil.Enums;
using static System.Net.WebRequestMethods;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Xml.Linq;
using Microsoft.SqlServer.Server;

namespace ArchiveUtil
{
	/// <summary>
	/// A base type of file filter.
	/// </summary>
	public abstract class FileFilterBase
	{
		public FileFilterBase( FileProcessorBase fileProcessor )
		{
			FileProcessor = fileProcessor;
		}

		protected FileProcessorBase FileProcessor { get; private set; }

		/// <summary>
		/// Performs a file filter action.
		/// </summary>
		public abstract void Execute();
	}

	/// <summary>
	/// File filter that performs data slice reading.
	/// </summary>
	public class FileReadFilter : FileFilterBase
	{
		public FileReadFilter( FileProcessorBase fileProcessor )
			: base( fileProcessor )
		{
		}

		public event ProgressCallback Progress;

		public override void Execute()
		{
			//Console.WriteLine($"{Thread.CurrentThread.Name} start");
			using( var sourceStream = new FileStream( FileProcessor.SourceFilename, FileMode.Open, FileAccess.Read, FileShare.None ) )
			{
				UpdateProgressBar( sourceStream.Length, sourceStream.Position );
				switch( FileProcessor.Mode )
				{
					case FileProcessorMode.Compress:
						CoderExecute( sourceStream );
						break;

					case FileProcessorMode.Decompress:
						DecoderExecute( sourceStream );
						break;

					default:
						throw new Exception( "Unknown archiver mode." );
				}
			}
			FileProcessor.ProcessQueue.Stop();
			//Console.WriteLine($"{Thread.CurrentThread.Name} end");
		}

		private void CoderExecute( FileStream sourceStream )
		{
			var readBuffer = PrepairReadBuffer( sourceStream );
			while( true )
			{
				//Console.WriteLine($"{Thread.CurrentThread.Name} reading");
				if( sourceStream.Read( readBuffer, 0, readBuffer.Length ) <= 0 )
				{
					//Console.WriteLine($"{Thread.CurrentThread.Name} end on stream reading");
					break;
				}
				var slice = new FileSlice( FileProcessor.ProcessQueue.AddedSlicesNumber, readBuffer );
				if( !FileProcessor.ProcessQueue.Add( slice ) )
				{
					//Console.WriteLine($"{Thread.CurrentThread.Name} end on data putting");
					break;
				}
				readBuffer = PrepairReadBuffer( sourceStream );
				UpdateProgressBar( sourceStream.Length, sourceStream.Position );
				//Console.WriteLine($"{Thread.CurrentThread.Name} {slice.SerialNumber} slice, {readBuffer.Length} bytes. {Settings.ProcessQueue.Queue.Count} {Settings.WriteQueue.Queue.Count}");
			}
		}

		private void DecoderExecute( FileStream sourceStream )
		{
			// Reading the source file format from the compressed file.
			//ReadFileFormat(sourceStream);

			while( true )
			{
				//Console.WriteLine($"{Thread.CurrentThread.Name} reading");
				var sliceBuffer = new byte[ 4 ];
				if( sourceStream.Read( sliceBuffer, 0, sliceBuffer.Length ) <= 0 )
				{
					//Console.WriteLine($"{Thread.CurrentThread.Name} end on stream reading");
					break;
				}
				var sliceLength = BitConverter.ToInt32( sliceBuffer, 0 );

				sliceBuffer = new byte[ sliceLength ];
				if( sourceStream.Read( sliceBuffer, 0, sliceBuffer.Length ) <= 0 )
				{
					//Console.WriteLine($"{Thread.CurrentThread.Name} end on stream reading");
					break;
				}
				var slice = new FileSlice( FileProcessor.ProcessQueue.AddedSlicesNumber, sliceBuffer );
				if( !FileProcessor.ProcessQueue.Add( slice ) )
				{
					//Console.WriteLine($"{Thread.CurrentThread.Name} end on data putting");
					break;
				}
				UpdateProgressBar( sourceStream.Length, sourceStream.Position );
				//Console.WriteLine($"{Thread.CurrentThread.Name} {slice.SerialNumber} slice, {sliceBuffer.Length} bytes. {Settings.ProcessQueue.Queue.Count} {Settings.WriteQueue.Queue.Count}");
			}
		}

		/// <summary>
		/// Calling the progress bar update method.
		/// </summary>
		private void UpdateProgressBar( long streamLength, long streamPosition )
		{
			// The ratio of the length of the stream and the number of bytes processed.
			var coef = streamPosition == 0 ? 0 : ( double )streamLength / ( double )streamPosition;
			Progress?.Invoke( coef == 0 ? 0 : ( int )( 100 / coef ), 100 );
		}

		/// <summary>
		/// Считывание формата файла из сжатого файла.
		/// </summary>
		private void ReadFileFormat( FileStream sourceStream )
		{
			// Reading the length of the buffer allocated for storing the file format.
			var bufferLength = 4;
			var buffer = new byte[ bufferLength ];
			if( sourceStream.Read( buffer, 0, buffer.Length ) <= 0 )
				throw new Exception( "Could not read length of header." );

			// Reading the file format.
			bufferLength = BitConverter.ToInt32( buffer, 0 );
			buffer = new byte[ bufferLength ];
			if( sourceStream.Read( buffer, 0, buffer.Length ) <= 0 )
				throw new Exception( "Could not read header." );

			var extension = Encoding.ASCII.GetString( buffer );
			FileProcessor.ResultFilename = Path.GetDirectoryName( FileProcessor.ResultFilename ) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension( FileProcessor.ResultFilename ) + extension;
		}

		/// <summary>
		/// Preparing a buffer for reading a slice from a stream.
		/// </summary>
		private byte[] PrepairReadBuffer( FileStream stream )
		{
			var streamRemainder = stream.Length - stream.Position;
			return new byte[ streamRemainder < FileProcessor.SliceSize ? streamRemainder : FileProcessor.SliceSize ];
		}
	}

	/// /// <summary>
	/// File filter that performs data slice writing.
	/// </summary>
	public class FileWriteFilter : FileFilterBase
	{
		public FileWriteFilter( FileProcessorBase fileProcessor )
			: base( fileProcessor )
		{ }

		public override void Execute()
		{
			//Console.WriteLine($"{Thread.CurrentThread.Name} start");
			using( var resultStream = new FileStream( FileProcessor.ResultFilename, FileMode.Append, FileAccess.Write, FileShare.None, 8, FileOptions.WriteThrough ) )
			{
				// If it is an encoder, then first we write the format of the source file to the file.
				//if (_settings.ArchiverMode == ArchiverMode.Coder)
				//{
				//    WriteFileFormat(resultStream);
				//}
				while( true )
				{
					//Console.WriteLine($"{Thread.CurrentThread.Name} writing");
					var writeSlice = FileProcessor.WriteQueue.Take();
					if( writeSlice == null )
					{
						//Console.WriteLine($"{Thread.CurrentThread.Name} end on data taking");
						//_settings.WriteQueue.Dispose();
						break;
					}

					if( FileProcessor.Mode == FileProcessorMode.Compress )
					{
						CoderExecute( writeSlice, resultStream );
					}
					else
					{
						DecoderExecute( writeSlice, resultStream );
					}

					// If need to complete the work and all queue elements have been processed, execute Dispose.
					if( FileProcessor.ProcessQueue.Stopped && FileProcessor.ProcessQueue.AddedSlicesNumber == FileProcessor.WriteQueue.AddedSlicesNumber && FileProcessor.WriteQueue.Queue.Count == 0 )
						FileProcessor.WriteQueue.Stop();

					//Console.WriteLine($"{Thread.CurrentThread.Name} {writeSlice.SerialNumber} slice, {writeSlice.Buffer.Length} bytes. {Settings.ProcessQueue.Queue.Count} {Settings.WriteQueue.Queue.Count}");
				}
			}
			//Console.WriteLine($"{Thread.CurrentThread.Name} end");
		}

		private void CoderExecute( FileSlice writeSlice, FileStream resultStream )
		{
			var bytes = BitConverter.GetBytes( writeSlice.Data.Length );
			resultStream.Write( bytes, 0, 4 );
			resultStream.Write( writeSlice.Data, 0, writeSlice.Data.Length );
		}

		private void DecoderExecute( FileSlice writeSlice, FileStream resultStream )
		{
			resultStream.Write( writeSlice.Data, 0, writeSlice.Data.Length );
		}

		/// <summary>
		/// Writing the format of the source file to the resulting file.
		/// </summary>
		/// <param name="resultStream"></param>
		private void WriteFileFormat( FileStream resultStream )
		{
			var buffer = Encoding.ASCII.GetBytes( Path.GetExtension( FileProcessor.SourceFilename ) );
			// Writing the length of the buffer allocated for recording the format.
			resultStream.Write( BitConverter.GetBytes( buffer.Length ), 0, 4 );
			// Writing the format.
			resultStream.Write( buffer, 0, buffer.Length );
		}
	}

	/// <summary>
	/// File filter that performs data slice processing.
	/// </summary>
	public class FileProcessFilter : FileFilterBase
	{
		public FileProcessFilter( FileProcessorBase fileProcessor )
			: base( fileProcessor )
		{
		}

		public override void Execute()
		{
			//Console.WriteLine($"{Thread.CurrentThread.Name} start");
			while( true )
			{
				//Console.WriteLine($"{Thread.CurrentThread.Name} processing");
				var readSlice = FileProcessor.ProcessQueue.Take();
				if( readSlice == null )
				{
					// If need to complete the work and all queue elements have been processed, execute Dispose.
					if( FileProcessor.ProcessQueue.Stopped && FileProcessor.ProcessQueue.AddedSlicesNumber == FileProcessor.WriteQueue.AddedSlicesNumber && FileProcessor.WriteQueue.Queue.Count == 0 )
						FileProcessor.WriteQueue.Stop();

					//Console.WriteLine($"{Thread.CurrentThread.Name} end on data taking");
					break;
				}

				FileSlice processedSlice;
				switch( FileProcessor.Mode )
				{
					case FileProcessorMode.Compress:
						processedSlice = CoderExecute( readSlice );
						break;

					case FileProcessorMode.Decompress:
						processedSlice = DecoderExecute( readSlice );
						break;

					default:
						throw new Exception( "Unknown archiver mode." );
				}

				if( !FileProcessor.WriteQueue.Add( processedSlice ) )
				{
					//Console.WriteLine($"{Thread.CurrentThread.Name} end on data putting");
					break;
				}

				// If need to complete the work and all queue elements have been processed, execute Dispose.
				//if (_settings.ProcessQueue.Completed && _settings.ProcessQueue.AddedSlicesNumber == _settings.WriteQueue.AddedSlicesNumber && _settings.WriteQueue.Queue.Count == 0)
				//    _settings.WriteQueue.Dispose();

				//Console.WriteLine($"{Thread.CurrentThread.Name} {processedSlice.SerialNumber} slice, input - {readSlice.Buffer.Length} bytes, output - {processedSlice.Buffer.Length} bytes. {Settings.ProcessQueue.Queue.Count} {Settings.WriteQueue.Queue.Count}");
			}
			//Console.WriteLine($"{Thread.CurrentThread.Name} end");
		}

		private FileSlice CoderExecute( FileSlice readSlice )
		{
			FileSlice processedSlice;
			using( var memoryStream = new MemoryStream() )
			{
				using( var gzipStream = new GZipStream( memoryStream, CompressionMode.Compress ) )
				{
					gzipStream.Write( readSlice.Data, 0, readSlice.Data.Length );
				}
				processedSlice = new FileSlice( readSlice.Index, memoryStream.ToArray() );
			}
			return processedSlice;
		}

		private FileSlice DecoderExecute( FileSlice readSlice )
		{
			FileSlice processedSlice;
			using( var readSliceStream = new MemoryStream( readSlice.Data ) )
			{
				using( var gzipStream = new GZipStream( readSliceStream, CompressionMode.Decompress ) )
				{
					using( var processedSliceStream = new MemoryStream() )
					{
						gzipStream.CopyTo( processedSliceStream );
						processedSlice = new FileSlice( readSlice.Index, processedSliceStream.ToArray() );
					}
				}
			}
			return processedSlice;
		}
	}
}
