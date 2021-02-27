FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

WORKDIR /build/restore

COPY src/**/*.csproj .
RUN dotnet restore

WORKDIR /build/src

COPY SynerdocsBot.sln .
COPY src/ src/

RUN dotnet publish -c Release -o /build/publish

FROM base

COPY --from=build /build/publish .

USER nobody
ENTRYPOINT [ "./SynerdocsBot" ]
