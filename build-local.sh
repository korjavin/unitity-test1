#!/bin/bash

################################################################################
# Unity Local Build Script for macOS
################################################################################
# This script builds your Unity project locally on your Mac without needing
# GitHub Actions or cloud services.
#
# Usage:
#   ./build-local.sh
#
# Or make it executable first:
#   chmod +x build-local.sh
#   ./build-local.sh
################################################################################

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
PROJECT_PATH="$(pwd)"
BUILD_PATH="$PROJECT_PATH/Builds/macOS"
BUILD_NAME="ZeldaLikeStarter"
UNITY_VERSION="2022.3.10f1"

################################################################################
# Helper Functions
################################################################################

print_header() {
    echo ""
    echo -e "${BLUE}═══════════════════════════════════════════════════════════${NC}"
    echo -e "${BLUE}  $1${NC}"
    echo -e "${BLUE}═══════════════════════════════════════════════════════════${NC}"
    echo ""
}

print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠ $1${NC}"
}

print_info() {
    echo -e "${BLUE}ℹ $1${NC}"
}

################################################################################
# Find Unity Installation
################################################################################

find_unity() {
    print_header "Finding Unity Installation"

    # Common Unity installation paths on macOS
    UNITY_PATHS=(
        "/Applications/Unity/Hub/Editor/$UNITY_VERSION/Unity.app/Contents/MacOS/Unity"
        "/Applications/Unity/Hub/Editor/$UNITY_VERSION/PlaybackEngines/MacStandaloneSupport/Unity.app/Contents/MacOS/Unity"
        "/Applications/Unity/Unity.app/Contents/MacOS/Unity"
    )

    # Try to find Unity Hub's Editor folder with any version
    if [ -d "/Applications/Unity/Hub/Editor" ]; then
        # Find the latest Unity version installed
        LATEST_UNITY=$(ls -1 /Applications/Unity/Hub/Editor | sort -V | tail -n 1)
        if [ -n "$LATEST_UNITY" ]; then
            UNITY_PATHS+=("/Applications/Unity/Hub/Editor/$LATEST_UNITY/Unity.app/Contents/MacOS/Unity")
            print_info "Found Unity version: $LATEST_UNITY"
        fi
    fi

    # Search for Unity executable
    for path in "${UNITY_PATHS[@]}"; do
        if [ -f "$path" ]; then
            UNITY_PATH="$path"
            print_success "Found Unity at: $UNITY_PATH"
            return 0
        fi
    done

    print_error "Unity not found!"
    echo ""
    print_warning "Please install Unity $UNITY_VERSION or update UNITY_VERSION in this script"
    echo ""
    echo "Unity Hub download: https://unity.com/download"
    echo ""
    echo "If Unity is installed in a custom location, you can specify it:"
    echo "  UNITY_PATH=/path/to/Unity ./build-local.sh"
    echo ""
    exit 1
}

################################################################################
# Verify Project
################################################################################

verify_project() {
    print_header "Verifying Project"

    if [ ! -d "$PROJECT_PATH/Assets" ]; then
        print_error "Assets folder not found! Are you in the project root?"
        exit 1
    fi

    if [ ! -d "$PROJECT_PATH/ProjectSettings" ]; then
        print_error "ProjectSettings folder not found! Are you in the project root?"
        exit 1
    fi

    print_success "Project structure verified"
    print_info "Project path: $PROJECT_PATH"
}

################################################################################
# Create Build Directory
################################################################################

prepare_build_directory() {
    print_header "Preparing Build Directory"

    # Create build directory if it doesn't exist
    mkdir -p "$BUILD_PATH"

    # Clean previous build if it exists
    if [ -d "$BUILD_PATH/$BUILD_NAME.app" ]; then
        print_info "Removing previous build..."
        rm -rf "$BUILD_PATH/$BUILD_NAME.app"
    fi

    print_success "Build directory ready: $BUILD_PATH"
}

################################################################################
# Build Project
################################################################################

