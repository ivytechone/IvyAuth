FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY ./IvyAuth/bin/Release/net6.0/* /ivyauth/
WORKDIR ./ivyauth
ENTRYPOINT ./ivyauth
