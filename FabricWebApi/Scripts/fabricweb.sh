#!/bin/bash

if [ -z $FabricIp ]; then
	read -p "Enter FabricWebApi Ip > " FabricIp
fi

FabricAddress="http://$FabricIp:81"

if [ -z $1 ] || [ $1 = "request" ]; then
	read -p "Enter your request > " FabricRequest
	read -p "Enter session name (leave empty for no session) > " FabricSession

	curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer $FabricToken" -d "{\"request\":\"$FabricRequest\", \"pattern\": \"$2\", \"session\": \"$FabricSession\"}" "$FabricAddress/Fabric"
	echo
else if [ $1 = "sessions" ]; then
	curl -X GET -H "Authorization: Bearer $FabricToken" "$FabricAddress/Fabric/Sessions"
	echo
else if [ $1 = "wipesession" ]; then
	read -p "Enter name of session to wipe > " FabricSession

	curl -X POST -H "Authorization: Bearer $FabricToken" "$FabricAddress/Fabric/WipeSession/$FabricSession"
	echo
else if [ $1 = "login" ]; then
	read -p "Enter username > " FabricUsername
	read -s -p "Enter password > " FabricPassword

	FabricToken=`curl -X POST -H "Content-Type: application/json" -d "{\"username\": \"$FabricUsername\", \"password\": \"$FabricPassword\"}" "$FabricAddress/Auth/Login"`
	echo "Your token is: $FabricToken"
else if [ $1 = "register" ]; then
	read -p "Enter new username > " FabricUsername
	read -s -p "Enter new password > " FabricPassword

	FabricResult=`curl -X POST -H "Content-Type: application/json" -d "{\"username\": \"$FabricUsername\", \"password\": \"$FabricPassword\"}" "$FabricAddress/Auth/Register"`
	echo $FabricResult
else if [ $1 = "user" ]; then
	curl -X GET -H "Authorization: Bearer $FabricToken" "$FabricAddress/Auth/User"
	echo
else if [ $1 = "users" ]; then
	curl -X GET -H "Authorization: Bearer $FabricToken" "$FabricAddress/Admin/Users"
	echo
else if [ $1 = "recent" ]; then
	curl -X GET -H "Authorization: Bearer $FabricToken" "$FabricAddress/Admin/RecentRequests"
	echo
else if [ $1 = "block" ]; then
	read -p "Enter username > " FabricUsername

	curl -X POST -H "Authorization: Bearer $FabricToken" "$FabricAddress/Admin/BlockUser/$FabricUsername/true"
	echo
else if [ $1 = "unblock" ]; then
	read -p "Enter username > " FabricUsername

	curl -X POST -H "Authorization: Bearer $FabricToken" "$FabricAddress/Admin/BlockUser/$FabricUsername/false"
	echo
else
	echo "Could not find operation to execute"
fi
fi
fi
fi
fi
fi
fi
fi
fi
fi
