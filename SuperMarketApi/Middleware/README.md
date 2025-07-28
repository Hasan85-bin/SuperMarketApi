# Exception Handling Middleware

This middleware provides comprehensive exception handling for the SuperMarket API in release/production mode.

## Features

- **Automatic Exception Catching**: Catches all unhandled exceptions in the request pipeline
- **User-Friendly Error Messages**: Returns generic, user-friendly error messages instead of exposing internal details
- **Comprehensive Logging**: Logs all exception details including inner exceptions with appropriate log levels
- **HTTP Status Code Mapping**: Maps different exception types to appropriate HTTP status codes
- **Environment-Based Activation**: Only active in production/release mode (not in development)

## How It Works

### In Development Mode
- Uses the standard ASP.NET Core developer exception page
- Shows detailed error information for debugging

### In Production/Release Mode
- Uses the custom exception handling middleware
- Hides internal error details from users
- Logs all exception information for debugging

## Exception Type Mapping

| Exception Type | HTTP Status Code | User Message |
|----------------|------------------|--------------|
| BadHttpRequestException | 400 Bad Request | "Invalid request. Please check your input and try again." |
| UnauthorizedAccessException | 401 Unauthorized | "You are not authorized to perform this action." |
| ArgumentException | 400 Bad Request | "Invalid parameters provided." |
| ArgumentNullException | 400 Bad Request | "Required parameters are missing." |
| InvalidOperationException | 400 Bad Request | "The requested operation cannot be performed." |
| KeyNotFoundException | 404 Not Found | "The requested resource was not found." |
| All Other Exceptions | 500 Internal Server Error | "An error occurred while processing your request. Please try again later." |

## Logging

The middleware logs exceptions with different levels based on the exception type:

- **Warning Level**: Client errors (BadHttpRequestException, ArgumentException, etc.)
- **Information Level**: Resource not found errors (KeyNotFoundException)
- **Error Level**: All other unhandled exceptions

### Logged Information

For each exception, the following information is logged:
- Exception message
- Exception type
- Stack trace
- All inner exceptions (recursively)
- Inner exception messages, types, and stack traces



## Configuration

The middleware is automatically configured in `Program.cs`:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    // Use custom exception handling middleware in production/release mode
    app.UseExceptionHandling();
}
```

## Usage

The middleware is automatically applied to all requests in production mode. No additional configuration is required.

## Security Benefits

- Prevents exposure of sensitive internal error details to end users
- Maintains detailed logging for debugging and monitoring
- Provides consistent error responses across the application 