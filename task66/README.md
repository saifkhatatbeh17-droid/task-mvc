# task66 - Sunday Cafeteria MVC Task

تكملة على نسخة مستقلة من مشروع task5d/Code First Approach. الكود بسيط ويستخدم نفس قاعدة CafeteriaEmployeesDB ونفس جداول Task6. أي سجلات تضيفها هنا تظهر أيضًا في المشروع السابق لأنه يستخدم القاعدة نفسها.

## التشغيل

افتح Task66.slnx في Visual Studio، ثم شغل المشروع باستخدام Ctrl + F5. إذا تغير اسم SQL Server عدّل ConnectionStrings:Cafeteria داخل appsettings.json.

على جهاز جديد نفذ من مجلد CafeteriaCodeFirst:

```powershell
dotnet ef database update
```

الجداول موجودة محليًا مسبقًا، وفحصنا أن نماذج المشروع لا تختلف عن آخر Migration. اسم DbContext أصبح ApplicationDbContext وتم تحديث التسجيل والمراجع وملفات Migration داخل هذه النسخة.

## الملفات المضافة

- Controllers/CategoryController.cs: Index لقراءة الفئات، Create GET للنموذج وCreate POST للتحقق والحفظ.
- Controllers/SupplierController.cs: عرض وإضافة الموردين مع التحقق من الاسم والهاتف والإيميل.
- Controllers/ProductController.cs: عرض المنتجات مع Include للفئة والمورد، وإضافة منتج عبر قائمتين منسدلتين.
- Views/Category/Index.cshtml وCreate.cshtml.
- Views/Supplier/Index.cshtml وCreate.cshtml.
- Views/Product/Index.cshtml وCreate.cshtml.

## الملفات المعدلة

- Data/ApplicationDbContext.cs: نفس سياق البيانات السابق بعد تغيير الاسم حسب المطلوب.
- Program.cs: تسجيل ApplicationDbContext وفتح الموقع على Category/Index.
- Views/Shared/_Layout.cshtml: روابط الفئات والموردين والمنتجات باستخدام Html.ActionLink.
- HomeController والمراجع في Migrations: تحديث اسم السياق فقط.

## HTML Helpers

استخدمنا BeginForm وAntiForgeryToken وLabelFor وTextBoxFor وTextAreaFor وDropDownListFor وValidationMessageFor وValidationSummary وDisplayFor وActionLink.

## تجربة الحل

1. أضف فئة.
2. أضف موردًا بإيميل صالح.
3. أضف منتجًا وحدد الفئة والمورد من القوائم.
4. تأكد من ظهور المنتج واسم الفئة والمورد في الجدول.
5. السعر يجب أن يكون موجبًا والمخزون غير سالب.

نجح البناء دون أخطاء أو تحذيرات. اختبرنا الصفحات الست، والتحقق من المدخلات، والحفظ في قاعدة البيانات، وإعادة عرض السجلات، وبقاء القوائم عند فشل التحقق. تم تنظيف سجلات الاختبار فقط بعد الانتهاء.
