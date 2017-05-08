#!/usr/bin/env bash

echo "running dotnet core app"
if [ -d /vagrant/netcore-redis-app/ ]; then
        pushd /vagrant/netcore-redis-app/
        dotnet restore
        dotnet build
        pushd bin/Debug/netcoreapp1.1
        time dotnet netcore-redis-app.dll
else
        echo "ERROR: .net core app could not be found"
fi