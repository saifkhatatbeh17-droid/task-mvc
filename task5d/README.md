# تاسك 6

الحل مقسم حسب الجزأين الموجودين في الملف:

- Database First Approach: قاعدة CafeteriaDB، والجداول مكتوبة في Database/CreateCafeteriaDB.sql ثم تم توليد Models وData/CafeteriaContext.cs بأمر Scaffold.
- Code First Approach: قاعدة CafeteriaEmployeesDB، والكلاسات مكتوبة داخل Models ثم أنشأنا Migration باسم InitialCreate وطبقناها باستخدام database update.

افتح CafeteriaTask6.slnx في Visual Studio. يحتوي الحل المشروعين، ويمكن اختيار Startup Project لكل جزء.

## ملاحظة القواعد الموجودة

كانت القاعدتان موجودتين أصلًا بجداول dbo. لذلك وضعنا الجداول المطلوبة داخل schema مستقل Task6، دون تعديل جداول dbo. اسم كل قاعدة كما هو مطلوب. سجل Migrations للجزء الثاني مستقل أيضًا داخل Task6.__Task6MigrationsHistory.

## الجداول والعلاقات

- Categories: Id, Name, Description.
- Suppliers: Id, Name, Email, Phone.
- Products: Id, Name, Price, Quantity, CategoryId, SupplierId.
- PurchaseOrders: Id, OrderDate, Status, SupplierId.
- PurchaseOrderItems: Id, PurchaseOrderId, ProductId, Quantity, UnitPrice.

العلاقات:

- Category واحدة لها Products متعددة.
- Supplier واحد له Products متعددة.
- Supplier واحد له PurchaseOrders متعددة.
- PurchaseOrder واحدة لها PurchaseOrderItems متعددة.
- Product واحد يمكن أن يظهر في PurchaseOrderItems متعددة عبر طلبات مختلفة.

كل Id مفتاح أساسي Identity. الأسعار decimal(18,2) وموجبة. المخزون لا يقبل قيمة سالبة، وكمية بند الطلب يجب أن تكون موجبة. الحالة Draft أو Received. يمنع تكرار المنتج داخل الطلب نفسه بمفتاح فريد مركب، ويمكن تمثيل التكرار بزيادة كمية البند. حذف الطلب يحذف بنوده، لكن حذف منتج أو مورد أو فئة مستخدمة في سجلات أخرى مرفوض.

## تشغيل Database First

1. شغّل SQL Server المحلي.
2. على جهاز جديد فقط: افتح Database/CreateCafeteriaDB.sql في SSMS ونفذه. السكربت ينشئ القاعدة إذا لم توجد، ولا يستبدل جداول Task6 موجودة.
3. عدل الاتصال في CafeteriaDatabaseFirst/appsettings.json إذا لزم.
4. افتح المشروع وشغله؛ الصفحة الرئيسية تقرأ عدد السجلات من الجداول الخمسة للتأكد من الربط.
5. أمر التوليد موجود داخل ملف الجزء README.md.

## تشغيل Code First

1. عدل الاتصال في CafeteriaCodeFirst/appsettings.json إذا لزم.
2. من Terminal داخل مجلد المشروع نفذ dotnet ef database update.
3. شغّل المشروع. الجداول لا تُنشأ يدويًا ولا نستخدم EnsureCreated؛ تُنشأ باستخدام Migration المرفقة.

## نطاق الحل

الخطوات التفصيلية داخل ملف التاسك تطلب إنشاء الجداول والمفاتيح والعلاقات بالطريقتين. نفذنا هذه المتطلبات وأضفنا صفحة بسيطة لفحص اتصال كل مشروع. لا توجد شاشات CRUD أو تسجيل دخول أو دورة استلام طلبات في هذه النسخة؛ الوصف العام يذكرها، لكنه لا يحدد خطوات تنفيذها ضمن الجزأين.

## شرح سريع للكوتش

في Database First بدأت بقاعدة البيانات وكتبت CREATE TABLE مع المفاتيح والقيود، ثم استخدمت Scaffold لتوليد الكلاسات من القاعدة. في Code First بدأت بالكلاسات وحددت العلاقات داخل OnModelCreating، ثم migrations add وdatabase update لتحويل الكلاسات إلى جداول. كل جزء مربوط بقاعدة مستقلة عبر UseSqlServer والاتصال الموجود في appsettings.json.
