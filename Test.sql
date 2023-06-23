create database Test
go
use Test
go
select * from TABLEFOOD
create table ACCOUNT
(
	USERNAME NVARCHAR(100) PRIMARY KEY,
	DISPLAYNAME NVARCHAR(100) NOT NULL,
	PASSWORD NVARCHAR(100) NOT NULL,
	TYPE TINYINT NOT NULL
)
GO
CREATE TABLE TABLEFOOD
(
	ID INT IDENTITY PRIMARY KEY,
	NAME NVARCHAR(100),
	STATUS TINYINT NOT NULL,--0: TRỐNG,1: ĐÃ CÓ KHÁCH--
)
GO
CREATE TABLE FOODCATEGORY --LOẠI THỨC ĂN--
(
	ID INT IDENTITY PRIMARY KEY,
	NAME NVARCHAR(100)
)
GO
CREATE TABLE FOOD
(
	ID INT IDENTITY PRIMARY KEY,
	NAME NVARCHAR(100) NOT NULL,
	IDCATEGORY INT NOT NULL,
	PRICE MONEY,
	FOREIGN KEY (IDCATEGORY) REFERENCES dbo.FOODCATEGORY(ID)
)
GO
CREATE TABLE BILL
(
	ID INT IDENTITY PRIMARY KEY,
	DATECHECKIN DATE,
	DATECHECKOUT DATE,
	IDTABLE INT NOT NULL,
	STATUS INT NOT NULL, --1:ĐÃ THANH TOÁN, 0:CHƯA THANH TOÁN
	DISCOUNT INT NOT NULL DEFAULT 0,
	TOTALPRICE FLOAT 
	FOREIGN KEY (IDTABLE) REFERENCES dbo.TABLEFOOD(ID)
)
GO
CREATE TABLE BILLINFO
(
	ID INT IDENTITY PRIMARY KEY NOT NULL,
	IDBILL INT NOT NULL,
	IDFOOD INT NOT NULL,
	COUNT INT NOT NULL DEFAULT 0
	FOREIGN KEY (IDBILL) REFERENCES DBO.BILL(ID),
	FOREIGN KEY (IDFOOD) REFERENCES DBO.FOOD(ID)
)
GO

insert into dbo.ACCOUNT
			(USERNAME,
			DISPLAYNAME,
			PASSWORD,
			TYPE)
values (N'N2',N'Nhat',N'1',1)

insert into dbo.ACCOUNT
			(USERNAME,
			DISPLAYNAME,
			PASSWORD,
			TYPE)
values (N'staff',N'staff',N'1',0)
go

create proc USP_GetListAccountByUserName
@username nvarchar(100)
As
begin
	select * from dbo.ACCOUNT where UserName = @username
end
go

exec dbo.USP_GetListAccountByUserName @username = N'N2' 

 --Add bàn vào form chính
declare @i int =0
while @i<=15
begin
	insert TABLEFOOD(name,STATUS) values (N'Bàn '+ cast(@i as nvarchar(100)),0)
	set @i=@i+1
end
select*from TABLEFOOD

create proc USP_GetTableList
as select * from dbo.TABLEFOOD
go
update  dbo.TABLEFOOD set status =1 where ID= 2
exec dbo.USP_GetTableList
go

---Tạo bảo mật đăng nhập:
create proc USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
as
begin
	select * from dbo.ACCOUNT where USERNAME=@userName and PASSWORD=@passWord
