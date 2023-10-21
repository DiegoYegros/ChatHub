# Chat Service with SignalR

This repository contains a chat service implementation using ASP.NET Core SignalR. The main component, `ChatHub`, facilitates user-to-user and group messaging.

## Features
- **User Connection Management**: Tracks connected users and their respective chat rooms.
- **Room-Based Messaging**: Enables sending messages to specific chat rooms.
- **Real-Time Updates**: Updates the client in real-time using SignalR when users connect, disconnect, or change rooms.
- **System Notifications**: Provides automated messages to notify users of events such as someone joining or leaving a chat room.

## Getting Started

### Prerequisites
- .NET Core SDK (preferably .NET 7.0 or higher)
- An IDE or editor of your choice (e.g., Visual Studio, Visual Studio Code)
