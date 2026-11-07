# Code First Approach

المشروع: CafeteriaCodeFirst
القاعدة: CafeteriaEmployeesDB
Schema: Task6

كتبنا Category وSupplier وProduct وPurchaseOrder وPurchaseOrderItem داخل Models. عرفنا العلاقات والقيود داخل Data/CafeteriaContext.cs، ثم نفذنا من هذا المجلد:

```powershell
dotnet ef migrations add InitialCreate --project CafeteriaCodeFirst
dotnet ef database update --project CafeteriaCodeFirst
```

Migration المرفقة داخل CafeteriaCodeFirst/Migrations هي التي أنشأت الجداول. لا تعِد إنشاء InitialCreate عند تنزيل الحل؛ يكفي database update لتطبيق الملفات الموجودة. سجل هذه النسخة هو Task6.__Task6MigrationsHistory، حتى لا يتعارض مع سجل قديم موجود في dbo.
