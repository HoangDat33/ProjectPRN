USE [master]
GO

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Project_PRN212')
BEGIN
    ALTER DATABASE Project_PRN212 SET OFFLINE WITH ROLLBACK IMMEDIATE;
    ALTER DATABASE Project_PRN212 SET ONLINE;
    DROP DATABASE Project_PRN212;
END

GO

CREATE DATABASE Project_PRN212
GO

USE Project_PRN212
GO

-- Table Roles: Admin:1, User:2
CREATE TABLE Roles ( 
    ID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(55) NOT NULL,
    Description NVARCHAR(255),
    CreatedAt DATE,
    UpdatedAt DATE,
    DeletedAt DATE,
    IsDelete BIT DEFAULT 0,
	DeleteByID INT
);

-- TABLE Departments:
CREATE TABLE Departments (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(55),
    Description NVARCHAR(55),
    CreatedAt DATE,
    UpdatedAt DATE,
    DeletedAt DATE,
    IsDelete BIT DEFAULT 0,
    DeletedByID INT
);

-- TABLE Positions:
CREATE TABLE Positions (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(55),
    Description NVARCHAR(100),
    Salary DECIMAL(10, 2),
    CreatedAt DATE,
    UpdatedAt DATE,
    DeletedAt DATE,
    IsDelete BIT DEFAULT 0,
    DeletedByID INT
);

-- TABLE Employees:
-- TABLE Employees:
CREATE TABLE Employees (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(55),
    LastName NVARCHAR(55),
    Email VARCHAR(55) NOT NULL,
    Phone VARCHAR(20), -- Tăng kích thước của Phone nếu cần thiết
    Salary DECIMAL(10, 2),
    RoleID INT DEFAULT 2,
    DepartmentID INT,
    ManagerID INT,
    PositionID INT,
    DateOfBirth DATE,
    Gender BIT DEFAULT 0, -- Giá trị mặc định cho Gender
    Address NVARCHAR(100),
    CreatedAt DATE,
    UpdatedAt DATE,
    DeletedAt DATE,
    IsDelete BIT DEFAULT 0,
    DeletedByID INT,
    FOREIGN KEY (RoleID) REFERENCES Roles(ID) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (PositionID) REFERENCES Positions(ID) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(ID) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (ManagerID) REFERENCES Employees(ID) ON DELETE NO ACTION ON UPDATE NO ACTION, -- Thay đổi thành ON DELETE NO ACTION
    CONSTRAINT CHK_Employees_Salary CHECK (Salary >= 0) -- Ràng buộc để đảm bảo Salary không âm
);


-- TABLE Authentication:
CREATE TABLE Authentication (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT,
    Username VARCHAR(40),
    PassWord VARCHAR(255),
    CreatedAt DATE,
    UpdatedAt DATE,
    DeletedAt DATE,
    IsDelete BIT DEFAULT 0,
    DeletedByID INT,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(ID),
    CONSTRAINT UQ_Authentication_Username UNIQUE (Username) -- Unique constraint on Username
);

-- Table JobStatus:
CREATE TABLE JobStatus (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(100),
    CreatedAt DATE,
    UpdatedAt DATE,
    DeletedAt DATE,
    IsDelete BIT DEFAULT 0,
    DeletedByID INT
);

-- Table Jobs:
CREATE TABLE Jobs (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
	AssignedBy INT,
	DepartmentID INT,
    StartDate DATE,
    EndDate DATE,
    JobStatusID INT,
    CreatedAt DATE,
    UpdatedAt DATE,
    DeletedAt DATE,
    IsDelete BIT DEFAULT 0,
    DeletedByID INT,
    FOREIGN KEY (JobStatusID) REFERENCES JobStatus(ID)  ON DELETE SET NULL ON UPDATE CASCADE,
	FOREIGN KEY (AssignedBy) REFERENCES Employees(ID),
	FOREIGN KEY (DepartmentID) REFERENCES Departments(ID)
);

-- Table EmployeeJobs:
CREATE TABLE EmployeeJobs (
    EmployeeJobID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT,
    JobID INT,
    AssignmentDate DATE,
    CreatedAt DATE,
    UpdatedAt DATE,
    DeletedAt DATE,
    IsDelete BIT DEFAULT 0,
    DeletedByID INT,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(ID)  ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (JobID) REFERENCES Jobs(ID)  ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT UQ_EmployeeJobs UNIQUE (EmployeeID, JobID) 
);


-- Insert into database:
-- Roles:
INSERT INTO Roles (RoleName, Description, CreatedAt, UpdatedAt)
VALUES 
(N'Quản trị viên', N'Vai trò quản trị', GETDATE(), GETDATE()),
(N'Nhân viên', N'Vai trò nhân viên', GETDATE(), GETDATE()),
(N'Quản lí', N'Vai trò quản lí', GETDATE(), GETDATE());

-- Departments:
INSERT INTO Departments (Name, Description, CreatedAt, UpdatedAt)
VALUES 
(N'Công nghệ thông tin', N'Phòng công nghệ thông tin', GETDATE(), GETDATE()),
(N'Nhân sự', N'Phòng nhân sự', GETDATE(), GETDATE()),
(N'Kế toán', N'Phòng kế toán', GETDATE(), GETDATE()),
(N'Marketing', N'Phòng marketing', GETDATE(), GETDATE()),
(N'Bán hàng', N'Phòng bán hàng', GETDATE(), GETDATE());

-- Positions:
INSERT INTO Positions (Name, Description, Salary, CreatedAt, UpdatedAt)
VALUES 
(N'Trưởng phòng', N'Trưởng phòng - Quản lí công việc', 35000000.00, GETDATE(), GETDATE()),
(N'Nhân viên chính', N'Nhân viên chính thức của công ty, đã kí hợp đồng', 20000000.00, GETDATE(), GETDATE()),
(N'Nhân viên parttime', N'Nhân viên làm việc partime', 15000000.00, GETDATE(), GETDATE()),
(N'Thực tập sinh', N'Sinh viên học tập, thực tập, làm việc ở công ty', 8000000.00, GETDATE(), GETDATE());

