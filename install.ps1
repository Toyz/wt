dotnet pack -c Release -o nupkg
dotnet tool uninstall -g wt-tool
dotnet tool install --add-source .\nupkg -g wt-tool
