#!/bin/bash

echo "🚀 Notes Application Setup Script"
echo "================================="

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "❌ Node.js is not installed. Please install Node.js 16+ and try again."
    exit 1
fi

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is not installed. Please install .NET 8 SDK and try again."
    exit 1
fi

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker and try again."
    echo "   Visit: https://docs.docker.com/get-docker/"
    exit 1
fi

echo "✅ Prerequisites check passed"

# Setup SQL Server with Docker
echo ""
echo "🐳 Setting up SQL Server with Docker..."

# Check if container is already running
if docker ps | grep -q "notes-sqlserver"; then
    echo "✅ SQL Server container is already running"
else
    # Start SQL Server container
    echo "Starting SQL Server container..."
    docker-compose up -d sqlserver
    
    if [ $? -eq 0 ]; then
        echo "✅ SQL Server container started successfully"
        echo "⏳ Waiting for SQL Server to be ready..."
        sleep 10
        
        # Initialize database
        echo "📊 Initializing database..."
        docker exec -i notes-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -i /dev/stdin < Web_Project/Scripts/init-database.sql
        
        if [ $? -eq 0 ]; then
            echo "✅ Database initialized successfully"
        else
            echo "⚠️ Database initialization may have failed. You can run it manually later."
        fi
    else
        echo "❌ Failed to start SQL Server container"
        exit 1
    fi
fi

# Setup Frontend
echo ""
echo "📦 Setting up Frontend (Vue.js + TypeScript)..."
cd note-app

if [ ! -f package.json ]; then
    echo "❌ package.json not found in note-app directory"
    exit 1
fi

echo "Installing npm dependencies..."
npm install

if [ $? -eq 0 ]; then
    echo "✅ Frontend dependencies installed successfully"
else
    echo "❌ Failed to install frontend dependencies"
    exit 1
fi

cd ..

# Setup Backend
echo ""
echo "🔧 Setting up Backend (.NET Core Web API)..."
cd Web_Project

if [ ! -f Web_Project.csproj ]; then
    echo "❌ Web_Project.csproj not found in Web_Project directory"
    exit 1
fi

echo "Restoring .NET packages..."
dotnet restore

if [ $? -eq 0 ]; then
    echo "✅ Backend packages restored successfully"
else
    echo "❌ Failed to restore backend packages"
    exit 1
fi

cd ..

# Final Instructions
echo ""
echo "🎉 Setup Complete!"
echo "=================="
echo ""
echo "📊 SQL Server is running in Docker:"
echo "   → Container: notes-sqlserver"
echo "   → Port: 1433"
echo "   → Database: NotesAppDB"
echo "   → Username: sa"
echo "   → Password: YourStrong@Passw0rd"
echo ""
echo "To start the application:"
echo ""
echo "1. Start the Backend (Terminal 1):"
echo "   cd Web_Project"
echo "   dotnet run"
echo "   → API will be available at: https://localhost:7175"
echo "   → Swagger docs at: https://localhost:7175/swagger"
echo ""
echo "2. Start the Frontend (Terminal 2):"
echo "   cd note-app"
echo "   npm run dev"
echo "   → Frontend will be available at: http://localhost:5173"
echo ""
echo "🛠️ Useful Docker commands:"
echo "   docker-compose up -d sqlserver    # Start SQL Server"
echo "   docker-compose down               # Stop all services"
echo "   docker logs notes-sqlserver       # View SQL Server logs"
echo ""
echo "🗄️ To manually run database scripts:"
echo "   docker exec -it notes-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Passw0rd'"
echo ""
echo "📖 For more information, see README.md"

# Make script executable
chmod +x setup.sh 