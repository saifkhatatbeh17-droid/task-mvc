SET XACT_ABORT OFF;
BEGIN TRANSACTION;
BEGIN TRY
    INSERT Task6.Categories(Name,Description) VALUES(N'Task6 verification',N'Temporary transaction');
    DECLARE @category int = SCOPE_IDENTITY();
    INSERT Task6.Suppliers(Name,Email,Phone) VALUES(N'Test supplier',N'test@example.com',N'000');
    DECLARE @supplier int = SCOPE_IDENTITY();
    INSERT Task6.Products(Name,Price,Quantity,CategoryId,SupplierId) VALUES(N'Test coffee',2.50,5,@category,@supplier);
    DECLARE @product int = SCOPE_IDENTITY();
    INSERT Task6.PurchaseOrders(OrderDate,Status,SupplierId) VALUES(SYSUTCDATETIME(),N'Draft',@supplier);
    DECLARE @order int = SCOPE_IDENTITY();
    INSERT Task6.PurchaseOrderItems(PurchaseOrderId,ProductId,Quantity,UnitPrice) VALUES(@order,@product,3,2.50);
    IF (SELECT SUM(Quantity * UnitPrice) FROM Task6.PurchaseOrderItems WHERE PurchaseOrderId=@order) <> 7.50
        THROW 50001, 'Decimal calculation failed',1;
    BEGIN TRY
        INSERT Task6.Products(Name,Price,Quantity,CategoryId,SupplierId) VALUES(N'Invalid',-1,0,@category,@supplier);
        THROW 50002, 'Negative price was accepted',1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() <> 547 THROW;
    END CATCH;
    BEGIN TRY
        INSERT Task6.PurchaseOrderItems(PurchaseOrderId,ProductId,Quantity,UnitPrice) VALUES(@order,@product,1,2.50);
        THROW 50003, 'Duplicate order product was accepted',1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() NOT IN (2601,2627) THROW;
    END CATCH;
    BEGIN TRY
        INSERT Task6.Products(Name,Price,Quantity,CategoryId,SupplierId) VALUES(N'Invalid FK',1,0,-1,@supplier);
        THROW 50004, 'Invalid foreign key was accepted',1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() <> 547 THROW;
    END CATCH;
    DELETE Task6.PurchaseOrders WHERE Id=@order;
    IF EXISTS(SELECT 1 FROM Task6.PurchaseOrderItems WHERE PurchaseOrderId=@order)
        THROW 50005,'Order items were not cascaded',1;
    ROLLBACK TRANSACTION;
    PRINT 'PASS: relationships, decimal values, price constraint, duplicate prevention, foreign key and cascade delete. Test data rolled back.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
