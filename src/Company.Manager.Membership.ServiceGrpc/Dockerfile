FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
ARG Config
WORKDIR /
COPY src/Company.Grpc.Common/Company.Grpc.Common.csproj src/Company.Grpc.Common/
COPY src/Company.Grpc.Client/Company.Grpc.Client.csproj src/Company.Grpc.Client/
COPY src/Company.Grpc.Server/Company.Grpc.Server.csproj src/Company.Grpc.Server/
COPY src/Company.Common.Data/Company.Common.Data.csproj src/Company.Common.Data/
COPY src/Company.Manager.Membership.ServiceGrpc/Company.Manager.Membership.ServiceGrpc.csproj src/Company.Manager.Membership.ServiceGrpc/
COPY src/Company.Manager.Membership.Interface/Company.Manager.Membership.Interface.csproj src/Company.Manager.Membership.Interface/
COPY src/Company.Manager.Membership.InterfaceGrpc/Company.Manager.Membership.InterfaceGrpc.csproj src/Company.Manager.Membership.InterfaceGrpc/
COPY src/Company.Manager.Membership.Impl/Company.Manager.Membership.Impl.csproj src/Company.Manager.Membership.Impl/
COPY src/Company.Engine.Registration.Interface/Company.Engine.Registration.Interface.csproj src/Company.Engine.Registration.Interface/
COPY src/Company.Engine.Registration.Client/Company.Engine.Registration.Client.csproj src/Company.Engine.Registration.Client/
COPY src/Company.Engine.Registration.InterfaceGrpc/Company.Engine.Registration.InterfaceGrpc.csproj src/Company.Engine.Registration.InterfaceGrpc/

RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Manager.Membership.ServiceGrpc/

COPY . .
WORKDIR /src/Company.Manager.Membership.ServiceGrpc
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
ENTRYPOINT ["dotnet", "Company.Manager.Membership.ServiceGrpc.dll"]
