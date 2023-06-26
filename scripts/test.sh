#!/usr/bin/env bash

set -ex

pushd examples
ara pi.ara
./pi
ara fib.ara
./fib
popd
