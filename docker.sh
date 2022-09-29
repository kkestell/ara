#!/usr/bin/env bash
set -e

podman build --tag=ara .
podman run -it --mount type=bind,source="$(pwd)/examples",target=/ara/examples -h=docker --rm ara /bin/bash
