# QMS Pertamina Patra Niaga RU II Dumai

Aplikasi Quality Management System (QMS) Pertamina berbasis **ASP.NET Core Web API (Backend)** dan **Blazor WebAssembly (Frontend)** dalam satu Solution terintegrasi (`Qms_Pertamina.sln`).

---

## 🔑 Akun & Hak Akses Pengguna

| Role | Username / Email | Password | Hak Akses & Tombol Aksi |
| :--- | :--- | :--- | :--- |
| **Administrator** | `adminqms` (atau `admin`) | `admin` | **Akses Penuh**: Muncul tombol **Tambah**, dan tombol aksi icon (**View**, **Unduh**, **Edit**, **Hapus**). |
| **Pegawai (Terdaftar)** | *Email Gmail yang didaftarkan* (misal `rian.pratama.pertamina@gmail.com`) | *Password saat daftar* | **Akses Pegawai**: Tombol Tambah, Edit, dan Hapus **disembunyikan secara otomatis**. Kolom aksi hanya berisi icon **View** dan **Unduh**. |
| **User Biasa (Tamu)** | *Username apa saja* (misal `user` / `pekerja`) | `user123` | **Akses Lihat & Unduh**: Tombol Tambah, Edit, dan Hapus **disembunyikan secara otomatis**. Kolom aksi hanya berisi icon **View** dan **Unduh**. |

> *Ganti role atau login ulang kapan saja via menu dropdown profil di pojok kanan atas atau via halaman Login.*

---

## 🎯 Fitur & Pembaruan Utama

1. **Akses Kontrol Berbasis Peran (Admin vs User/Pegawai)**:
   - **User / Pegawai**: Tombol **Tambah**, **Edit**, dan **Hapus** disembunyikan. Kolom aksi tabel hanya menyediakan tombol icon:
     - 👁️ **View**: Pratinjau dokumen / modal detail
     - 📥 **Unduh**: Download berkas resmi
   - **Admin**: Tombol **Tambah** tersedia di header dan berfungsi penuh. Kolom aksi tabel memiliki tombol icon lengkap (32x32px):
     - 👁️ **View**: Pratinjau dokumen / modal detail
     - 📥 **Unduh**: Download berkas resmi
     - ✏️ **Edit**: Form modal pengubahan data
     - 🗑️ **Hapus**: Modal konfirmasi penghapusan data

2. **Registrasi Pegawai Khusus Gmail di Halaman Login**:
   - Di halaman `/login` tersedia tab toggle **"Masuk Portal"** dan **"Registrasi Pegawai"**.
   - Khusus mendaftar sebagai **Pegawai** (otomatis mendapatkan role Pegawai).
   - Wajib menggunakan alamat email aktif **Google Mail (`@gmail.com`)**.
   - Data otomatis tersinkronisasi ke basis data pengguna terdaftar (`tb_registered_user`) dan master data pegawai (`tb_employee`).
   - Dapat langsung digunakan untuk login dengan kredensial Gmail dan password yang dibuat.

3. **Menu "User" & "Daftar Pengguna" (Khusus Administrator)**:
   - Menu **"User"** di navbar dan **"Daftar Pengguna"** di dropdown profil hanya ditampilkan untuk role **Administrator** (disembunyikan sepenuhnya untuk user biasa / pegawai).
   - Menampilkan daftar pegawai dan akun yang telah mendaftar di portal QMS.
   - Dilengkapi fitur pencarian multi-kolom (Nama, NIP, Gmail, Fungsi, Jabatan).
   - Khusus Administrator dapat menambahkan akun pengguna baru, mengedit, dan menghapus akun. User biasa yang mencoba mengakses `/users` langsung dialihkan ke `/dashboard`.

