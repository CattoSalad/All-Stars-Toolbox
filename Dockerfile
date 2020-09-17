FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY publish .
ENTRYPOINT ["/app/astoolbox"]
