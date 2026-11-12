@echo off
title QMS Pertamina System
echo ==========================================================
echo            Memulai QMS Pertamina System
echo ==========================================================

echo [1/3] Memeriksa & Kompilasi Solusi (dotnet build)...
dotnet build "%~dp0Qms_Pertamina.sln" --nologo
if %errorlevel% neq 0 (
    echo [ERROR] Terjadi kesalahan saat build.
    pause
    exit /b %errorlevel%
)

echo [2/3] Menjalankan Backend API (http://localhost:5260)...
start "QMS Backend API" cmd /k "dotnet run --project \"%~dp0QMS.Api\QMS.Api.csproj\" --launch-profile http --no-build"

timeout /t 3 /nobreak >nul

echo [3/3] Menjalankan Frontend Web Blazor (http://localhost:5123)...
start "QMS Frontend Blazor" cmd /k "dotnet run --project \"%~dp0QMS.Wasm\QMS.Wasm.csproj\" --launch-profile http --no-build"

timeout /t 3 /nobreak >nul

echo Membuka browser ke http://localhost:5123 ...
start http://localhost:5123

echo ==========================================================
echo  Aplikasi QMS Pertamina berhasil dijalankan!
echo  - Web Application : http://localhost:5123
echo  - REST API        : http://localhost:5260
echo  - Swagger UI      : http://localhost:5260/swagger
echo ==========================================================
