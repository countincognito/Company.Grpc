# Company.Grpc

This a minimalist template for building microservice architectures, using the [IDesign Method](http://www.idesign.net/), using Docker Compose, gRPC, .NET Core 2.0, a HTTPS RESTful public API, and Swagger. It is heavily influenced by code samples that can be downloaded from the [IDesign website](http://www.idesign.net/Downloads).

It requires a local installation of [Seq](https://getseq.net/) for logging, OpenSSL (installing [OpenSSL.Light](https://chocolatey.org/packages/OpenSSL.Light) with [chocolatey](https://chocolatey.org/) should work), and [Docker for Windows](https://www.docker.com/docker-windows).

This solution uses the brilliant [MagicOnion](https://github.com/neuecc/MagicOnion) to dynamically build gRPC clients/servers without having to use protoc. It also uses [MessagePack](https://github.com/neuecc/MessagePack-CSharp) rather than Protobuf.

Details on how to enable SSL using gRPC can be found [here](https://stackoverflow.com/questions/37714558/how-to-enable-server-side-ssl-for-grpc). Please note: this solution uses the simpler client-side authentication rather than server-side authentication, but the same principles still apply.

## Set up

1. Edit the `.env` file to change any environmental variables as necessary (e.g. the host or port for the local Seq server, or the build configuration).
1. From powershell run `create_certs.ps1` to generate the self-signed security certificates for each of the docker images.
1. Run `docker-compose build` to generate the docker images.
1. Run `docker-compose up` to spin up the cluster.
1. Go to [https://localhost:12345/swagger](https://localhost:12345/swagger) (ignore the warning about the invalid certificate).

The **Company.Grpc.sln** solution includes all component, framework and configuration projects. The **Company.InProc.sln** solution includes only the component interfaces and implementations - this is to demonstrate just one possible way of separating business code from plumbing in order to make development and testing easier.
