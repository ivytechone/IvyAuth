FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY ./IvyAuth/bin/Release/net7.0/* /ivyauth/
WORKDIR ./ivyauth
ENTRYPOINT ./IvyAuth