-- Employees:
-- Manager : Phòng công nghệ thông tin
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Nguyễn', N'Trí Thắng', 'nguyen.tri.thang@gmail.com', '0987654321', 35000000.00, 3, 1, null, 1, '1980-01-01', 1, N'97 HAO NAM, P.O CHO DUA, DONG DA, HA NOI ', GETDATE(), GETDATE());
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender,  Address, CreatedAt, UpdatedAt)
VALUES 
(N'Trần', N'Thị Lan', 'tran.thi.lan@gmail.com', '0987654321', 28000000.00, 2, 1, 1, 2, '1985-02-28', 0, N'101 Mai Hắc Đế, Bùi Thị Xuân, Hai Bà Trưng, Hà Nội', GETDATE(), GETDATE()),
(N'Lê', N'Tuấn Tú', 'le.tuan.tu@gmail.com', '0976543210', 28000000.00, 2, 1, 1, 2, '1986-05-15',1, N'24B Lò Đúc, Phạm Đình Hổ, Hai Bà Trưng, Hà Nội ', GETDATE(), GETDATE()),
(N'Phan', N'Thị Tâm', 'phan.thi.tam@gmail.com', '0965432109', 28000000.00, 2, 1, 1, 3, '1987-08-20',0, N'26 Tuệ Tĩnh, Bùi Thị Xuân, Hai Bà Trưng, Hà Nội', GETDATE(), GETDATE()),
(N'Trịnh', N'Tuấn Tài', 'trinh.tuan.tai@gmail.com', '0954321098', 28000000.00, 2, 1, 1, 2, '1988-11-25',1, N'SO19, Trần Quốc Hoàn, Đinh Văn Hợi, Cầu Giấy, Hà Nội', GETDATE(), GETDATE()),
(N'Đặng', N'Thị Hoa', 'dang.thi.hoa@gmail.com', '0943210987', 28000000.00, 2, 1, 1, 3, '1996-04-30',0, N'8C Hoàng Ngọc Phách, Láng Hạ, Đống Đa, Hà Nội', GETDATE(), GETDATE()),
(N'Võ', N'Tuấn Minh', 'vo.tuan.minh@gmail.com', '0932109876', 28000000.00, 2, 1, 1, 3, '1997-07-05',1, N'30 Hàng Bông, Hàng Gai, Hoàn Kiếm, Hà Nội', GETDATE(), GETDATE()),
(N'Bùi', N'Thị Thắm', 'bui.thi.tham@gmail.com', '0921098765', 28000000.00, 2, 1, 1, 4, '2003-10-10',0, N'2 Nhà Chung, Hàng Trống, Hoàn Kiếm, Hà Nội', GETDATE(), GETDATE()),
(N'Mai', N'Tuấn Tài', 'mai.tuan.tai@gmail.com', '0910987654', 28000000.00, 2, 1, 1, 4, '2002-01-15',1, N'6 Ngách 371/9 Kim Hạ, Ba Đình, Hà Nội', GETDATE(), GETDATE());

-- Manager : Phòng nhân sự
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth,  Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Phạm', N'Trí Thanh', 'pham.tri.thanh@gmail.com', '0987654321', 35000000.00, 3, 2, NULL, 1, '1980-02-01', 1, N'SO 47 Lương Văn Cần, Hoàn Kiếm, Hà Nội', GETDATE(), GETDATE());
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Lê', N'Thị Thắm', 'le.thi.tham@gmail.com', '0987654321', 28000000.00, 2, 2, 10, 2, '1985-03-28',0, N'SO 48 Lê Đài Hanh, Hai Bà Trưng, Hà Nội', GETDATE(), GETDATE()),
(N'Hoàng', N'Tuấn Dũng', 'hoang.tuan.dung@gmail.com', '0976543210', 28000000.00, 2, 2, 10, 2, '1986-06-15',1, N'Số 5B Nghách 34/68/7 Hoàng Cầu, Đống Đa, Hà Nội ', GETDATE(), GETDATE()),
(N'Nguyễn', N'Thị Tâm', 'nguyen.thi.tam@gmail.com', '0965432109', 28000000.00, 2, 2, 10, 3, '2000-09-20',0, N'110 Nguyễn Ngọc Mai, Khương Mai, Thanh Xuân, Hà Nội', GETDATE(), GETDATE()),
(N'Vũ', N'Tuấn Đức', 'vu.tuan.duc4@gmail.com', '0954321098', 28000000.00, 2, 2, 10, 4, '2001-12-25',1, N'65B Hai Bà Trưng, Hoàn Kiếm, Hà Nội ', GETDATE(), GETDATE()),
(N'Bùi', N'Thị Tươi', 'bui.thi.tuoi5@gmail.com', '0943210987', 28000000.00, 2, 2, 10, 2, '1995-05-30', 0,N'79E Hai Bà Trưng , Cửa Nam, Hoàn Kiếm, Hà Nội', GETDATE(), GETDATE()),
(N'Đỗ', N'Tuấn Minh', 'do.tuan.minh6@gmail.com', '0932109876', 28000000.00, 2, 2, 10, 2, '1990-08-05',1, N'Phố Mới , Thọ Xuân, Đan Phượng, Hà Nội', GETDATE(), GETDATE()),
(N'Phan', N'Thị Lâm', 'phan.thi.lam@gmail.com', '0921098765', 28000000.00, 2, 2, 10, 2, '1991-11-10',0, N'10B Tràng Thi, Hoàn Kiếm, Hà Nội', GETDATE(), GETDATE());

