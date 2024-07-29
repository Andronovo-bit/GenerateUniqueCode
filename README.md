# GenerateCampaignCode

GenerateCampaignCode is a .NET 8.0 project designed to generate and validate campaign codes using different hashing algorithms (SHA1 and HMACSHA256). The project includes API endpoints to generate and validate codes, as well as extensive testing to ensure reliability and performance.

## Table of Contents

- [Getting Started](#getting-started)
- [Usage](#usage)
- [Configuration](#configuration)
- [Running the Tests](#running-the-tests)
- [Contributing](#contributing)
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

