-- USE master
-- alter database SHOPDIENTHOAI set single_user with rollback immediate
-- drop database SHOPDIENTHOAI

CREATE DATABASE SHOPDIENTHOAI
GO

USE SHOPDIENTHOAI
GO

CREATE TABLE [BRAND]
(
    BrandID INT PRIMARY KEY IDENTITY(1, 1),
    BrandName NVARCHAR(255) UNIQUE NOT NULL,
    BrandURL NVARCHAR(50),

    CreatedDate DATETIME DEFAULT GETDATE()
)
GO


create TABLE [PRODUCT]
(
    ProductID INT PRIMARY KEY IDENTITY(1, 1),
    ProductName NVARCHAR(255) NOT NULL,
    ProductDescription NVARCHAR(MAX),--mô tả
    ProductPrice DECIMAL(18, 0) NOT NULL,
    PromotionPrice DECIMAL(18, 0) DEFAULT 0,
    Rating INT CHECK (RATING >=0 AND RATING <= 5),
    ProductImage NVARCHAR(4000) DEFAULT N'',
    ProductStock INT DEFAULT 1,--tồn kho
    ProductURL NVARCHAR(255),
    Viewcount INT,
    ProductStatus BIT,
    CreatedDate DATETIME DEFAULT GETDATE(),

    BrandID INT REFERENCES [BRAND](BrandID) NOT NULL,
)
GO

CREATE TABLE [CONFIGURATION]
(
    ConfigID INT PRIMARY KEY IDENTITY(1, 1),
    OSName NVARCHAR(255),
    OSVersion NVARCHAR(255),
    SizeDisplay NVARCHAR(255),
    FrontCamera NVARCHAR(255),
    RearCamera NVARCHAR(255),
    Cpu NVARCHAR(255),
    Ram NVARCHAR(255),
    Rom NVARCHAR(255),
    SimStatus NVARCHAR(255),
    Battery NVARCHAR(255),

    ProductID INT REFERENCES [PRODUCT](ProductID) ON DELETE CASCADE
)
GO

