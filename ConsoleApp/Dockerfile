#FROM  microsoft/dotnet-framework:4.7.2-sdk AS build
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ConsoleApp/*.csproj ./ConsoleApp/
COPY ConsoleApp/*.config ./ConsoleApp/
RUN dir

RUN dotnet restore ./ConsoleApp

COPY . ./
RUN dir
WORKDIR /app/ConsoleApp
RUN dotnet build

FROM build AS publish
WORKDIR /app/ConsoleApp
RUN dotnet publish -c Release -o out

#FROM microsoft/aspnet:4.7.2 AS runtime
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8 AS runtime
WORKDIR /app
COPY --from=publish /app/ConsoleApp/out ./
ENTRYPOINT ["ConsoleApp.exe"]