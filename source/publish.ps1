dotnet clean
dotnet restore
dotnet build -c Release
dotnet publish swi -c Release -r win-x64 --self-contained true -o ../publish