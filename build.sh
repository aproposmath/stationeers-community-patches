#! /bin/bash
set -e

export VERSION=`./get_version.py`
~/.local/share/nvim/mason/bin/csharpier format *.cs
xmllint --format Patches.csproj -o Patches.csproj
dotnet build -c Debug Patches.csproj

cp bin/Debug/net46/Patches.dll ~/.sa/Stationeers/BepInEx/plugins/pytrapic/
