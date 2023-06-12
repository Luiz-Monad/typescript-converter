Push-Location $PSScriptRoot

New-Item -ItemType Directory ../../demo-baileys/ast -ErrorAction Ignore

node ../build/lib/Program.js ../../demo-baileys/input ../../demo-baileys/ast
dotnet ../build/TypeScriptConverter.dll -c tscconfig-demo-baileys.json

Pop-Location