4. **Fitur Foto Profil (Unggah & Pilih Avatar)**:
   - Pada halaman `/profile`, pengguna dapat menambah atau mengubah foto profil pribadi.
   - Mendukung unggah gambar dari komputer (JPG, PNG, WebP hingga 2 MB) dengan konversi Base64 otomatis.
   - Pratinjau foto langsung sebelum disimpan.
   - Pilihan 3 template avatar korporat cepat.
   - Foto profil langsung terpasang di avatar header navigasi dan profil pengguna.

5. **Navigasi Ramping & Elegan Tanpa Icon**:
   - Seluruh icon font-awesome pada menu navigasi telah dihilangkan sehingga menghasilkan tampilan yang bersih, minimalis, dan profesional.
   - Lebar dan spasi navbar telah dioptimalkan agar tidak terlalu lebar, proporsional, dan nyaman di mata pengguna.

6. **Tombol Khusus "Masuk STK"**:
   - Menu STK di navbar telah dipindahkan menjadi tombol tersendiri **"Masuk STK"** di pojok paling kanan atas header.
   - Klik tombol **"Masuk STK"** akan langsung membuka tautan resmi panduan tata kerja video Pertamina di YouTube: `https://youtu.be/ag7MoapQjAE?si=0lOGpNRP-Akrzi0A`.