-- Manager : Phòng Kế toán
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Lê', N'Trí Khang', 'le.tri.khang3@gmail.com', '0987654321', 35000000.00, 3, 3, NULL, 1, '1980-03-01',1, N'SO 02 Hoa Lư, Lê Đại Hành, Hai Bà Trưng, Hà Nội', GETDATE(), GETDATE());
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Lý', N'Thị Hồng', 'ly.thi.hong1@gmail.com', '0987654321', 28000000.00, 2, 3, 18, 2, '1985-04-28',0, N'SO 09 Đào Duy Anh, Đống Đa, Hà Nội', GETDATE(), GETDATE()),
(N'Ma', N'Tuấn Tài', 'ma.tuan.tai4@gmail.com', '0976543210', 28000000.00, 2, 3, 18, 2, '1986-07-15',1, N'Tòa A, Chung cư An Sing, Mỹ Đình, Nam Từ Liêm, Hà Nội', GETDATE(), GETDATE()),
(N'Trần', N'Thị Hồng', 'tran.thi.hong22@gmail.com', '0965432109', 28000000.00, 2, 3, 18, 2, '1987-10-20',0, N'SO 1 Phạm Huy Thông, Ba Đình, Hà Nội', GETDATE(), GETDATE()),
(N'Đặng', N'Tuấn Tú', 'dang.tuan.tu23@gmail.com', '0954321098', 28000000.00, 2, 3, 18, 2, '1988-01-25',1, N'42 Lê Thái Tổ, Hàng Trống, Hoàn Kiếm, Hà Nội', GETDATE(), GETDATE()),
(N'Phạm', N'Thị Chính', 'pham.thi.chinh5@gmail.com', '0943210987', 28000000.00, 2, 3, 18, 2, '1989-04-30',0, N'920 Lê Đại Thành, Ba Đình, Hà Nội', GETDATE(), GETDATE()),
(N'Hoàng', N'Đức Mĩ', 'hoang.duc.mi3@gmail.com', '0932109876', 28000000.00, 2, 3, 18, 2, '1990-07-05',1, N'158 Thái Hà, Đống Đa, Hà Nội ', GETDATE(), GETDATE()),
(N'Mai', N'Thị Lan', 'mai.thi.lan7@gmail.com', '0921098765', 28000000.00, 2, 3, 18, 2, '1991-10-10',0, N'164 Nguyễn Tuân, Thanh Xuân, Hà Nội ', GETDATE(), GETDATE());

-- Manager : Phòng marketing
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Vũ', N'Trí Thịnh', 'vu.tri.thinh8@gmail.com', '0987654321', 35000000.00, 3, 4, NULL, 1, '1980-04-01',1, N'102 H4 Thành Công, Ba Đình, Hà Nội ', GETDATE(), GETDATE());
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Trần', N'Thị Bình', 'tran.thi.binh@gmail.com', '0987654321', 28000000.00, 2, 4, 26, 2, '1985-05-28',0, N'49 Ngô Thì Nhậm, Hai Bà Trưng, Hà Nội ', GETDATE(), GETDATE()),
(N'Phạm', N'Tuấn Trọng', 'pham.tuan.trong43@gmail.com', '0976543210', 28000000.00, 2, 4, 26, 2, '1986-08-15',1, N'402 La Thành, Chợ Dừa, Đống Đa, Hà Nội', GETDATE(), GETDATE()),
(N'Nguyễn', N'Thị Hoa', 'nguyen.thi.hoa5@gmail.com', '0965432109', 28000000.00, 2, 4, 26, 2, '1987-11-20',0, N'57A Nguyễn Khắc Hiếu, Ba Đình, Hà Nội ', GETDATE(), GETDATE()),
(N'Vũ', N'Tuấn Chân', 'vu.tuan.chan@gmail.com', '0954321098', 28000000.00, 2, 4, 26, 3, '2000-02-25',1, N'10B Tràng Thi, Hoàn Kiếm, Hà Nội', GETDATE(), GETDATE()),
(N'Bùi', N'Thị Nhàn', 'bui.thi.nhan@gmail.com', '0943210987', 28000000.00, 2, 4, 26, 3, '1998-05-30',0, N'79E Hai Bà Trưng, Cửa Nam, Hoàn Kiếm, Hà Nội ', GETDATE(), GETDATE()),
(N'Đỗ', N'Tuấn Đức', 'do.tuan.duc9@gmail.com', '0932109876', 28000000.00, 2, 4, 26, 4, '2001-08-05',1, N'10B Tràng Thi , Hoàn Kiếm, Hà Nội ', GETDATE(), GETDATE()),
(N'Mai', N'Thị Lâm', 'mai.thi.lam@gmail.com', '0921098765', 28000000.00, 2, 4, 26, 4, '2002-11-10',0, N'12,172 Lạc Long Quân, Phường Bưởi, Tây Hồ, Hà Nội', GETDATE(), GETDATE());

-- Phòng bán hàng:
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Hồ', N'Trí Thức', 'ho.tri.thuc5@company.com', '0987654321', 35000000.00, 3, 5, NULL, 1, '1980-05-01', 1, N'84 Mai Hắc Đế, Hai Bà Trưng, Hà Nội ', GETDATE(), GETDATE());
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Lý', N'Thị Cân', 'ly.thi.can5@gmail.com', '0987654321', 28000000.00, 2, 5, 34, 3, '1985-06-28',0,N'56 Bạch Thái Bưởi, Văn Quan, Hà Đông, Hà Nội', GETDATE(), GETDATE()),
(N'Ma', N'Trí Tài', 'ma.tri.tai2@gmail.com', '0976543210', 28000000.00, 2, 5, 34, 3, '1986-09-15',1, N'SO 1 Phạm Huy Thông, Ba Đình, Hà Nội ', GETDATE(), GETDATE()),
(N'Trần', N'Thị Lụa', 'tran.thi.lua3@gmail.com', '0965432109', 28000000.00, 2, 5, 34, 2, '1987-12-20',0, N'158 Thái Hà, Đống Đa, Hà Nội', GETDATE(), GETDATE()),
(N'Đặng', N'Khôi Nguyên', 'dang.khoi.nguyen4@gmail.com', '0954321098', 28000000.00, 2, 5, 34, 2, '1988-03-25', 1,N'77 Tây Sơn, Đống Đa, Hà Nội ', GETDATE(), GETDATE()),
(N'Phạm', N'Văn Đồng', 'pham.van.dong5@gmail.com', '0943210987', 28000000.00, 2, 5, 34, 4, '1999-06-30',1, N'194 Lê Duẩn, Hai Bà Trưng, Hà Nội', GETDATE(), GETDATE()),
(N'Hoàng', N'Minh Đức', 'hoang.minh.duc6@gmail.com', '0932109876', 28000000.00, 2, 5, 34, 2, '1990-09-05',1, N'95B Phố Huế, Ngô Thì Nhậm, Hai Bà Trưng, Hà Nội', GETDATE(), GETDATE()),
(N'Mai', N'Hóa Vàng', 'mai.hoa.vang7@gmail.com', '0921098765', 28000000.00, 2, 5, 34, 2, '1991-12-10',1, N'402 La Thành, Chợ Dừa, Đống Đa, Hà Nội', GETDATE(), GETDATE());
Select * From Employees