end 
go
--Thêm category--
insert dbo.FOODCATEGORY (NAME) values('Coffee')
insert dbo.FOODCATEGORY (NAME) values('Nước Ngọt')
insert dbo.FOODCATEGORY (NAME) values('Thức ăn nhanh')
insert dbo.FOODCATEGORY (NAME) values('Detox')
insert dbo.FOODCATEGORY (NAME) values('Trà')
--Thêm food--
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Expresso',1,35000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Americano',1,40000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Cold Brew',1,35000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Bạc xỉu',1,35000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Cà phê đen',1,35000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Sting',2,12000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('7Up',2,12000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Number One',2,12000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Coca Cola',2,12000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('PepSi',2,12000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Bánh Mì',3,40000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Mì xào',3,25000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Bò Né',3,40000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Tropical Detox',4,40000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Trà đào',5,35000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Trà vải',5,35000)
insert dbo.FOOD(NAME,IDCATEGORY,PRICE)values('Hồng trà dâu',5,35000)
--Thêm Bill--
insert dbo.BILL(DATECHECKIN,DATECHECKOUT,IDTABLE,STATUS) values(GETDATE(),GETDATE(),9,0)
insert dbo.BILL(DATECHECKIN,DATECHECKOUT,IDTABLE,STATUS) values(GETDATE(),GETDATE(),8,1)
insert dbo.BILL(DATECHECKIN,DATECHECKOUT,IDTABLE,STATUS) values(GETDATE(),GETDATE(),4,1)
--Thêm BillInfo--
insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(1,3,2)
insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(1,6,2)
insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(1,11,2)
insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(2,2,1)
insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(2,12,1)
insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(2,8,1)
insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(2,9,1)
insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(3,2,1)
select fod.NAME,bilinf.COUNT,fod.PRICE,fod.PRICE*bilinf.COUNT as TotalPrice from dbo.BILLINFO as bilinf, BILL as bil,dbo.FOOD as fod
where bilinf.IDBILL=bil.ID and bilinf.IDFOOD=fod.ID and bil.STATUS=0 and bil.IDTABLE=9
select*from TABLEFOOD
select*from BILL where IDTABLE=9
select fod.NAME,bilinf.COUNT,fod.PRICE,fod.PRICE*bilinf.COUNT as TotalPrice from dbo.BILLINFO as bilinf, BILL as bil,dbo.FOOD as fod 
where bilinf.IDBILL = bil.ID and bilinf.IDFOOD = fod.ID and bil.STATUS=0 and bil.IDTABLE = 9
CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN
	INSERT dbo. Bill
		(DateCheckIn,
		DateCheckOut,
		idTable,
		status,
		discount
		)
	VALUES ( GETDATE(),
			NULL,
			@idTable,
			0,
			0)
END
GO

CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood int, @count int
as
begin
	insert dbo.BILLINFO(IDBILL,IDFOOD,COUNT) values(@idBill,@idFood,@count)
end
go

create PROC USP_InsertBillInfo 
@idBill INT, @idFood INT, @count INT
AS
BEGIN
	DECLARE @IsExitsBillInfo INT
	DECLARE @foodCount INT = 1

	SELECT @IsExitsBillInfo = id , @foodCount = b.count
	FROM dbo.BillInfo AS b
	WHERE idBill = @idBill AND idFood = @idFood 

	IF (@IsExitsBillInfo > 0)
	BEGIN
			DECLARE @newCount INT = @foodCount + @count
			IF (@newCount > 0)
				UPDATE dbo.BillInfo SET count = @foodCount + @count where IDFOOD=@idFood
			ELSE
				DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE
	BEGIN
		INSERT dbo.BillInfo
				(idBill, idFood, count)
		VALUES (@idBill, -- idBill -- int
				@idFood, -- idFood - int
				@count -- count - int
				)
	END
END
GO

update dbo.BILL set STATUS=1 where ID=1
create trigger UTG_UpdateBillInfor 
on dbo.BILLINFO FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill=idBill from inserted 

	DECLARE @idTable INT
	SELECT @idTable=idTable FROM dbo.BILL WHERE id=@idBill and status=0

	DECLARE @count INT
	SELECT @count = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idBill

	IF (@count > 0)
	BEGIN
		UPDATE dbo.TableFood SET status = 1 WHERE id = @idTable
	END	
	
	ELSE
	BEGIN
		UPDATE dbo.TableFood SET status = 0 WHERE id = @idTable
	END
END
GO

CREATE TRIGGER UTG_UpdateBill
on dbo.BILL for UPDATE
as
begin
	DECLARE @idBill int
	SELECT @idBill =id FROM inserted 
	
	DECLARE @idTable INT
	SELECT @idTable=idTable FROM dbo.BILL WHERE id=@idBill 

	DECLARE @count int=0
	SELECT @count=COUNT(*) FROM dbo.BILL WHERE idTable=@idTable and status=0

	IF(@count=0)
		UPDATE dbo.TABLEFOOD SET STATUS=0

end
go
ALTER TRIGGER UTG_UpdateBill
on dbo.BILL for UPDATE
as
begin
	DECLARE @idBill int
	SELECT @idBill =id FROM inserted 
	
	DECLARE @idTable INT
	SELECT @idTable=idTable FROM dbo.BILL WHERE id=@idBill 

	DECLARE @count int=0
	SELECT @count=COUNT(*) FROM dbo.BILL WHERE idTable=@idTable and status=0

	IF(@count=0)
		UPDATE dbo.TABLEFOOD SET STATUS=0 WHERE ID=@idTable
end
go


------Chuyển bàn------------------------------
create proc USP_SwitchTable
@idTable1 int , @idTable2 int
as begin 
		declare @idFirstBill int
		declare @idSecondBill int

		DECLARE @isFirstTablEmty INT = 1
		DECLARE @isSecondTablEmty INT = 1

		select @idSecondBill = id from dbo.BILL where idTable=@idTable2 and status = 0 
		select @idFirstBill = id from dbo.BILL where idTable=@idTable1 and status = 0

		if (@idFirstBill is null)
		begin			
				insert dbo.BILL
							(DATECHECKIN,
							 DATECHECKOUT,
							 IDTABLE,
							 STATUS
							 )
				values ( GETDATE(),
						NULL,
						@idTable1,
						0)
				select @idFirstBill = max(id) from dbo.BILL where idTable=@idTable1 and status = 0
		end

		SELECT @isFirstTablEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idFirstBill

		if (@idSecondBill is null)
		begin			
				insert dbo.BILL
							(DATECHECKIN,
							 DATECHECKOUT,
							 IDTABLE,
							 STATUS
							 )
				values ( GETDATE(),
						NULL,
						@idTable2,
						0)
				select @idSecondBill = max(id) from dbo.BILL where idTable=@idTable2 and status = 0
		end

		SELECT @isSecondTablEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idSecondBill

		select id into IDBillInfoTable from dbo.BILLINFO where idBill = @idSecondBill

		update dbo.BILLINFO set idBill = @idSecondBill where idBill = @idFirstBill

		update dbo.BILLINFO set idBill = @idFirstBill where id in (select * from IDBillInfoTable)	

		IF (@isFirstTablEmty = 0)
			UPDATE dbo.TableFood SET status = 0 WHERE id = @idTable2
		
		IF (@isSecondTablEmty= 0)
			UPDATE dbo.TableFood SET status = 0 WHERE id = @idTable1

		drop table IDBillInfoTable
end
go
------------------------------------------------------
----------Thay đổi thông tin tài khoản----------------
CREATE PROC USP_UpdateAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0
	
	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE USERName = @userName AND PassWord = @password
	
	IF (@isRightPass = 1)
	BEGIN
		IF (@newPassword = NULL OR @newPassword = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END		
		ELSE
			UPDATE dbo.Account SET DisplayName = @displayName, PassWord = @newPassword WHERE UserName = @userName
	end
END
GO
-----------------------------------------------------

------Sửa kiểu dữ liệu của datecheckin, datecheckout
alter table BILL alter column DATECHECKIN DATETIME
alter table BILL alter column DATECHECKOUT DATETIME
--------------------------------------------------

-------------------------------------------

--------Hiển thị danh sách hóa đơn-------------
CREATE PROC USP_GetListBillByDate
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT t.name AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá]
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable
END
GO
-----------------------------------------------
CREATE TRIGGER UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
AS 
BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = id, @idBill = Deleted.idBill FROM Deleted
	
	DECLARE @idTable INT
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count INT = 0
	
	SELECT @count = COUNT(*) FROM dbo.BillInfo AS bi, dbo.Bill AS b WHERE b.id = bi.idBill AND b.id = @idBill AND b.status = 0
	
	IF (@count = 0)
		UPDATE dbo.TableFood SET status = 0 WHERE id = @idTable
END
GO
------------------------------------------------------------------

----------Hàm chuyển kí tự có dấu thành không dấu-----------------
CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
GO
------------------------------------------------------------------
--Thêm, xóa, sửa tài khoản--
alter table account add constraint set_default_accountPas default ('1') for password
-----------------------------------------------------------------
--Phân trang cho hóa đơn--
alter PROC USP_GetListBillByDateAndPage
@checkIn date, @checkOut date, @page int
AS 
BEGIN
	DECLARE @pageRows INT = 12
	DECLARE @selectRows INT = @pageRows
	DECLARE @exceptRows INT = (@page - 1) * @pageRows
	
	;WITH BillShow AS( SELECT b.ID AS[ID], t.name AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá]
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable)
	
	SELECT TOP (@selectRows) * FROM BillShow WHERE id NOT IN (SELECT TOP (@exceptRows) id FROM BillShow)
END
GO

CREATE PROC USP_GetNumBillByDate
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT COUNT(*)
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable
END
GO
---Thêm xóa sửa danh mục--
select*from foodcategory
insert FOODCATEGORY (NAME) VALUES(N'Thức ăn chậm')
update FOODCATEGORY set NAME=N'ĂN CHẬM' WHERE ID=6
delete FOODCATEGORY where id=6
--Thêm xóa, sửa bàn--
alter table TABLEFOOD add constraint set_default_TABSTATUS default ('0') for status
select*from TABLEFOOD
insert TABLEFOOD (NAME) VALUES(N'Bàn 16')
update TABLEFOOD set NAME= N'BÀN 16,1', STATUS=1 WHERE ID=20
exec USP_GetTableList
select*from TABLEFOOD
SELECT t.name AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá]
	FROM dbo.Bill AS b,dbo.TableFood AS t

SELECT a.ID as [Mã danh mục],a.NAME as[Tên danh mục] from FOODCATEGORY as a
select*from TABLEFOOD
select*from bill

------------------------------------------
