call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat" x86

@msbuild build.msbuild /tv:14.0  /p:VisualStudioVersion=14.0 /p:ToolsVersion=14.0 %*
