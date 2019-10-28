FROM  microsoft/dotnet-framework:4.7.2-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ConsoleApp/*.csproj ./ConsoleApp/
COPY ConsoleApp/*.config ./ConsoleApp/
COPY ./Configs/*.config ./ConfigurationFiles/
COPY ./Configs/Services.*.json ./ConfigurationFiles/

RUN dir c:\app\ConfigurationFiles
RUN nuget restore 
#set environment variables
ENV ENVIRONMENT DEBUG
ENV SERVICE_NAME EMPTY

#copy all and build
COPY . ./

WORKDIR /app/ConsoleApp
RUN msbuild /p:Configuration=Release

FROM microsoft/aspnet:4.7.2 AS runtime
WORKDIR /app
COPY --from=build /app/ConsoleApp/bin/Release/. ./

#copy configuration files
COPY --from=build /app/ConfigurationFiles/. ./cfg

#copy data for transformation
COPY --from=build /app/Tools/Microsoft.Web.XmlTransform.dll ./
COPY --from=build /app/Scripts/AppConfigTransform.ps1 ./

ENTRYPOINT powershell -File AppConfigTransform.ps1