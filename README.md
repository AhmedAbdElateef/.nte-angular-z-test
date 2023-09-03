# DotNet-Vue-SocialApp


# backend

# backend-sociaApp-Dotnet

# create app

    dotnet new webapi -n backend

    cd ./backend

[install-packages]

    dotnet add package AspNetCore.Identity.MongoDbCore

    dotnet add package MongoDB.Bson
    

    dotnet add package MongoDB.Driver


    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer



# crtl + shift + p 

    docker add .. 


# run the app with ports

    dotnet watch run --urls http://localhost:5000



# client

[note]
define or update the angular.json
    Make            "outputPath": "dist",




    npm i

    ng serve


# DEVOPS 
# Run With DevOps

    docker-compose up -d 




# realtime
# !!!!!!!!!! backend !!!!!!!!!!!!



# install app

    dotnet new webapi  -n backend


# realtime Service

    dotnet new webapi  -n realTimeServices


update the code base 
remove 

    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>

from api.csproj


remove 

    "https": {
        "commandName": "Project",
        "dotnetRunMessages": ...
        }

upate http & IIS Express change 
    "launchBrowser": true,
    "launchBrowser": true,
      to 
    "launchBrowser": false,
    "launchBrowser": false,

from Properties



[update-Program.cs]



# RealTime
# Client

install signalr

    npm install @microsoft/signalr


to add environments

    ng generate environments

# to create service 
    create folder names services
# inside services folder hit this command

    ng g s services/realtimechat --skip-tests


# backend GRPC 
# GRPC SERVER

after creating proto file add package to the app

    dotnet add package Grpc.AspNetCore  
    dotnet add package Google.Protobuf 
    dotnet add package Grpc.Tools 

# add this to .csproj

      <Protobuf Include="Protos/*.proto" GrpcServices="Server" />

# Add a Grpc.AspNetCore.Server.Reflection package reference.


    dotnet add package Grpc.AspNetCore.Server.Reflection 

# update program.cs
# update Properties/launchSettings.json


     grpcurl localhost:7288 list

# use postman import proto first 

    grpcurl localhost:7288 describe TrackingService

# ############## CLIENT ################ #

    dotnet add package Grpc.Net.Client

    dotnet add package Google.Protobuf

    dotnet add package Grpc.Tools


[add-to-csproj]

<Protobuf Include="Protos/*.proto"  GrpcServices="Client" />


# Angular After GRPC 

we need now to use the https calling 

so replace 
    
    http://localhost:5000

with

    https://localhost:7288


# NOTIFICATION frontend

    ng g s services/realtime-notifiy --skip-tests


    ng g s services/notification.service --skip-tests



# Notification backend

    dotnet new webapi  -n realTimeNotification


# we need to make api work as client Grpc

[Run]

    dotnet add package Grpc.Net.Client

    dotnet add package Google.Protobuf

    dotnet add package Grpc.Tools

# GRPC SERVER

after creating proto file add package to the app

    dotnet add package Grpc.AspNetCore  
    dotnet add package Google.Protobuf 
    dotnet add package Grpc.Tools 

    dotnet add package Grpc.AspNetCore.Server.Reflection 