--Insert authentication:
INSERT INTO Authentication (EmployeeID, Username, PassWord, CreatedAt, UpdatedAt)
VALUES 
(1, 'a', '1', GETDATE(), GETDATE()),
(2, 'b', '1', GETDATE(), GETDATE()),
(3, 'le.tuan.tu@gmail.com', '12345678', GETDATE(), GETDATE()),
(4, 'phan.thi.tam@gmail.com', '12345678', GETDATE(), GETDATE()),
(5, 'trinh.tuan.tai@gmail.com', '12345678', GETDATE(), GETDATE()),
(6, 'dang.thi.hoa@gmail.com', '12345678', GETDATE(), GETDATE()),
(7, 'vo.tuan.minh@gmail.com', '12345678', GETDATE(), GETDATE()),
(8, 'bui.thi.tham@gmail.com', '12345678', GETDATE(), GETDATE()),
(9, 'mai.tuan.tai@gmail.com', '12345678', GETDATE(), GETDATE()),

(10, 'pham.tri.thanh@gmail.com', '12345678', GETDATE(), GETDATE()),
(11, 'le.thi.tham@gmail.com', '12345678', GETDATE(), GETDATE()),
(12, 'hoang.tuan.dung@gmail.com', '12345678', GETDATE(), GETDATE()),
(13, 'nguyen.thi.tam@gmail.com', '12345678', GETDATE(), GETDATE()),
(14, 'vu.tuan.duc4@gmail.com', '12345678', GETDATE(), GETDATE()),
(15, 'bui.thi.tuoi5@gmail.com', '12345678', GETDATE(), GETDATE()),
(16, 'do.tuan.minh6@gmail.com', '12345678', GETDATE(), GETDATE()),
(17, 'phan.thi.lam@gmail.com', '12345678', GETDATE(), GETDATE()),

(18, 'le.tri.khang3@gmail.com', '12345678', GETDATE(), GETDATE()),
(19, 'ly.thi.hong1@gmail.com', '12345678', GETDATE(), GETDATE()),
(20, 'ma.tuan.tai4@gmail.com', '12345678', GETDATE(), GETDATE()),
(21, 'tran.thi.hong22@gmail.com', '12345678', GETDATE(), GETDATE()),
(22, 'dang.tuan.tu23@gmail.com', '12345678', GETDATE(), GETDATE()),
(23, 'pham.thi.chinh5@gmail.com', '12345678', GETDATE(), GETDATE()),
(24, 'hoang.duc.mi3@gmail.com', '12345678', GETDATE(), GETDATE()),
(25, 'mai.thi.lan7@gmail.com', '12345678', GETDATE(), GETDATE()),

(26, 'vu.tri.thinh8@gmail.com', '12345678', GETDATE(), GETDATE()),
(27, 'tran.thi.binh@gmail.com', '12345678', GETDATE(), GETDATE()),
(28, 'pham.tuan.trong43@gmail.com', '12345678', GETDATE(), GETDATE()),
(29, 'nguyen.thi.hoa5@gmail.com', '12345678', GETDATE(), GETDATE()),
(30, 'vu.tuan.chan@gmail.com', '12345678', GETDATE(), GETDATE()),
(31, 'bui.thi.nhan@gmail.com', '12345678', GETDATE(), GETDATE()),
(32, 'do.tuan.duc9@gmail.com', '12345678', GETDATE(), GETDATE()),
(33, 'mai.thi.lam@gmail.com', '12345678', GETDATE(), GETDATE()),

(34, 'ho.tri.thuc5@company.com', '12345678', GETDATE(), GETDATE()),
(35, 'ly.thi.can5@gmail.com', '12345678', GETDATE(), GETDATE()),
(36, 'ma.tri.tai2@gmail.com', '12345678', GETDATE(), GETDATE()),
(37, 'tran.thi.lua3@gmail.com', '12345678', GETDATE(), GETDATE()),
(38, 'dang.khoi.nguyen4@gmail.com', '12345678', GETDATE(), GETDATE()),
(39, 'pham.van.dong5@gmail.com', '12345678', GETDATE(), GETDATE()),
(40, 'hoang.minh.duc6@gmail.com', '12345678', GETDATE(), GETDATE()),
(41, 'mai.hoa.vang7@gmail.com', '12345678', GETDATE(), GETDATE());

--Insert job status:
INSERT INTO JobStatus (Name, Description, CreatedAt, UpdatedAt)
VALUES 
(N'Mở', N'Công việc đang mở và nhận đơn xin việc', GETDATE(), GETDATE()),
(N'Đang tiến hành', N'Công việc đang được thực hiện', GETDATE(), GETDATE()),
(N'Hoàn thành', N'Công việc đã hoàn thành', GETDATE(), GETDATE()),
(N'Tạm dừng', N'Công việc đang tạm dừng', GETDATE(), GETDATE());
Select * From JobStatus

