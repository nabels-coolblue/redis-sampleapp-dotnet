#!/usr/bin/env bash

echo "running server provisioning"

# Got this from https://gist.github.com/helloIAmPau/8149357#file-vagrantfile-L10-L15

sudo apt-get install -y redis-server
sudo cp /etc/redis/redis.conf /etc/redis/redis.conf.old
sudo cat /etc/redis/redis.conf.old | grep -v bind > /etc/redis/redis.conf
echo "bind 0.0.0.0" >> /etc/redis/redis.conf
sudo update-rc.d redis-server defaults
sudo /etc/init.d/redis-server start
