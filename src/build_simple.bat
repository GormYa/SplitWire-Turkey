@echo off
echo Building SplitWire-Turkey C# WPF Application (Simple Build)...

REM Check if .NET SDK is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo Error: .NET SDK not found. Please install .NET 6.0 SDK or later.
    pause
    exit /b 1
)

REM Clean previous builds
if exist "SplitWireTurkey\bin" rmdir /s /q "SplitWireTurkey\bin"
if exist "SplitWireTurkey\obj" rmdir /s /q "SplitWireTurkey\obj"

REM Create Resources directory if it doesn't exist
if not exist "SplitWireTurkey\Resources" mkdir "SplitWireTurkey\Resources"

REM Copy resource files if they exist in the main directory
if exist "splitwire.ico" copy "splitwire.ico" "SplitWireTurkey\Resources\"
if exist "splitwire-logo-128.png" copy "splitwire-logo-128.png" "SplitWireTurkey\Resources\"
if exist "splitwireturkeytext.png" copy "splitwireturkeytext.png" "SplitWireTurkey\Resources\"
if exist "loading.gif" copy "loading.gif" "SplitWireTurkey\Resources\"
if exist "wgcf.exe" copy "wgcf.exe" "SplitWireTurkey\Resources\"

REM Build the application
cd SplitWireTurkey
dotnet restore
dotnet build -c Release

REM Create res folder in output directory
if not exist "bin\Release\net6.0-windows\res" mkdir "bin\Release\net6.0-windows\res"

REM Copy resource files to res folder
if exist "Resources\splitwire.ico" copy "Resources\splitwire.ico" "bin\Release\net6.0-windows\res\"
if exist "Resources\splitwire-logo-128.png" copy "Resources\splitwire-logo-128.png" "bin\Release\net6.0-windows\res\"
if exist "Resources\splitwireturkeytext.png" copy "Resources\splitwireturkeytext.png" "bin\Release\net6.0-windows\res\"

REM Clean up unnecessary files (keep only .exe and .dll)
if exist "bin\Release\net6.0-windows\SplitWire-Turkey.deps.json" del "bin\Release\net6.0-windows\SplitWire-Turkey.deps.json"
if exist "bin\Release\net6.0-windows\SplitWire-Turkey.pdb" del "bin\Release\net6.0-windows\SplitWire-Turkey.pdb"
if exist "bin\Release\net6.0-windows\SplitWire-Turkey.xml" del "bin\Release\net6.0-windows\SplitWire-Turkey.xml"

echo.
echo Build complete!
echo Executable created in: SplitWireTurkey\bin\Release\net6.0-windows\SplitWire-Turkey.exe
echo Resource files copied to: SplitWireTurkey\bin\Release\net6.0-windows\res\
echo.
echo Note: This is a simple build without publishing. Run the executable from the bin folder.
pause 