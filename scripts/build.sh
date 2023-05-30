#!/usr/bin/env bash

set -e

pushd parser/tree-sitter
make
popd
pushd parser
npm install
make
popd
pushd libara
make
popd
pushd compiler
dotnet publish Ara -c release -r linux-x64 -o /ara/bin
