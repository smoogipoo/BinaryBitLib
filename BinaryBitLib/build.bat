del /f /q *.nupkg
nuget pack BinaryBitLib.nuspec -Prop Configuration=Release
nuget push *.nupkg