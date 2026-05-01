-- =============================================
-- 人事管理系統 - SQL Server 資料庫結構
-- =============================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'HRSystemDB')
    CREATE DATABASE HRSystemDB;
GO

USE HRSystemDB;
GO

-- =============================================
-- 部門表
-- =============================================
CREATE TABLE Departments (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(100) NOT NULL,
    Code        NVARCHAR(20)  NOT NULL UNIQUE,
    Description NVARCHAR(500),
    ManagerId   INT,
    CreatedAt   DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt   DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsActive    BIT NOT NULL DEFAULT 1
);

-- =============================================
-- 職位表
-- =============================================
CREATE TABLE Positions (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Title       NVARCHAR(100) NOT NULL,
    Code        NVARCHAR(20)  NOT NULL UNIQUE,
    Level       INT NOT NULL DEFAULT 1,
    MinSalary   DECIMAL(18,2),
    MaxSalary   DECIMAL(18,2),
    Description NVARCHAR(500),
    CreatedAt   DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt   DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsActive    BIT NOT NULL DEFAULT 1
);

-- =============================================
-- 員工表
-- =============================================
CREATE TABLE Employees (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeNo     NVARCHAR(20)  NOT NULL UNIQUE,
    FirstName      NVARCHAR(50)  NOT NULL,
    LastName       NVARCHAR(50)  NOT NULL,
    Gender         TINYINT NOT NULL DEFAULT 0, -- 0=未設定, 1=男, 2=女
    BirthDate      DATE,
    IdCardNo       NVARCHAR(20),
    Email          NVARCHAR(150) NOT NULL UNIQUE,
    Phone          NVARCHAR(20),
    Address        NVARCHAR(300),
    Photo          NVARCHAR(500),
    DepartmentId   INT NOT NULL,
    PositionId     INT NOT NULL,
    ManagerId      INT,
    HireDate       DATE NOT NULL,
    ResignDate     DATE,
    EmploymentType TINYINT NOT NULL DEFAULT 1, -- 1=正職, 2=約聘, 3=兼職
    Status         TINYINT NOT NULL DEFAULT 1, -- 1=在職, 2=留職停薪, 3=離職
    BaseSalary     DECIMAL(18,2) NOT NULL DEFAULT 0,
    BankAccount    NVARCHAR(50),
    EmergencyName  NVARCHAR(50),
    EmergencyPhone NVARCHAR(20),
    Remarks        NVARCHAR(1000),
    CreatedAt      DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt      DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Employees_Department FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),
    CONSTRAINT FK_Employees_Position   FOREIGN KEY (PositionId)   REFERENCES Positions(Id),
    CONSTRAINT FK_Employees_Manager    FOREIGN KEY (ManagerId)    REFERENCES Employees(Id)
);

-- 更新部門主管外鍵
ALTER TABLE Departments
    ADD CONSTRAINT FK_Departments_Manager FOREIGN KEY (ManagerId) REFERENCES Employees(Id);

-- =============================================
-- 考勤記錄表
-- =============================================
CREATE TABLE Attendances (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId   INT NOT NULL,
    AttendDate   DATE NOT NULL,
    CheckIn      TIME,
    CheckOut     TIME,
    WorkHours    DECIMAL(5,2),
    Status       TINYINT NOT NULL DEFAULT 1, -- 1=正常, 2=遲到, 3=早退, 4=缺勤, 5=公假, 6=事假, 7=病假
    Remarks      NVARCHAR(300),
    CreatedAt    DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Attendances_Employee FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    CONSTRAINT UQ_Attendance UNIQUE (EmployeeId, AttendDate)
);

-- =============================================
-- 請假申請表
-- =============================================
CREATE TABLE LeaveRequests (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId  INT NOT NULL,
    LeaveType   TINYINT NOT NULL, -- 1=年假, 2=事假, 3=病假, 4=婚假, 5=喪假, 6=產假, 7=陪產假
    StartDate   DATE NOT NULL,
    EndDate     DATE NOT NULL,
    Days        DECIMAL(5,1) NOT NULL,
    Reason      NVARCHAR(500),
    Status      TINYINT NOT NULL DEFAULT 0, -- 0=待審, 1=核准, 2=拒絕, 3=撤銷
    ApproverId  INT,
    ApprovedAt  DATETIME2,
    ApproveNote NVARCHAR(300),
    CreatedAt   DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt   DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_LeaveRequests_Employee FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    CONSTRAINT FK_LeaveRequests_Approver FOREIGN KEY (ApproverId) REFERENCES Employees(Id)
);

