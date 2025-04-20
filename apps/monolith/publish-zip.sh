#!/bin/bash

set -e

APP_NAME="ContosoUniversity.Mvc"
PROJECT_PATH="src/$APP_NAME/$APP_NAME.csproj"
PUBLISH_DIR="artifacts"
ZIP_FILE="app.zip"

echo "🧹 Cleaning previous build..."
rm -rf "$ZIP_FILE"
mkdir -p "$PUBLISH_DIR"
rm -rf "$PUBLISH_DIR"/*

echo "📦 Publishing $PROJECT_PATH to $PUBLISH_DIR..."
dotnet publish "$PROJECT_PATH" -c Release -o "$PUBLISH_DIR"

echo "🗜 Creating ZIP archive (excluding nested publish/): $ZIP_FILE..."
cd "$PUBLISH_DIR"
find . -type f ! -path "./publish/*" | zip -@ "../$ZIP_FILE"
cd - > /dev/null

echo "✅ Done! ZIP created at: $ZIP_FILE"
echo
unzip -l "$ZIP_FILE"
