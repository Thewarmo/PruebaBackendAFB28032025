# CargoPay Web API

A secure payment processing API for managing card transactions with dynamic fee structures.

## Overview

CargoPay is a RESTful API that allows you to:
- Create virtual payment cards
- Load funds onto cards
- Process payments with dynamic fee structures
- Check card balances
- Track transaction history

The API uses JWT authentication to secure endpoints and implements the Universal Fee Exchange (UFE) system that dynamically adjusts transaction fees.

## Features

- **Card Management**: Create and manage virtual payment cards
- **Secure Transactions**: Process payments with built-in security measures
- **Dynamic Fee Structure**: Fees are adjusted hourly through the UFE system
- **Balance Management**: Load funds and check balances
- **JWT Authentication**: Secure API access

## Technologies

- ASP.NET Core 7.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI

## Getting Started

### Prerequisites

- .NET 7.0 SDK or later
- SQL Server (local or remote)
- Visual Studio 2022 or VS Code

### Setup

1. Clone the repository
2. Configure user secrets for development:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your_DB_Connection_String"
dotnet user-secrets set "JwtSettings:Key" "Your_JWT_Secret_Key_Min_32_Chars"
dotnet user-secrets set "JwtSettings:Issuer" "CargoPay"
dotnet user-secrets set "JwtSettings:Audience" "CargoPay"
dotnet user-secrets set "JwtSettings:ExpirationInMinutes" "60"