#!/usr/bin/env bash

echo "running client provisioning"

which redis-cli 2>/dev/null || {
  export DEBIAN_FRONTEND=noninteractive
  apt-get update
  apt-get install -y redis-tools
}
