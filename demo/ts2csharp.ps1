Push-Location $PSScriptRoot

New-Item -Type Directory ../../demo/ast -ErrorAction Ignore

node ../build/lib/Program.js ../../demo/input ../../demo/ast
dotnet ../build/TypeScriptConverter.dll -c tscconfig-demo.json

Pop-Location
