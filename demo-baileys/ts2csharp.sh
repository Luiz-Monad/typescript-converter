#!/bin/sh

node ../build/lib/Program.js ../../demo-baileys/input ../../demo-baileys/ast

dotnet ../build/TypeScriptConverter.dll -c tscconfig-demo-baileys.json