using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Autholas
{
    public class AuthService
    {
        // Your API configuration
        private const string API_KEY = "YOUR_API_KEY_HERE";
        private const string API_URL = "https://autholas.web.id/api/auth";
        
        private static readonly HttpClient client = new HttpClient();

        // Public properties for session management
        public string SessionToken { get; private set; } = "";
        public string SessionExpires { get; private set; } = "";
        public bool IsAuthenticated { get; private set; } = false;

        private static void HandleAuthError(string errorCode, string errorMessage)
        {
            var errorMessages = new Dictionary<string, (string title, string message)>
            {
                ["INVALID_CREDENTIALS"] = ("Login Failed", "Username or password is incorrect.\nPlease double-check your credentials and try again."),
                ["USER_BANNED"] = ("Account Banned", "Your account has been suspended.\nPlease contact support for assistance."),
                ["SUBSCRIPTION_EXPIRED"] = ("Subscription Expired", "Your subscription has ended.\nPlease renew your subscription to continue."),
                ["MAX_DEVICES_REACHED"] = ("Device Limit Reached", "Maximum number of devices exceeded.\nPlease contact support to reset your devices."),
                ["HWID_BANNED"] = ("Device Banned", "This device has been banned.\nPlease contact support for assistance."),
                ["INVALID_API_KEY"] = ("Service Error", "Authentication service unavailable.\nPlease try again later or contact support."),
                ["RATE_LIMIT_EXCEEDED"] = ("Too Many Attempts", "Please wait before trying again."),
                ["DEVELOPER_SUSPENDED"] = ("Service Unavailable", "Authentication service is temporarily unavailable.\nPlease contact support."),
                ["SERVICE_ERROR"] = ("Service Error", "Authentication service is temporarily unavailable.\nPlease try again later.")
            };

            if (errorMessages.ContainsKey(errorCode))
            {
                var error = errorMessages[errorCode];
                Console.WriteLine($"[{error.title}]");
                Console.WriteLine(error.message);
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }

        public async Task<AuthResult> AuthenticateUserAsync(string username, string password, string hwid, string deviceName = "User PC")
        {
            var payload = new
            {
                api_key = API_KEY,
                username = username,
                password = password,
                hwid = hwid,
                device_name = deviceName
            };

            try
            {
                Console.WriteLine("Authenticating...");
                
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                // Set timeout for the request
                client.Timeout = TimeSpan.FromSeconds(30);
                
                var response = await client.PostAsync(API_URL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                
                var result = JsonConvert.DeserializeObject<dynamic>(responseString);
                
                if (result.success == true)
                {
                    Console.WriteLine("✓ Authentication successful!");
                    Console.WriteLine($"Welcome, {username}!");
                    
                    // Store session information
                    SessionToken = result.session_token ?? "";
                    SessionExpires = result.expires_at ?? "";
                    IsAuthenticated = true;
                    
                    if (result.user?.expires_at != null)
                        Console.WriteLine($"Subscription expires: {result.user.expires_at}");

                    if (!string.IsNullOrEmpty(SessionExpires))
                        Console.WriteLine($"Session expires: {SessionExpires}");
                    
                    return new AuthResult 
                    { 
                        Success = true, 
                        SessionToken = SessionToken,
                        Message = result.message ?? "Authentication successful"
                    };
                }
                else
                {
                    string errorCode = result.error_code ?? "UNKNOWN";
                    string errorMessage = result.error ?? "Unknown error";
                    
                    HandleAuthError(errorCode, errorMessage);
                    return new AuthResult 
                    { 
                        Success = false, 
                        Error = errorMessage, 
                        ErrorCode = errorCode 
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Connection Error: Unable to reach authentication server.");
                Console.WriteLine("Please check your internet connection and try again.");
                return new AuthResult { Success = false, Error = ex.Message, ErrorCode = "CONNECTION_ERROR" };
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Request Timeout: Server is taking too long to respond.");
                Console.WriteLine("Please try again later.");
                return new AuthResult { Success = false, Error = "Request timeout", ErrorCode = "TIMEOUT" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new AuthResult { Success = false, Error = ex.Message, ErrorCode = "UNEXPECTED_ERROR" };
            }
        }

        public static string GetHardwareID()
        {
            try
            {
                string hostname = Environment.MachineName;
                string username = Environment.UserName;
                string architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86";
                string combined = $"{hostname}|{username}|{architecture}";
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combined));
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            catch
            {
                return "unknown";
            }
        }

        private static string GenerateFallbackHWID()
        {
            try
            {
                string fallback = $"{Environment.MachineName}|{Environment.UserName}|{DateTime.Now.Ticks}";
                using (var sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(fallback));
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            catch
            {
                return "unknown_device";
            }
        }

        // Check if current session is still valid
        public bool IsSessionValid()
        {
            if (!IsAuthenticated || string.IsNullOrEmpty(SessionToken))
                return false;

            if (string.IsNullOrEmpty(SessionExpires))
                return true; // No expiration set

            if (DateTime.TryParse(SessionExpires, out DateTime expireDate))
            {
                return DateTime.UtcNow < expireDate;
            }

            return true; // If we can't parse the date, assume it's valid
        }

        // Logout and clear session data
        public void Logout()
        {
            SessionToken = "";
            SessionExpires = "";
            IsAuthenticated = false;
            Console.WriteLine("Logged out successfully.");
        }

        // Get session token
        public string GetSessionToken()
        {
            return SessionToken;
        }
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string SessionToken { get; set; } = "";
        public string Error { get; set; } = "";
        public string ErrorCode { get; set; } = "";
        public string Message { get; set; } = "";
    }

}

