FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
ARG Config
WORKDIR /
COPY src/Company.Grpc.Common/Company.Grpc.Common.csproj src/Company.Grpc.Common/
COPY src/Company.Grpc.Client/Company.Grpc.Client.csproj src/Company.Grpc.Client/
COPY src/Company.Grpc.Server/Company.Grpc.Server.csproj src/Company.Grpc.Server/
COPY src/Company.Common.Data/Company.Common.Data.csproj src/Company.Common.Data/
COPY src/Company.Engine.Registration.ServiceGrpc/Company.Engine.Registration.ServiceGrpc.csproj src/Company.Engine.Registration.ServiceGrpc/
COPY src/Company.Engine.Registration.InterfaceGrpc/Company.Engine.Registration.InterfaceGrpc.csproj src/Company.Engine.Registration.InterfaceGrpc/
COPY src/Company.Engine.Registration.Interface/Company.Engine.Registration.Interface.csproj src/Company.Engine.Registration.Interface/
COPY src/Company.Access.User.Client/Company.Access.User.Client.csproj src/Company.Access.User.Client/
COPY src/Company.Access.User.InterfaceGrpc/Company.Access.User.InterfaceGrpc.csproj src/Company.Access.User.InterfaceGrpc/
COPY src/Company.Access.User.Interface/Company.Access.User.Interface.csproj src/Company.Access.User.Interface/
COPY src/Company.Engine.Registration.Impl/Company.Engine.Registration.Impl.csproj src/Company.Engine.Registration.Impl/

RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Engine.Registration.ServiceGrpc/

COPY . .
WORKDIR /src/Company.Engine.Registration.ServiceGrpc
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
ENTRYPOINT ["dotnet", "Company.Engine.Registration.ServiceGrpc.dll"]
