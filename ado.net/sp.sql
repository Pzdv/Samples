USE Northwind
GO

if OBJECT_ID('GetOrderDetails') is not null drop procedure GetOrderDetails
go

create procedure GetOrderDetails
	@OrderID int
as
	select od.OrderID, od.ProductID, od.UnitPrice, od.Quantity, od.Discount, p.ProductName from [Order Details] as od
	join Products as p on p.ProductID = od.ProductID
	where od.OrderID = @OrderID
go

if OBJECT_ID('GetOrders') is not null drop procedure GetOrders
go

create procedure GetOrders
as
	select * from Orders
go

if OBJECT_ID('CreateNewOrderDetails') is not null drop procedure CreateNewOrderDetails
go

create procedure CreateNewOrderDetails
	@OrderID int,
	@ProductID int,
	@UnitPrice money,
	@Quantity smallint,
	@Discount real
as
insert into [Order Details]
values(@OrderID, @ProductID, @UnitPrice, @Quantity, @Discount)
go

if OBJECT_ID('CreateNewOrder') is not null drop procedure CreateNewOrder
go

create procedure CreateNewOrder
	@CustomerID nchar(5) = null,
	@EmployeeID int = null,
	@RequiredDate datetime = null,
	@ShipVia int = null,
	@Freight money = null,
	@ShipName nvarchar(40) = null,
	@ShipAddress nvarchar(40) = null,
	@ShipCity nvarchar(15) = null,
	@ShipRegion nvarchar(15) = null,
	@ShipPostalCode nvarchar(10) = null,
	@ShipCountry nvarchar(15) = null
as
insert into Orders (CustomerID, EmployeeID, RequiredDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
values(@CustomerID, @EmployeeID, @RequiredDate,  @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry)

select IDENT_CURRENT('Orders') 
go

if OBJECT_ID('UpdateOrder') is not null drop procedure UpdateOrder
go

create procedure UpdateOrder
	@OrderID int,
	@CustomerID nchar(5),
	@EmployeeID int,
	@RequiredDate datetime,
	@ShipVia int,
	@Freight money,
	@ShipName nvarchar(40),
	@ShipAddress nvarchar(60),
	@ShipCity nvarchar(15),
	@ShipRegion nvarchar(15),
	@ShipPostalCode nvarchar(10),
	@ShipCountry nvarchar(15)
as
update Orders
set CustomerID = @CustomerID, 
	EmployeeID = @EmployeeID,
	RequiredDate = @RequiredDate,
	ShipVia = @ShipVia,
	Freight = @Freight,
	ShipName = @ShipName,
	ShipAddress = @ShipAddress,
	ShipCity = @ShipCity,
	ShipRegion = @ShipRegion,
	ShipPostalCode = @ShipPostalCode,
	ShipCountry = @ShipCountry
from Orders as o
where o.OrderID = @OrderID

select @@ROWCOUNT
go

if OBJECT_ID('DeleteOrder') is not null drop procedure DeleteOrder
go

create procedure DeleteOrder
	@OrderID int
as
delete from [Order Details]
where OrderID = @OrderID

delete from Orders
where OrderID = @OrderID

select @@ROWCOUNT
go

if OBJECT_ID('SetShippedDate') is not null drop procedure SetShippedDate
go

create procedure SetShippedDate
	@OrderID int,
	@ShippedDate datetime
as
update Orders
set ShippedDate = @ShippedDate
from Orders
where OrderID = @OrderID

select @@ROWCOUNT
go

if OBJECT_ID('SetOrderDate') is not null drop procedure SetOrderDate
go

create procedure SetOrderDate
	@OrderID int,
	@OrderDate datetime
as
update Orders
set OrderDate = @OrderDate
from Orders
where OrderID = @OrderID

select @@ROWCOUNT
go
