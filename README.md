# Notes Application

A full-stack note-taking application built with Vue.js (TypeScript + Tailwind CSS) frontend and .NET Core Web API backend with SQL Server database.

## Features

### Frontend (Vue.js + TypeScript + Tailwind CSS)
- ✅ Create, Read, Update, Delete (CRUD) operations for notes
- ✅ Search functionality across note titles and content
- ✅ Sorting by title, creation date, or last updated date
- ✅ Responsive design with Tailwind CSS
- ✅ State management with Pinia
- ✅ TypeScript for type safety
- ✅ Modern UI with modal dialogs

### Backend (.NET Core Web API)
- ✅ RESTful API with Swagger documentation
- ✅ CRUD operations for notes
- ✅ User-specific note access (demo mode with hardcoded user)
- ✅ Dapper ORM for efficient database operations
- ✅ SQL Server database
- ✅ CORS configuration for frontend communication

### Database
- ✅ SQL Server with Notes table
- ✅ Automatic timestamps for created/updated dates
- ✅ User isolation (ready for authentication)

## Tech Stack

- **Frontend**: Vue.js 3, TypeScript, Tailwind CSS, Pinia, Axios, Vite
- **Backend**: .NET 8, ASP.NET Core Web API, Dapper, SQL Server
- **Database**: SQL Server (LocalDB for development)

## Prerequisites

- Node.js 16+ and npm
- .NET 8 SDK
- Docker and Docker Compose
- SQL Server (via Docker container)

## Quick Start

### Automatic Setup (Recommended)

```bash
# Run the setup script (handles everything for you)
chmod +x setup.sh
./setup.sh
```

### Manual Setup

### 1. Database Setup with Docker

```bash
# Start SQL Server container
docker-compose up -d sqlserver

# Wait for SQL Server to be ready (about 10-15 seconds)

# Initialize database
docker exec -i notes-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -i /dev/stdin < Web_Project/Scripts/init-database.sql
```

### 2. Backend Setup

```bash
# Navigate to backend directory
cd Web_Project

# Restore packages
dotnet restore

# Run the API
dotnet run
```

The API will start at `https://localhost:7175` with Swagger documentation at `https://localhost:7175/swagger`.

### 3. Frontend Setup

```bash
# Navigate to frontend directory
cd note-app

# Install dependencies
npm install

# Start development server
npm run dev
```

The frontend will start at `http://localhost:5173`.

## Docker Commands

```bash
# Start SQL Server
docker-compose up -d sqlserver

# Stop all services
docker-compose down

# View SQL Server logs
docker logs notes-sqlserver

# Connect to SQL Server manually
docker exec -it notes-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Passw0rd'
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/notes` | Get all notes with optional filtering |
| GET    | `/api/notes/{id}` | Get specific note by ID |
| POST   | `/api/notes` | Create new note |
| PUT    | `/api/notes/{id}` | Update existing note |
| DELETE | `/api/notes/{id}` | Delete note |

### Query Parameters for GET /api/notes
- `search` - Search in title and content
- `sortBy` - Sort by: `title`, `createdAt`, `updatedAt`
- `isDescending` - Sort order: `true` or `false`

## Database Schema

```sql
CREATE TABLE Notes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(500) NOT NULL,
    Content NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    UserId INT NOT NULL DEFAULT 1
);
```

## Development

### Backend Development
```bash
cd Web_Project
dotnet watch run  # Hot reload during development
```

### Frontend Development
```bash
cd note-app
npm run dev  # Hot reload during development
```

### Type Checking (Frontend)
```bash
cd note-app
npm run type-check
```

### Build for Production
```bash
# Frontend
cd note-app
npm run build

# Backend
cd Web_Project
dotnet publish -c Release
```

## Configuration

### Database Connection
The default connection string is configured for Docker SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=NotesAppDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;"
  }
}
```

For other SQL Server instances, update the connection string accordingly.

### CORS Configuration
The backend is configured to allow requests from:
- `http://localhost:5173` (Vite dev server)
- `http://localhost:3000` (Alternative port)

## Project Structure

```
├── Web_Project/                 # .NET Core Web API
│   ├── Controllers/            # API Controllers
│   ├── Models/                 # Data models
│   ├── Repositories/           # Data access layer
│   ├── Scripts/               # Database scripts
│   └── Program.cs             # Application entry point
│
├── note-app/                   # Vue.js Frontend
│   ├── src/
│   │   ├── components/        # Vue components
│   │   ├── stores/           # Pinia stores
│   │   ├── services/         # API services
│   │   ├── types/           # TypeScript types
│   │   └── assets/          # Static assets
│   ├── tailwind.config.js    # Tailwind configuration
│   └── vite.config.js       # Vite configuration
│
└── README.md                  # This file
```

## Troubleshooting

### Common Issues

1. **Database Connection Failed**
   - Ensure SQL Server LocalDB is installed
   - Check the connection string in appsettings.json
   - Run the database creation script

2. **CORS Errors**
   - Ensure the frontend URL is in the CORS policy
   - Check that both frontend and backend are running

3. **Module Not Found Errors (Frontend)**
   - Run `npm install` in the note-app directory
   - Check that all dependencies are correctly installed

4. **TypeScript Errors**
   - Run `npm run type-check` to see detailed TypeScript errors
   - Ensure all dependencies are properly typed

## Future Enhancements

- [ ] User authentication and registration
- [ ] Note categories/tags
- [ ] Rich text editor
- [ ] File attachments
- [ ] Note sharing
- [ ] Offline support
- [ ] Dark mode
- [ ] Note templates

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License. 