# Northwind OMS — Order Management System

RSM Trainee Program — Software Dev Final Project

## Tech Stack

- **Backend**: ASP.NET Core (.NET 10), Entity Framework Core, Clean Architecture
- **Frontend**: Vue 3 + Quasar Framework
- **Database**: SQL Server (Northwind)

## Prerequisites

- .NET 10 SDK
- Node.js 22+
- SQL Server
- Google Maps API Key

## Database Setup

1. Open SQL Server Management Studio
2. Run the Northwind script:
link de github 

## Backend Setup

1. Open `Northwind.API/appsettings.json`
2. Update the connection string:
```json
{
  "ConnectionStrings": {
    "NorthwindConnection": "Server=.;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "GoogleMaps": {
    "ApiKey": "YOUR_GOOGLE_MAPS_API_KEY"
  }
}
```
3. Run the API:
```bash
dotnet run --project Northwind.API
```
API runs on `http://localhost:5164`

## Frontend Setup

```bash
cd northwind-frontend
npm install
npx quasar dev
```
App runs on `http://localhost:9000`

## Google Maps Setup

1. Go to [console.cloud.google.com](https://console.cloud.google.com)
2. Create a new project
3. Enable **Maps JavaScript API** and **Address Validation API**
4. Create an API Key under Credentials
5. Add the key to `appsettings.json` and in `OrderFormPage.vue`

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/orders | Get all orders |
| GET | /api/orders/{id} | Get order by ID |
| POST | /api/orders | Create order |
| PUT | /api/orders/{id} | Update order |
| DELETE | /api/orders/{id} | Delete order |
| GET | /api/customers | Get all customers |
| GET | /api/employees | Get all employees |
| GET | /api/shippers | Get all shippers |
| GET | /api/reports/summary | Dashboard summary |
| GET | /api/reports/orders-by-month | Orders chart data |
| GET | /api/reports/shipments-by-region | Region chart data |

## Running Tests

```bash
dotnet test Northwind.Tests
```

## Project Structure