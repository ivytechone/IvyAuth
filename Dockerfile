FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY ./IvyAuth/bin/Release/net6.0/* /IvyAuth/
WORKDIR ./IvyAuth
ENTRYPOINT ./IvyAuth/IvyAuth
