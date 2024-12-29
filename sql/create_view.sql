-- 请先执行create_table.sql

START TRANSACTION;

-- 用户视图

CREATE VIEW V_UserInfos AS
SELECT
    ROW_NUMBER() OVER (ORDER BY UserType, UserId) AS Id,
    UserId,
    Name,
    Phone,
    RegisterTime,
    DeleteTime,
    UserType
FROM (
    SELECT
        Id AS UserId,
        Name,
        Phone,
        RegisterTime,
        DeleteTime,
        0 AS UserType
    FROM
        Customers
    UNION ALL
    SELECT
        Id AS UserId,
        Name,
        Phone,
        RegisterTime,
        DeleteTime,
        1 AS UserType
    FROM
        Suppliers
    UNION ALL
    SELECT
        Id AS UserId,
        Name,
        Phone,
        RegisterTime,
        DeleteTime,
        2 AS UserType
    FROM
        Traders
) AS Combined;

CREATE VIEW V_Needs AS
SELECT
    n.Id AS Id,
    c.Id As ComponentId,
    u.Id As CustomerId,
    n.Name AS Name,
    u.Name AS CustomerName,
    c.Name As ComponentName,
    n.Price AS Price,
    n.Amount AS Amount
FROM
    Needs AS n
INNER JOIN
    Components AS c ON n.ComponentId = c.Id
INNER JOIN
    Customers AS u ON n.CustomerId = u.Id
WHERE
    n.DeleteTime IS NULL AND
    c.DeleteTime IS NULL
;

CREATE VIEW V_Supplies AS
SELECT
    s.Id AS Id,
    c.Id As ComponentId,
    s.SupplierId As SupplierId,
    s.Name AS Name,
    u.Name AS SupplierName,
    c.Name As ComponentName,
    s.Price AS Price,
    s.Amount AS Amount
FROM
    Supplies AS s
INNER JOIN
    Components AS c ON s.ComponentId = c.Id
INNER JOIN
    Suppliers AS u ON s.SupplierId = u.Id
WHERE
    s.DeleteTime IS NULL AND
    u.DeleteTime IS NULL
;

-- 供需信息视图

CREATE VIEW V_TradableInfos AS
SELECT
    ROW_NUMBER() OVER (ORDER BY n.Id) AS Id,
    n.ComponentId,
    n.Id AS NeedId,
    n.Name AS NeedName,
    n.Price AS NeedPrice,
    n.Amount AS NeedAmount,
    c.Name AS ComponentName,
    s.Id AS SupplyId,
    s.Name AS SupplyName,
    s.Price AS SupplyPrice,
    s.Amount AS SupplyAmount
FROM
    Needs AS n
INNER JOIN
    Supplies AS s ON n.ComponentId = s.ComponentId
INNER JOIN
    Components AS c ON s.ComponentId = c.Id
WHERE
    n.DeleteTime IS NULL AND
    s.DeleteTime IS NULL AND
    c.DeleteTime IS NULL
;

CREATE VIEW V_TradeSuggests AS
SELECT
    s.Id As Id,
    s.Name AS Name,
    s.Price AS Price,
    s.Amount AS Amount,
    s.CustomerIntention AS CustomerIntention,
    s.SupplierIntention AS SupplierIntention,
    s.TraderIntention AS TraderIntention,
    s.TraderId AS TraderId,
    s.NeedId AS NeedId,
    s.SupplyId AS SupplyId,
    t.Name AS TraderName,
    n.Name AS NeedName,
    n.CustomerId AS CustomerId,
    u.Name AS SupplyName,
    u.SupplierId AS SupplierId,
    m.Name AS ComponentName
FROM
    TradeSuggests AS s
INNER JOIN
    Needs AS n ON s.NeedId = n.Id
INNER JOIN
    Supplies AS u ON s.SupplyId = u.Id
INNER JOIN
    Traders AS t ON s.TraderId = t.Id
INNER JOIN
    Components AS m ON n.ComponentId = m.Id
WHERE
    s.DeleteTime IS NULL
;

CREATE VIEW V_Agreements AS
SELECT
    a.Id AS Id,
    a.Name AS Name,
    a.CustomerSignature AS CustomerSignature,
    a.SupplierSignature AS SupplierSignature,
    a.TraderIntention AS TraderIntention,
    a.SuggestId AS SuggestId,
    s.Price AS Price,
    s.Amount AS Amount,
    c.Name AS ComponentName,
    n.CustomerId AS CustomerId,
    n.Name AS NeedName,
    p.SupplierId AS SupplierId,
    p.Name AS SupplyName,
    t.Id AS TraderId,
    t.Name AS TraderName
FROM
    Agreements AS a
INNER JOIN
    TradeSuggests AS s ON a.SuggestId = s.Id
INNER JOIN
    Needs AS n ON s.NeedId = n.Id
INNER JOIN
    Components AS c ON n.ComponentId = c.Id
INNER JOIN
    Supplies AS p ON s.SupplyId = p.Id
INNER JOIN
    Traders AS t ON s.TraderId = t.Id
WHERE
    a.DeleteTime IS NULL
;

CREATE VIEW V_Trades AS
SELECT
    t.Id AS Id,
    t.Name AS Name,
    t.AgreementId AS AgreementId,
    t.CompleteTime AS CompleteTime,
    s.Price AS Price,
    s.Amount AS Amount,
    c.Name AS ComponentName,
    n.Name AS NeedName,
    n.CustomerId AS CustomerId,
    p.Name AS SupplyName,
    p.SupplierId AS SupplierId,
    r.Id AS TraderId,
    r.Name AS TraderName
FROM
    Trades AS t
INNER JOIN
    Agreements AS a ON t.AgreementId = a.Id
INNER JOIN
    TradeSuggests AS s ON a.SuggestId = s.Id
INNER JOIN
    Needs AS n ON s.NeedId = n.Id
INNER JOIN
    Components AS c ON n.ComponentId = c.Id
INNER JOIN
    Supplies AS p ON s.SupplyId = p.Id
INNER JOIN
    Traders AS r ON s.TraderId = r.Id
WHERE
    t.DeleteTime IS NULL
;

COMMIT;