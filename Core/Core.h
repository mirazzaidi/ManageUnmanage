#pragma once

#include "IUnmanaged.h"
#include <string>
#include <vector>

namespace Unmanaged
{
    namespace  Core
    {

        // Concrete class for the interface class ICore.
        // Implements methods to exposed APIs.
        class Files : public Unmanaged::API::ICore
        {
            // The client's callback is registered here. 
            Unmanaged::API::ClientCallback m_clientCallback;

            // A list for all the filenames.
            std::vector<std::wstring> m_fileNameList;

            // We dont want any copies for this class.
            Files( const Files& ) = delete;

        public:
            explicit Files( Unmanaged::API::ClientCallback clientCallback ) :
                m_clientCallback( clientCallback ),
                m_fileNameList( {} )
            {}

            virtual ~Files() = default;

            // Interface methods.
            virtual void LoadData( const std::wstring& path ) override;
            virtual void GetFilename( int recordNum, wchar_t* buffer ) override;
        };
    }
}