CREATE TABLE [CUSTOMER]
(
    CustomerID INT PRIMARY KEY IDENTITY(1, 1),
    CustomerUsername NVARCHAR(255) UNIQUE NOT NULL,
    CustomerPassword NVARCHAR(255) NOT NULL,
    CustomerEmail NVARCHAR(255),
    CustomerName NVARCHAR(255) NOT NULL,
    CustomerPhone NVARCHAR(20),
    CustomerAddress NVARCHAR(255),

    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE [ORDERSTATUS]
(
    StatusID INT PRIMARY KEY,
    StatusName NVARCHAR(255),

    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE [ORDER]
(
    OrderID INT PRIMARY KEY IDENTITY(1, 1),
    OrderDate DATETIME DEFAULT GETDATE(),
    Total DECIMAL(18, 2),
    CustomerName NVARCHAR(255) NOT NULL,
    CustomerPhone NVARCHAR(20) NOT NULL,
    CustomerAddress NVARCHAR(255) NOT NULL,

    OrderStatusID INT REFERENCES [ORDERSTATUS](StatusID),
    CustomerID INT REFERENCES [CUSTOMER](CustomerID)
)
GO

CREATE TABLE [ORDERDETAIL]
(
    DetailID INT PRIMARY KEY IDENTITY(1, 1),

    Quantity INT,
    OrderID INT REFERENCES [ORDER](OrderID),
    ProductID INT REFERENCES [PRODUCT](ProductID)
)
GO

CREATE TABLE [ADMIN]
(
    UserId INT PRIMARY KEY IDENTITY(1, 1),
    UserUsername NVARCHAR(255) UNIQUE NOT NULL,
    UserPassword NVARCHAR(255) NOT NULL,
    UserName NVARCHAR(255),

    CreatedDate DATETIME
)
GO

INSERT INTO [BRAND]
VALUES
    (N'Apple', N'apple', GETDATE()),
    (N'Samsung', N'samsung', GETDATE()),
    (N'Xiaomi', N'xiaomi', GETDATE()),
    (N'Vmart', N'vsmart', GETDATE()),
    (N'Huawei', N'huawei', GETDATE())
GO

INSERT INTO [ORDERSTATUS]
VALUES
    (1, N'Đang xử lý', GETDATE()),
    (2, N'Đang giao hàng', GETDATE()),
    (3, N'Đã giao hàng', GETDATE()),
    (4, N'Hàng có lỗi', GETDATE()),
    (5, N'Đã hủy', GETDATE())
GO

--Admin
INSERT INTO [ADMIN]
VALUES(N'hoang', N'4297f44b13955235245b2497399d7a93', N'Nguyễn Hoàng', GETDATE())
GO
                                       
INSERT INTO [ADMIN]
VALUES(N'quang', N'e10adc3949ba59abbe56e057f20f883e', N'Vũ Minh Quang', GETDATE())
GO                                                       

--------------------------------------------
-- public void FixEfProviderServicesProblem()
--         {
--             var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
--         }
--------------------------------------------

-- select * from PRODUCT
-- SELECT * FROM PRODUCT ORDER BY ProductID OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

-- select count(*) as 'Count' FROM PRODUCT 

INSERT [dbo].[PRODUCT]
VALUES
    ( N'iPhone 11 Pro Max 512GB Gold', N'Để tìm kiếm một chiếc smartphone có hiệu năng mạnh mẽ và có thể sử dụng mượt mà trong 2-3 năm tới thì không có chiếc máy nào xứng đang hơn chiếc iPhone 11 Pro Max 512GB mới ra mắt trong năm 2019 của Apple.',
        CAST(43990000 AS Decimal(18, 0)),
        CAST(41990000 AS Decimal(18, 0)), 5, N'https://cdn.tgdd.vn/Products/Images/42/210654/iphone-11-pro-max-512gb-gold-400x460.png', 50, N'iphone-11-pro-max-512gb-gold', 170, 1,
        CAST(N'2020-05-14T18:44:04.920' AS DateTime), 1)

insert into [CONFIGURATION]
values
    (N'iOS',
        N'iOS 13',
        N'6.5" 1242 x 2688 Pixels',
        N'12 MP',
        N'3 camera 12 MP',
        N'Apple A13 Bionic 6 nhân',
        N'4 GB',
        N'512 GB',
        N'1 eSIM & 1 Nano SIM',
        N'3969 mAh',
        1)

INSERT [dbo].[PRODUCT]
VALUES
    ( N'iPhone 11 Pro Max 256GB Black', N'Để tìm kiếm một chiếc smartphone có hiệu năng mạnh mẽ và có thể sử dụng mượt mà trong 2-3 năm tới thì không có chiếc máy nào xứng đang hơn chiếc iPhone 11 Pro Max 512GB mới ra mắt trong năm 2019 của Apple.',
        CAST(37990000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/210653/iphone-11-pro-max-256gb-black-400x460.png', 50, N'iphone-11-pro-max-256gb-black', 199, 1,
        CAST(N'2020-05-14T18:44:04.920' AS DateTime), 1)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'iPhone Xs Max 256GB White', N'Để tìm kiếm một chiếc smartphone có hiệu năng mạnh mẽ và có thể sử dụng mượt mà trong 2-3 năm tới thì không có chiếc máy nào xứng đang hơn chiếc iPhone 11 Pro Max 512GB mới ra mắt trong năm 2019 của Apple.',
        CAST(37990000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/190322/iphone-xs-max-256gb-white-400x460.png', 50, N'iphone-xs-max-256gb-white', 199, 1,
        CAST(N'2020-05-14T18:44:04.920' AS DateTime), 1)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy Fold Black', N'Sau rất nhiều chờ đợi thì Samsung Galaxy Fold - chiếc smartphone màn hình gập đầu tiên của Samsung cũng đã chính thức trình làng với thiết kế mới lạ.',
        CAST(50000000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/198158/samsung-galaxy-fold-black-400x460.png', 50, N'samsung-galaxy-fold-black', 1, 1,
        CAST(N'2020-05-14T18:44:04.920' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy S20 Ultra', N'Samsung Galaxy S20 Ultra siêu phẩm công nghệ hàng đầu của Samsung mới ra mắt với nhiều đột phá công nghệ, màn hình tràn viền không khuyết điểm, hiệu năng đỉnh cao, camera độ phân giải siêu khủng 108 MP cùng khả năng zoom 100X thách thức mọi giới hạn smartphone.',
        CAST(29990000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/217937/samsung-galaxy-s20-ultra-400x460-1-400x460.png', 50, N'samsung-galaxy-s20-ultra', 66, 1,
        CAST(N'2020-05-14T18:44:04.920' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Xiaomi Mi Note 10 Pro Black', N'Siêu phẩm tầm trung Xiaomi Mi Note 10 Pro, chiếc smartphone đầu tiên sở hữu ống kính độ phân giải 108 MP cùng hệ thống 5 camera độc đáo, công nghệ sạc siêu nhanh 30W đi kèm nhiều tính năng nổi trội khác.',
        CAST(13990000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/213590/xiaomi-mi-note-10-pro-black-400x460.png', 50, N'xiaomi-mi-note-10-pro-black', 80, 1,
        CAST(N'2020-05-14T18:44:04.920' AS DateTime), 3)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'iphone 8plus 64GB Rose Gold', N'Thừa hưởng những thiết kế đã đạt đến độ chuẩn mực, thế hệ iPhone 8 Plus thay đổi phong cách bóng bẩy hơn và bổ sung hàng loạt tính năng cao cấp cho trải nghiệm sử dụng vô cùng tuyệt vời.',
        CAST(15000000 AS Decimal(18, 0)),
        CAST(14999000 AS Decimal(18, 0)), 4, N'https://cdn.tgdd.vn/Products/Images/42/114110/iphone-8-plus-hh-600x600-600x600.jpg', 40, N'iphone-8plus-64GB-Rose-Gold', 120, 1,
        CAST(N'2020-05-14T19:08:30.053' AS DateTime), 1)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'iphone 11 128GB Green', N'Được xem là phiên bản iPhone "giá rẻ" trong bộ 3 iPhone mới ra mắt nhưng iPhone 11 128GB vẫn sở hữu cho mình rất nhiều ưu điểm mà hiếm có một chiếc smartphone nào khác sở hữu.',
        CAST(23990000 AS Decimal(18, 0)), NULL, 4, N'https://cdn.tgdd.vn/Products/Images/42/210644/iphone-11-128gb-green-600x600.jpg', 100, N'iphone-11-128GB-Green', 420, 1,
        CAST(N'2020-05-14T19:11:38.233' AS DateTime), 1)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'iphone 11 256GB White', N'iPhone 11 256GB là chiếc máy có mức giá "dễ chịu" trong bộ 3 iPhone vừa được Apple giới thiệu và nếu bạn muốn được trải nghiệm những nâng cấp về camera mới hay hiệu năng hàng đầu mà lại không muốn bỏ ra quá nhiều tiền thì đây thực sự là lựa chọn hàng đầu dành cho bạn.',
        CAST(25690000 AS Decimal(18, 0)),
        CAST(23490000 AS Decimal(18, 0)), 4, N'https://cdn.tgdd.vn/Products/Images/42/210648/iphone-11-256gb-white-600x600.jpg', 100, N'iphone-11-256GB-White', 90, 1,
        CAST(N'2020-05-14T19:13:30.810' AS DateTime), 1)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'iphone 11 Pro 64GB Black', N'Nếu như bây giờ để lựa chọn một thiết bị có thể sử dụng ổn định và được cập nhật trong khoảng 3 năm nữa thì không có sự lựa chọn nào xuất sắc hơn chiếc iPhone 11 Pro 64GB, siêu phẩm mới được giới thiệu cách đây không lâu tới từ Apple.',
        CAST(30490000 AS Decimal(18, 0)),
        CAST(29990000 AS Decimal(18, 0)), 4, N'https://cdn.tgdd.vn/Products/Images/42/188705/iphone-11-pro-black-600x600.jpg', 10, N'iphone-11-Pro-64GB-Black', 38, 1,
        CAST(N'2020-05-14T19:15:16.880' AS DateTime), 1)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'iphone 11 64GB Red', N'Sau bao nhiêu chờ đợi cũng như đồn đoán thì cuối cùng Apple đã chính thức giới thiệu bộ 3 siêu phẩm iPhone 11 mạnh mẽ nhất của mình vào tháng 9/2019. Có mức giá rẻ nhất nhưng vẫn được nâng cấp mạnh mẽ như chiếc iPhone Xr năm ngoái, đó chính là phiên bản iPhone 11 64GB.',
        CAST(21690000 AS Decimal(18, 0)), NULL, 4, N'https://cdn.tgdd.vn/Products/Images/42/153856/iphone-11-red-600x600.jpg', 100, N'iphone-11-64GB-Red', 23, 1,
        CAST(N'2020-05-14T19:18:13.560' AS DateTime), 1)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy Z Flip Black', N'Cuối cùng sau bao nhiêu thời gian chờ đợi, chiếc điện thoại Samsung Galaxy Z Flip đã được Samsung ra mắt tại sự kiện Unpacked 2020. Siêu phẩm với thiết kế màn hình gập vỏ sò độc đáo, hiệu năng tuyệt đỉnh cùng nhiều công nghệ thời thượng, dẫn dầu 2020.',
        CAST(36000000 AS Decimal(18, 0)),
        CAST(35999000 AS Decimal(18, 0)), 4, N'https://cdn.tgdd.vn/Products/Images/42/213022/samsung-galaxy-z-flip-den-600x600-600x600.jpg', 55, N'samsung-galaxy-z-flip-black', 55, 1,
        CAST(N'2020-05-14T19:22:41.770' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy Note 10plus Blue', N'Trông ngoại hình khá giống nhau, tuy nhiên Samsung Galaxy Note 10plus sở hữu khá nhiều điểm khác biệt so với Galaxy Note 10 và đây được xem là một trong những chiếc máy đáng mua nhất trong năm 2019, đặc biệt dành cho những người thích một chiếc máy màn hình lớn, camera chất lượng hàng đầu.',
        CAST(27000000 AS Decimal(18, 0)),
        CAST(25490000 AS Decimal(18, 0)), 4, N'https://cdn.tgdd.vn/Products/Images/42/206176/samsung-galaxy-note-10-plus-blue-600x600.jpg', 100, N'samsung-galaxy-note-10plus-blue', 23, 1,
        CAST(N'2020-05-14T19:24:33.727' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy Note 10 Silver', N'Nếu như từ trước tới nay dòng Galaxy Note của Samsung thường ít được các bạn nữ sử dụng bởi kích thước màn hình khá lớn khiến việc cầm nắm trở nên khó khăn thì Samsung Galaxy Note 10 sẽ là chiếc smartphone nhỏ gọn, phù hợp với cả những bạn có bàn tay nhỏ.',
        CAST(22990000 AS Decimal(18, 0)),
        CAST(12990000 AS Decimal(18, 0)), 5, N'https://cdn.tgdd.vn/Products/Images/42/191276/samsung-galaxy-note-10-silver-600x600.jpg', 100, N'samsung-galaxy-note-10-silver', 11, 1,
        CAST(N'2020-05-14T19:27:19.773' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy S20 Pink', N'Samsung Galaxy S20 là chiếc điện thoại với thiết kế màn hình tràn viền không khuyết điểm, camera sau ấn tượng, hiệu năng khủng cùng nhiều những đột phá công nghệ nổi bật, dẫn đầu thế giới.',
        CAST(20000000 AS Decimal(18, 0)),
        CAST(19999000 AS Decimal(18, 0)), 5, N'https://cdn.tgdd.vn/Products/Images/42/217935/samsung-galaxy-s20-600x600-hong-600x600.jpg', 100, N'samsung-galaxy-s20-pink', 199, 1,
        CAST(N'2020-05-14T19:29:38.877' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy S10plus White', N'Samsung Galaxy S10plus 128GB là một trong những chiếc smartphone được trông chờ nhiều nhất trong năm 2019 và không phụ sự kỳ vọng của mọi người thì chiếc Galaxy S thứ 10 của Samsung thực sự gây ấn tượng mạnh cho người dùng',
        CAST(18000000 AS Decimal(18, 0)),
        CAST(17000000 AS Decimal(18, 0)), 5, N'https://cdn.tgdd.vn/Products/Images/42/179530/samsung-galaxy-s10-plus-white-600x600.jpg', 100, N'samsung-galaxy-S10plus-white', 123, 1,
        CAST(N'2020-05-14T19:32:00.807' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy S10 Lite Blue', N'Samsung Galaxy S10 Lite là một phiên bản khác của dòng Galaxy S10 đã ra mắt trước đó nhưng mang trong mình khá nhiều khác biệt về ngoại hình cũng như bên trong.',
        CAST(14990000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/194251/samsung-galaxy-s10-lite-blue-thumb-600x600.jpg', 100, N'samsung-galaxy-S10-lite-blue', 420, 1,
        CAST(N'2020-05-14T19:34:05.977' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Samsung Galaxy Note 10 Lite Silver', N'Sau bao đồn đoán và trông ngóng thì cuối cùng "người em út" trong series Note 10 cũng đã chính thức trình làng với tên gọi Samsung Galaxy Note 10 Lite với những thay đổi nhất định về ngoại hình',
        CAST(10490000 AS Decimal(18, 0)),
        CAST(99999000 AS Decimal(18, 0)), 5, N'https://cdn.tgdd.vn/Products/Images/42/214909/samsung-galaxy-note-10-lite-thumb-600x600.jpg', 1000, N'samsung-galaxy-note-10-lite-silver', 53, 1,
        CAST(N'2020-05-14T19:35:56.540' AS DateTime), 2)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Xiaomi Mi Note 10 Lite White', N'Xiaomi Mi Note 10 Lite là thế hệ Mi Note tiếp theo vừa được Xiaomi chính thức ra mắt vào tháng 5/2020. Máy sở hữu nhiều trang bị hấp dẫn với bộ 4 camera lên đến 64 MP, thiết kế thời trang cùng màn hình AMOLED cong 3D quyến rũ.',
        CAST(10490000 AS Decimal(18, 0)),
        CAST(99999000 AS Decimal(18, 0)), 5, N'https://cdn.tgdd.vn/Products/Images/42/220851/xiaomi-mi-note-10-lite-trang-600x600-600x600.jpg', 100, N'xiaomi-mi-note-10-lite-white', 310, 1,
        CAST(N'2020-05-14T19:38:25.270' AS DateTime), 3)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Vsmart Active 3 (6GB/64GB)', N'Vsmart Active 3 (4GB/64GB) là một smartphone có hiệu năng ổn định, thời lượng pin cả ngày dài và còn nhiều tính năng đặc biệt khác nữa, hứa hẹn sẽ mang đến cho bạn một thiết bị công nghệ chẳng những thời trang còn rất hiện đại.',
        CAST(4000000 AS Decimal(18, 0)), NULL, 4, N'https://cdn.tgdd.vn/Products/Images/42/217438/vsmart-active-3-6gb-emerald-green-600x600-600x600.jpg', 100, N'vsmart-active-3', 210, 1,
        CAST(N'2020-05-14T20:57:28.030' AS DateTime), 4)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Vsmart Joy 3 (4GB/64GB)', N'Vsmart Joy 3 4GB/64GB là một phiên bản mới, được nâng cấp nhẹ về RAM và bộ nhớ trong so với 2 mẫu smartphone cùng tên trước đó, mang đến thêm một sự lựa chọn hấp dẫn ở phân khúc giá rẻ cho người dùng.',
        CAST(3490000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/219208/vsmart-joy-3-4gb-den-600x600-600x600.jpg', 100, N'vsmart-joy-3', 300, 1,
        CAST(N'2020-05-14T20:59:01.590' AS DateTime), 4)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Vsmart Joy 3 (3GB/32GB)', N'Chiếc điện thoại Vsmart Joy 3 3GB/32GB mang trong mình thiết kế trẻ trung, hiện đại, hiệu năng tốt cùng thời lượng pin lớn với giá bán hấp dẫn phù hợp với đại đa số người dùng. Đây sẽ là chiếc điện thoại gây nhiều chú ý nhất trong thời gian sắp tới.',
        CAST(2690000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/217920/vsmart-joy-3-tim-600x600-600x600.jpg', 100, N'vsmart-joy-3', 199, 1,
        CAST(N'2020-05-14T21:00:09.860' AS DateTime), 4)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Vsmart Joy 3 (2GB/32GB)', N'Chiếc điện thoại Vsmart Joy 3 3GB/32GB mang trong mình thiết kế trẻ trung, hiện đại, hiệu năng tốt cùng thời lượng pin lớn với giá bán hấp dẫn phù hợp với đại đa số người dùng. Đây sẽ là chiếc điện thoại gây nhiều chú ý nhất trong thời gian sắp tới.',
        CAST(2190000 AS Decimal(18, 0)),
        CAST(2189000 AS Decimal(18, 0)), 5, N'https://cdn.tgdd.vn/Products/Images/42/217921/vsmart-joy-3-2gb-tim-600x600-600x600.jpg', 100, N'vsmart-joy-3', 199, 1,
        CAST(N'2020-05-14T21:01:53.703' AS DateTime), 4)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Huawei P40 Pro Silver', N'Huawei P40 Pro là một trong 3 mẫu smartphone đầu bảng năm 2020 đến từ nhà Huawei. Chiếc máy sở hữu cụm 4 camera Leica chụp ảnh và quay phim đỉnh cao, thiết kế màn hình siêu tràn ấn tượng cùng hiệu năng khủng xứng tầm flagship.',
        CAST(24000000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/215773/huawei-p40-pro-600x600-3-600x600.jpg', 100, N'huawei-P40-pro', 199, 1,
        CAST(N'2020-05-14T21:05:44.240' AS DateTime), 5)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Huawei Mate 30 Pro Silver', N'Những năm gần đây thì Huawei luôn mang tới cho người dùng sự bất ngờ với những trang bị đột phá lần đầu tiên xuất hiện trên chiếc smartphone của mình và mới đây chiếc Huawei Mate 30 Pro đã chính thức ra mắt và nó vẫn mang trong mình rất nhiều công nghệ mang tính đột phá của Huawei.',
        CAST(19990000 AS Decimal(18, 0)), NULL, 5, N'https://cdn.tgdd.vn/Products/Images/42/199046/huawei-mate-30-pro-600x600-1-600x600.jpg', 100, N'huawei-mate-30-pro', 199, 1,
        CAST(N'2020-05-14T21:08:18.930' AS DateTime), 5)
INSERT [dbo].[PRODUCT]
VALUES
    ( N'Huawei P40 Blue', N'Chiếc điện thoại cao cấp Huawei P40 mới được Huawei giới thiệu vào tháng 3/2020. Với thiết kế tinh tế, hiệu năng khủng cùng hệ thống camera ấn tượng, chiếc smartphone hứa hẹn sẽ tạo nên làn sóng mới cho thị trường điện thoại di động 2020.',
        CAST(18000000 AS Decimal(18, 0)),
        CAST(17999000 AS Decimal(18, 0)), 5, N'https://cdn.tgdd.vn/Products/Images/42/211157/huawei-p40-600x600-2-200x200.jpg', 100, N'huawei-p40-blue', 199, 1,
        CAST(N'2020-05-14T21:11:03.127' AS DateTime), 5)


		use SHOPDIENTHOAI
		select * from [CONFIGURATION]




		----
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'iOS', N'iOS 13', N'6.5" 1242 x 2688 Pixels', N'12 MP', N'3 camera 12 MP', N'Apple A13 Bionic 6 nhân', N'4 GB', N'512 GB', N'1 eSIM & 1 Nano SIM', N'3969 mAh', 2)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'IOS', N'IOS 13', N'6.5" 1242 x 2688 Pixels', N'7 MP', N'2 camera 12 MP', N'Apple A12 Bionic', N'4 GB', N'256 GB', N'1 eSIM & 1 Nano SIM', N'3174 mAh', 3)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'Chính 7.3" & Phụ 4.6" 2152 x 1536 Pixels', N'Trong: 10 MP, 8 MP; Ngoài: 10 MP', N'Chính 12 MP & Phụ 12 MP, 16 MP', N'Snapdragon 855 8 nhân', N'12 GB', N'512 GB', N'	1 eSIM & 1 Nano SIM', N'	4380 mAh', 4)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.9" 2K+ (1440 x 3200 Pixels)', N'40 MP', N'Chính 108 MP & phụ 48 MP, 12 MP, TOF 3D', N'Snapdragon 855 8 nhân', N'12 GB', N'128 GB', N'	1 eSIM & 1 Nano SIM', N'4500 mAh', 5)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.5" 	Full HD+ (1080 x 2340 Pixels)', N'32 MP', N'Chính 108 MP & Phụ 20 MP, 12 MP, 5 MP, 2 MP', N'Snapdragon 730G 8 nhân', N'8 GB', N'256 GB', N'2 Nano SIM', N'5000 mAh', 6)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'IOS', N'IOS 12', N'5.5" Full HD', N'7 MP', N'	Chính 12 MP & Phụ 12 MP', N'A12 Bionic', N'3 GB', N'64 GB', N'1 sim', N'2700 mAh', 7)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'IOS', N'IOS 13', N'6.1" 828 x 1792 Pixels', N'	12 MP', N'Chính 12 MP & Phụ 12 MP', N'A13 Bionic', N'4 GB', N'128 GB', N'1 sim', N'3110 mAh', 8)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'IOS', N'IOS 13', N'6.1" 828 x 1792 Pixels', N'	12 MP', N'Chính 12 MP & Phụ 12 MP', N'A13 Bionic', N'4 GB', N'256 GB', N'1 sim', N'3110 mAh', 9)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'IOS', N'IOS 13', N'5.8" 1125 x 2436 Pixels', N'12 MP', N'3 camera 12 MP', N'A13 Bionic', N'4 GB', N'64 GB', N'1 sim', N'3000 mAh', 10)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES (N'IOS', N'IOS 13', N'6.1" 828 x 1792 Pixels', N'	12 MP', N'Chính 12 MP & Phụ 12 MP', N'A13 Bionic', N'4 GB', N'64 GB', N'1 sim', N'3110 mAh', 11)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES (N'Android', N'Andoid 10', N'6.9" 2K+ (1440 x 3200 Pixels)', N'40 MP', N'Chính 108 MP & phụ 48 MP, 12 MP, TOF 3D', N'Snapdragon 730G 8 nhân', N'6 GB', N'128 GB', N'2 sim', N'2500 mAh', 12)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES (N'Android', N'Andoird 9 Pie', N'6.1" 828 x 1792 Pixels', N'12 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'4 GB', N'128 GB', N'2 sim', N'4500 mAh', 13)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES (N'Android', N'Andoird 9 Pie', N'6.5" 1242 x 2688 Pixels', N'32 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'6 GB', N'64 GB', N'2 sim', N'3969 mAh', 14)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.1" 828 x 1792 Pixels', N'12 MP', N'3 camera 12 MP', N'Snapdragon 730G 8 nhân', N'4 GB', N'128 GB', N'2 sim', N'4500 mAh', 15)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.1" 828 x 1792 Pixels', N'32 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'6 GB', N'64 GB', N'2 sim', N'3969 mAh', 16)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.5" 1242 x 2688 Pixels', N'32 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'4 GB', N'128 GB', N'2 sim', N'4500 mAh', 17)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.1" 828 x 1792 Pixels', N'12 MP', N'3 camera 12 MP', N'Snapdragon 730G 8 nhân', N'6 GB', N'64 GB', N'2 sim', N'3969 mAh', 18)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.5" 1242 x 2688 Pixels', N'40 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'4 GB', N'128 GB', N'2 sim', N'4500 mAh', 19)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.1" 828 x 1792 Pixels', N'12 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'6 GB', N'64 GB', N'2 sim', N'3969 mAh', 20)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.5" 1242 x 2688 Pixels', N'32 MP', N'3 camera 12 MP', N'Snapdragon 730G 8 nhân', N'4 GB', N'128 GB', N'2 sim', N'4500 mAh', 21)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.1" 828 x 1792 Pixels', N'12 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'6 GB', N'64 GB', N'2 sim', N'5000 mAh', 22)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.5" 1242 x 2688 Pixels', N'40 MP', N'3 camera 12 MP', N'Snapdragon 730G 8 nhân', N'4 GB', N'128 GB', N'2 sim', N'4500 mAh', 23)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.1" 828 x 1792 Pixels', N'12 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'6 GB', N'64 GB', N'2 sim', N'5000 mAh', 24)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.5" 1242 x 2688 Pixels', N'40 MP', N'3 camera 12 MP', N'Snapdragon 730G 8 nhân', N'4 GB', N'128 GB', N'2 sim', N'4500 mAh', 25)
INSERT [dbo].[CONFIGURATION] ( [OSName], [OSVersion], [SizeDisplay], [FrontCamera], [RearCamera], [Cpu], [Ram], [Rom], [SimStatus], [Battery], [ProductID]) VALUES ( N'Android', N'Andoird 9 Pie', N'6.1" 828 x 1792 Pixels', N'12 MP', N'3 camera 12 MP', N'Snapdragon 855 8 nhân', N'6 GB', N'64 GB', N'2 sim', N'5000 mAh', 26)
