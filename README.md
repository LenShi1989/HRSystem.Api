# 人事管理系統 (HR Management System)

## 技術架構

| 層次   | 技術                                        |
| ------ | ------------------------------------------- |
| 前端   | Vue 3 + TypeScript + Element Plus + ECharts |
| 後端   | ASP.NET Core 8 Web API + EF Core 8          |
| 資料庫 | SQL Server 2019+                            |
| 認證   | JWT Bearer Token                            |

---

## 專案結構

```
HRSystem/
├── database/
│   └── schema.sql              # 資料庫建置腳本
├── backend/
│   └── HRSystem.Api/
│       ├── Controllers/        # API 控制器
│       ├── Data/               # EF Core DbContext
│       ├── Models/
│       │   ├── Entities/       # 資料庫實體
│       │   └── DTOs/           # 資料傳輸物件 + AutoMapper
│       ├── Program.cs          # 應用程式進入點
│       └── appsettings.json    # 設定檔
└── frontend/
    └── src/
        ├── assets/             # 全域樣式
        ├── layouts/            # 主版面配置
        ├── router/             # Vue Router
        ├── services/           # API 服務層 (axios)
        ├── stores/             # Pinia 狀態管理
        └── views/              # 頁面元件
```

---

## 功能模組

- **儀表板** — 統計卡片、部門人數圖表、請假狀態圖
- **員工管理** — 新增/編輯/離職、搜尋篩選、分頁列表
- **部門管理** — 部門 CRUD、部門主管設定
- **職位管理** — 職位等級、薪資範圍設定
- **考勤管理** — 打卡記錄、上下班時間、工時計算
- **請假管理** — 申請假單、主管審核核准/拒絕
- **薪資管理** — 薪資單建立、各項加扣款計算、發放確認

---

## 快速開始

### 1. 資料庫

```sql
-- 在 SQL Server 執行
sqlcmd -S localhost -i database/schema.sql
```

### 2. 後端 (.NET 8)

```bash
cd backend/HRSystem.Api

# 修改連線字串 appsettings.json
# "DefaultConnection": "Server=YOUR_SERVER;Database=HRSystemDB;..."

# 還原套件並執行
dotnet restore
dotnet run
# API 運行在 http://localhost:5000
# Swagger UI: http://localhost:5000/swagger
```

### 3. 前端 (Node.js 18+)

```bash
cd frontend
npm install
npm run dev
# 開啟 http://localhost:5173
```

---

## 帳號設定

資料庫建立後需設定管理員密碼（使用 BCrypt）：

```csharp
// 在程式中產生 hash：
string hash = BCrypt.Net.BCrypt.HashPassword("Admin@1234");
// 更新 Users 資料表：
UPDATE Users SET PasswordHash = '<generated_hash>' WHERE Username = 'admin';
```

---

## API 端點一覽

| 方法  | 路徑                            | 說明                 |
| ----- | ------------------------------- | -------------------- |
| POST  | /api/auth/login                 | 登入取得 JWT         |
| GET   | /api/employees                  | 取得員工列表（分頁） |
| POST  | /api/employees                  | 新增員工             |
| PUT   | /api/employees/{id}             | 更新員工             |
| GET   | /api/employees/stats            | 員工統計             |
| GET   | /api/departments                | 取得部門列表         |
| POST  | /api/departments                | 新增部門             |
| GET   | /api/positions                  | 取得職位列表         |
| GET   | /api/attendances                | 考勤記錄             |
| POST  | /api/attendances                | 新增/更新考勤        |
| GET   | /api/leaverequests              | 請假申請列表         |
| POST  | /api/leaverequests              | 新增請假申請         |
| PATCH | /api/leaverequests/{id}/approve | 審核假單             |
| GET   | /api/payrolls                   | 薪資記錄             |
| POST  | /api/payrolls                   | 新增薪資單           |
| PATCH | /api/payrolls/{id}/pay          | 標記已發放           |

---

## EF Core Migration 指令

```bash
cd backend/HRSystem.Api
dotnet ef migrations add InitialCreate
dotnet ef database update
```
