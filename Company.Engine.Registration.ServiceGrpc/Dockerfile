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
COPY Company.Engine.Registration.ServiceGrpc/Company.Engine.Registration.ServiceGrpc.csproj Company.Engine.Registration.ServiceGrpc/
COPY Company.Engine.Registration.InterfaceGrpc/Company.Engine.Registration.InterfaceGrpc.csproj Company.Engine.Registration.InterfaceGrpc/
COPY Company.Engine.Registration.Interface/Company.Engine.Registration.Interface.csproj Company.Engine.Registration.Interface/
COPY Company.Access.User.ClientGrpc/Company.Access.User.ClientGrpc.csproj Company.Access.User.ClientGrpc/
COPY Company.Access.User.InterfaceGrpc/Company.Access.User.InterfaceGrpc.csproj Company.Access.User.InterfaceGrpc/
COPY Company.Access.User.Interface/Company.Access.User.Interface.csproj Company.Access.User.Interface/
COPY Company.Engine.Registration.Impl/Company.Engine.Registration.Impl.csproj Company.Engine.Registration.Impl/
RUN dotnet restore -nowarn:msb3202,nu1503
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