--Table jobs:
INSERT INTO Jobs (Title, Description, AssignedBy, DepartmentID, StartDate , EndDate, JobStatusID, CreatedAt, UpdatedAt)
VALUES 
-- Công nghệ thông tin
(N'Nâng cấp hệ thống mạng', N'Nâng cấp và bảo trì hệ thống mạng công ty - Phòng Công nghệ thông tin',1, 1, '2024-07-01', '2024-09-30', 1, GETDATE(), GETDATE()),
(N'Bảo trì hệ thống phần mềm', N'Bảo trì và nâng cấp các hệ thống phần mềm hiện có - Phòng Công nghệ thông tin',1, 1, '2024-07-10', '2024-09-10', 2, GETDATE(), GETDATE()),
(N'Tái cấu trúc phòng IT', N'Tái cấu trúc tổ chức và quy trình làm việc của phòng IT - Phòng Công nghệ thông tin',1, 1,'2024-07-01', '2024-09-30', 2, GETDATE(), GETDATE()),
(N'Kiểm tra và đánh giá an ninh mạng', N'Thực hiện kiểm tra và đánh giá an ninh mạng công ty - Phòng Công nghệ thông tin',1, 1, '2024-07-01', '2024-08-15', 2, GETDATE(), GETDATE()),
(N'Nâng cấp hệ thống máy chủ', N'Nâng cấp và bảo trì hệ thống máy chủ - Phòng Công nghệ thông tin',1, 1, '2024-07-01', '2024-09-30', 2, GETDATE(), GETDATE()),
(N'Triển khai hệ thống CRM', N'Triển khai hệ thống quản lý quan hệ khách hàng - Phòng Công nghệ thông tin',1, 1, '2024-08-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Phát triển ứng dụng di động', N'Phát triển ứng dụng di động cho khách hàng - Phòng Công nghệ thông tin',1, 1, '2024-06-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Triển khai hệ thống ERP', N'Triển khai hệ thống ERP cho công ty - Phòng Công nghệ thông tin',1, 1, '2024-07-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Tối ưu hóa quy trình làm việc', N'Tối ưu hóa quy trình làm việc trong các phòng ban - Phòng Công nghệ thông tin',1, 1, '2024-07-01', '2024-09-30', 2, GETDATE(), GETDATE()),
(N'Triển khai hệ thống quản lý dự án', N'Triển khai hệ thống quản lý dự án cho công ty - Phòng Công nghệ thông tin',1, 1, '2024-08-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Phát triển hệ thống lưu trữ đám mây', N'Triển khai và phát triển hệ thống lưu trữ đám mây cho công ty - Phòng Công nghệ thông tin',1, 1, '2024-06-15', '2024-12-31', 3, GETDATE(), GETDATE()),
(N'Nâng cấp hệ thống bảo mật', N'Nâng cấp hệ thống bảo mật thông tin - Phòng Công nghệ thông tin',1, 1, '2024-07-01', '2024-09-30', 1, GETDATE(), GETDATE()),
(N'Triển khai phần mềm quản lý tài sản', N'Triển khai phần mềm quản lý tài sản cho công ty - Phòng Công nghệ thông tin',1, 1, '2024-07-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Kiểm tra hệ thống phòng chống virus', N'Thực hiện kiểm tra và nâng cấp hệ thống phòng chống virus - Phòng Công nghệ thông tin',1, 1,  '2024-07-01', '2024-08-15', 2, GETDATE(), GETDATE()),
(N'Triển khai hệ thống quản lý nhân sự', N'Triển khai hệ thống quản lý nhân sự cho công ty - Phòng Công nghệ thông tin',1,1, '2024-06-01', '2024-11-30', 1, GETDATE(), GETDATE()),

-- Nhân sự
(N'Tuyển dụng nhân viên', N'Tuyển dụng nhân viên mới cho phòng nhân sự - Phòng Nhân sự',10, 2, '2024-07-15', '2024-08-15', 2, GETDATE(), GETDATE()),
(N'Đào tạo nhân viên mới', N'Chương trình đào tạo cho nhân viên mới - Phòng Nhân sự',10, 2, '2024-07-05', '2024-07-31', 2, GETDATE(), GETDATE()),
(N'Phát triển chiến lược tuyển dụng', N'Xây dựng chiến lược tuyển dụng dài hạn - Phòng Nhân sự',10, 2,'2024-06-01', '2024-08-31', 1, GETDATE(), GETDATE()),
(N'Tổ chức sự kiện nội bộ', N'Tổ chức sự kiện nội bộ cho nhân viên công ty - Phòng Nhân sự',10, 2, '2024-07-01', '2024-07-15', 3, GETDATE(), GETDATE()),
(N'Đánh giá hiệu suất làm việc', N'Thực hiện đánh giá hiệu suất làm việc của nhân viên - Phòng Nhân sự',10, 2,'2024-07-01', '2024-07-31', 3, GETDATE(), GETDATE()),
(N'Xây dựng chương trình đào tạo', N'Triển khai và xây dựng các chương trình đào tạo mới - Phòng Nhân sự',10, 2,  '2024-07-01', '2024-09-30', 1, GETDATE(), GETDATE()),
(N'Triển khai hệ thống quản lý nhân sự', N'Triển khai hệ thống quản lý nhân sự cho công ty - Phòng Nhân sự',10, 2,  '2024-06-01', '2024-11-30', 1, GETDATE(), GETDATE()),
(N'Đánh giá và phát triển nhân tài', N'Thực hiện đánh giá và phát triển nhân tài trong công ty - Phòng Nhân sự',10, 2, '2024-07-01', '2024-12-31', 3, GETDATE(), GETDATE()),
(N'Triển khai chương trình phúc lợi', N'Triển khai các chương trình phúc lợi cho nhân viên - Phòng Nhân sự',10, 2, '2024-07-01', '2024-09-30', 2, GETDATE(), GETDATE()),
(N'Tuyển dụng thực tập sinh', N'Tuyển dụng thực tập sinh cho các phòng ban - Phòng Nhân sự',10, 2,  '2024-07-15', '2024-08-15', 2, GETDATE(), GETDATE()),
(N'Triển khai hệ thống quản lý hiệu suất', N'Triển khai hệ thống quản lý hiệu suất làm việc của nhân viên - Phòng Nhân sự',10, 2,  '2024-06-01', '2024-11-30', 1, GETDATE(), GETDATE()),
(N'Khảo sát hài lòng nhân viên', N'Thực hiện khảo sát đánh giá độ hài lòng của nhân viên - Phòng Nhân sự',10,  2, '2024-07-01', '2024-07-31', 4, GETDATE(), GETDATE()),
(N'Xây dựng chính sách lương thưởng', N'Triển khai và xây dựng các chính sách lương thưởng mới - Phòng Nhân sự',10,  2, '2024-07-01', '2024-09-30', 1, GETDATE(), GETDATE()),
(N'Triển khai chương trình sức khỏe', N'Triển khai các chương trình chăm sóc sức khỏe cho nhân viên - Phòng Nhân sự',10,  2, '2024-07-01', '2024-12-31', 3, GETDATE(), GETDATE()),
(N'Phát triển văn hóa doanh nghiệp', N'Triển khai các hoạt động phát triển văn hóa doanh nghiệp - Phòng Nhân sự',10,  2, '2024-07-01', '2024-12-31', 3, GETDATE(), GETDATE()),