build_project() {
    print_header "Building Unity Project"

    print_info "This may take 5-15 minutes depending on your Mac..."
    print_info "Unity will run in batch mode (no window will appear)"
    echo ""

    # Build arguments
    BUILD_ARGS=(
        -quit                                    # Quit Unity after build
        -batchmode                               # Run without UI
        -nographics                              # Don't initialize graphics (faster)
        -silent-crashes                          # No crash dialogs
        -logFile -                               # Log to stdout
        -projectPath "$PROJECT_PATH"             # Project to build
        -buildOSXUniversalPlayer "$BUILD_PATH/$BUILD_NAME.app"  # Output path
    )

    # Start time
    START_TIME=$(date +%s)

    # Run Unity build
    if "$UNITY_PATH" "${BUILD_ARGS[@]}"; then
        END_TIME=$(date +%s)
        DURATION=$((END_TIME - START_TIME))

        print_success "Build completed in $DURATION seconds!"
    else
        print_error "Build failed! Check the log above for errors."
        exit 1
    fi
}

################################################################################
# Verify Build
################################################################################

verify_build() {
    print_header "Verifying Build"

    if [ ! -d "$BUILD_PATH/$BUILD_NAME.app" ]; then
        print_error "Build artifact not found!"
        exit 1
    fi

    # Check if executable exists
    if [ ! -f "$BUILD_PATH/$BUILD_NAME.app/Contents/MacOS/$BUILD_NAME" ]; then
        print_error "Executable not found in build!"
        exit 1
    fi

    # Get build size
    BUILD_SIZE=$(du -sh "$BUILD_PATH/$BUILD_NAME.app" | cut -f1)

    print_success "Build verified!"
    print_info "Size: $BUILD_SIZE"
    print_info "Location: $BUILD_PATH/$BUILD_NAME.app"
}

################################################################################
# Summary
################################################################################

print_summary() {
    print_header "Build Complete! 🎉"

    echo "Your game has been built successfully!"
    echo ""
    echo -e "${GREEN}To run your game:${NC}"
    echo "  1. Open Finder"
    echo "  2. Navigate to: $BUILD_PATH"
    echo "  3. Double-click $BUILD_NAME.app"
    echo ""
    echo -e "${GREEN}Or run from terminal:${NC}"
    echo "  open \"$BUILD_PATH/$BUILD_NAME.app\""
    echo ""
    echo -e "${BLUE}To distribute your game:${NC}"
    echo "  1. Zip the .app file"
    echo "  2. Share with others"
    echo "  3. They can extract and run on any Mac"
    echo ""

    # Offer to open build location
    echo -e "${YELLOW}Open build location in Finder? [y/N]${NC} "
    read -r response
    if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
        open "$BUILD_PATH"
    fi
}

################################################################################
# Main Script
################################################################################

main() {
    clear

    print_header "Unity Local Build Script for macOS"

    echo "This script will build your Unity project locally."
    echo "Build will be created at: $BUILD_PATH"
    echo ""

    # Allow custom Unity path via environment variable
    if [ -n "$UNITY_PATH" ]; then
        print_info "Using custom Unity path: $UNITY_PATH"
    else
        find_unity
    fi

    verify_project
    prepare_build_directory
    build_project
    verify_build
    print_summary

    print_success "All done!"
}

# Run main function
main

################################################################################
# TROUBLESHOOTING
################################################################################
#
# Build fails with "Unity not found":
#   → Install Unity Hub and Unity 2022.3.10f1 (or update UNITY_VERSION variable)
#   → Or specify Unity path: UNITY_PATH=/path/to/Unity ./build-local.sh
#
# Build fails with licensing error:
#   → Open Unity Editor and activate your license first
#   → Unity → Preferences → License Management
#
# Build is very slow:
#   → First build is always slower (15-30 minutes)
#   → Subsequent builds are faster (5-10 minutes)
#   → Close other applications for better performance
#
# Build fails with "Assets not found":
#   → Make sure you run this script from the project root folder
#   → cd /path/to/unitity-test1
#   → ./build-local.sh
#
# Want to change build settings?
#   → Edit the variables at the top of this script:
#     - BUILD_PATH: Where to save the build
#     - BUILD_NAME: Name of the .app file
#     - UNITY_VERSION: Target Unity version
#
################################################################################
