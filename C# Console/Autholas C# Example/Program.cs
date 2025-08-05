using System;
using System.Threading.Tasks;

namespace Autholas
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("        Autholas Login System        ");
            Console.WriteLine("           C# Example Code           ");
            Console.WriteLine("═════════════════════════════════════");
            
            // Initialize the authentication service
            var authService = new AuthService();
            
            // Get hardware ID
            var hwid = AuthService.GetHardwareID();
            Console.WriteLine($"Device ID: {hwid.Substring(0, Math.Min(8, hwid.Length))}...");
            Console.WriteLine();
            
            // Get user credentials
            Console.Write("Username: ");
            var username = Console.ReadLine();
            
            Console.Write("Password: ");
            var password = ReadPassword();
            Console.WriteLine();
            
            // Attempt authentication
            var result = await authService.AuthenticateUserAsync(username, password, hwid);
            
            if (result.Success)
            {
                Console.WriteLine();
                Console.WriteLine("Authentication successful!");
                Console.WriteLine("Starting application...");
                Console.WriteLine();
                
                // Start your main application
                await StartApplication(authService);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Authentication failed.");
                
                // Provide helpful tips based on error code
                ProvideErrorTips(result.ErrorCode);
                
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            
            return password;
        }

        static void ProvideErrorTips(string errorCode)
        {
            Console.WriteLine();
            Console.WriteLine("Troubleshooting Tips:");
            
            switch (errorCode)
            {
                case "INVALID_CREDENTIALS":
                    Console.WriteLine("Double-check your username and password spelling");
                    Console.WriteLine("Make sure Caps Lock is not enabled");
                    Console.WriteLine("Verify your account is still active");
                    break;
                case "MAX_DEVICES_REACHED":
                    Console.WriteLine("Contact support to reset your device limit");
                    Console.WriteLine("Try logging in from a previously used device");
                    break;
                case "HWID_BANNED":
                    Console.WriteLine("This specific device has been banned");
                    Console.WriteLine("Try from a different device or contact support");
                    break;
                case "USER_BANNED":
                    Console.WriteLine("Your account has been suspended");
                    Console.WriteLine("Contact support to appeal the ban");
                    break;
                case "SUBSCRIPTION_EXPIRED":
                    Console.WriteLine("Your subscription has ended");
                    Console.WriteLine("Renew your subscription to continue access");
                    break;
                case "RATE_LIMIT_EXCEEDED":
                    Console.WriteLine("Too many login attempts detected");
                    Console.WriteLine("Wait a few minutes before trying again");
                    break;
                case "CONNECTION_ERROR":
                    Console.WriteLine("Check your internet connection");
                    Console.WriteLine("Verify the authentication server is accessible");
                    break;
                case "TIMEOUT":
                    Console.WriteLine("Server response timeout");
                    Console.WriteLine("Try again in a few moments");
                    break;
                default:
                    Console.WriteLine("Check your internet connection");
                    Console.WriteLine("Verify your credentials are correct");
                    Console.WriteLine("Contact support if the problem persists");
                    break;
            }
        }

        static async Task StartApplication(AuthService authService)
        {
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine(" APPLICATION STARTED SUCCESSFULLY! ");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine();
            
            // Display session information
            Console.WriteLine("Session Information:");
            Console.WriteLine($"Session Token: {authService.GetSessionToken().Substring(0, Math.Min(20, authService.GetSessionToken().Length))}...");
            Console.WriteLine($"Session Valid: {(authService.IsSessionValid() ? "Yes" : "No")}");
            Console.WriteLine($"Authenticated: {(authService.IsAuthenticated ? "Yes" : "No")}");
            
            if (!string.IsNullOrEmpty(authService.SessionExpires))
            {
                Console.WriteLine($"Session Expires: {authService.SessionExpires}");
            }
            
            Console.WriteLine();
            Console.WriteLine("Your main application logic goes here...");
            Console.WriteLine();
            
            // Example application loop
            bool running = true;
            while (running)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Check session status");
                Console.WriteLine("2. Get session token");
                Console.WriteLine("3. Logout");
                Console.WriteLine("4. Exit application");
                Console.Write("Enter your choice (1-4): ");
                
                var choice = Console.ReadLine();
                Console.WriteLine();
                
                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"Session Status: {(authService.IsSessionValid() ? "Valid" : "Invalid")}");
                        Console.WriteLine($"Authenticated: {(authService.IsAuthenticated ? "Yes" : "No")}");
                        break;
                        
                    case "2":
                        if (authService.IsAuthenticated)
                        {
                            Console.WriteLine($"Session Token: {authService.GetSessionToken()}");
                        }
                        else
                        {
                            Console.WriteLine("Not authenticated");
                        }
                        break;
                        
                    case "3":
                        authService.Logout();
                        Console.WriteLine("Logged out successfully!");
                        running = false;
                        break;
                        
                    case "4":
                        Console.WriteLine("Exiting application...");
                        running = false;
                        break;
                        
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                
                if (running)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("═══════════════════════════════════════");
                    Console.WriteLine("APPLICATION RUNNING");
                    Console.WriteLine("═══════════════════════════════════════");
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Thank you for using Autholas Authentication System!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}