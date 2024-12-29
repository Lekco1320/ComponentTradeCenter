-- 请先执行create_table.sql

START TRANSACTION;

DELIMITER $$

CREATE TRIGGER AFTER_DELETE_CUSTOMER
AFTER UPDATE ON Customers
FOR EACH ROW
BEGIN
    IF OLD.DeleteTime IS NULL OR OLD.DeleteTime <> NEW.DeleteTime THEN
        UPDATE Needs
        SET DeleteTime = NEW.DeleteTime
        WHERE CustomerId = NEW.Id;
    END IF;
END$$

CREATE TRIGGER AFTER_DELETE_SUPPLIER
AFTER UPDATE ON Suppliers
FOR EACH ROW
BEGIN
    IF OLD.DeleteTime IS NULL OR OLD.DeleteTime <> NEW.DeleteTime THEN
        UPDATE Supplies
        SET DeleteTime = NEW.DeleteTime
        WHERE SupplierId = NEW.Id;
    END IF;
END$$

DELIMITER ;

COMMIT;