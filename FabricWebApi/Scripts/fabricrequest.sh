#!/bin/bash

cd /home/fabricweb/
if [[ $1 == https://www.youtube.com/watch* ]]; then
	./fabric -y "$1" --transcript | ./fabric -sp extract_wisdom
else if [ -z $2 ]; then
	./fabric "$1"
else
	./fabric -sp "$2" "$1"
fi
fi
