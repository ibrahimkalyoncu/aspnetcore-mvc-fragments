FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY src/AspNetCore.Mvc.Fragments/AspNetCore.Mvc.Fragments/*.csproj ./src/AspNetCore.Mvc.Fragments/AspNetCore.Mvc.Fragments/
COPY demo/AspNetCore.Mvc.Fragments.Demo/AspNetCore.Mvc.Fragments.Demo/*.csproj ./demo/AspNetCore.Mvc.Fragments.Demo/AspNetCore.Mvc.Fragments.Demo/

RUN dotnet restore
COPY . .

WORKDIR /src/src/AspNetCore.Mvc.Fragments/AspNetCore.Mvc.Fragments
RUN dotnet build -c Release -o /app

WORKDIR /src/demo/AspNetCore.Mvc.Fragments.Demo/AspNetCore.Mvc.Fragments.Demo
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AspNetCore.Mvc.Fragments.Demo.dll"]