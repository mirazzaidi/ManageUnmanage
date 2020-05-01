#pragma once

#include <string>
#include <filesystem>

namespace fs = std::experimental::filesystem;
namespace Unmanaged
{
    // Common place for our utility functions.
    namespace Utils
    {
        // Gets files in a directory, ignores folders.
        void GetFilesInDir( std::wstring path, std::vector<std::wstring> &files )
        {
            for( auto &current : fs::directory_iterator( path ) ) // Not considering recursive part
            {
                if( !fs::is_directory( current.path() ) )
                    files.push_back( std::wstring( current.path().filename().c_str() ) );
            }
        }
    }
}