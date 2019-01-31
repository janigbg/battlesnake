FROM microsoft/dotnet:2.1-aspnetcore-runtime
ARG source
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "NetSnake.dll"]
