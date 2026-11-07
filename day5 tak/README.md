# day5 tak - Graduation Supply Management

## التشغيل

1. افتح AcademyGraduation.slnx في Visual Studio.
2. تأكد أن خدمة SQL Server المحلية تعمل، وعدل ConnectionStrings:Graduation في appsettings.json إذا كان اسم السيرفر مختلفًا.
3. اسم قاعدة البيانات المطلوبة AcademyGraduationPreparationDB. ينشئ التطبيق القاعدة والجداول والبيانات عند أول تشغيل حسب صلاحيات اتصال Windows.
4. اضغط Ctrl + F5 ثم اختر المرحلة من الصفحة الرئيسية واضغط Open Phase.
5. اختر عرضًا واحدًا يدويًا من كل فئة، ثم اضغط Review Request.

## المراحل

- Phase 1: مفاتيح Session منفصلة هي CertificateOffer وHospitalityOffer وGiftOffer.
- Phase 2: List<OfferChoice> واحدة محفوظة بصيغة JSON في Session بمفتاح OffersPhase2.
- Phase 3: بيانات الفئات والموردين والعروض تأتي من SQL Server، والاختيارات تحفظ في قائمة Session بمفتاح OffersPhase3.
- لكل مرحلة اختيارات مستقلة. اختيار عرض آخر للفئة نفسها يستبدل العرض السابق.
- Cookie باسم GraduationPhase تحفظ المرحلة المفضلة 7 أيام؛ لا تختار عروضًا نيابة عن الموظف.

## الملفات المهمة

- Controllers/GraduationController.cs: اختيار العروض، Session، التحقق، وحساب الإجمالي والخصم داخل Review.
- Models/GraduationData.cs: Category وSupplier وOffer.
- Models/OfferChoice.cs: رقم الفئة ورقم العرض المختار فقط؛ السعر يعاد قراءته من المصدر ولا يستقبل من النموذج.
- Data/OfferData.cs: بيانات الفئات الثلاث والعروض التسعة للمراحل الأولى وتهيئة قاعدة البيانات.
- Data/GraduationContext.cs: الربط بقاعدة البيانات وتعريف العلاقات.
- Program.cs: تسجيل Session وDbContext، إعداد المسار، وإنشاء الجداول والبيانات عند الحاجة.
- Views/Graduation/Index.cshtml: عرض الفئات والعروض وأزرار الاختيار اليدوي.
- Views/Graduation/Review.cshtml: عرض النتائج المحسوبة في Controller دون حسابات أسعار داخل الصفحة.
- Views/Graduation/About.cshtml: وصف التطبيق ورابط العودة باستخدام Url.Action.
- Views/Shared/_MainLayout.cshtml: روابط التنقل باستخدام Url.Action.

## العلاقات

Category (1) -> (many) Supplier
Supplier (1) -> (1) Offer

الجداول داخل schema باسم Day5Task في AcademyGraduationPreparationDB، لأن قاعدة بهذا الاسم كانت موجودة مسبقًا بمخطط مختلف. هذا يعزل جداول الحل ويحافظ على الجداول السابقة. العلاقة بين Supplier وOffer تستخدم مفتاحًا أجنبيًا فريدًا؛ بيانات التهيئة تضيف عرضًا واحدًا لكل مورد.

## الحسابات

- مجموع العرض = الكمية * سعر الوحدة.
- المجموع أكبر من 500: خصم 15%.
- وإلا إذا كان أكبر من 300: خصم 10%.
- وإلا: دون خصم.
- الخصم يقرب إلى منزلتين، والنهائي = المجموع - الخصم.
- أقل مجموع ممكن بالعروض الحالية هو 322.50، لذلك فرع دون خصم موجود وفق المطلوب لكن لا يمكن الوصول إليه باختيار صحيح من الأسعار المعطاة حاليًا.

## فحص الحل

نجح البناء دون أخطاء أو تحذيرات. اختبرت المراحل الثلاث، ومنع المراجعة الناقصة، ورفض عرض تابع لفئة أخرى، واستبدال العرض دون تكرار، وبقاء الاختيارات عند التنقل، وعزل جلسة جديدة.

مثال: Standard Certificates + Coffee & Drinks + Customized Mugs يعطي 322.50 JD، خصم 10%، ونهائي 290.25 JD.

هذا مشروع تدريبي يستخدم Session دون تسجيل دخول. لا توجد خطوة تأكيد أو حفظ طلب نهائي لأن الملف المطلوب يقتصر على الاختيار والمراجعة.
