#!/usr/bin/env bash
set -e

docker build --tag=ara .
docker run -it --mount type=bind,source="$(pwd)/examples",target=/ara/examples -h=docker --rm ara /bin/bash