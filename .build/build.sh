#!/bin/bash
source `dirname "$0"`/functions.sh

# Install pre-reqs
dotnet tool install -g GitVersion.Tool

# Clear artifacts folder
rm .artifacts/ -rf
mkdir .artifacts

# Calculate the version
GitVersionOutput=dotnet-gitversion
Version=$($GitVersionOutput | jq -r '.MajorMinorPatch')
NuGetVersion=$($GitVersionOutput | jq -r '.NuGetVersion')

# Write version to file
touch .artifacts/version.json
$GitVersionOutput > .artifacts/version.json

# Build
WriteHeading "Building version: $Version"
dotnet build -c Release /p:Version=$Version

# Test
WriteHeading "Running tests"
dotnet test --logger "trx;LogFileName=LazyStorage.Tests.trx"

# Create NuGet Packag
WriteHeading "Creating NuGet Package: $NuGetVersion"
dotnet pack -c Release -o ".artifacts/" /p:Version=$Version /p:PackageVersion=$NuGetVersion