-- =============================================
-- 薪資記錄表
-- =============================================
CREATE TABLE Payrolls (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId    INT NOT NULL,
    PayYear       INT NOT NULL,
    PayMonth      INT NOT NULL,
    BaseSalary    DECIMAL(18,2) NOT NULL DEFAULT 0,
    Bonus         DECIMAL(18,2) NOT NULL DEFAULT 0,
    Allowance     DECIMAL(18,2) NOT NULL DEFAULT 0,
    Overtime      DECIMAL(18,2) NOT NULL DEFAULT 0,
    Deduction     DECIMAL(18,2) NOT NULL DEFAULT 0,
    Insurance     DECIMAL(18,2) NOT NULL DEFAULT 0,
    Tax           DECIMAL(18,2) NOT NULL DEFAULT 0,
    NetSalary     DECIMAL(18,2) NOT NULL DEFAULT 0,
    Status        TINYINT NOT NULL DEFAULT 0, -- 0=草稿, 1=已發放
    PaidAt        DATETIME2,
    Remarks       NVARCHAR(500),
    CreatedAt     DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt     DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Payrolls_Employee FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    CONSTRAINT UQ_Payroll UNIQUE (EmployeeId, PayYear, PayMonth)
);

-- =============================================
-- 系統使用者表
-- =============================================
CREATE TABLE Users (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Username     NVARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    EmployeeId   INT,
    Role         TINYINT NOT NULL DEFAULT 1, -- 1=一般, 2=HR, 3=主管, 4=管理員
    IsActive     BIT NOT NULL DEFAULT 1,
    LastLoginAt  DATETIME2,
    CreatedAt    DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Users_Employee FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);

-- =============================================
-- 初始資料
-- =============================================
INSERT INTO Departments (Name, Code, Description) VALUES
    (N'總經理室',   'CEO',  N'總經理直屬單位'),
    (N'人力資源部', 'HR',   N'人才招募與員工管理'),
    (N'資訊技術部', 'IT',   N'系統開發與維護'),
    (N'財務部',     'FIN',  N'財務管理與會計'),
    (N'業務部',     'SALES',N'業務開發與客戶管理'),
    (N'行政部',     'ADMIN',N'行政庶務管理');

INSERT INTO Positions (Title, Code, Level, MinSalary, MaxSalary) VALUES
    (N'總經理',     'CEO',    10, 200000, 500000),
    (N'副總經理',   'VP',      9, 150000, 300000),
    (N'部門主管',   'MGR',     7,  80000, 150000),
    (N'資深工程師', 'SR_ENG',  5,  60000,  90000),
    (N'工程師',     'ENG',     3,  40000,  65000),
    (N'初級工程師', 'JR_ENG',  2,  30000,  45000),
    (N'業務主管',   'SALES_M', 6,  70000, 120000),
    (N'業務專員',   'SALES_R', 3,  35000,  55000),
    (N'HR主管',     'HR_M',    6,  65000, 100000),
    (N'HR專員',     'HR_R',    3,  35000,  55000),
    (N'財務主管',   'FIN_M',   6,  70000, 110000),
    (N'財務專員',   'FIN_R',   3,  38000,  58000),
    (N'行政助理',   'ADMIN_R', 2,  28000,  40000);

-- 初始管理員帳號（密碼: Admin@1234, 需替換為實際 bcrypt hash）
INSERT INTO Users (Username, PasswordHash, Role) VALUES
    ('admin', '$2a$11$placeholder_hash_replace_in_production', 4);

GO

-- =============================================
-- 索引
-- =============================================
CREATE INDEX IX_Employees_Department ON Employees(DepartmentId);
CREATE INDEX IX_Employees_Position   ON Employees(PositionId);
CREATE INDEX IX_Employees_Status     ON Employees(Status);
CREATE INDEX IX_Attendances_Date     ON Attendances(EmployeeId, AttendDate);
CREATE INDEX IX_LeaveRequests_Status ON LeaveRequests(EmployeeId, Status);
CREATE INDEX IX_Payrolls_Period      ON Payrolls(PayYear, PayMonth);

GO