-- Kế toán
(N'Báo cáo tài chính', N'Lập báo cáo tài chính cuối năm - Phòng Kế toán',18, 3, '2024-10-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Kiểm toán nội bộ', N'Thực hiện kiểm toán nội bộ quý 3 - Phòng Kế toán',18, 3,  '2024-08-01', '2024-08-31', 1, GETDATE(), GETDATE()),
(N'Chiến lược tài chính 2025', N'Lập kế hoạch và chiến lược tài chính cho năm 2025 - Phòng Kế toán',18, 3,  '2024-10-01', '2024-12-31', 4, GETDATE(), GETDATE()),
(N'Phân tích dữ liệu tài chính', N'Phân tích dữ liệu tài chính năm 2024 - Phòng Kế toán',18, 3, '2024-08-01', '2024-09-30', 1, GETDATE(), GETDATE()),
(N'Quản lý tài sản công ty', N'Cải thiện quy trình quản lý tài sản công ty - Phòng Kế toán',18, 3, '2024-07-01', '2024-09-30', 1, GETDATE(), GETDATE()),
(N'Lập kế hoạch ngân sách', N'Lập kế hoạch ngân sách cho năm 2025 - Phòng Kế toán',18, 3, '2024-08-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Thực hiện báo cáo thuế', N'Thực hiện báo cáo thuế quý 3 - Phòng Kế toán',18, 3, '2024-07-01', '2024-07-31', 2, GETDATE(), GETDATE()),
(N'Phân tích chi phí hoạt động', N'Phân tích chi phí hoạt động của công ty - Phòng Kế toán',18, 3, '2024-07-01', '2024-09-30', 1, GETDATE(), GETDATE()),
(N'Kiểm tra và đối chiếu sổ sách', N'Thực hiện kiểm tra và đối chiếu sổ sách kế toán - Phòng Kế toán',18, 3, '2024-07-01', '2024-08-31', 2, GETDATE(), GETDATE()),
(N'Triển khai phần mềm kế toán', N'Triển khai phần mềm kế toán mới cho công ty - Phòng Kế toán',18, 3, '2024-07-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Lập báo cáo tài chính hàng tháng', N'Lập báo cáo tài chính hàng tháng - Phòng Kế toán',18, 3, '2024-07-01', '2024-07-31', 1, GETDATE(), GETDATE()),
(N'Phân tích dòng tiền', N'Phân tích dòng tiền của công ty - Phòng Kế toán',18, 3,  '2024-07-01', '2024-08-31', 1, GETDATE(), GETDATE()),
(N'Quản lý chi phí dự án', N'Quản lý và kiểm soát chi phí các dự án - Phòng Kế toán',18, 3, '2024-07-01', '2024-09-30', 1, GETDATE(), GETDATE()),
(N'Triển khai hệ thống báo cáo tài chính', N'Triển khai hệ thống báo cáo tài chính tự động - Phòng Kế toán',18, 3, '2024-07-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Phân tích tỷ suất lợi nhuận', N'Phân tích tỷ suất lợi nhuận của công ty - Phòng Kế toán', 18, 3, '2024-07-01', '2024-09-30', 1, GETDATE(), GETDATE()),

-- Marketing
(N'Chiến dịch marketing', N'Thực hiện chiến dịch marketing mùa hè - Phòng Marketing',26, 4, '2024-06-01', '2024-08-31', 3, GETDATE(), GETDATE()),
(N'Chiến dịch email marketing', N'Thực hiện chiến dịch email marketing cho khách hàng - Phòng Marketing',26, 4,  '2024-06-15', '2024-07-31', 4, GETDATE(), GETDATE()),
(N'Khảo sát thị trường', N'Thực hiện khảo sát thị trường để phát triển sản phẩm mới - Phòng Marketing',26, 4, '2024-07-01', '2024-08-31', 4, GETDATE(), GETDATE()),
(N'Chiến dịch quảng cáo trực tuyến', N'Thực hiện chiến dịch quảng cáo trên các nền tảng trực tuyến - Phòng Marketing',26, 4, '2024-06-01', '2024-08-31', 4, GETDATE(), GETDATE()),
(N'Chiến dịch quảng bá sản phẩm mới', N'Thực hiện chiến dịch quảng bá sản phẩm mới trên thị trường - Phòng Marketing',26, 4, '2024-06-01', '2024-08-31', 1, GETDATE(), GETDATE()),
(N'Triển khai hệ thống CRM', N'Triển khai hệ thống quản lý quan hệ khách hàng - Phòng Marketing',26, 4, '2024-08-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Phát triển chiến lược tiếp thị', N'Lập kế hoạch và phát triển chiến lược tiếp thị mới - Phòng Marketing',26,  4, '2024-07-01', '2024-09-30', 3, GETDATE(), GETDATE()),
(N'Triển khai chiến dịch SEO', N'Triển khai chiến dịch tối ưu hóa công cụ tìm kiếm - Phòng Marketing',26, 4,  '2024-07-01', '2024-09-30', 4, GETDATE(), GETDATE()),
(N'Phân tích dữ liệu khách hàng', N'Phân tích dữ liệu khách hàng để cải thiện dịch vụ - Phòng Marketing',26, 4, '2024-08-01', '2024-09-30', 3, GETDATE(), GETDATE()),
(N'Phát triển kênh bán hàng online', N'Phát triển và tối ưu hóa kênh bán hàng online - Phòng Marketing',26, 4, '2024-06-01', '2024-12-31', 4, GETDATE(), GETDATE()),
(N'Tối ưu hóa chiến lược nội dung', N'Tối ưu hóa chiến lược nội dung cho các kênh truyền thông - Phòng Marketing',26, 4, '2024-07-01', '2024-09-30', 4, GETDATE(), GETDATE()),
(N'Quản lý quan hệ công chúng', N'Triển khai chiến dịch quản lý quan hệ công chúng - Phòng Marketing',26, 4, '2024-07-01', '2024-12-31', 3, GETDATE(), GETDATE()),
(N'Triển khai hệ thống phân tích dữ liệu', N'Triển khai hệ thống phân tích dữ liệu khách hàng - Phòng Marketing',26,  4, '2024-07-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Phát triển chiến lược quảng cáo', N'Lập kế hoạch và phát triển chiến lược quảng cáo mới - Phòng Marketing',26, 4,  '2024-07-01', '2024-09-30', 3, GETDATE(), GETDATE()),
(N'Triển khai hệ thống quản lý chiến dịch', N'Triển khai hệ thống quản lý chiến dịch tiếp thị - Phòng Marketing', 26, 4, '2024-07-01', '2024-12-31', 1, GETDATE(), GETDATE()),

