# DeviceConfig

## Downloads

- .NET 10.0 SDK
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)
- Visual Studio 2022

## Database Connection Setup

1. Open SQL Server Managment Studio.
2. Create a new connection using `localhost\SQLEXPRESS` as the Server Name.
3. Run `01-setup.sql` from the repo.
4. Run the statement and copy out the `ConnectionString`
  `select
    'data source=' + @@servername +
    ';TrustServerCertificate=True' +
    ';initial catalog=' + db_name() +
    case type_desc
        when 'WINDOWS_LOGIN' 
            then ';trusted_connection=true'
        else
            ';user id=' + suser_name() + ';password=<<YourPassword>>'
    end
    as ConnectionString
from sys.server_principals
where name = suser_name()`
5. Paste it inside the return statement of `GetConnectionString()` method in `Program.cs`

## How to Run

1. Clone the repository inside VS
2. Open Powershell inside VS
  1. run `dotnet restore`
  2. run `dotnet build`
  3. run `cd .\DeviceConfig\bin\Debug\net10.0\`
  4. run ` .\DeviceConfig.exe`

## Test Cases

- ` .\DeviceConfig.exe` should return all records from the db inside a table
- ` .\DeviceConfig.exe 1` should return the record where DeviceId = 1
- ` .\DeviceConfig.exe 101` should return no records
- ` .\DeviceConfig.exe t` should return `Invalid Device ID.`
