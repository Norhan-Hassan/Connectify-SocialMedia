![image](https://github.com/user-attachments/assets/ffd2de00-3e24-47e6-939a-38e327db9e08)



# API Features

This API offers a rich set of real-time and interactive social media features powered by **SignalR** and **Cloudinary**:

### 💬 Real-Time Messaging
- **👥 Group Chats:** Join and leave groups to chat with multiple users instantly. Messages are broadcast to all group members for lively discussions.  
- **🔒 Private Chats:** Send direct, secure one-on-one messages by targeting specific user connections.  
- **🔗 Connection Management:** The server efficiently manages user connections and group memberships to ensure messages arrive exactly where they should.  

### 📸 Media Upload
- **☁️ Cloudinary Integration:** Seamlessly upload and manage user images via Cloudinary, providing reliable, scalable media storage and delivery.  

### ✍️ Posts & Reactions
- **📝 Post Creation:** Users can create textual posts to share thoughts and updates with an anonymous option.  
- **👍 React & Engage:** React to posts with likes or other reactions.
- **👉🏻 Pokes:** Poke another user and see poked users

### 🟢 User Presence
- **📡 Online Status:** Track and display whether users are currently online for real-time presence awareness.  
- **⏳ Last Online:** Store and show the last time a user was active to provide context on availability.

## ⚙️ How to Run – Cloudinary Configuration

To enable image upload and storage, follow these steps:

1. 🔗 Go to [https://cloudinary.com/](https://cloudinary.com/) and sign up or log in.
2.  From the Cloudinary Dashboard, copy the following credentials:
   - `CloudName`
   - `ApiKey`
   - `ApiSecret`
3.  Add  `appsettings.json` file in your project and add:

```json
"CloudinarySettings": {
  "CloudName": "your_cloud_name",
  "ApiKey": "your_api_key",
  "ApiSecret": "your_api_secret"
}
```
These features combined deliver a dynamic, engaging social media experience focused on instant communication, rich content, presence awareness, and seamless user interactions — all managed efficiently on the server side! ✨
