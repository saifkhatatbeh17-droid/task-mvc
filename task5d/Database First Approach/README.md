# Database First Approach

المشروع: CafeteriaDatabaseFirst
القاعدة: CafeteriaDB
Schema: Task6

الجداول أنشئت أولًا بواسطة Database/CreateCafeteriaDB.sql. بعدها تم تنفيذ:

```powershell
dotnet ef dbcontext scaffold "Server=localhost;Database=CafeteriaDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project CafeteriaDatabaseFirst --schema Task6 --context CafeteriaContext --context-dir Data --output-dir Models --no-onconfiguring --no-pluralize
```

ملفات Models وData/CafeteriaContext.cs مولدة من قاعدة البيانات. لم نستخدم Migrations لإنشاء جداول هذا الجزء. قيود CHECK موجودة فعليًا في SQL حتى لو لم تظهر في ملف Scaffold المولد.
