﻿// 
// TypeScriptCompilerIOHost.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013 Matthew Ward
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using System.Linq;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TypeScriptCompilerIOHost : IIO
	{
		string executingFilePath;
		
		public TypeScriptCompilerIOHost()
		{
			arguments = new string[0];
			stderr = new StandardOutputTextWriter();
			stdout = stderr;
			SetExecutingFilePath();
			
			Environment = new TypeScriptCompilerEnvironment(this);
		}
		
		void SetExecutingFilePath()
		{
			string fileName = new ScriptLoader().TypeScriptCompilerFileName;
			executingFilePath = System.IO.Path.GetDirectoryName(fileName) + System.IO.Path.DirectorySeparatorChar;
		}
		
		public TypeScriptCompilerEnvironment Environment { get; private set; }
		
		public string[] arguments { get; set; }
		
		public ITextWriter stderr { get; set; }
		public ITextWriter stdout { get; set; }
		
		public FileInformation readFile(string path, int codepage)
		{
			LogFormat("readFile() codepage {0} '{1}'", codepage, path);
			string contents = File.ReadAllText(path);
			return new FileInformation(contents, ByteOrderMark.None);
		}
		
		public void writeFile(string path, string contents, bool writeByteOrderMark)
		{
			LogFormat("writeFile() '{0}'", path);
			File.WriteAllText(path, contents);
		}
		
		public void deleteFile(string path)
		{
			LogFormat("deleteFile() '{0}'", path);
		}
		
		public string[] dir(string path, object spec, object options)
		{
			LogFormat("dir() '{0}'", path);
			return new string[0];
		}
		
		public bool fileExists(string path)
		{
			LogFormat("fileExists() '{0}'", path);
			return System.IO.File.Exists(path);
		}
		
		public bool directoryExists(string path)
		{
			LogFormat("directoryExists() '{0}'", path);
			return System.IO.Directory.Exists(path);
		}
		
		public void createDirectory(string path)
		{
			LogFormat("createDirectory() '{0}'", path);
		}
		
		public string resolvePath(string path)
		{
			LogFormat("resolvePath() '{0}'", path);
			if (System.IO.Path.IsPathRooted(path)) {
				return path;
			}
			return System.IO.Path.Combine(getExecutingFilePath(), path);
		}
		
		public string dirName(string path)
		{
			LogFormat("dirName '{0}'", path);
			
			if (String.IsNullOrEmpty(path))
				return path;
			
			return System.IO.Path.GetDirectoryName(path);
		}
		
		public IFindFileResult findFile(string rootPath, string partialFilePath)
		{
			LogFormat("findFile '{0}'", partialFilePath);
			return null;
		}
		
		public void print(string str)
		{
			TaskService.BuildMessageViewCategory.AppendText(str);
		}
		
		public void printLine(string str)
		{
			TaskService.BuildMessageViewCategory.AppendLine(str);
		}
		
		public void run(string source, string filename)
		{
			LogFormat("run filename: '{0}'", filename);
		}
		
		public string getExecutingFilePath()
		{
			LogFormat("getExecutingFilePath()");
			return executingFilePath;
		}
		
		public void quit(int exitCode)
		{
			QuitExitCode = exitCode;
			LogFormat("quit {0}", exitCode);
		}
		
		void LogFormat(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}
		
		internal int QuitExitCode { get; set; }
		
		public void appendFile(string path, string contents)
		{
			LogFormat("appendFile() '{0}'", path);
			File.AppendAllText(path, contents);
		}
	}
}