-- Bán hàng
(N'Kế hoạch bán hàng', N'Lập kế hoạch bán hàng cho quý 3 - Phòng Bán hàng',34, 5, '2024-07-01', '2024-09-30', 4, GETDATE(), GETDATE()),
(N'Phân tích dữ liệu bán hàng', N'Phân tích dữ liệu bán hàng quý 2 - Phòng Bán hàng', 34, 5,'2024-07-15', '2024-08-15', 3, GETDATE(), GETDATE()),
(N'Khảo sát khách hàng', N'Khảo sát độ hài lòng của khách hàng - Phòng Bán hàng',34, 5,'2024-07-01', '2024-07-31', 4, GETDATE(), GETDATE()),
(N'Phát triển kênh bán hàng online', N'Phát triển và tối ưu hóa kênh bán hàng online - Phòng Bán hàng',34, 5, '2024-06-01', '2024-12-31', 4, GETDATE(), GETDATE()),
(N'Phân tích dữ liệu khách hàng', N'Phân tích dữ liệu khách hàng để cải thiện dịch vụ - Phòng Bán hàng',34, 5, '2024-08-01', '2024-09-30', 3, GETDATE(), GETDATE()),
(N'Triển khai chương trình khuyến mãi', N'Triển khai các chương trình khuyến mãi cho khách hàng - Phòng Bán hàng',34, 5, '2024-07-01', '2024-08-31', 4, GETDATE(), GETDATE()),
(N'Tăng cường dịch vụ khách hàng', N'Cải thiện và nâng cao chất lượng dịch vụ khách hàng - Phòng Bán hàng',34, 5, '2024-07-01', '2024-09-30', 3, GETDATE(), GETDATE()),
(N'Triển khai hệ thống quản lý khách hàng', N'Triển khai hệ thống quản lý khách hàng mới - Phòng Bán hàng',34, 5,'2024-06-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Phân tích doanh số bán hàng', N'Phân tích doanh số bán hàng hàng tháng - Phòng Bán hàng',34, 5, '2024-07-01', '2024-07-31', 1, GETDATE(), GETDATE()),
(N'Triển khai chiến dịch bán hàng', N'Triển khai chiến dịch bán hàng mùa hè - Phòng Bán hàng',34, 5, '2024-06-01', '2024-08-31', 4, GETDATE(), GETDATE()),
(N'Phát triển chiến lược bán hàng', N'Lập kế hoạch và phát triển chiến lược bán hàng mới - Phòng Bán hàng', 34, 5,'2024-07-01', '2024-09-30', 3, GETDATE(), GETDATE()),
(N'Tối ưu hóa quy trình bán hàng', N'Tối ưu hóa quy trình bán hàng trong các phòng ban - Phòng Bán hàng',34, 5, '2024-07-01', '2024-09-30', 4, GETDATE(), GETDATE()),
(N'Triển khai hệ thống quản lý đơn hàng', N'Triển khai hệ thống quản lý đơn hàng mới - Phòng Bán hàng',34, 5, '2024-07-01', '2024-12-31', 1, GETDATE(), GETDATE()),
(N'Triển khai chương trình chăm sóc khách hàng', N'Triển khai các chương trình chăm sóc khách hàng đặc biệt - Phòng Bán hàng',34,  5,'2024-07-01', '2024-12-31', 3, GETDATE(), GETDATE()),
(N'Triển khai hệ thống quản lý bán hàng', N'Triển khai hệ thống quản lý bán hàng tự động - Phòng Bán hàng', 34, 5,'2024-07-01', '2024-12-31', 1, GETDATE(), GETDATE());

Select * From Jobs

--Insert jobs for IT: 9 nhân sự: -> 9
INSERT INTO EmployeeJobs (EmployeeID, JobID, AssignmentDate, CreatedAt, UpdatedAt)
VALUES 
(1, 1, '2024-07-01', GETDATE(), GETDATE()),
(2, 2, '2024-07-10', GETDATE(), GETDATE()),
(3, 3, '2024-07-01', GETDATE(), GETDATE()),
(4, 4, '2024-07-01', GETDATE(), GETDATE()),
(5, 5, '2024-07-01', GETDATE(), GETDATE()),
(6, 6, '2024-08-01', GETDATE(), GETDATE()),
(7, 7, '2024-06-01', GETDATE(), GETDATE()),
(8, 8, '2024-07-01', GETDATE(), GETDATE()),
(9, 9, '2024-07-01', GETDATE(), GETDATE()),
(1, 10, '2024-07-01', GETDATE(), GETDATE()),
(2, 11, '2024-08-01', GETDATE(), GETDATE()),
(3, 12, '2024-06-01', GETDATE(), GETDATE()),
(4, 14, '2024-07-01', GETDATE(), GETDATE()),
(5, 13, '2024-07-01', GETDATE(), GETDATE()),
(6, 14, '2024-07-01', GETDATE(), GETDATE()),
(7, 15, '2024-07-01', GETDATE(), GETDATE());

