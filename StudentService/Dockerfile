# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory inside the container
WORKDIR /app

# Copy the csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the project files
COPY . ./

# Build the project
RUN dotnet publish -c Release -o out

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set working directory in runtime container
WORKDIR /app

# Copy the published output from build stage
COPY --from=build /app/out ./

# Expose the port your app will run on
EXPOSE 5000

# Set the command to run your app
ENTRYPOINT ["dotnet", "StudentService.dll"]
