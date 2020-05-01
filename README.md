# Communicating between managed and unmanaged code

## Summary
This project demonstrates simple communication between unmanaged C++ DLL and a managed WPF APP.
We are using C++ Interop (Implicit PInvoke) method for this communication.

https://docs.microsoft.com/en-us/cpp/dotnet/using-cpp-interop-implicit-pinvoke?view=vs-2019

## C++ Core DLL.
It exposes following methods.

- CreateCore:

    Creates an ICore object given a callback function from the PInvoke client.

- LoadData

    This function allows client to send signal to Core DLL to load the data from provided path. After loading the  data this function also feeds back the client about number of records it loaded.

- GetFilename
    
    To reduce the memory load on client intially only file names are sent. This method returns a file name at a given record numbe in list.

- DestroyCore:

    Destroys an ICore object

## C# WPF Client

It has following main components:
- GUI Update Delegate
    
    This is a delegate function that it registers with the C++ DLL to signal it when data is available so we can update the GUI list.

- FileList

    This is a UI component that gets updated with the fetched file names from core. 
