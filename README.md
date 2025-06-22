### Backend (.NET Core)
- **.NET 8.0** hoặc cao hơn
- **MY SQL**
- **Entity Framework Core**
- **Visual Studio 2022** hoặc **VS Code**

### Frontend (React)
- **Node.js 18.0** hoặc cao hơn
- **npm** hoặc **yarn**

## 🚀 Cài Đặt

### 1. Clone Repository
```bash
git clone <repository-url>
cd treasure_hunt
```

### 2. Cài Đặt Backend

#### Bước 1: Cấu hình Database
Mở file `backend/appsettings.json` và cấu hình connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TreasureHuntDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

#### Bước 2: Tạo Database và Migration
```bash
cd backend

# Tạo migration đầu tiên
dotnet ef migrations add InitialCreate

# Cập nhật database
dotnet ef database update
```

#### Bước 3: Chạy Backend
```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Chạy ứng dụng
dotnet run
```

Backend sẽ chạy tại: `http://localhost:5000`

### 3. Cài Đặt Frontend

#### Bước 1: Cài đặt Dependencies
```bash
cd frontend

# Cài đặt dependencies
npm install
```

#### Bước 2: Cấu hình Environment Variables
Tạo file `.env` trong thư mục `frontend`:

```env
REACT_APP_API_URL=http://localhost:5000
```

#### Bước 3: Chạy Frontend
```bash
npm start
```

Frontend sẽ chạy tại: `http://localhost:3000`

## 🎮 Hướng Dẫn Sử Dụng

### 1. Giao Diện Chính

Khi mở ứng dụng tại `http://localhost:3000`, bạn sẽ thấy:

- **Tiêu đề**: "Tìm kho báu"
- **Form nhập liệu**: Số hàng, số cột, dương kho báu
- **Bản đồ kho báu**: Grid để nhập các giá trị
- **Nút "Tìm kho báu"**: Để tính toán đường đi
- **Nút "Xem lịch sử"**: Để xem các lần tìm kiếm trước đó

### 2. Cách Sử Dụng

#### Bước 1: Nhập Thông Tin Cơ Bản
1. Nhập **Số Hàng** (ví dụ: 3)
2. Nhập **Số Cột** (ví dụ: 3)  
3. Nhập **Dương Kho Báu** (ví dụ: 3)

#### Bước 2: Tạo Bản Đồ Kho Báu
- Sau khi nhập thông tin, grid bản đồ sẽ xuất hiện
- Nhập các giá trị vào từng ô theo quy tắc:
  - Giá trị phải từ 1 đến số dương kho báu
  - Chỉ có duy nhất 1 ô chứa giá trị bằng dương kho báu
  - Ví dụ với dương kho báu = 3:
    ```
    1 2 3
    4 5 6
    7 8 9
    ```

#### Bước 3: Tính Toán Đường Đi
- Click nút **"Tìm kho báu"**
- Hệ thống sẽ tính toán đường đi tối ưu
- Kết quả hiển thị:
  - Tổng năng lượng cần thiết
  - Các vị trí cần đi qua theo thứ tự

### 3. Tính Năng Lịch Sử

#### Xem Lịch Sử
- Click nút **"Xem lịch sử"**
- Dialog hiển thị danh sách các lần tìm kiếm trước đó
- Thông tin hiển thị:
  - ID
  - Số hàng, số cột
  - Số kho báu
  - Tổng năng lượng
  - Ngày tạo

#### Sử Dụng Lịch Sử
- Click vào bất kỳ hàng nào trong bảng lịch sử
- Hệ thống sẽ tự động:
  - Điền thông tin vào form
  - Tạo lại bản đồ kho báu
  - Hiển thị kết quả đường đi
  - Đóng dialog lịch sử

## 🔧 API Documentation

### Base URL
```
http://localhost:5000/api/TreasuareHunt
```

### 1. Tìm Kho Báu
**POST** `/api/TreasuareHunt`Server** hoặc **SQLite**

**Request Body:**
```json
{
  "row": 3,
  "col": 3,
  "treasure": 3,
  "treasure_Map": [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
  ]
}
```

**Response:**
```json
{
  "message": "Treasure hunt request validated successfully",
  "solution": {
    "nodes": [
      {
        "row": 0,
        "col": 0,
        "step": 0,
        "key": 1
      }
    ],
    "totalEnergyConsumed": 2.0
  },
  "databaseSave": {
    "success": true,Server** hoặc **SQLite**
{
  "message": "History retrieved successfully",
  "data": [
    {
      "id": 1,
      "row": 3,
      "col": 3,
      "treasure": 3,
      "treasure_Map": [[1,2,3],[4,5,6],[7,8,9]],
      "minimum_Path": 2.0,
      "track_Path": [
        {
          "row": 0,
          "col": 0,
          "step": 0,
          "key": 1Server** hoặc **SQLite**
}
```

### 3. Lấy Lịch Sử Theo ID
**GET** `/api/TreasuareHunt/history/{id}`

## 📁 Cấu Trúc Project

```
treasure_hunt/
├── backend/
│   ├── Controllers/
│   │   └── TreasuareHuntController.cs    # API endpoints
│   │   └── TreasureHunt.cs               # Request/Response models
│   │   └── TreasureHuntService.cs        # Business logic
│   │   └── HistoryTreasure.cs        # Database model
│   │   └── Migrations/                       # Database migrations
│   │   └── Program.cs                        # Application entry point
│   │   └── appsettings.json                  # Configuration
│   └── frontend/
│       ├── src/
│       │   ├── components/
│       │   │   ├── Toast.js                  # Toast notification
│       │   │   └── useToast.js               # Toast hook
│       │   │   └── App.js                        # Main application
│       │   │   └── appStyle.css                  # Styles
│       │   ├── package.json                      # Dependencies
│       │   └── .env                              # Environment variables
```

## 🐛 Xử Lý Lỗi Thường Gặp

### Backend
1. **Lỗi Connection String**
   - Kiểm tra connection string trong `appsettings.json`
   - Đảm bảo SQL Server đang chạy

2. **Lỗi Migration**
   ```bash
   # Xóa thư mục Migrations/ và tạo lại
   rm -rf Migrations/
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

3. **Lỗi Port**
   - Kiểm tra port 5000 có đang được sử dụng không
   - Thay đổi port trong `launchSettings.json`

### Frontend
1. **Lỗi API Connection**
   - Kiểm tra backend có đang chạy không
   - Kiểm tra `REACT_APP_API_URL` trong file `.env`

2. **Lỗi CORS**
   - Đảm bảo backend đã cấu hình CORS
   - Kiểm tra URL trong environment variables

## 🚀 Deploy

### Backend (Production)
```bash
cd backend
dotnet publish -c Release
```

### Frontend (Production)
```bash
cd frontend
npm run build
```

## 🤝 Đóng Góp

1. Fork project
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Mở Pull Request

## 📄 License

Project này được phân phối dưới giấy phép MIT. Xem file `LICENSE` để biết thêm chi tiết.

## 📞 Hỗ Trợ

Nếu gặp vấn đề, hãy kiểm tra:
1. Console của browser (F12)
2. Terminal chạy backend
3. Logs trong Visual Studio/VS Code
4. Database connection

---

**Lưu ý**: Hệ thống này sử dụng thuật toán tìm đường đi tối ưu để thu thập kho báu với năng lượng tiêu thụ ít nhất. Mỗi lần tìm kiếm sẽ được lưu vào database để có thể xem lại sau này.