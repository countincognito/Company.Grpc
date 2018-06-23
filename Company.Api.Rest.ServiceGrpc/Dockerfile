FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
ARG Config
WORKDIR /src
COPY Company.Grpc.sln ./
COPY Company.Grpc.Common/Company.Grpc.Common.csproj Company.Grpc.Common/
COPY Company.Grpc.Client/Company.Grpc.Client.csproj Company.Grpc.Client/
COPY Company.Common.Data/Company.Common.Data.csproj Company.Common.Data/
COPY Company.Api.Rest.ServiceGrpc/Company.Api.Rest.ServiceGrpc.csproj Company.Api.Rest.ServiceGrpc/
COPY Company.Manager.Membership.ClientGrpc/Company.Manager.Membership.ClientGrpc.csproj Company.Manager.Membership.ClientGrpc/
COPY Company.Manager.Membership.InterfaceGrpc/Company.Manager.Membership.InterfaceGrpc.csproj Company.Manager.Membership.InterfaceGrpc/
COPY Company.Manager.Membership.Interface/Company.Manager.Membership.Interface.csproj Company.Manager.Membership.Interface/
COPY Company.Api.Rest.Impl/Company.Api.Rest.Impl.csproj Company.Api.Rest.Impl/
COPY Company.Api.Rest.Data/Company.Api.Rest.Data.csproj Company.Api.Rest.Data/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Company.Api.Rest.ServiceGrpc
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
