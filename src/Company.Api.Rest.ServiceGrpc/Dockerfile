FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
ARG Config
WORKDIR /src
COPY Company.Grpc.sln ./
COPY src/Company.Grpc.Common/Company.Grpc.Common.csproj src/Company.Grpc.Common/
COPY src/Company.Grpc.Client/Company.Grpc.Client.csproj src/Company.Grpc.Client/
COPY src/Company.Common.Data/Company.Common.Data.csproj src/Company.Common.Data/
COPY src/Company.Api.Rest.ServiceGrpc/Company.Api.Rest.ServiceGrpc.csproj src/Company.Api.Rest.ServiceGrpc/
COPY src/Company.Manager.Membership.ClientGrpc/Company.Manager.Membership.ClientGrpc.csproj src/Company.Manager.Membership.ClientGrpc/
COPY src/Company.Manager.Membership.InterfaceGrpc/Company.Manager.Membership.InterfaceGrpc.csproj src/Company.Manager.Membership.InterfaceGrpc/
COPY src/Company.Manager.Membership.Interface/Company.Manager.Membership.Interface.csproj src/Company.Manager.Membership.Interface/
COPY src/Company.Api.Rest.Impl/Company.Api.Rest.Impl.csproj src/Company.Api.Rest.Impl/
COPY src/Company.Api.Rest.Data/Company.Api.Rest.Data.csproj src/Company.Api.Rest.Data/
RUN dotnet restore -nowarn:msb3202,nu1503
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
