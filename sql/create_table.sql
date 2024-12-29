-- 请先建立数据库ComponentTradeCenter
-- 数据库连接字符串存放在src/ComponentTradeCenter.Server/appsettings.json中

START TRANSACTION;

-- 人员表

CREATE TABLE Administrators (
    Id           INT          AUTO_INCREMENT PRIMARY KEY,
    Name         VARCHAR(64)  NOT NULL,
    Password     VARCHAR(16)  NOT NULL,
    Phone        VARCHAR(15),
    RegisterTime DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DeleteTime   DATETIME
);

CREATE TABLE Customers (
    Id           INT          AUTO_INCREMENT PRIMARY KEY,
    Name         VARCHAR(64)  NOT NULL,
    Password     VARCHAR(16)  NOT NULL,
    Phone        VARCHAR(15),
    Address      VARCHAR(128),
    RegisterTime DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DeleteTime   DATETIME
);

CREATE TABLE Suppliers (
    Id           INT          AUTO_INCREMENT PRIMARY KEY,
    Name         VARCHAR(64)  NOT NULL,
    Password     VARCHAR(16)  NOT NULL,
    Phone        VARCHAR(15),
    Address      VARCHAR(128),
    Brief        VARCHAR(255),
    RegisterTime DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DeleteTime   DATETIME
);

CREATE TABLE Traders (
    Id           INT          AUTO_INCREMENT PRIMARY KEY,
    Name         VARCHAR(64)  NOT NULL,
    Password     VARCHAR(16)  NOT NULL,
    Phone        VARCHAR(15),
    Brief        VARCHAR(255),
    RegisterTime DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DeleteTime   DATETIME
);

-- 业务表

CREATE TABLE Components (
    Id         INT          AUTO_INCREMENT PRIMARY KEY,
    Name       VARCHAR(64)  NOT NULL,
    Color      VARCHAR(16)  NOT NULL,
    Weight     FLOAT        NOT NULL,
    Brief      VARCHAR(255),
    DeleteTime DATETIME
);

CREATE TABLE Needs (
    Id          INT            AUTO_INCREMENT PRIMARY KEY,
    Name        VARCHAR(64)    NOT NULL,
    Price       DECIMAL(10, 2) NOT NULL CHECK (Price  > 0),
    Amount      INT            NOT NULL CHECK (Amount > 0),
    CustomerId  INT            NOT NULL,
    ComponentId INT            NOT NULL,
    DeleteTime  DATETIME,
    FOREIGN KEY (CustomerId)  REFERENCES Customers(Id),
    FOREIGN KEY (ComponentId) REFERENCES Components(Id)
);

CREATE TABLE Supplies (
    Id          INT            AUTO_INCREMENT PRIMARY KEY,
    Name        VARCHAR(64)    NOT NULL,
    Price       DECIMAL(10, 2) NOT NULL CHECK (Price  > 0),
    Amount      INT            NOT NULL CHECK (Amount > 0),
    SupplierId  INT            NOT NULL,
    ComponentId INT            NOT NULL,
    DeleteTime  DATETIME,
    FOREIGN KEY (SupplierId)  REFERENCES Suppliers(Id),
    FOREIGN KEY (ComponentId) REFERENCES Components(Id)
);

CREATE TABLE TradeSuggests (
    Id                INT            AUTO_INCREMENT PRIMARY KEY,
    Name              VARCHAR(64)    NOT NULL,
    Price             DECIMAL(10, 2) NOT NULL CHECK (Price  > 0),
    Amount            INT            NOT NULL CHECK (Amount > 0),
    CustomerIntention BOOLEAN,
    SupplierIntention BOOLEAN,
    TraderIntention   BOOLEAN,
    NeedId            INT            NOT NULL,
    SupplyId          INT            NOT NULL,
    TraderId          INT            NOT NULL,
    DeleteTime        DATETIME,
    FOREIGN KEY (NeedId)   REFERENCES Needs(Id),
    FOREIGN KEY (SupplyId) REFERENCES Supplies(Id),
    FOREIGN KEY (TraderId) REFERENCES Traders(Id)
);

CREATE TABLE Agreements (
    Id                INT         AUTO_INCREMENT PRIMARY KEY,
    Name              VARCHAR(64) NOT NULL,
    SuggestId         INT         NOT NULL,
    CustomerSignature BLOB,
    SupplierSignature BLOB,
    TraderIntention   BOOLEAN,
    DeleteTime        DATETIME,
    FOREIGN KEY (SuggestId) REFERENCES TradeSuggests(Id)
);

CREATE TABLE Trades (
    Id           INT         AUTO_INCREMENT PRIMARY KEY,
    Name         VARCHAR(64) NOT NULL,
    AgreementId  INT         NOT NULL,
    CompleteTime DATETIME    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DeleteTime   DATETIME,
    FOREIGN KEY (AgreementId) REFERENCES Agreements(Id)
);

COMMIT;