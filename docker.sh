#!/usr/bin/env bash
set -e

docker build --tag=ara .
docker run -it \
    --mount type=bind,source="$(pwd)/parser",target=/ara/parser \
    --mount type=bind,source="$(pwd)/libara",target=/ara/libara \
    --mount type=bind,source="$(pwd)/compiler",target=/ara/compiler \
    --mount type=bind,source="$(pwd)/scripts",target=/ara/scripts \
    --mount type=bind,source="$(pwd)/examples",target=/ara/examples \
    -h=docker --rm ara /bin/bash
