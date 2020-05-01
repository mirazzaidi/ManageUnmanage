#pragma once

#include <string>

namespace Unmanaged
{
    namespace API
    {

        // Callback for the client to update the record numbers.
        typedef void( *ClientCallback )( const int recordSize );

        // Exported interface
        class __declspec( dllexport ) ICore
        {
        public:
            virtual void LoadData( const std::wstring& dirPath ) = 0;
            virtual void GetFilename( int recordNum, wchar_t* buffer ) = 0;
        };

        // Exported methods

        // Creates an ICore object given a callback function from the PInvoke client
        extern "C" __declspec( dllexport ) ICore* CreateCore( ClientCallback );

        // Signals the FileExteractor object to start fetching the data.
        extern "C" __declspec( dllexport ) void LoadData( ICore* instance, const wchar_t* dirPath );

        // Gets the filename at recordNum.
        extern "C" __declspec( dllexport ) void GetFilename( ICore* instance, int recordNumber, wchar_t* buffer );

        // Destroys a core object
        extern "C" __declspec( dllexport ) void DestroyCore( ICore* instance );

    }
}