# Fabric Web API
This API was created to be able to host Fabric on Linux. With this API it's possible to host Fabric in the LAN on a Linux x64 server with NGINX. Authentication is done with a JWT token.

## Installation
This is a quick guide to install the Fabric web API on an Ubuntu server. It's assumed that Fabric has already been downloaded and configured.

### Copying compiled web API to server
**If not already on the Linux server:** Transfer web API contents to home directory. Please adjust the local path and user name on Linux side. This command should be run in powershell **on your local machine in windows**. Other ways to transfer the API are also possible.

`scp -r C:\Users\silva\source\repos\FabricWebApi\FabricWebApi\bin\Release\net8.0\publish silvan@192.168.1.109:/home/silvan`

**If a current installation is already present:** Stop the web api service and remove the old installation.

`sudo systemctl stop kestrel-fabricwebapi.service`

`sudo rm -r /var/www/fabricwebapi`

Move the published directory to the destination and give the owner rights in case you have another user running this API. It's recommended to create another user to run the web API. I created a user named fabricweb. It may be neccessary to create the '/var/www' directory before running this with `sudo mkdir /var/www/`.

`sudo mv publish /var/www/fabricwebapi`

`sudo chown -R fabricweb:fabricweb /var/www/fabricwebapi`

**If a current installation was already present:** Start the web API service again.

`sudo systemctl start kestrel-fabricwebapi.service`

### Install NGINX and create a service

Install .NET runtime

`sudo apt-get install -y aspnetcore-runtime-8.0`

Install NGINX

`sudo apt install nginx`

Start NGINX

`sudo service nginx start`

`sudo systemctl status nginx`

Configure sites_available/default

`sudo nano /etc/nginx/sites-available/default`

The content of default:

```
map $http_connection $connection_upgrade {
  "~*Upgrade" $http_connection;
  default keep-alive;
}

server {
  listen        81;
  location / {
    proxy_pass         http://127.0.0.1:5000/;
    proxy_http_version 1.1;
    proxy_set_header   Upgrade $http_upgrade;
    proxy_set_header   Connection $connection_upgrade;
    proxy_set_header   Host $host;
    proxy_cache_bypass $http_upgrade;
    proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
    proxy_set_header   X-Forwarded-Proto $scheme;
  }
}
```

Configure NGINX timeout

`sudo nano /etc/nginx/nginx.conf`

Add to the http section:

```
fastcgi_read_timeout 5m;
proxy_read_timeout 5m;
```

Reload NGINX

`sudo service nginx reload`

Create service for web API

`sudo nano /etc/systemd/system/kestrel-fabricwebapi.service`

The content of the service file:

```
[Unit]
Description=Fabric Web Api

[Service]
WorkingDirectory=/var/www/fabricwebapi
ExecStart=/usr/bin/dotnet /var/www/fabricwebapi/FabricWebApi.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-fabricwebapi
User=fabricweb # Sets the executing user
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_NOLOGO=true

[Install]
WantedBy=multi-user.target
```

Enable autostart and start the new service.

`sudo systemctl enable kestrel-fabricwebapi.service`

`sudo systemctl start kestrel-fabricwebapi.service`

`sudo systemctl status kestrel-fabricwebapi.service`

### Copy fabricrequest.sh script

To be able to call Fabric from the web API you need to get the fabricrequest.sh in the Scripts directory to the server and make sure that the executing user of the service can also execute this file. The location can be configured in the appsettings.json in the web api directory.

To make the linux clients to be able to call the api you should copy the fabricweb.sh to the client and make it executable.

## Appsettings

There are several things that currently can be configured in the appsettings.json.

 Property | Description
--- | ---
JwtSettings | Settings to configure how the JWT tokens are generated
UseInMemory | Set to true to use in memory database
ConnectionString | Add your database here if you don't use the in memory one
Administrators | A string array containing the names of the administrator users
IsLinux | Currently should always be true and is only used for testing purposes
CommandLin | The path to the fabricrequest.sh file
CommandWin | Only for testing purposes

## Using fabricweb.sh on a client
First the fabricweb.sh needs to be copied to a linux client. Once it is there the fabricweb.sh can be used in various ways to call the API.

Command | Description
--- | ---
. ./fabricweb.sh login | Login with username and password
. ./fabricweb.sh register | Register a new user with username and password
. ./fabricweb.sh request \[pattern\] | Call fabric and optionally specify the pattern used. Alternatively you can use `. ./fabricweb.sh`
. ./fabricweb.sh users | Admin only function which let's you display the users in the database
. ./fabricweb.sh recent | Admin only function which let's you display the 50 last requests and the users who made them

It is also possible to make a youtube summary by pasting a youtube link as the request.

