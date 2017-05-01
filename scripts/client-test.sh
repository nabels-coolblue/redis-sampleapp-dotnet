#!/usr/bin/env bash

echo "running client test"
SERVER=192.168.56.11
redis-cli -h ${SERVER} -p 6379 ping
