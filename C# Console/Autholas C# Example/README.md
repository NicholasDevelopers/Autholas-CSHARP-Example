# Autholas C# Authentication System

A comprehensive C# implementation for Autholas authentication service with hardware ID verification and session management.

## Features

- User authentication via Autholas API
- Hardware ID generation using system information
- Session management with token validation
- Comprehensive error handling
- Password masking during input
- Interactive application menu
- Cross-platform support (Windows, Linux, macOS)

## Prerequisites

- **.NET Core 3.1** or later / **.NET Framework 4.7.2** or later
- **Visual Studio 2019+** or **JetBrains Rider** or **VS Code**
- Internet connection for package restoration

## Installation Guide

### Method 1: Visual Studio (Recommended)

#### Step 1: Create New Project

1. Open Visual Studio
2. Create new project → **Console App (.NET Core)** or **Console App (.NET Framework)**
3. Name your project (e.g., "AutholasAuth")
4. Choose target framework (.NET Core 3.1+ recommended)

#### Step 2: Install Required NuGet Packages

**Using Package Manager Console:**
```powershell
# Install JSON.NET for JSON handling
Install-Package Newtonsoft.Json

# Install HTTP Client extensions (optional, for enhanced HTTP features)
Install-Package Microsoft.Extensions.Http
```

**Using Package Manager UI:**
1. Right-click project → **Manage NuGet Packages**
2. Browse tab → Search **"Newtonsoft.Json"**
3. Install the package by James Newton-King

**Using .csproj (PackageReference):**
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

#### Step 3: Add Source Files

1. Replace the default `Program.cs` with the provided code
2. Add `Autholas.cs` class file to your project
3. Update the namespace if different from "Autholas"

### Method 2: .NET CLI (Cross-Platform)

#### Step 1: Create Project

```bash
# Create new console application
dotnet new console -n AutholasAuth
cd AutholasAuth

# Add required NuGet packages
dotnet add package Newtonsoft.Json
```

#### Step 2: Replace Files

```bash
# Remove default Program.cs and add your files
rm Program.cs

# Add your source files (copy the provided code)
# - Program.cs
# - Autholas.cs
```

#### Step 3: Build and Run

```bash
# Build the project
dotnet build

# Run the application
dotnet run
```

### Method 3: VS Code

#### Step 1: Setup Environment

```bash
# Install .NET SDK (if not already installed)
# Download from: https://dotnet.microsoft.com/download

# Verify installation
dotnet --version

# Create project
dotnet new console -n AutholasAuth
cd AutholasAuth
```

#### Step 2: Install Dependencies

```bash
# Add Newtonsoft.Json package
dotnet add package Newtonsoft.Json

# Restore packages
dotnet restore
```

#### Step 3: Development

1. Open folder in VS Code
2. Install **C# extension** by Microsoft
3. Replace `Program.cs` with provided code
4. Add `Autholas.cs` file
5. Press **F5** to debug or **Ctrl+F5** to run

## Project Structure

```
AutholasAuth/
├── Program.cs              # Main application entry point
├── Autholas.cs            # Authentication service class
├── AutholasAuth.csproj    # Project configuration
└── README.md              # This documentation
```

## Configuration

### 1. Set Your API Key

Edit `Autholas.cs` and replace the API key:

```csharp
private const string API_KEY = "your_actual_api_key_here";
```

### 2. Customize Device Name (Optional)

You can modify the device name in the authentication call:

```csharp
var result = await authService.AuthenticateUserAsync(username, password, hwid, "My Custom Device");
```

## Building the Application

### Debug Build

```bash
# Visual Studio: Build → Build Solution (F6)
# CLI:
dotnet build --configuration Debug
```

### Release Build

```bash
# Visual Studio: Build → Configuration Manager → Release
# CLI:
dotnet build --configuration Release
dotnet publish --configuration Release --self-contained false
```

### Self-Contained Deployment

```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained true

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained true

# macOS x64
dotnet publish -c Release -r osx-x64 --self-contained true
```

## Usage

### Running the Application

```bash
# From Visual Studio: Press F5 (Debug) or Ctrl+F5 (Run)
# From CLI:
dotnet run

# Or run the compiled executable:
# Windows:
./bin/Release/net6.0/AutholasAuth.exe

# Linux/macOS:
./bin/Release/net6.0/AutholasAuth
```

### Example Session

```
═════════════════════════════════════
        Autholas Login System        
           C# Example Code           
═════════════════════════════════════
Device ID: a1b2c3d4...

Username: your_username
Password: ********

Authenticating...
✓ Authentication successful!
Welcome, your_username!
Session expires: 2024-12-31T23:59:59Z

🎉 Authentication successful!
Starting application...

═══════════════════════════════════════
🚀 APPLICATION STARTED SUCCESSFULLY! 🚀
═══════════════════════════════════════

📋 Session Information:
Session Token: abcd1234567890123456...
Session Valid: ✅ Yes
Authenticated: ✅ Yes
Session Expires: 2024-12-31T23:59:59Z

🎯 Your main application logic goes here...

Choose an option:
1. Check session status
2. Get session token
3. Logout
4. Exit application
Enter your choice (1-4):
```

## Troubleshooting

### Common Issues and Solutions

#### 1. Build Errors

**Missing Newtonsoft.Json:**
```bash
# Solution: Install the package
dotnet add package Newtonsoft.Json
dotnet restore
```

