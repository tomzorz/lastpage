cd lastpage
dotnet build -c release
dotnet pack -c release -o nupkg
dotnet tool uninstall -g lastpage
dotnet tool install --add-source .\nupkg -g lastpage
cd ..