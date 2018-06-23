FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
ARG Config
WORKDIR /src
COPY Company.Grpc.sln ./
COPY Company.Grpc.Common/Company.Grpc.Common.csproj Company.Grpc.Common/
COPY Company.Grpc.Client/Company.Grpc.Client.csproj Company.Grpc.Client/
COPY Company.Grpc.Server/Company.Grpc.Server.csproj Company.Grpc.Server/
COPY Company.Common.Data/Company.Common.Data.csproj Company.Common.Data/
COPY Company.Manager.Membership.ServiceGrpc/Company.Manager.Membership.ServiceGrpc.csproj Company.Manager.Membership.ServiceGrpc/
COPY Company.Manager.Membership.Interface/Company.Manager.Membership.Interface.csproj Company.Manager.Membership.Interface/
COPY Company.Manager.Membership.InterfaceGrpc/Company.Manager.Membership.InterfaceGrpc.csproj Company.Manager.Membership.InterfaceGrpc/
COPY Company.Manager.Membership.Impl/Company.Manager.Membership.Impl.csproj Company.Manager.Membership.Impl/
COPY Company.Engine.Registration.Interface/Company.Engine.Registration.Interface.csproj Company.Engine.Registration.Interface/
COPY Company.Engine.Registration.ClientGrpc/Company.Engine.Registration.ClientGrpc.csproj Company.Engine.Registration.ClientGrpc/
COPY Company.Engine.Registration.InterfaceGrpc/Company.Engine.Registration.InterfaceGrpc.csproj Company.Engine.Registration.InterfaceGrpc/
RUN dotnet restore -nowarn:msb3202,nu1503
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
