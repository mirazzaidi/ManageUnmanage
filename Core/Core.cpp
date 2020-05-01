// Files.cpp : Defines the exported functions and class for the DLL application.
//
#include "stdafx.h"
#include "Core.h"
#include "Utils/Utils.hpp"

namespace Unmanaged
{
    namespace API
    {
        // Here we create a fileExtractor object and register the callback from WPF client
        ICore* CreateCore( ClientCallback clientCallback )
        {
            return new Unmanaged::Core::Files( clientCallback );
        }

        void GetFilename( ICore* obj, int recordNumber, wchar_t* buffer )
        {
            obj->GetFilename( recordNumber, buffer );
        }

        void DestroyCore( ICore * instance )
        {
            delete instance;
        }

        void LoadData( ICore* obj, const wchar_t* dirPath )
        {
            obj->LoadData( std::wstring( dirPath ) );
        }

    }
}

namespace Unmanaged
{
    namespace Core
    {
        void Files::GetFilename( int recordNumber, wchar_t* buffer )
        {
            wcscpy_s( buffer, MAX_PATH, m_fileNameList[ recordNumber ].c_str() );
        }

        
        void Files::LoadData( const std::wstring& path )
        {
            m_fileNameList.clear(); // clean previous values as client asked to refresh the data.
            std::vector<std::wstring> files;
            Unmanaged::Utils::GetFilesInDir( path, files );

            for( auto filename : files )
            {
                std::wstring filePath = path;
                if( filePath[ filePath.size() - 1 ] != '\\' )
                    filePath.append( L"\\" );
                filePath.append( filename );
                m_fileNameList.push_back( filename );
            }
            m_clientCallback( static_cast<const int>( m_fileNameList.size() ) );  // Update the caller about the numberof records available.
        }
    }
}