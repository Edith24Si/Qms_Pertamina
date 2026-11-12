#!/usr/bin/env bash
# ==============================================================================
# Script untuk menjalankan QMS Backend (QMS.Api) dan Frontend (QMS.Wasm)
# ==============================================================================

set -e

# Pindah ke direktori root repository
cd "$(dirname "$0")"

# Pastikan DOTNET_CLI_HOME terdefinisi
export DOTNET_CLI_HOME="${DOTNET_CLI_HOME:-$HOME}"

echo "=========================================================="
echo "           🚀 Memulai QMS Pertamina System                "
echo "=========================================================="

# Fungsi untuk mematikan semua proses anak saat script dihentikan (Ctrl+C)
cleanup() {
    echo ""
    echo "🛑 Menghentikan QMS.Api dan QMS.Wasm..."
    if [ -n "$API_PID" ] && kill -0 "$API_PID" 2>/dev/null; then
        kill "$API_PID" 2>/dev/null || true
    fi
    if [ -n "$WASM_PID" ] && kill -0 "$WASM_PID" 2>/dev/null; then
        kill "$WASM_PID" 2>/dev/null || true
    fi
    wait 2>/dev/null || true
    echo "✅ Semua proses berhasil dihentikan."
    exit 0
}

trap cleanup INT TERM EXIT

# Build terlebih dahulu untuk memastikan tidak ada error kompilasi
echo "🔨 Membangun solusi (dotnet build)..."
dotnet build Qms_Pertamina.sln -m:1 --nologo -v q

echo "🌐 Menjalankan Backend (QMS.Api)..."
dotnet run --project QMS.Api/QMS.Api.csproj --launch-profile https --no-build &
API_PID=$!

echo "💻 Menjalankan Frontend (QMS.Wasm)..."
dotnet run --project QMS.Wasm/QMS.Wasm.csproj --launch-profile http --no-build &
WASM_PID=$!

sleep 2

echo ""
echo "=========================================================="
echo " Aplikasi berhasil dijalankan:"
echo " 📡 Backend API   : https://localhost:7109 atau http://localhost:5260"
echo " 📑 Swagger UI    : https://localhost:7109/swagger"
echo " 🖥️  Frontend Wasm : http://localhost:5123"
echo "=========================================================="
echo " Tekan Ctrl + C untuk menghentikan kedua aplikasi."
echo "=========================================================="
echo ""

# Tunggu hingga kedua proses selesai
wait $API_PID $WASM_PID
