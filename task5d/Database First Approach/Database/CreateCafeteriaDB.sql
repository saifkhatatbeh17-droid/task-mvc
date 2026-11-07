USE master;
IF DB_ID(N'CafeteriaDB') IS NULL CREATE DATABASE CafeteriaDB;
GO
USE CafeteriaDB;
GO
IF SCHEMA_ID(N'Task6') IS NULL EXEC(N'CREATE SCHEMA Task6');
GO
IF OBJECT_ID(N'Task6.Categories', N'U') IS NOT NULL
    THROW 50001, 'Task6 tables already exist. This script will not overwrite them.', 1;
GO
CREATE TABLE Task6.Categories (
    Id int IDENTITY PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    Description nvarchar(500) NULL
);
CREATE TABLE Task6.Suppliers (
    Id int IDENTITY PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    Email nvarchar(150) NOT NULL,
    Phone nvarchar(30) NOT NULL
);
CREATE TABLE Task6.Products (
    Id int IDENTITY PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    Price decimal(18,2) NOT NULL CHECK (Price > 0),
    Quantity int NOT NULL CHECK (Quantity >= 0),
    CategoryId int NOT NULL REFERENCES Task6.Categories(Id),
    SupplierId int NOT NULL REFERENCES Task6.Suppliers(Id)
);
CREATE TABLE Task6.PurchaseOrders (
    Id int IDENTITY PRIMARY KEY,
    OrderDate datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Status nvarchar(20) NOT NULL CHECK (Status IN ('Draft','Received')),
    SupplierId int NOT NULL REFERENCES Task6.Suppliers(Id)
);
CREATE TABLE Task6.PurchaseOrderItems (
    Id int IDENTITY PRIMARY KEY,
    PurchaseOrderId int NOT NULL REFERENCES Task6.PurchaseOrders(Id) ON DELETE CASCADE,
    ProductId int NOT NULL REFERENCES Task6.Products(Id),
    Quantity int NOT NULL CHECK (Quantity > 0),
    UnitPrice decimal(18,2) NOT NULL CHECK (UnitPrice > 0),
    CONSTRAINT UQ_Task6_OrderProduct UNIQUE (PurchaseOrderId,ProductId)
);
GO
