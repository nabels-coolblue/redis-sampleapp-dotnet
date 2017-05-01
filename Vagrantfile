# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.configure("2") do |config|
  config.vm.box = "cbednarski/ubuntu-1604"
  config.vm.provision "shell", path: "scripts/global.sh"
  config.vm.provider "virtualbox"
  config.vm.define "server" do |server|
    server.vm.provision "shell", path: "scripts/server.sh"
    server.vm.hostname = "server"
    server.vm.network "private_network", ip: "192.168.56.11"
  end
  config.vm.define "client1" do |client|
    client.vm.provision "shell", path: "scripts/client.sh"
    client.vm.provision "shell", path: "scripts/client-test.sh"
    client.vm.provision "shell", path: "netcore-redis-app/runme.sh"
    client.vm.hostname = "client1"
    client.vm.network "private_network", ip: "192.168.56.12"
  end
  config.vm.define "client2" do |client|
    client.vm.provision "shell", path: "scripts/client.sh"
    client.vm.provision "shell", path: "scripts/client-test.sh"
    client.vm.hostname = "client2"
    client.vm.network "private_network", ip: "192.168.56.13"
  end
end