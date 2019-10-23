FROM  microsoft/dotnet-framework:4.7.2-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ./*.sln ./
COPY ./CustomService/*.csproj ./CustomService/
COPY ./CustomService/*.config ./CustomService/
RUN nuget restore

#COPY ./Tools/InstallUtil.exe ./

# copy everything else and build app
COPY ./CustomService/ ./CustomService/
COPY ./Tools/InstallUtil.exe ./CustomService/
WORKDIR /app/CustomService
RUN msbuild /p:Configuration=Release

FROM microsoft/aspnet:4.7.2 AS runtime
WORKDIR /app
COPY --from=build /app/CustomService/bin/Release/. ./
COPY --from=build /app/CustomService/InstallUtil.exe ./

RUN "C:/app/InstallUtil.exe" /LogToConsole=true /ShowCallStack CustomService.exe

RUN powershell -Command Set-Service Service1 -startuptype automatic
RUN powershell -Command Start-Service -Name \""Service1\""
ENTRYPOINT ["cmd"]


