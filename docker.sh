#!/usr/bin/env bash
set -e

docker build --tag=ara .
docker run -it -h=docker --rm ara /bin/bash