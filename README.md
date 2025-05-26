# Library

Library is a client-server application developed in C# that allows searching PDF files using concurrent programming and TCP socket communication.

The server performs searches based on the file name, title, author, or textual content extracted from PDF files. It uses parallel processing with .NET's native concurrency tools (`Parallel.ForEach` and `Task`) to optimize performance. The client connects to the server, sends the search term, receives the results, and selects which file to open.

The project also uses the PdfPig library to extract metadata (title and author) and textual content from PDF files.

## Features

- Search PDFs by title, author, file name, or content.
- Concurrent processing for faster searches.
- TCP client-server communication.
- Opens PDF files directly after selection.
- Allows multiple searches in the same session.

## How It Works

- The client sends a search term to the server.
- The server searches all PDFs located in the `Books` folder.
- If one result is found, it automatically opens the PDF.
- If multiple results are found, the client receives a numbered list to choose from.
- The client remains connected until the user types `q` to quit.