7. **Integrasi REST API & Swagger UI**:
   - Solusi terpadu 1 solution: Frontend Blazor WASM dan Backend ASP.NET Core Web API terhubung penuh via HTTP REST API.
   - Dokumentasi Swagger REST API interaktif tersedia di [http://localhost:5260/swagger](http://localhost:5260/swagger) atau [https://localhost:7109/swagger](https://localhost:7109/swagger).

8. **Dukungan Penuh Microsoft SQL Server & Database Terintegrasi**:
   - Backend telah dikonfigurasi penuh menggunakan provider **`Microsoft.EntityFrameworkCore.SqlServer`**.
   - Skrip inisialisasi lengkap siap pakai disediakan di file **[`SqlServer_Init_db_qms.sql`](file:///Applications/nonaa/Qms_Pertamina/SqlServer_Init_db_qms.sql)** (dapat langsung dieksekusi di SQL Server Management Studio / SSMS, Azure Data Studio, atau `sqlcmd`). Skrip ini otomatis membuat database `db_qms`, semua tabel, serta mengimpor **410 dokumen mutu** (`tb_upload`), **1.882 pegawai** (`tb_employee`), **58 master menu** (`tb_menu`), dan data pengguna (`tb_registered_user`).
   - Pengaturan koneksi SQL Server tersedia di `appsettings.json` (`ConnectionStrings:DefaultConnection`).
   - Sistem memiliki mekanisme cerdas (*graceful fallback*): jika dijalankan di macOS tanpa container SQL Server aktif, server otomatis menggunakan database cadangan lokal SQLite `qms.db` agar aplikasi tidak crash dan tetap dapat diuji. Saat dijalankan di Windows dengan SQL Server aktif, backend otomatis terhubung langsung ke Microsoft SQL Server.

9. **Backend Controllers Lengkap & Terpadu**:
   - Seluruh controllers REST API kini telah tersedia lengkap baik pada project **`Backend`** maupun **`QMS.Api`**:
     - `DocumentsController.cs`: Manajemen 410 berkas dokumen mutu, filter kategori/submenu, pencarian multi-kolom, upload, edit, dan hapus.
     - `EmployeesController.cs`: Data 1.882 pekerja/pegawai RU II Dumai, pencarian NIP/Nama/Fungsi/Jabatan, tambah, ubah, dan hapus.
     - `UsersController.cs`: Pendaftaran pegawai khusus akun `@gmail.com`, otentikasi login, profil, foto profil, dan manajemen akun pengguna.
     - `MenusController.cs`: Master 58 kategori menu dan submenu sistem QMS.
     - `SasaranMutuController.cs`: Data KPI dan gugus mutu.
   - Dokumentasi Swagger interaktif aktif di [http://localhost:5260/swagger](http://localhost:5260/swagger).

---

## 🚀 Cara Menjalankan di Visual Studio Code

### Persyaratan:
- [.NET SDK](https://dotnet.microsoft.com/download) (versi 10.x)
- Visual Studio Code dengan ekstensi C# / C# Dev Kit (opsional namun direkomendasikan)

---

### Opsi 1: Menggunakan Script Terminal (Paling Mudah & Direkomendasikan)
Buka Terminal di VS Code (`Ctrl + ~` atau `Cmd + ~`), lalu jalankan:

```bash
./run.sh
```

Script ini otomatis membangun solusi (`-m:1`) dan menjalankan kedua server secara bersamaan:
- **Frontend Blazor**: [http://localhost:5123](http://localhost:5123)
- **Backend REST API**: [http://localhost:5260](http://localhost:5260) / [https://localhost:7109](https://localhost:7109)
- **Swagger Documentation**: [http://localhost:5260/swagger](http://localhost:5260/swagger)

Untuk menghentikan kedua server, tekan `Ctrl + C` di terminal.

---

### Opsi 2: Run & Debug di Visual Studio Code (F5)
1. Buka tab **Run and Debug** di sidebar kiri VS Code (`Cmd + Shift + D` / `Ctrl + Shift + D`).
2. Pilih konfigurasi **"Start Both (API + WASM)"**.
3. Tekan **F5** (Play).

---

### Opsi 3: Menjalankan Manual via Terminal

**1. Jalankan Backend API:**
```bash
dotnet run --project QMS.Api/QMS.Api.csproj --launch-profile http
```

**2. Jalankan Frontend Blazor WASM:**
```bash
dotnet run --project QMS.Wasm/QMS.Wasm.csproj --launch-profile http
```

---

## 📁 Struktur Solution (`Qms_Pertamina.sln`)

```
Qms_Pertamina/
├── Qms_Pertamina.sln           # Solution file utama (.NET 10)
├── run.sh                      # Script 1-click startup (Backend + Frontend)
├── SqlServer_Init_db_qms.sql   # Skrip DDL & DML lengkap untuk Microsoft SQL Server
├── qms.db                      # Basis data cadangan lokal SQLite
├── .vscode/                    # Konfigurasi VS Code (launch.json & tasks.json)
├── QMS/                        # QMS.Core (Entities, DbContext, Models)
│   ├── Data/AppDbContext.cs
│   └── Models/ (TbUpload, TbEmployee, TbMenu, TbRegisteredUser, DocumentDto)
├── Backend/                    # ASP.NET Core Web API Backend
│   ├── Controllers/ (DocumentsController, EmployeesController, MenusController, UsersController, SasaranMutuController)
│   └── Program.cs              # REST API & SQL Server DbContext configuration
├── QMS.Api/                    # ASP.NET Core Web API Backend (QMS.Api)
│   ├── Controllers/ (DocumentsController, EmployeesController, MenusController, UsersController, SasaranMutuController)
│   └── Program.cs              # REST API & Swagger UI configuration
└── QMS.Wasm/                   # Blazor WebAssembly Frontend
    ├── Services/ (QmsApiService.cs, UserState.cs)
    ├── Layout/ (MainLayout.razor, NavMenu.razor)
    └── Pages/
        ├── Auth/ (Login.razor)
        ├── Users/ (UserList.razor)
        ├── Profile.razor
        ├── Documents/ (DocumentList.razor, DocumentUpload.razor)
        ├── Pegawai/ (PegawaiList.razor)
        ├── SPSK/ (DokumenSPSK.razor)
        ├── SasaranMutu/ (SasaranMutu.razor)
        ├── Peraturan/ (PeraturanList.razor)
        ├── CIP/ (CipContoh.razor, CipGugus.razor)
        └── SM/ (SmProsesBisnis.razor, SmInternalAudit.razor, SmHiradc.razor, dll.)
```
    ├── Services/QmsApiService.cs
    ├── Layout/ (MainLayout.razor, NavMenu.razor)
    └── Pages/ (Documents, Pegawai, SPSK, SasaranMutu, Auth)
```