**Target framework issues:**
```xml
<!-- Update .csproj file -->
<TargetFramework>net6.0</TargetFramework>
<!-- or -->
<TargetFramework>net48</TargetFramework>
```

#### 2. Runtime Errors

**"Could not load file or assembly" errors:**
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

**Network/SSL issues:**
```csharp
// Add to beginning of Main method (for development only)
ServicePointManager.ServerCertificateValidationCallback = 
    (sender, certificate, chain, sslPolicyErrors) => true;
```

#### 3. Authentication Issues

**Connection timeouts:**
- Check internet connection
- Verify API endpoint is accessible
- Check firewall settings

**Invalid API key:**
- Ensure API key is correctly set in `Autholas.cs`
- Contact Autholas support for valid API key

#### 4. Platform-Specific Issues

**Windows:**
- Ensure .NET Framework/Core is installed
- Run as Administrator if needed

**Linux:**
- Install .NET runtime: `sudo apt install dotnet-runtime-6.0`
- Check permissions: `chmod +x AutholasAuth`

**macOS:**
- Install .NET via Homebrew: `brew install dotnet`
- Allow app execution in Security settings

## Advanced Configuration

### Custom HTTP Client Settings

```csharp
// In Autholas.cs, modify the HttpClient initialization
private static readonly HttpClient client = new HttpClient()
{
    Timeout = TimeSpan.FromSeconds(30),
    DefaultRequestHeaders = 
    {
        {"User-Agent", "Autholas-CSharp-Client/1.0"}
    }
};
```

### Logging Configuration

```csharp
// Add logging support
using Microsoft.Extensions.Logging;

// Install package: Microsoft.Extensions.Logging.Console
// Then add logging to your AuthService class
```

### Configuration File Support

Create `appsettings.json`:
```json
{
  "Autholas": {
    "ApiKey": "your_api_key_here",
    "ApiUrl": "https://autholas.nicholasdevs.xyz/api/auth",
    "DeviceName": "My Custom Device",
    "Timeout": 30
  }
}
```

## Error Handling

The system handles various authentication scenarios:

### Authentication Errors
- `INVALID_CREDENTIALS` - Wrong username/password
- `USER_BANNED` - Account suspended
- `SUBSCRIPTION_EXPIRED` - Subscription ended
- `MAX_DEVICES_REACHED` - Device limit exceeded
- `HWID_BANNED` - Device hardware ID banned
- `RATE_LIMIT_EXCEEDED` - Too many authentication attempts
- `DEVELOPER_SUSPENDED` - API developer account suspended

### Network Errors
- `CONNECTION_ERROR` - Network connectivity issues
- `TIMEOUT` - Server response timeout
- `UNEXPECTED_ERROR` - Unhandled exceptions

## Security Features

- **Hardware ID Generation**: Uses system information (hostname, username, OS architecture)
- **Secure Communication**: HTTPS-only API calls
- **Password Masking**: Console password input is masked
- **Session Management**: Token-based authentication with expiration
- **Error Sanitization**: Sensitive information is not logged

## Performance Optimization

### Memory Management

```csharp
// The HttpClient is static and reused
// Implement IDisposable for your services if needed
public class AuthService : IDisposable
{
    public void Dispose()
    {
        // Cleanup resources
    }
}
```

### Async Best Practices

```csharp
// Use ConfigureAwait(false) for library code
var response = await client.PostAsync(API_URL, content).ConfigureAwait(false);
```

## Testing

### Unit Testing Setup

```bash
# Add testing packages
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
```

### Example Test

```csharp
[Test]
public void GetHardwareID_ShouldReturnValidHash()
{
    // Arrange & Act
    var hwid = AuthService.GetHardwareID();
    
    // Assert
    Assert.IsNotNull(hwid);
    Assert.AreEqual(64, hwid.Length); // SHA256 hex string length
}
```

## Dependencies

- **Newtonsoft.Json** (13.0.3+): JSON serialization/deserialization
- **System.Net.Http**: HTTP client functionality
- **System.Security.Cryptography**: Hardware ID hashing
- **.NET Standard 2.0** compatible libraries

## Compatibility

- **Windows**: Windows 7+ with .NET Framework 4.7.2+ or .NET Core 3.1+
- **Linux**: Most distributions with .NET Core 3.1+ runtime
- **macOS**: macOS 10.14+ with .NET Core 3.1+ runtime
- **Architecture**: x86, x64, ARM32, ARM64

## Deployment Options

### Standalone Executable
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### Framework-Dependent
```bash
dotnet publish -c Release --self-contained false
```

### Container Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/runtime:6.0
COPY bin/Release/net6.0/publish/ app/
WORKDIR /app
ENTRYPOINT ["dotnet", "AutholasAuth.dll"]
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Follow C# coding standards and conventions
4. Add unit tests for new functionality
5. Commit your changes (`git commit -m 'Add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## License

This project is provided as-is for educational and development purposes. Please respect the terms of service of the Autholas API.

## Support

For issues related to:
- **Autholas API**: Contact Autholas support
- **.NET/C# specific issues**: Check [Microsoft .NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- **Code issues**: Create an issue in this repository

## Changelog

### v1.0.0
- Initial C# implementation
- Hardware ID generation
- Session management
- Comprehensive error handling
- Interactive console application

---

**Note**: This application requires an active internet connection and valid Autholas API credentials to function properly.