# Crm Integration app

This repository contains the source code for a CrmIntegrationApp and MockCrmApi, built using .NET 8.0. CrmIntegrationApp performs periodic
Http request every 5 hours(can be changed in appsetings.json) to get a list of tickets from MockCrmApi. The application also processes incoming webhooks. Communication is secured and uses JWT tokens. 

## Prerequisites

* **.NET 8 SDK**: Ensure you have the appropriate .NET SDK installed on your system. 

## How to Run the App

1.  **Clone the Repository:**

    ```bash
    git clone https://github.com/bohdantiranu/BankingWebappTestTask.git
    ```

2.  **Restore NuGet Packages:**:

    * Open your terminal and navigate to the root folder of the CrmIntegrationApp project (where the .csproj file is located).
    * Execute command: 
    
    ```bash
    dotnet restore
    ```
    
    * Navigate to MockCrmApi and repeat
    

3.  **Start MockCrmApi:**

    This API simulates a CRM API that the CrmIntegrationApp queries to retrieve tickets. MockCrmApi must be started first! 
    * Open your terminal in MockCrmApi folder.
    * Execute command: 
    
    ```bash
    dotnet run
    ```

4.  **Start CrmIntegrationApp:**

    * Open your terminal in CrmIntegrationApp folder.
    * Execute command: 
    
    ```bash
    dotnet run
    ```

## How to test webhook processing

After CrmIntegrationApp starts, you can use swagger to test it. The swagger will be available on https://localhost:7072/.
Execute getFakeToken(app generate fake jwt token because we don't have real authentication server) and copy the generated token, then press authorize button and paste token -> press authorize, then you can newTicket method to process the ticket you send.

## Notes
* To avoid duplicate processing, a database can be integrated, along with a cache for improved performance.
* Logs are available in \CrmIntegration\CrmIntegrationApp\logs .
* Unit tests are not added because the recruiter told that are not needed.
