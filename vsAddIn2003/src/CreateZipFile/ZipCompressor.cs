using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;

namespace Mfconsulting.Vsprj2make
{
	public class ZipCompressor
	{
		/// <summary>
		/// Level of compression
		/// </summary>
		public int m_nCompressionLevel = 6;

		/// <summary>
		/// Level of compression
		/// </summary>
		public int CompressionLevel
		{
			get { return m_nCompressionLevel; }
			set 
			{ 
				if(value > 9)
				{
					m_nCompressionLevel = 9;
					return;
				}

				if(value < 1)
				{
					m_nCompressionLevel = 1;
					return;
				}

				m_nCompressionLevel = value;
			}
		}

		public string GetFileNameWithoutDrive(string strInFullPath)
		{
			System.IO.DirectoryInfo diObj = null;
			string strRetVal;
			string strFileName = System.IO.Path.GetFileName(strInFullPath);
			string strDirectory = System.IO.Path.GetDirectoryName(strInFullPath);

			diObj = new DirectoryInfo(strDirectory);

			strRetVal = String.Format("{0}{1}{2}",
                diObj.FullName.Substring(diObj.Root.Name.Length),
				System.IO.Path.DirectorySeparatorChar,
				strFileName
				);

			return strRetVal.Replace("\\", "/");
		}

		public void CreateZipFile(string[] straFilenames, string strOutputFilename)
		{
			Crc32 crc = new Crc32();
			ZipOutputStream zos = new ZipOutputStream(File.Create(strOutputFilename));
		
			zos.SetLevel(m_nCompressionLevel);
		
			foreach (string strFileName in straFilenames) 
			{
				FileStream fs = File.OpenRead(strFileName);
			
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				ZipEntry entry = new ZipEntry(GetFileNameWithoutDrive(strFileName));
			
				entry.DateTime = DateTime.Now;
			
				entry.Size = fs.Length;
				fs.Close();
			
				crc.Reset();
				crc.Update(buffer);
			
				entry.Crc  = crc.Value;
			
				zos.PutNextEntry(entry);
			
				zos.Write(buffer, 0, buffer.Length);			
			}
		
			zos.Finish();
			zos.Close();
		}
	}
}
