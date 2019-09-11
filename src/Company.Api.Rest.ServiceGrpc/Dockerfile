FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
ARG Config
WORKDIR /src
COPY src/Company.Grpc.Common/Company.Grpc.Common.csproj src/Company.Grpc.Common/
COPY src/Company.Grpc.Client/Company.Grpc.Client.csproj src/Company.Grpc.Client/
COPY src/Company.Common.Data/Company.Common.Data.csproj src/Company.Common.Data/
COPY src/Company.Api.Rest.ServiceGrpc/Company.Api.Rest.ServiceGrpc.csproj src/Company.Api.Rest.ServiceGrpc/
COPY src/Company.Manager.Membership.Client/Company.Manager.Membership.Client.csproj src/Company.Manager.Membership.Client/
COPY src/Company.Manager.Membership.InterfaceGrpc/Company.Manager.Membership.InterfaceGrpc.csproj src/Company.Manager.Membership.InterfaceGrpc/
COPY src/Company.Manager.Membership.Interface/Company.Manager.Membership.Interface.csproj src/Company.Manager.Membership.Interface/
COPY src/Company.Api.Rest.Impl/Company.Api.Rest.Impl.csproj src/Company.Api.Rest.Impl/
COPY src/Company.Api.Rest.Data/Company.Api.Rest.Data.csproj src/Company.Api.Rest.Data/

RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Grpc.Common/
RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Grpc.Client/
RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Common.Data/
RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Api.Rest.ServiceGrpc/
RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Manager.Membership.Client/
RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Manager.Membership.InterfaceGrpc/
RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Manager.Membership.Interface/
RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Api.Rest.Impl/
RUN dotnet restore -nowarn:msb3202,nu1503 src/Company.Api.Rest.Data/

COPY . .
WORKDIR /src/src/Company.Api.Rest.ServiceGrpc
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
ENTRYPOINT ["dotnet", "Company.Api.Rest.ServiceGrpc.dll"]
