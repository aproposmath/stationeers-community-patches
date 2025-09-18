#! /bin/bash
set -e

export VERSION=`./get_version.py`
~/.local/share/nvim/mason/bin/csharpier format *.cs
xmllint --format CommunityPatches.csproj -o CommunityPatches.csproj
dotnet build -c Release CommunityPatches.csproj

cp bin/Debug/net46/CommunityPatches.dll .
zip -r CommunityPatches-${VERSION}.zip CommunityPatches.dll
