# MarketPriceAPI
A .NET Core REST API for retrieving market asset data using the Fintacharts platform.  
Includes support for MySQL database, real-time and historical data access via WebSocket and REST, and Docker support for deployment.
## 1. Clone the Repository

Clone the project repository to your local machine:

```bash
git clone https://github.com/matviiv8/MarketPriceAPI.git
cd MarketPriceAPI
```

## 2. Setup Dependencies
### 2.1 appsettings.json
Open the project in your IDE. Your appsettings.json should have the basic structure like this:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=default_server;port=3306;database=default_db;user=default_user;password=default_password"
  },
  "FintachartsAPI": {
    "RestUrl": "https://platform.fintacharts.com",
    "WssUrl": "wss://platform.fintacharts.com",
    "ClientId": "your-client-id",
    "GrantType": "your-grant-type",
    "Username": "your-username",
    "Password": "your-password"
  }
}
```
**ConnectionStrings:DefaultConnection**  
This is your database connection string. You need to replace:

- `default_server` with the hostname or IP address of your database server (e.g., localhost or an IP).  
- `port` with the port number your database listens on (default for MySQL is usually 3306).  
- `database` with the name of your database.  
- `user` with the database username.  
- `password` with the password for the database user.  

---

**FintachartsAPI**  
These settings are required to connect to the Fintacharts API service:

- **RestUrl:** The base URL for REST API calls (usually `"https://platform.fintacharts.com"`).  
- **WssUrl:** The WebSocket URL for real-time data streaming (usually `"wss://platform.fintacharts.com"`).  
- **ClientId:** Your client identifier provided by Fintacharts for authentication.  
- **GrantType:** The OAuth grant type your application uses (e.g., `"password"`).  
- **Username:** Your Fintacharts API username.  
- **Password:** Your Fintacharts API password.  

Note: For security, sensitive data like passwords and API credentials should be stored in the .env file (see next section), and not committed to source control.

### 2.2 .env File
Create a .env file in the project root directory:
```bash
# MySQL settings
MYSQL_DATABASE=YourDbName
MYSQL_ROOT_PASSWORD=YourPassword

# Connection string for the app
ConnectionStrings__DefaultConnection=Server=host.docker.internal;Port=3306;Database=YourDbName;Uid=YourUser;Pwd=YourPassword;

# External API credentials (Fintacharts)
FintachartsAPI__RestUrl=https://platform.fintacharts.com
FintachartsAPI__WssUrl=wss://platform.fintacharts.com
FintachartsAPI__ClientId=app-cli
FintachartsAPI__GrantType=password
FintachartsAPI__Username=YourUsername
FintachartsAPI__Password=YourPassword
```
## 3. Building and Running with Docker (Single Container)
### 3.1 Build Docker Image
Make sure Docker is installed and running. Then build your image:
```bash
docker build -t marketpriceapi .
```
### 3.2 Run Docker Container with .env
Run the container, passing the .env file to set environment variables and map ports:
```bash
docker run --env-file .env -d -p 8080:80 --name marketpriceapi marketpriceapi
```
### 3.3 Verify the Application is Running
To see the logs and confirm the app is running:
```bash
docker logs -f marketpriceapi
```
### 3.4 Stop and Remove the Container
If you want to stop and remove the container:
```bash
docker stop marketpriceapi
docker rm marketpriceapi
```
## 4. Running the Application with Docker Compose
Make sure you have a valid docker-compose.yml file and a .env file in your project root.
### 4.1 Start All Services
```bash
docker-compose up -d
```
### 4.2 Check Service Status
```bash
docker-compose ps
```
### 4.3 Stop and Remove All Services
```bash
docker-compose down
```
This method is recommended for development environments where multiple services need to start and stop together.
## 5. API Testing  
You can test the API endpoints using various tools such as:

- Swagger UI  
- Postman  
- curl  
- Browser  

As an example, Swagger UI provides an interactive interface where you can explore and test all API endpoints directly in your browser([http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)).
![Swagger UI Screenshot](https://github.com/user-attachments/assets/1decc1b1-c3c7-48fd-b2b4-d10743c60b3e)

## Author

Created by [Andrij Matviiv](https://github.com/matviiv8)
