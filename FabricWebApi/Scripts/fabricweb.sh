#!/bin/bash

if [ -z $FabricIp ]; then
	read -p "Enter FabricWebApi Ip > " FabricIp
fi

FabricAddress="http://$FabricIp:81"

if [ -z $1 ] || [ $1 = "login" ]; then
	read -p "Enter username > " FabricUsername
	read -p "Enter password > " FabricPassword

	FabricToken=`curl -X POST -H "Content-Type: application/json" -d "{\"username\": \"$FabricUsername\", \"password\": \"$FabricPassword\"}" "$FabricAddress/Auth/Login"`
	echo "Your token is: $FabricToken"
else if [ $1 = "register" ]; then
	read -p "Enter new username > " FabricUsername
	read -p "Enter new password > " FabricPassword

	FabricResult=`curl -X POST -H "Content-Type: application/json" -d "{\"username\": \"$FabricUsername\", \"password\": \"$FabricPassword\"}" "$FabricAddress/Auth/Register"`
	echo $FabricResult
else if [ $1 = "request" ]; then
	if [ -z $2 ]; then
		echo "Please enter a request to fabric"
	else
		curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer $FabricToken" -d "{\"request\":\"$2\"}" "$FabricAddress/Fabric"
		echo
	fi
else if [ $1 = "users" ]; then
	curl -X GET -H "Authorization: Bearer $FabricToken" "$FabricAddress/Admin/Users"
	echo
else if [ $1 = "recent" ]; then
	curl -X GET -H "Authorization: Bearer $FabricToken" "$FabricAddress/Admin/RecentRequests"
	echo
else
	echo "Could not find operation to execute"
fi
fi
fi
fi
fi
