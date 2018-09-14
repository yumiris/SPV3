#!/bin/sh
# Rudimentary script to compile the CLI DLLs & EXEs to an ISO file then sign it!

# Directories
BIN_DIR="YuMi.Bbkpify.Build"
ISO_DIR="YuMi.Bbkpify.ISO"

# Preparation
mkdir -p "${BIN_DIR}" "${ISO_DIR}"
rsync -rav --progress ./*/bin/Release/*.{dll,exe} README.md "${BIN_DIR}"

# Compiling
ISO_NAME="YuMi.Bbkpify.CLI.$(date +%s)"
ISO_PATH="${ISO_DIR}/${ISO_NAME}.iso" && mkisofs -v -udf -iso-level 4 -V "${ISO_NAME}" -o "${ISO_PATH}" "${BIN_DIR}"

# Signing
gpg --sign "${ISO_PATH}"
