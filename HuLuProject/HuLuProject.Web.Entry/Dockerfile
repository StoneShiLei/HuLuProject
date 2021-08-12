#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HuLuProject.Web.Entry/HuLuProject.Web.Api.csproj", "HuLuProject.Web.Entry/"]
COPY ["HuLuProject.Web.Core/HuLuProject.Web.Core.csproj", "HuLuProject.Web.Core/"]
COPY ["Furion.Extras.Logging.Serilog/Furion.Extras.Logging.Serilog.csproj", "Furion.Extras.Logging.Serilog/"]
COPY ["HuLuProject.Application/HuLuProject.Application.csproj", "HuLuProject.Application/"]
COPY ["HuLuProject.Core/HuLuProject.Core.csproj", "HuLuProject.Core/"]
COPY ["Furion.Extras.DatabaseAccessor.MongoDB/Furion.Extras.DatabaseAccessor.MongoDB.csproj", "Furion.Extras.DatabaseAccessor.MongoDB/"]
COPY ["Furion.Extras.DatabaseAccessor.FreeSql/Furion.Extras.DatabaseAccessor.FreeSql.csproj", "Furion.Extras.DatabaseAccessor.FreeSql/"]
COPY ["Furion.Extras.DependencyModel.CodeAnalysis/Furion.Extras.DependencyModel.CodeAnalysis.csproj", "Furion.Extras.DependencyModel.CodeAnalysis/"]
COPY ["Furion/Furion.csproj", "Furion/"]
COPY ["Furion.Extras.Authentication.JwtBearer/Furion.Extras.Authentication.JwtBearer.csproj", "Furion.Extras.Authentication.JwtBearer/"]
COPY ["Furion.Extras.ObjectMapper.Mapster/Furion.Extras.ObjectMapper.Mapster.csproj", "Furion.Extras.ObjectMapper.Mapster/"]
RUN dotnet restore "HuLuProject.Web.Entry/HuLuProject.Web.Api.csproj"
COPY . .
WORKDIR "/src/HuLuProject.Web.Entry"
RUN dotnet build "HuLuProject.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HuLuProject.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HuLuProject.Web.Api.dll"]