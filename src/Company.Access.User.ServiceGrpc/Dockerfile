FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
ARG Config
WORKDIR /
COPY src/Company.Grpc.Common/Company.Grpc.Common.csproj src/Company.Grpc.Common/
COPY src/Company.Grpc.Server/Company.Grpc.Server.csproj src/Company.Grpc.Server/
COPY src/Company.Access.User.ServiceGrpc/Company.Access.User.ServiceGrpc.csproj src/Company.Access.User.ServiceGrpc/
COPY src/Company.Access.User.InterfaceGrpc/Company.Access.User.InterfaceGrpc.csproj src/Company.Access.User.InterfaceGrpc/
COPY src/Company.Access.User.Impl/Company.Access.User.Impl.csproj src/Company.Access.User.Impl/
COPY src/Company.Access.User.Interface/Company.Access.User.Interface.csproj src/Company.Access.User.Interface/

RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Access.User.ServiceGrpc/

COPY . .
WORKDIR /src/Company.Access.User.ServiceGrpc
RUN dotnet build -c ${Config:-Release} -o /app

FROM build AS publish
ARG Config
RUN dotnet publish -c ${Config:-Release} -o /app

FROM base AS final
ARG CaDir
ARG CertsDir
WORKDIR /app
COPY --from=publish /app .
COPY ${CaDir} ${CaDir}
COPY ${CertsDir} ${CertsDir}
ENTRYPOINT ["dotnet", "Company.Access.User.ServiceGrpc.dll"]
