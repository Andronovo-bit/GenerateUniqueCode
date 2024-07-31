# GenerateCampaignCode

GenerateCampaignCode is a .NET 8.0 project designed to generate and validate campaign codes using different hashing algorithms (SHA1 and HMACSHA256). The project includes API endpoints to generate and validate codes, as well as extensive testing to ensure reliability and performance.

#### You can try this
 - Click this link -> https://generatecode.azurewebsites.net/swagger (maybe open slowly you should wait a little first open)

### CodeGenerator

The `CodeGenerator` class provides methods to generate and validate secure campaign codes using SHA1 and HMACSHA256 hashing algorithms. It includes caching functionality to optimize performance and reduce redundant computations. This service is designed for applications that require generating unique, secure codes for various campaigns or promotions.

#### Key Features:

1. **Configuration and Dependencies**:
    - `CampaignCodeSettings`: Configuration settings for the campaign code, including characters set and code length.
    - `IMemoryCache`: Caching service to store and retrieve generated codes.
    - `ILogger<CodeGenerator>`: Logging service to record informational messages and errors.

2. **Main Methods**:
    - `GenerateCodeSHA1(string id, string salt)`: Generates a campaign code using the SHA1 hashing algorithm, with optional caching to improve performance.
    - `ValidateCodeSHA1(string id, string salt, string code)`: Validates a given code by comparing it with a newly generated code using SHA1.
    - `GenerateCodeWithHMACSHA256(string id, string salt)`: Generates a campaign code using the HMACSHA256 hashing algorithm.
    - `ValidateCodeHMACSHA256(string id, string salt, string code)`: Validates a given code by comparing it with a newly generated code using HMACSHA256.

3. **Caching**:
    - Caches generated SHA1 codes to reduce redundant computations and improve performance. Cached codes have a sliding expiration of one day.

4. **Logging**:
    - Logs the generation and retrieval of codes, as well as any errors encountered during the process.


## Table of Contents

- [Getting Started](#getting-started)
- [Usage](#usage)
- [Configuration](#configuration)
- [Running the Tests](#running-the-tests)
- [Contributing](#contributing)
- [Azure Web App Setup and GitHub Secrets](#azure-web-app-setup-and-github-secrets)
- [License](#license)

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or any other preferred IDE

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/Andronovo-bit/GenerateCampaignCode.git
    cd GenerateCampaignCode
    ```

2. Build the solution:
    ```sh
    dotnet build
    ```

## Usage

### Running the API

1. Navigate to the `src/Api` directory:
    ```sh
    cd src/Api
    ```

2. Run the application:
    ```sh
    dotnet run
    ```

The API will be available at `https://localhost:5109/swagger` by default.

### API Endpoints

- **Generate Code (SHA1)**
    ```
    POST /api/code/generate/sha1
    {
      "id": "string",
      "salt": "string"
    }
    ```

- **Validate Code (SHA1)**
    ```
    POST /api/code/validate/sha1
    {
      "id": "string",
      "salt": "string",
      "code": "string"
    }
    ```

- **Generate Code (HMACSHA256)**
    ```
    POST /api/code/generate/hmacsha256
    {
      "id": "string",
      "salt": "string"
    }
    ```

- **Validate Code (HMACSHA256)**
    ```
    POST /api/code/validate/hmacsha256
    {
      "id": "string",
      "salt": "string",
      "code": "string"
    }
    ```

## Configuration

The application settings are stored in `appsettings.json`. You can configure the campaign code settings in this file:

```json
{
    ...
  "CampaignCodeSettings": {
    "Characters": "ACDEFGHKLMNPRTXYZ234579",
    "Length": 8,
    "PrivateKey": "YOUR_PRIVATE_KEY"
    }
    ...
}
```

## Running the Tests

The solution includes extensive unit tests to ensure code reliability. To run the tests, use the following command:

1. Navigate to the `GenerateCampaignCode.Tests` directory:
    ```sh
    cd ./GenerateCampaignCode.Tests
    ```

2. Test the application:
    ```sh
    dotnet test
    ```

The tests cover code generation, validation, and uniqueness checks.

## Azure Web App Setup and GitHub Secrets

### Creating an Azure Web App

1. Sign in to the [Azure Portal](https://portal.azure.com/).
2. From the Azure portal menu, select "Create a resource".
3. In the "Search the Marketplace" field, type 'Web App' and press enter.
4. Select "Web App" from the results, then click "Create".
5. Fill in the details for your web app, such as Subscription, Resource Group, Name, Publish (Code), and Runtime Stack (.NET Core).
6. Click "Review + create" and then "Create" after verifying your details.

### Configuring GitHub Secrets for Azure Deployment

1. Navigate to your GitHub repository.
2. Go to "Settings" > "Secrets" > "Actions".
3. Click on "New repository secret".
4. Add the following secrets required for Azure deployment:
   - `AZURE_PUBLISH_PROFILE`: The publish profile XML content. (You can download this from your Azure Web App's "Deployment Center".)
5. Use these secrets in your `publish.yml` GitHub Action workflow to deploy your application.


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### Explanation

- **About The Project**: Provides a brief overview of what the project is about and the technologies used.
- **Getting Started**: Includes instructions on prerequisites, installation, and running the API.
- **API Endpoints**: Details the available endpoints for generating and validating codes.
- **Configuration**: Describes how to configure the application settings.
- **Running the Tests**: Instructions on how to run the unit tests.
- **License**: Information about the project's license.

This README file serves as a comprehensive guide for users and contributors, helping them understand the project, set it up, and contribute effectively.

