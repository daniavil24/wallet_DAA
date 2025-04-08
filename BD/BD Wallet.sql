------------------------------------------------------------
CREATE DATABASE WalletDb;
GO
------------------------------------------------------------
USE WalletDb;
GO
------------------------------------------------------------
CREATE TABLE Wallets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DocumentId NVARCHAR(255) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

------------------------------------------------------------
CREATE TABLE Movements (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    WalletId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (WalletId) REFERENCES Wallets(Id)
);

------------------------------------------------------------
CREATE INDEX IX_Movements_WalletId ON Movements(WalletId);
