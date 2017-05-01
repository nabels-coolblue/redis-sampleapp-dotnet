#!/usr/bin/env bash

echo "running client provisioning"

which redis-cli 2>/dev/null || {
  export DEBIAN_FRONTEND=noninteractive
  apt-get update
  apt-get install -y redis-tools
}

#dotnet
which dotnet 2>/dev/null || {
  export DEBIAN_FRONTEND=noninteractive
  echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list
  apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893
  apt-get update
  apt-get install -y dotnet-dev-1.0.3
}

dotnet --version