--Insert job for Nhân sự: 8 nhân sự: 10 -> 17
INSERT INTO EmployeeJobs (EmployeeID, JobID, AssignmentDate, CreatedAt, UpdatedAt)
VALUES 
(10, 16, '2024-07-01', GETDATE(), GETDATE()),
(11, 17, '2024-07-10', GETDATE(), GETDATE()),
(12, 18, '2024-07-01', GETDATE(), GETDATE()),
(13, 19, '2024-07-01', GETDATE(), GETDATE()),
(14, 20, '2024-07-01', GETDATE(), GETDATE()),
(15, 21, '2024-08-01', GETDATE(), GETDATE()),
(16, 22, '2024-06-01', GETDATE(), GETDATE()),
(17, 23, '2024-07-01', GETDATE(), GETDATE()),
(10, 24, '2024-07-01', GETDATE(), GETDATE()),
(11, 25, '2024-08-01', GETDATE(), GETDATE()),
(12, 26, '2024-06-01', GETDATE(), GETDATE()),
(13, 27, '2024-07-01', GETDATE(), GETDATE()),
(14, 28, '2024-07-01', GETDATE(), GETDATE()),
(15, 29, '2024-07-01', GETDATE(), GETDATE()),
(16, 30, '2024-07-01', GETDATE(), GETDATE());

--Insert job for Kế toán: 8 nhân sự 18 -> 25
INSERT INTO EmployeeJobs (EmployeeID, JobID, AssignmentDate, CreatedAt, UpdatedAt)
VALUES 
(18, 31, '2024-07-01', GETDATE(), GETDATE()),
(19,32, '2024-07-10', GETDATE(), GETDATE()),
(20, 33, '2024-07-01', GETDATE(), GETDATE()),
(21, 34, '2024-07-01', GETDATE(), GETDATE()),
(22, 35, '2024-07-01', GETDATE(), GETDATE()),
(23, 36, '2024-08-01', GETDATE(), GETDATE()),
(24, 37, '2024-06-01', GETDATE(), GETDATE()),
(25, 38, '2024-07-01', GETDATE(), GETDATE()),
(18, 39, '2024-07-01', GETDATE(), GETDATE()),
(19, 40, '2024-08-01', GETDATE(), GETDATE()),
(20, 41, '2024-06-01', GETDATE(), GETDATE()),
(21, 42, '2024-07-01', GETDATE(), GETDATE()),
(22, 43, '2024-07-01', GETDATE(), GETDATE()),
(23, 44, '2024-07-01', GETDATE(), GETDATE()),
(24, 45, '2024-07-01', GETDATE(), GETDATE());

--Insert job for Marketing: 8 nhân sự 26 -> 33
INSERT INTO EmployeeJobs (EmployeeID, JobID, AssignmentDate, CreatedAt, UpdatedAt)
VALUES 
(26, 46, '2024-07-01', GETDATE(), GETDATE()),
(27, 47, '2024-07-10', GETDATE(), GETDATE()),
(28, 48, '2024-07-01', GETDATE(), GETDATE()),
(29, 49, '2024-07-01', GETDATE(), GETDATE()),
(30, 50, '2024-07-01', GETDATE(), GETDATE()),
(31, 51, '2024-08-01', GETDATE(), GETDATE()),
(32, 52, '2024-06-01', GETDATE(), GETDATE()),
(33, 53, '2024-07-01', GETDATE(), GETDATE()),
(26, 54, '2024-07-01', GETDATE(), GETDATE()),
(27, 55, '2024-08-01', GETDATE(), GETDATE()),
(28, 56, '2024-06-01', GETDATE(), GETDATE()),
(29, 57, '2024-07-01', GETDATE(), GETDATE()),
(30, 58, '2024-07-01', GETDATE(), GETDATE()),
(31, 59, '2024-07-01', GETDATE(), GETDATE()),
(32, 60, '2024-07-01', GETDATE(), GETDATE());

--Insert job for Bán hàng: 8 nhân sự 34 -> 41
INSERT INTO EmployeeJobs (EmployeeID, JobID, AssignmentDate, CreatedAt, UpdatedAt)
VALUES 
(34, 61, '2024-07-01', GETDATE(), GETDATE()),
(35, 62, '2024-07-10', GETDATE(), GETDATE()),
(36, 63, '2024-07-01', GETDATE(), GETDATE()),
(37, 64, '2024-07-01', GETDATE(), GETDATE()),
(38, 65, '2024-07-01', GETDATE(), GETDATE()),
(39, 66, '2024-08-01', GETDATE(), GETDATE()),
(40, 67, '2024-06-01', GETDATE(), GETDATE()),
(41, 68, '2024-07-01', GETDATE(), GETDATE()),
(34, 69, '2024-07-01', GETDATE(), GETDATE()),
(35, 70, '2024-08-01', GETDATE(), GETDATE()),
(36, 71, '2024-06-01', GETDATE(), GETDATE()),
(37, 72, '2024-07-01', GETDATE(), GETDATE()),
(38, 73, '2024-07-01', GETDATE(), GETDATE()),
(39, 74, '2024-07-01', GETDATE(), GETDATE()),
(40, 75, '2024-07-01', GETDATE(), GETDATE());

Select * From EmployeeJobs
Select * 
From Employees e 
INNER JOIN EmployeeJobs j on e.ID = j.EmployeeID
INNER JOIN Jobs s on s.ID = j.JobID
WHERE e.ID = 1



--Insert admin:
INSERT INTO Employees (FirstName, LastName, Email, Phone, Salary, RoleID, DepartmentID, ManagerID, PositionID, DateOfBirth, Gender, Address, CreatedAt, UpdatedAt)
VALUES 
(N'Hoàng', N'Tiến Đạt', 'daththe173228@fpt.edu.vn', '0981153101', 50000000.00, 1, null, null, null, '2003-09-20', 1, N'Tiên Sơn, Duy Tiên, Hà Nam', GETDATE(), GETDATE());
INSERT INTO Authentication (EmployeeID, Username, PassWord, CreatedAt, UpdatedAt)
VALUES 
(42, 'c', '1', GETDATE(), GETDATE());

