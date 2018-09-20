#!/bin/sh
# Rudimentary script to compile the CLI DLLs & EXEs to an ZIP file then sign it!

TAG=$(git describe --tags $(git rev-list --tags --max-count=1))

# Directories
BIN_DIR="YuMi.Bbkpify.${TAG}"
ZIP_DIR="YuMi.Bbkpify.ZIP"

# Building
msbuild /t:Build /p:Configuration=Release /p:TargetFramework=v4.5

# Preparation
rm -rvf "${BIN_DIR}"
mkdir -p "${BIN_DIR}" "${ZIP_DIR}"
rsync -rav --progress ./*/bin/Release/*.{dll,exe} README.md "${BIN_DIR}"

# Compiling
ZIP_NAME="${BIN_DIR}.zip"
ZIP_PATH="${ZIP_DIR}/${ZIP_NAME}"
zip -r0vo "${ZIP_PATH}" "${BIN_DIR}"

# Signing
gpg --sign "${ZIP_PATH}"

# Clean up
rm -rvf "${BIN_DIR}"
