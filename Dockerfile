FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app

RUN dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Dev@password

ENTRYPOINT [ "dotnet", "watch", "run", "--project", "GitInsight.Api"]
