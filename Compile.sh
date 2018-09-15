#!/bin/sh
# Rudimentary script to compile the CLI DLLs & EXEs to an ISO file then sign it!

# Directories
BIN_DIR="YuMi.Bbkpify.Build"
ISO_DIR="YuMi.Bbkpify.ISO"

# Building
msbuild /t:Build /p:Configuration=Release /p:TargetFramework=v4.5

# Preparation
mkdir -p "${BIN_DIR}" "${ISO_DIR}"
rsync -rav --progress ./*/bin/Release/*.{dll,exe} README.md "${BIN_DIR}"

# Compiling
ISO_NAME="YuMi.Bbkpify.CLI.$(git describe --tags $(git rev-list --tags --max-count=1))"
ISO_PATH="${ISO_DIR}/${ISO_NAME}.iso" && mkisofs -v -udf -iso-level 4 -V "${ISO_NAME}" -o "${ISO_PATH}" "${BIN_DIR}"

# Signing
gpg --sign "${ISO_PATH}"
