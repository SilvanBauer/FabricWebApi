#!/bin/bash

cd /home/fabricweb/
if [[ $1 == https://www.youtube.com/* ]] || [[ $1 == https://youtu.be/* ]]; then
	./fabric -y "$1" --transcript | ./fabric -sp extract_wisdom
else if [ -z $2 ]; then
	./fabric "$1"
else if [ $2 == "sessiononly" ]; then
	./fabric "$1" --session="$3"
else if [ -z $3 ]; then
	./fabric -sp "$2" "$1"
else
	./fabric -sp "$2" "$1" --session="$3"
fi
fi
fi
fi
